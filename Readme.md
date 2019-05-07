# Vehicles Application

The whole application consists of a UI part and an API part. UI part was written on C# with .NET Core Blazor Framework. API part was written on C# with .NET Core WebAPI Framework and SQLite Database. 
### Communication protocol
UI part communicates with API part thru HTTP REST API, that implements 3 endpoints:

**GET /customers**
Return all customers.

**GET/vehicles**
Search for vehicles by customerId or status.

**POST /vehicles/{id}/connect**
Change status of the correspondent vehicle.

### Web API
Web API implemented as a REST HTTP service. The base URL for API service is https://altenvehiclesapi.azurewebsites.net/api
API doesn't have any security and available publicly.

To run Web API locally you need to
1. Install the last preview of .NET Core Framework SDK from https://dotnet.microsoft.com/download/dotnet-core/3.0
2. Checkout repository to the VehiclesAPI folder:
`git clone https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI VehiclesAPI`
3. Inside the VehiclesAPI folder run command: `dotnet run --urls http://localhost:3001`
Service should start and be accessible on http://localhost:3001

### Web UI
The Web UI you can find on https://altenvehiclesui.azurewebsites.net/
API doesn't have any security and available publicly.

To run Web UI locally you need to
1. Install the last preview of .NET Core Framework SDK from https://dotnet.microsoft.com/download/dotnet-core/3.0
2. Checkout repository to the VehiclesUI folder:
`git clone https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesUI VehiclesUI`
3. Inside the VehiclesUI folder run command: `dotnet run --urls http://localhost:3000`
Service should start and be accessible on http://localhost:3000

### Run locally in Docker
Both parts of repositories contain Docker files that allow running the application in containers. For this you need to build two containers:
```
cd VehiclesAPI
docker build -t vehiclesapi .
cd..
cd VehiclesUI
docker build -t vehiclesui .
cd..
```
And than run them:
```
docker run -d -p 3001:80 vehiclesapi
docker run -d -p 3000:80 vehiclesui
```
To stop the containers you need to run in command line:
```
FOR /f "tokens=*" %i IN ('docker ps -q') DO docker stop %i
```
### Hosting
The whole project hosted as a public Azure DevOps Project: https://dev.azure.com/AlexMartyniuk/AltenChallenge
Both UI and API services hosted in App Services in Azure. During a build process, docker images with services are deployed to the Azure Container Registry. During a release, docker images are injected into appropriate App Services and the App Services are restarted. Both build and release pipelines are started automatically after every code push into the master branch.

Build pipelines you can find in:
* Web UI: https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesUI?path=%2Fazure-pipelines.yml&version=GBmaster
* Web API: https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI?path=%2Fazure-pipelines.yml&version=GBmaster

The deployment of an application to Azure performed via the next command. 
```
az webapp config container set -n AltenVehiclesAPI -g AltenChallenge-RG --docker-custom-image-name $(DockerCustomImageName) --docker-registry-server-url $(DockerRegistryServerUrl) --docker-registry-server-user $(DockerRegistryServerUser) --docker-registry-server-password $(DockerRegistryServerPassword)
```
This command just changs the configuration for the existed App Service. The command runs as a part of an automatic release pipeline that started after a successful build.

### Static Analyzing and Unit Testing
Web API part of the application has connected StyleCop rules check, that perform during each build. Also in [Tests folder](https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI?path=%2FTests&version=GBmaster). Tests cover VehicleService as a main part of the functionality and working with storage.



