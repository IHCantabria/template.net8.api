# This script automates the installation and execution of SonarScanner for .NET on Windows.
# It also includes validation of configuration parameters, connectivity tests to the SonarQube server,
# and checks the Quality Gate status after the scan.

#usage:
#   .\sonarqube-windows.ps1 -SonarScannerVersion "11.0.0" -ProjectKey "my_project_key" -SonarHostUrl "http://mysonarqube.server:9000" -SonarToken "my_sonar_token" -ProjectDir "path\to\my\project" -SolutionFile "MySolution.sln" -SkipConnectivityTest -SkipQualityGateCheck
#
# Parameters:
#   -SonarScannerVersion  : SonarScanner for .NET version (default: "11.0.0")
#   -ProjectKey           : SonarQube project key (default: "PROJECT_KEY")
#   -SonarHostUrl         : SonarQube server URL (default: "https://ihsonarqube.ihcantabria.com")
#   -SonarToken           : SonarQube authentication token (default: "SONAR_TOKEN_KEY")
#   -ProjectDir           : Project directory to scan (default: ".")
#   -SolutionFile         : Solution file to build (default: auto-detect *.sln)
#   -SkipConnectivityTest : Skip server connectivity test (switch)
#   -SkipQualityGateCheck : Skip Quality Gate verification (switch)

param(
    [string]$SonarScannerVersion = "11.0.0",
    [string]$ProjectKey = "PROJECT_KEY",
    [string]$SonarHostUrl = "https://ihsonarqube.ihcantabria.com",
    [string]$SonarToken = "SONAR_TOKEN_KEY",
    [string]$ProjectDir = ".",
    [string]$SolutionFile = "",
    [switch]$SkipConnectivityTest = $false,
    [switch]$SkipQualityGateCheck = $false
)

# Configuration
# (No working directory configuration needed; Scanner for .NET manages its own .sonarqube folder)

# Utility: Clean partial files and working directories

# Utility: Install SonarScanner for .NET
function Install-SonarScanner {
    Write-Output "Checking if SonarScanner for .NET is installed..."
    
    try {
        $dotnetToolList = dotnet tool list --global
        
        if ($dotnetToolList -match "dotnet-sonarscanner") {
            Write-Output "SonarScanner for .NET is already installed."
            
            # Check version
            $installedVersion = ($dotnetToolList | Select-String "dotnet-sonarscanner").ToString().Split()[1]
            Write-Output "Installed version: $installedVersion"
            
            # Update if needed
            if ($installedVersion -ne $SonarScannerVersion) {
                Write-Output "Updating SonarScanner for .NET to version $SonarScannerVersion..."
                dotnet tool update dotnet-sonarscanner --global --version $SonarScannerVersion
                
                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to update SonarScanner for .NET"
                }
                Write-Output "SonarScanner for .NET updated successfully."
            }
        } else {
            Write-Output "Installing SonarScanner for .NET version $SonarScannerVersion..."
            dotnet tool install dotnet-sonarscanner --global --version $SonarScannerVersion
            
            if ($LASTEXITCODE -ne 0) {
                throw "Failed to install SonarScanner for .NET"
            }
            Write-Output "SonarScanner for .NET installed successfully."
        }
    } catch {
        Write-Error "Error during SonarScanner installation: $_"
        exit 1
    }
}

# Utility: Validate configuration
function Validate-Configuration {
    Write-Output "Validating configuration..."
    $hasErrors = $false
    
    # Validate SonarQube token
    if ([string]::IsNullOrWhiteSpace($SonarToken) -or $SonarToken -eq "SONAR_TOKEN_KEY") {
        Write-Error "Invalid SONAR_TOKEN. Please provide a valid SonarQube authentication token."
        $hasErrors = $true
    }
    
    # Validate project key
    if ([string]::IsNullOrWhiteSpace($ProjectKey) -or $ProjectKey -eq "PROJECT_KEY") {
        Write-Error "Invalid PROJECT_KEY. Please provide a valid SonarQube project key."
        $hasErrors = $true
    }
    
    # Validate SonarQube host URL
    if ([string]::IsNullOrWhiteSpace($SonarHostUrl)) {
        Write-Error "Invalid SONAR_HOST_URL. Please provide a valid SonarQube server URL."
        $hasErrors = $true
    }
    
    # Validate project directory
    if (-not (Test-Path $ProjectDir)) {
        Write-Error "Project directory does not exist: $ProjectDir"
        $hasErrors = $true
    }
    
    # Validate SonarScanner for .NET version format
    if (-not ($SonarScannerVersion -match '^\d+\.\d+\.\d+$')) {
        Write-Warning "SonarScanner for .NET version format may be incorrect: $SonarScannerVersion"
    }
    
    # Check if dotnet CLI is available
    try {
        $dotnetVersion = dotnet --version
        Write-Output ".NET SDK version: $dotnetVersion"
    } catch {
        Write-Error ".NET SDK not found. Please install .NET SDK 8.0 or higher."
        $hasErrors = $true
    }
    
    if ($hasErrors) {
        Write-Error "Configuration validation failed. Please fix the errors above."
        exit 1
    }
    
    Write-Output "Configuration validation passed successfully."
}

# Utility: Test SonarQube server connectivity
function Test-SonarQubeConnectivity {
    if ($SkipConnectivityTest) {
        Write-Output "Connectivity test skipped by user request."
        return
    }
    
    Write-Output "Testing SonarQube server connectivity..."
    
    try {
        $response = Invoke-WebRequest -Uri "$SonarHostUrl/api/system/status" -Method GET -UseBasicParsing -TimeoutSec 10
        if ($response.StatusCode -eq 200) {
            Write-Output "SonarQube server is accessible."
            return $true
        } else {
            Write-Warning "SonarQube server responded with status: $($response.StatusCode)"
            return $false
        }
    } catch {
        Write-Warning "Could not connect to SonarQube server: $_"
        Write-Output "Proceeding with scan (server may still be accessible during scan)..."
        return $false
    }
}

# Utility: Find solution file
function Find-SolutionFile {
    param([string]$directory)
    
    if (-not [string]::IsNullOrWhiteSpace($SolutionFile)) {
        $solutionPath = Join-Path $directory $SolutionFile
        if (Test-Path $solutionPath) {
            Write-Host "Using specified solution file: $SolutionFile"
            return $SolutionFile
        } else {
            Write-Error "Specified solution file not found: $SolutionFile"
            exit 1
        }
    }
    
    $solutionFiles = Get-ChildItem -Path $directory -Filter "*.sln" -File
    
    if ($solutionFiles.Count -eq 0) {
        Write-Error "No solution file (*.sln) found in directory: $directory"
        exit 1
    } elseif ($solutionFiles.Count -eq 1) {
        Write-Host "Found solution file: $($solutionFiles[0].Name)"
        return $solutionFiles[0].Name
    } else {
        Write-Host "Multiple solution files found:"
        $solutionFiles | ForEach-Object { Write-Host "  - $($_.Name)" }
        Write-Host "Using first solution file: $($solutionFiles[0].Name)"
        Write-Host "To specify a different solution, use the -SolutionFile parameter"
        return $solutionFiles[0].Name
    }
}

# Utility: Run SonarQube scan
function Run-SonarScan {
    Write-Output "Verifying project directory..."
    $absoluteProjectDir = Resolve-Path $ProjectDir -ErrorAction SilentlyContinue
    if (-not $absoluteProjectDir -or -not (Test-Path $absoluteProjectDir)) {
        Write-Error "Project directory does not exist: $ProjectDir"
        exit 1
    }

    Write-Output "Project directory: $absoluteProjectDir"
    
    # Set working directory
    $originalLocation = Get-Location
    Set-Location -Path $absoluteProjectDir

    try {
        # Find solution file
        $solution = Find-SolutionFile -directory $absoluteProjectDir
        
        Write-Output "Starting SonarQube analysis for .NET..."
        
        # Step 1: Begin analysis
        Write-Output "Step 1: Begin SonarQube analysis..."
        dotnet sonarscanner begin `
            /k:"$ProjectKey" `
            /d:sonar.host.url="$SonarHostUrl" `
            /d:sonar.token="$SonarToken"
        
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to begin SonarQube analysis"
            exit 1
        }
        
        # Step 2: Build the solution
        Write-Output "Step 2: Building solution..."
        dotnet build $solution --configuration Release
        
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Build failed"
            exit 1
        }
        
        # Step 3: End analysis and upload results
        Write-Output "Step 3: Ending analysis and uploading results..."
        dotnet sonarscanner end /d:sonar.token="$SonarToken"
        
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to end SonarQube analysis"
            exit 1
        }
        
        Write-Output "SonarQube scan completed successfully."
        
    } catch {
        Write-Error "Error during SonarQube scan execution: $_"
        exit 1
    } finally {
        # Restore original location
        Set-Location -Path $originalLocation
        
        # No temporary directories to clean (Scanner for .NET uses .sonarqube internally)
    }
}

# Utility: Validate SonarQube token and permissions
function Test-SonarQubeAuthentication {
    Write-Output "Validating SonarQube authentication..."
    
    try {
        $headers = @{
            "Authorization" = "Bearer $SonarToken"
        }
        
        # Test authentication with a simple API call
        $authTestUrl = "$SonarHostUrl/api/authentication/validate"
        $response = Invoke-WebRequest -Uri $authTestUrl -Headers $headers -Method GET -UseBasicParsing -TimeoutSec 10
        
        if ($response.StatusCode -eq 200) {
            $authData = $response.Content | ConvertFrom-Json
            if ($authData.valid -eq $true) {
                Write-Output "Token authentication successful."
                return $true
            } else {
                Write-Error "Token is not valid."
                return $false
            }
        } else {
            Write-Warning "Authentication test returned status: $($response.StatusCode)"
            return $false
        }
    } catch {
        if ($_.Exception.Message -match "403") {
            Write-Error "Authentication failed (403 Forbidden). Please check your SonarQube token."
            Write-Output "Token troubleshooting:"
            Write-Output "1. Verify the token is correct and not expired"
            Write-Output "2. Ensure the token has 'Execute Analysis' permissions"
            Write-Output "3. Check if the project key '$ProjectKey' exists and you have access to it"
            Write-Output "4. Verify the SonarQube server URL: $SonarHostUrl"
        } elseif ($_.Exception.Message -match "401") {
            Write-Error "Authentication failed (401 Unauthorized). Token may be invalid or expired."
        } else {
            Write-Warning "Could not validate authentication: $_"
        }
        return $false
    }
}

# Utility: Check Quality Gate status
function Check-QualityGate {
    if ($SkipQualityGateCheck) {
        Write-Output "Quality Gate check skipped by user request."
        return
    }
    
    Write-Output "Checking Quality Gate status..."
    
    # First validate authentication
    if (-not (Test-SonarQubeAuthentication)) {
        Write-Error "Cannot proceed with Quality Gate check due to authentication issues."
        exit 1
    }
    
    try {
        $maxAttempts = 5
        $attempt = 0
        $qualityGateChecked = $false
        
        $headers = @{
            "Authorization" = "Bearer $SonarToken"
        }
        
        while ($attempt -lt $maxAttempts -and -not $qualityGateChecked) {
            $attempt++
            Write-Output "Attempt $attempt/$maxAttempts - Checking Quality Gate status..."
            
            try {
                # Get project status from SonarQube API
                $qualityGateUrl = "$SonarHostUrl/api/qualitygates/project_status?projectKey=$ProjectKey"
                $response = Invoke-WebRequest -Uri $qualityGateUrl -Headers $headers -Method GET -UseBasicParsing -TimeoutSec 15
                $qualityGateData = $response.Content | ConvertFrom-Json
                
                # Check if we have valid quality gate data
                if ($qualityGateData.projectStatus -and $qualityGateData.projectStatus.status) {
                    $projectStatus = $qualityGateData.projectStatus.status
                    Write-Output "Quality Gate Status: $projectStatus"
                    $qualityGateChecked = $true
                    
                    if ($projectStatus -eq "ERROR") {
                        Write-Host "Quality Gate FAILED! The project has critical issues that need to be addressed." -ForegroundColor Red
                        
                        # Show detailed information about failed conditions
                        if ($qualityGateData.projectStatus.conditions) {
                            Write-Host "Failed conditions:" -ForegroundColor Yellow
                            foreach ($condition in $qualityGateData.projectStatus.conditions) {
                                if ($condition.status -eq "ERROR") {
                                    Write-Host "  - $($condition.metricKey): $($condition.actualValue) (threshold: $($condition.errorThreshold))" -ForegroundColor Red
                                }
                            }
                        }
                        
                        Write-Host "Please check the SonarQube dashboard for detailed analysis: $SonarHostUrl/dashboard?id=$ProjectKey" -ForegroundColor Yellow
                        exit 1
                    } elseif ($projectStatus -eq "WARN") {
                        Write-Warning "Quality Gate passed with warnings. Consider reviewing the issues found."
                        
                        # Show warning conditions
                        if ($qualityGateData.projectStatus.conditions) {
                            Write-Host "Warning conditions:" -ForegroundColor Yellow
                            foreach ($condition in $qualityGateData.projectStatus.conditions) {
                                if ($condition.status -eq "WARN") {
                                    Write-Host "  - $($condition.metricKey): $($condition.actualValue) (threshold: $($condition.warningThreshold))" -ForegroundColor Yellow
                                }
                            }
                        }
                    } else {
                        Write-Output "Quality Gate PASSED! No critical issues found."
                    }
                    break
                } else {
                    Write-Warning "Quality Gate data not yet available. Analysis may still be processing..."
                    if ($attempt -lt $maxAttempts) {
                        Start-Sleep -Seconds 10
                    }
                }
                
            } catch {
                $errorMessage = $_.Exception.Message
                if ($errorMessage -match "403") {
                    Write-Error "Access denied (403) when checking Quality Gate. Possible causes:"
                    Write-Output "1. Token lacks permissions for project '$ProjectKey'"
                    Write-Output "2. Project key '$ProjectKey' does not exist"
                    Write-Output "3. Token does not have 'Browse' permissions on the project"
                    exit 1
                } elseif ($errorMessage -match "404") {
                    Write-Warning "Project not found (404). Analysis may not be complete yet..."
                    if ($attempt -lt $maxAttempts) {
                        Start-Sleep -Seconds 10
                    }
                } else {
                    Write-Warning "Could not check Quality Gate status (attempt $attempt): $errorMessage"
                    if ($attempt -lt $maxAttempts) {
                        Start-Sleep -Seconds 10
                    }
                }
            }
        }
        
        if (-not $qualityGateChecked) {
            Write-Warning "Could not retrieve Quality Gate status after $maxAttempts attempts."
            Write-Output "Analysis completed but Quality Gate status is unknown."
            Write-Output "Please check manually: $SonarHostUrl/dashboard?id=$ProjectKey"
        }
        
    } catch {
        Write-Warning "Could not retrieve Quality Gate status: $_"
        Write-Output "Analysis completed but Quality Gate status is unknown."
        Write-Output "Please check manually: $SonarHostUrl/dashboard?id=$ProjectKey"
    }
}



# Execution flow
try {
    Write-Output "=== SonarScanner for .NET Automation Script ==="
    Write-Output "Scanner Version: $SonarScannerVersion"
    Write-Output "Project Key: $ProjectKey"
    Write-Output "Host URL: $SonarHostUrl"
    Write-Output "Project Directory: $ProjectDir"
    Write-Output "Solution File: $SolutionFile"
    Write-Output "Skip Connectivity Test: $SkipConnectivityTest"
    Write-Output "Skip Quality Gate Check: $SkipQualityGateCheck"
    Write-Output "================================================"
    
    Validate-Configuration
    Test-SonarQubeConnectivity
    Install-SonarScanner
    Run-SonarScan
    Check-QualityGate
    
    Write-Output "=== SonarQube analysis completed successfully! ==="
} catch {
    Write-Error "Script execution failed: $_"
    # No temporary cleanup required
    exit 1
}

exit 0