# Replace "SONAR_TOKEN_KEY" with your user sonar key
# Replace "PROJECT_KEY" with your actual project key

# Check if dotnet-sonarscanner is installed
if (-not (dotnet tool list --global | Select-String -Pattern "dotnet-sonarscanner")) {
    # If not installed, install dotnet-sonarscanner
    dotnet tool install --global dotnet-sonarscanner 
}

# Example usage:
# dotnet sonarscanner begin /k:"PROJECT_KEY" /d:sonar.host.url="http://ihsonarqube.ihcantabria.com:9000" /d:sonar.token="SONAR_TOKEN_KEY"
dotnet sonarscanner begin /k:"{PROJECT_SONAR_REPO_KEY}" /d:sonar.host.url="http://ihsonarqube.ihcantabria.com:9000" /d:sonar.token="{PROJECT_ACCESS_TOKEN_KEY}"

# Build the project
dotnet build

# End the SonarQube analysis
# dotnet sonarscanner end /d:sonar.token="SONAR_TOKEN_KEY"
dotnet sonarscanner end /d:sonar.token="{PROJECT_ACCESS_TOKEN_KEY}"