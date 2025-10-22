# This script automates the download, configuration, and execution of SonarScanner for .NET on Windows.
# It properly analyzes C# projects using the MSBuild integration.

param(
    [string]$SonarScannerVersion = "8.3.0.105075",
    [string]$ProjectKey = "IHCantabria_template.net8.api_608e698e-7d6e-4fad-b3a6-58dee711500b",
    [string]$SonarHostUrl = "http://ihsonarqube.ihcantabria.com:9000",
    [string]$SonarToken = "sqp_140c06ff8b596e50718ed8aa20fb23e1bc85121b",
    [string]$SolutionFile = "template.net8.api.sln",
    [switch]$SkipConnectivityTest = $false,
    [switch]$SkipQualityGateCheck = $false
)

# Configuration
$WorkspaceRoot = $(Get-Location).Path
$TempDownloadDir = Join-Path $WorkspaceRoot "sonar-temp-download"
$SonarDirectory = Join-Path $WorkspaceRoot ".sonar"
$SonarScannerHome = Join-Path $SonarDirectory "sonar-scanner-$SonarScannerVersion-net"
$ScannerZip = Join-Path $TempDownloadDir "sonar-scanner-dotnet.zip"
$ScannerExecutable = Join-Path $SonarScannerHome "SonarScanner.MSBuild.dll"
$DotNetCommand = "dotnet"

# Utility: Clean partial files
function Cleanup-PartialFiles {
    param([bool]$IncludeScanner = $false)
    
    Write-Output "Cleaning up partial files..."
    
    $pathsToClean = @($ScannerZip, $TempDownloadDir)
    if ($IncludeScanner) { $pathsToClean += $SonarDirectory }
    
    foreach ($path in $pathsToClean) {
        if (Test-Path $path) {
            Remove-Item $path -Recurse -Force -ErrorAction SilentlyContinue
            Write-Output "Removed: $path"
        }
    }
}

# Utility: Download SonarScanner for .NET
function Download-SonarScannerDotNet {
    if (Test-Path $ScannerExecutable) {
        Write-Output "SonarScanner for .NET already exists and is functional. Reusing."
        return
    }

    try {
        Write-Output "Downloading SonarScanner for .NET v$SonarScannerVersion..."
        
        # Create directories
        New-Item -ItemType Directory -Force -Path $TempDownloadDir | Out-Null
        New-Item -ItemType Directory -Force -Path $SonarDirectory | Out-Null

        $ScannerUrl = "https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/$SonarScannerVersion/sonar-scanner-$SonarScannerVersion-net.zip"
        
        Write-Output "Downloading from: $ScannerUrl"
        try {
            Invoke-WebRequest -Uri $ScannerUrl -OutFile $ScannerZip -UseBasicParsing
        } catch {
            # Try alternative URL format (for older versions)
            Write-Warning "Primary URL failed, trying alternative format..."
            $ScannerUrl = "https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/$SonarScannerVersion/sonar-scanner-msbuild-$SonarScannerVersion-net.zip"
            Write-Output "Downloading from: $ScannerUrl"
            try {
                Invoke-WebRequest -Uri $ScannerUrl -OutFile $ScannerZip -UseBasicParsing
            } catch {
                # Try without msbuild in name (for very old versions)
                Write-Warning "Alternative URL failed, trying legacy format..."
                $ScannerUrl = "https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/$SonarScannerVersion/sonar-scanner-$SonarScannerVersion-net.zip"
                Write-Output "Downloading from: $ScannerUrl"
                Invoke-WebRequest -Uri $ScannerUrl -OutFile $ScannerZip -UseBasicParsing
            }
        }
        
        # Verify download
        if (-not (Test-Path $ScannerZip) -or (Get-Item $ScannerZip).Length -eq 0) {
            throw "Failed to download SonarScanner for .NET or file is empty"
        }

        Write-Output "Extracting SonarScanner for .NET..."
        Expand-Archive -Path $ScannerZip -DestinationPath $SonarScannerHome -Force
        
        # Verify extraction
        if (-not (Test-Path $ScannerExecutable)) {
            throw "SonarScanner for .NET executable not found after extraction"
        }

        # Cleanup download files
        Remove-Item $ScannerZip -Force -ErrorAction SilentlyContinue
        Remove-Item $TempDownloadDir -Recurse -Force -ErrorAction SilentlyContinue

        Write-Output "SonarScanner for .NET setup completed successfully."
    } catch {
        Write-Error "Error during SonarScanner for .NET download or extraction: $_"
        Cleanup-PartialFiles -IncludeScanner $true
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
    
    # Validate solution file
    if (-not (Test-Path $SolutionFile)) {
        Write-Error "Solution file does not exist: $SolutionFile"
        $hasErrors = $true
    }
    
    # Validate .NET SDK
    try {
        $dotnetVersion = & $DotNetCommand --version 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Error ".NET SDK is not installed or not in PATH"
            $hasErrors = $true
        } else {
            Write-Output ".NET SDK version: $dotnetVersion"
        }
    } catch {
        Write-Error ".NET SDK is not installed or not accessible: $_"
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

# Utility: Run SonarScanner for .NET
function Run-SonarScanDotNet {
    Write-Output "Starting SonarQube analysis for .NET project..."
    
    $originalLocation = Get-Location
    
    try {
        # Step 1: Begin analysis
        Write-Output "Step 1: Beginning SonarQube analysis..."
        $beginArgs = @(
            $ScannerExecutable,
            "begin",
            "/k:$ProjectKey",
            "/d:sonar.host.url=$SonarHostUrl",
            "/d:sonar.token=$SonarToken"
        )
        
        Write-Output "Executing: $DotNetCommand $($beginArgs -join ' ')"
        & $DotNetCommand @beginArgs
        
        if ($LASTEXITCODE -ne 0) {
            throw "SonarScanner begin step failed with exit code: $LASTEXITCODE"
        }
        
        Write-Output "Analysis begin step completed successfully."
        
        # Step 2: Build the solution
        Write-Output "Step 2: Building solution..."
        Write-Output "Executing: $DotNetCommand build $SolutionFile"
        & $DotNetCommand build $SolutionFile
        
        if ($LASTEXITCODE -ne 0) {
            throw "Build failed with exit code: $LASTEXITCODE"
        }
        
        Write-Output "Build completed successfully."
        
        # Step 3: End analysis
        Write-Output "Step 3: Ending SonarQube analysis..."
        $endArgs = @(
            $ScannerExecutable,
            "end",
            "/d:sonar.token=$SonarToken"
        )
        
        Write-Output "Executing: $DotNetCommand $($endArgs -join ' ')"
        & $DotNetCommand @endArgs
        
        if ($LASTEXITCODE -ne 0) {
            throw "SonarScanner end step failed with exit code: $LASTEXITCODE"
        }
        
        Write-Output "Analysis end step completed successfully."
        
    } catch {
        Write-Error "Error during SonarQube analysis: $_"
        Set-Location -Path $originalLocation
        exit 1
    } finally {
        Set-Location -Path $originalLocation
    }
}

# Utility: Check Quality Gate status
function Check-QualityGate {
    if ($SkipQualityGateCheck) {
        Write-Output "Quality Gate check skipped by user request."
        return
    }
    
    Write-Output "Checking Quality Gate status..."
    
    try {
        $maxAttempts = 10
        $attempt = 0
        $qualityGateChecked = $false
        
        $headers = @{
            "Authorization" = "Bearer $SonarToken"
        }
        
        while ($attempt -lt $maxAttempts -and -not $qualityGateChecked) {
            $attempt++
            Write-Output "Attempt $attempt/$maxAttempts - Checking Quality Gate status..."
            
            try {
                $qualityGateUrl = "$SonarHostUrl/api/qualitygates/project_status?projectKey=$ProjectKey"
                $response = Invoke-WebRequest -Uri $qualityGateUrl -Headers $headers -Method GET -UseBasicParsing -TimeoutSec 15
                $qualityGateData = $response.Content | ConvertFrom-Json
                
                if ($qualityGateData.projectStatus -and $qualityGateData.projectStatus.status) {
                    $projectStatus = $qualityGateData.projectStatus.status
                    Write-Output "Quality Gate Status: $projectStatus"
                    $qualityGateChecked = $true
                    
                    if ($projectStatus -eq "ERROR") {
                        Write-Host "Quality Gate FAILED! The project has critical issues that need to be addressed." -ForegroundColor Red
                        
                        if ($qualityGateData.projectStatus.conditions) {
                            Write-Host "Failed conditions:" -ForegroundColor Yellow
                            foreach ($condition in $qualityGateData.projectStatus.conditions) {
                                if ($condition.status -eq "ERROR") {
                                    Write-Host "  - $($condition.metricKey): $($condition.actualValue) (threshold: $($condition.errorThreshold))" -ForegroundColor Red
                                }
                            }
                        }
                        
                        Write-Host "Please check the SonarQube dashboard: $SonarHostUrl/dashboard?id=$ProjectKey" -ForegroundColor Yellow
                        exit 1
                    } elseif ($projectStatus -eq "WARN") {
                        Write-Warning "Quality Gate passed with warnings. Consider reviewing the issues found."
                        
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
                    Write-Error "Access denied (403) when checking Quality Gate."
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
            Write-Output "Please check manually: $SonarHostUrl/dashboard?id=$ProjectKey"
        }
        
    } catch {
        Write-Warning "Could not retrieve Quality Gate status: $_"
        Write-Output "Please check manually: $SonarHostUrl/dashboard?id=$ProjectKey"
    }
}

# Execution flow
try {
    Write-Output "=== SonarScanner for .NET Automation Script ==="
    Write-Output "Scanner Version: $SonarScannerVersion"
    Write-Output "Project Key: $ProjectKey"
    Write-Output "Host URL: $SonarHostUrl"
    Write-Output "Solution File: $SolutionFile"
    Write-Output "Skip Connectivity Test: $SkipConnectivityTest"
    Write-Output "Skip Quality Gate Check: $SkipQualityGateCheck"
    Write-Output "================================================"
    
    Validate-Configuration
    Test-SonarQubeConnectivity
    Download-SonarScannerDotNet
    Run-SonarScanDotNet
    Check-QualityGate
    
    Write-Output "=== SonarQube analysis completed successfully! ==="
} catch {
    Write-Error "Script execution failed: $_"
    Cleanup-PartialFiles
    exit 1
}

exit 0
