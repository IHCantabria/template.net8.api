# DEPLOY REQUIREMENTS

🚀
<br>

> Deploy requirements for IH-IT software
> <br>

## App Template

    - Api

## System 

    - Windows
	- Linux

## Environment

	- Dev
	- Pre
	- Prod

## ANSIBLE RECIPE NAME

_Development_

    - Deploy {PROJECT_NAME}.Api - DEV
	
_Pre_

    - Deploy {PROJECT_NAME}.Api - PRE
	
_Prod_

    - Deploy {PROJECT_NAME}.Api - PROD

## App folder

`{PROJECT_NAME}.Api`

## Distribution

    - Tag

## Settings site

    - feature_net45: 'no'
    - net_core: 'si'
	- core_version: '8'
    - httpplatform: 'no'
    - managedRuntimeVersion_pool: 'no managed'
    - enable32BitAppOnWin64_pool: 'false'
    - managedPipeLineMode_pool: 'integrated'
    - iiswin_aut: 'no'
    - thredds_whitelist: 'no'

## LOG

    - log: 'si'

## Url GIT

	- git@github.com:IHCantabria/{PROJECT_NAME}

## DNS

_Development_

    - {SUBDOMAIN_API}dev.ihcantabria.com

_Pre_

    - {SUBDOMAIN_API}-pre.ihcantabria.com
	
_Prod_

    - {SUBDOMAIN_API}.ihcantabria.com

## URL APPLICATION

_Development_

    - {SUBDOMAIN_API}dev.ihcantabria.com/swagger
	
_Pre_

    - {SUBDOMAIN_API}-pre.ihcantabria.com/swagger
	
_Prod_

	- {SUBDOMAIN_API}.ihcantabria.com/swagger
	
## COMPILATION SCRIPT PROFILES 

El Fichero package.json tiene los scripts de compilación.

_Development_

    - npm run build:dev
	
_Pre_

    - npm run build:pre
	
_Prod_

    - npm run build:prod

## Other settings

Select only if needed:

**Binary repo**

`_____________`

**Services to restart**

`_____________`

**Backup**

    - Tags
    - Snapshot

---

**Do you need any other configuration?**

- [ ] `Configuración de permisos en la carpeta Apilogs/{PROJECT_NAME}.Api para los logs de la api`
- [ ] `Variables de entorno, sustituir los sigientes placeholders en los ficheros appsettings: HOST_API_URL, HOST_API_DOMAIN, HOST_API_LOG_PATH, HOST_API_LOG_INTERNAL_FILE`

<br>

## Relationships

**What applications, services, or data sources is this application related to?**
``

## Credits

[IH Cantabria](https://github.com/IHCantabria)

## FAQ

- Document provided by the system administrators [David del Prado](https://ihcantabria.com/directorio-personal/david-del-prado-secadas/) y [Luis Gutiérrez](https://ihcantabria.com/directorio-personal/luis-gutierrez-2/)
