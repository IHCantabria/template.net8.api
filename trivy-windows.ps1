# TODO: NO QUIERO VER ESTE FICHERO EN LOS REPO DE PROYECTOS. RENOMBRADLO A trivy.ps1 AL BAJAROS LA PLANTILLA

# This script automates the download, configuration, and execution of Trivy security scanner on Windows.
# It also includes validation of configuration parameters, downloading of necessary templates,
# and checks for vulnerabilities in the scan results.

#usage:
#   .\trivy-windows.ps1 -TrivyVersion "0.66.0" -ScanTarget "." -ReportsFolder "trivy-reports" -SkipVulnerabilityCheck
#
# Parameters:
#   -TrivyVersion           : Trivy version to download (default: "0.66.0")
#   -ScanTarget            : Target directory or file to scan (default: ".")
#   -ReportsFolder         : Output folder for reports (default: "trivy-reports")
#   -SkipVulnerabilityCheck: Don't fail if vulnerabilities are found (switch)

param(
    [string]$TrivyVersion = "0.66.0",
    [string]$ScanTarget = ".",
    [string]$ReportsFolder = "trivy-reports",
    [switch]$SkipVulnerabilityCheck = $false
)

# Configuration
$WorkspaceRoot = $(Get-Location).Path
$TempDownloadDir = Join-Path $WorkspaceRoot "trivy-temp-download"
$TrivyDirectory = Join-Path $WorkspaceRoot ".trivy"
$TrivyZip = Join-Path $TempDownloadDir "trivy.zip"
$TrivyExecutable = Join-Path $TrivyDirectory "trivy.exe"
$HtmlTemplate = Join-Path $TrivyDirectory "html.tpl"
$JsonReport = Join-Path $ReportsFolder "trivy-report.json"
$HtmlReport = Join-Path $ReportsFolder "trivy-report.html"

# Utility: Clean partial files and working directories
function Cleanup-PartialFiles {
    param([bool]$IncludeScanner = $false)
    
    Write-Output "Cleaning up partial files..."
    
    $pathsToClean = @($TrivyZip, $TempDownloadDir)
    if ($IncludeScanner) { $pathsToClean += $TrivyDirectory }
    
    foreach ($path in $pathsToClean) {
        if (Test-Path $path) {
            Remove-Item $path -Recurse -Force -ErrorAction SilentlyContinue
            Write-Output "Removed: $path"
        }
    }
}

# Utility: Download Trivy binary
function Download-Trivy {
    if (Test-Path $TrivyExecutable) {
        Write-Output "Trivy binary already exists and is functional. Reusing."
        return
    }

    try {
        Write-Output "Downloading Trivy v$TrivyVersion..."
        
        # Create directories
        New-Item -ItemType Directory -Force -Path $TempDownloadDir | Out-Null
        New-Item -ItemType Directory -Force -Path $TrivyDirectory | Out-Null

        $TrivyUrl = "https://github.com/aquasecurity/trivy/releases/download/v$TrivyVersion/trivy_${TrivyVersion}_windows-64bit.zip"
        
        Write-Output "Downloading from: $TrivyUrl"
        Invoke-WebRequest -Uri $TrivyUrl -OutFile $TrivyZip -UseBasicParsing
        
        # Verify download
        if (-not (Test-Path $TrivyZip) -or (Get-Item $TrivyZip).Length -eq 0) {
            throw "Failed to download Trivy or file is empty"
        }

        Write-Output "Extracting Trivy..."
        Expand-Archive -Path $TrivyZip -DestinationPath $TrivyDirectory -Force
        
        # Verify extraction
        if (-not (Test-Path $TrivyExecutable)) {
            throw "Trivy executable not found after extraction"
        }

        # Cleanup download files
        Remove-Item $TrivyZip -Force -ErrorAction SilentlyContinue
        Remove-Item $TempDownloadDir -Recurse -Force -ErrorAction SilentlyContinue

        Write-Output "Trivy setup completed successfully."
    } catch {
        Write-Error "Error during Trivy download or extraction: $_"
        Cleanup-PartialFiles -IncludeScanner $true
        exit 1
    }
}

# Utility: Download HTML report template
function Download-TrivyTemplate {
    if (Test-Path $HtmlTemplate) {
        Write-Output "HTML template already exists. Reusing."
        return
    }
    
    try {
        Write-Output "Downloading HTML report template..."
        $templateUrl = "https://raw.githubusercontent.com/aquasecurity/trivy/main/contrib/html.tpl"
        Invoke-WebRequest -Uri $templateUrl -OutFile $HtmlTemplate -UseBasicParsing
        
        # Verify download
        if (-not (Test-Path $HtmlTemplate)) {
            throw "Failed to download HTML template"
        }
        
        Write-Output "HTML template downloaded successfully."
    } catch {
        Write-Warning "Warning: Could not download HTML template: $_"
        Write-Output "HTML report generation may not be available."
    }
}

# Utility: Validate configuration
function Validate-Configuration {
    Write-Output "Validating configuration..."
    $hasErrors = $false
    
    # Validate Trivy version format
    if (-not ($TrivyVersion -match '^\d+\.\d+\.\d+$')) {
        Write-Warning "Trivy version format may be incorrect: $TrivyVersion"
    }
    
    # Validate scan target
    if (-not (Test-Path $ScanTarget)) {
        Write-Error "Scan target does not exist: $ScanTarget"
        $hasErrors = $true
    }
    
    if ($hasErrors) {
        Write-Error "Configuration validation failed. Please fix the errors above."
        exit 1
    }
    
    Write-Output "Configuration validation passed successfully."
}

# Utility: Create reports directory
function Create-ReportsDirectory {
    if (-not (Test-Path $ReportsFolder)) {
        Write-Output "Creating reports directory: $ReportsFolder"
        New-Item -ItemType Directory -Path $ReportsFolder | Out-Null
    }
}

# Utility: Run Trivy scan
function Run-TrivyScan {
    Write-Output "Verifying scan target..."
    $absoluteScanTarget = Resolve-Path $ScanTarget -ErrorAction SilentlyContinue
    if (-not $absoluteScanTarget -or -not (Test-Path $absoluteScanTarget)) {
        Write-Error "Scan target does not exist: $ScanTarget"
        exit 1
    }

    Write-Output "Scan target: $absoluteScanTarget"
    Write-Output "Starting Trivy security scan..."
    
    try {
        Write-Output "Running Trivy scan (JSON report)..."
        Write-Output "Executing: $TrivyExecutable"
        
        $jsonArgs = @("fs", "--scanners", "vuln,secret,misconfig,license", "--license-full", "--ignore-unfixed", "--skip-dirs", ".vs,.vscode,.trivy,trivy-temp-download,trivy-reports,logs,.git", "--format", "json", "-o", $JsonReport, $absoluteScanTarget)
        $process = Start-Process -FilePath $TrivyExecutable -ArgumentList $jsonArgs -NoNewWindow -Wait -PassThru

        if ($process.ExitCode -ne 0) {
            Write-Error "Trivy JSON scan failed with exit code: $($process.ExitCode)"
            exit 1
        }

        if (-not (Test-Path $JsonReport)) {
            Write-Error "Failed to generate JSON report."
            exit 1
        }

        Write-Output "JSON report generated successfully."
        
        # Generate HTML report if template is available
        if (Test-Path $HtmlTemplate) {
            Write-Output "Generating HTML report..."
            $htmlArgs = @("fs", "--scanners", "vuln,secret,misconfig", "--ignore-unfixed", "--skip-dirs", ".vs,.vscode,.trivy,trivy-temp-download,trivy-reports,logs,.git", "--format", "template", "--template", "@$HtmlTemplate", "--output", $HtmlReport, $absoluteScanTarget)
            $htmlProcess = Start-Process -FilePath $TrivyExecutable -ArgumentList $htmlArgs -NoNewWindow -Wait -PassThru
            
            if ($htmlProcess.ExitCode -eq 0 -and (Test-Path $HtmlReport)) {
                Write-Output "HTML report generated successfully."
            } else {
                Write-Warning "Warning: Failed to generate HTML report."
            }
        } else {
            Write-Warning "HTML template not available. Skipping HTML report generation."
        }
        
    } catch {
        Write-Error "Error during Trivy scan execution: $_"
        exit 1
    }
}

# Utility: Check vulnerabilities
function Check-Vulnerabilities {
    if ($SkipVulnerabilityCheck) {
        Write-Output "Vulnerability check skipped by user request."
        return
    }
    
    Write-Output "Analyzing scan results..."
    
    try {
        $reportContent = Get-Content -Raw -Path $JsonReport | ConvertFrom-Json
        $vulnerabilityCount = 0

        foreach ($result in $reportContent.Results) {
            if ($result.Vulnerabilities -and $result.Vulnerabilities.Count -gt 0) {
                $vulnerabilityCount += $result.Vulnerabilities.Count
            }
        }
        
        if ($vulnerabilityCount -gt 0) {
            Write-Host "$vulnerabilityCount vulnerabilities detected. Check $JsonReport or $HtmlReport for details." -ForegroundColor Red
            exit 1
        }

        Write-Output "Scan completed with no vulnerabilities found."
    } catch {
        Write-Error "Error analyzing scan results: $_"
        exit 1
    }
}

# Execution flow
try {
    Write-Output "=== Trivy Security Scanner Automation Script ==="
    Write-Output "Scanner Version: $TrivyVersion"
    Write-Output "Scan Target: $ScanTarget"
    Write-Output "Reports Folder: $ReportsFolder"
    Write-Output "Skip Vulnerability Check: $SkipVulnerabilityCheck"
    Write-Output "================================================"
    
    Validate-Configuration
    Create-ReportsDirectory
    Download-Trivy
    Download-TrivyTemplate
    Run-TrivyScan
    Check-Vulnerabilities
    
    Write-Output "=== Trivy security scan completed successfully! ==="
} catch {
    Write-Error "Script execution failed: $_"
    Cleanup-PartialFiles
    exit 1
}

exit 0