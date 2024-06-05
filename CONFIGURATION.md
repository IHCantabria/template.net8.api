# PROJECT CONFIGURATION

## CUSTOMIZE .NET PROJECT
	1. Replace {PROJECT_NAMESPACE} with the project namespace value. 
		Ex: MiProject.Api
	1. Create {PROJECT_GUID} with a new GUID. 
		Ex: 9E4F3D66-752A-487C-80DE-EA733A0B111B
	2. Replace all the .net namespaces "template.net8.Api" with the project namespace. 
		Ex: MiProject.Api
	3. Remove the unused envs config and/or add custom envs config( Development, Pre, Prod...)
		- Envs.cs
		- appsettings."env".json
		- Properties/launchSettings.json
		- Properties/PublishProfiles/**
		
## CUSTOMIZE .GITIGNORE using .gitignore_template
	1. Replace {PROJECT_NAMESPACE} with the project namespace value.
		Ex: MiProject.Api

## CUSTOMIZE PACKAGE.JSON FILE
	1. Replace {PROJECT_NAMESPACE} with the project namespace value. 
		Ex: MiProject.Api
	2. Remove the unused envs compile scripts(build:"env") and/or add custom envs compile scripts( Development, Pre, Prod...)
		
## CONFIGURE SONARQUBE
	1. Link repository with sonarqube server project.
	2. Configure PROJECT_KEY y SONAR_TOKEN_KEY in the sonarqube server.
	3. Create file sonar.ps1 using the template recursosIT/sonarqube/sonarqube-windows-net.ps1
	
## COMPLETE DEPLOY REQUIREMENTS FILE
	1. Replace {PROJECT_NAME} with the project name value.
		Ex: MiProject.Api
	2. Replace {SUBDOMAIN_API} with the choosed dns subdomain value. 
		Ex: apimiapi
	3. Remove the unused envs config and/or add custom envs config( Development, Pre, Prod...)
	4. Add the Env variables needed for the Business/Infraestructure logic.
	5. Make the specific changes needed for your Business/Infraestructure logic.