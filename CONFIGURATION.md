# PROJECT CONFIGURATION

## RENAME .NET PROJECT
	1. Manual rename the next files and folders with the SAME NAME:
		- Solution file "template.net8.api.sln".
		- Project file "template.net8.api.csproj".
		- Project folder "template.net8.api".
		- Project settings "template.net8.api.csproj.DotSettings".
		- Http request file "template.net8.api.http".
		Ex: "MiProject.Api.sln", "MiProject.Api.csproj", "MiProject.Api", "MiProject.Api.csproj.DotSettings", "MiProject.Api.http"
	2. Open Solution With Visual Studio, ignore warning. Remove project "template.net8.api (not found)" from the solution.
	3. Add to the solution a existing project, select the renamed file "template.net8.api.csproj"
	4. Update namespace of the existing projects. 
		- Right-click on the project in Solution Explorer
		- Choose “Refactor” > “Adjust Namespaces” to rename the namespace across files.
		- Remove .Mappers from the namespaces of the files with errors

## CUSTOMIZE .NET PROJECT
	1. Replace {PROJECT_NAMESPACE} with the project namespace value. 
		- BusinessConstants.
		- appsettings.json
		- Renamed "template.net8.api"
		Ex: MiProject.Api
	2. Replace {PROJECT_NAME} with the project name value. 
		- Properties/launchSettings.json
		- appsettings.json
		- Renamed "template.net8.api"
		Ex: MiProject.Api
	3. Replace {PROJECT_GUID} with a new GUID. Visual Studio -> Tools -> Create GUID.
		- Properties/PublishProfiles/**
		Ex: 9E4F3D66-752A-487C-80DE-EA733A0B111B
	4. Remove the unused envs config and/or add custom envs config( Development, Pre, Prod...)
		- Envs.cs
		- appsettings."env".json
		- Properties/launchSettings.json
		- Properties/PublishProfiles/**
		
## CUSTOMIZE .GITIGNORE using .gitignore_template
	1. Use the file gitignore_template. to configure the .gitignore for the project, replace the old .gitignore. This file will be added to the repository.
	2. Replace {PROJECT_NAMESPACE} with the project namespace value.
		Ex: MiProject.Api

## CUSTOMIZE PACKAGE.JSON FILE
	1. Replace {PROJECT_NAME} with the project name value. 
		Ex: MiProject.Api
	2. Replace {PROJECT_NAME_LOWERCASE} with the project name value using only lowercase. 
		Ex: miproject.api
	3. Remove the unused envs compile scripts(build:"env") and/or add custom envs compile scripts( Development, Pre, Prod...)
		
## CONFIGURE SONARQUBE
	1. Link repository with sonarqube server project.
	2. Configure PROJECT_KEY y SONAR_TOKEN_KEY in the sonarqube server.
	3. Create file sonar.ps1 using the template recursosIT/sonarqube/sonarqube-windows-net.ps1, this file will be omitted from the repository.
	
## CONFIGURE appsettings.local.json
	1. Rename the file appsettings.local_template.json to appsettings.local.json. This file will be your configuration for the local env of the project, this file will be omitted from the repository.
	
## COMPLETE DEPLOY REQUIREMENTS FILE
	1. Replace {PROJECT_NAME} with the project name value.
		Ex: MiProject.Api
	2. Replace {SUBDOMAIN_API} with the choosed dns subdomain value. 
		Ex: apimiapi
	3. Remove the unused envs config and/or add custom envs config( Development, Pre, Prod...)
	4. Add the Env variables needed for the Business/Infraestructure logic.
	5. Make the specific changes needed for your Business/Infraestructure logic.
	
## CHECKS
	1. Check correct compilation and Startup
	2. Check correct build using the command npm build:"env"
	3. Delete this FILE