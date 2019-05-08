# Vehicles Application
### You can find application running on https://altenvehiclesui.azurewebsites.net/
The whole application consists of a UI part and an API part. UI part was written on C# with .NET Core Blazor Framework. API part was written on C# with .NET Core WebAPI Framework. 


### User Interface
To search for Vehicles on the start page select Customer and Status in dropdowns and press Search button.

![Search UI](https://dev.azure.com/AlexMartyniuk/73a1f480-1085-4872-94f9-7728e4b865bd/_apis/git/repositories/419e5f2b-0e33-4aa9-9d59-a14a416f162d/Items?path=%2FImages%2FSearchVehiclesUI.png&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=0&versionDescriptor%5Bversion%5D=master&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1)

To Connect specific vehicle on the Connect Vehicles page press the appropriate button Connect.

![Search UI](https://dev.azure.com/AlexMartyniuk/73a1f480-1085-4872-94f9-7728e4b865bd/_apis/git/repositories/419e5f2b-0e33-4aa9-9d59-a14a416f162d/Items?path=%2FImages%2FConnectVehiclesUI.png&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=0&versionDescriptor%5Bversion%5D=master&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1)


### Communication protocol
UI part communicates with API part thru HTTP REST API, that implements 3 endpoints:

**GET /customers**
Return all customers.

**GET /vehicles**
Search for vehicles by customerId or status. Parameters can be passed thru URL in query params, for example: /vehicles?connected=false&customerId=1

**POST /vehicles/{id}/connect**
Change status of the correspondent vehicle.

### Web API
Web API implemented as a REST HTTP service. The base URL for API service is https://altenvehiclesapi.azurewebsites.net/api
API doesn't have any security and available publicly.

To run Web API locally you need to:
1. Install the last preview of .NET Core Framework SDK from https://dotnet.microsoft.com/download/dotnet-core/3.0
2. Checkout repository to the VehiclesAPI folder:
```
git clone https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI VehiclesAPI
```
3. Inside the VehiclesAPI folder run command: 
```
dotnet run --urls http://localhost:3001
```
Service should start and be accessible on http://localhost:3001

### Web UI
The Web UI you can find on https://altenvehiclesui.azurewebsites.net/
Web UI application doesn't have any security and available publicly.

To run Web UI locally you need to
1. Install the last preview of .NET Core Framework SDK from https://dotnet.microsoft.com/download/dotnet-core/3.0
2. Checkout repository to the VehiclesUI folder:
```
git clone https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesUI VehiclesUI
```
3. Inside the VehiclesUI folder run command: 
```
dotnet run --urls http://localhost:3000
```
Service should start and be accessible on http://localhost:3000

### Run locally in Docker
Applications can be started in Docker containers. Both repositories contain Docker files that allow running the application in containers. 
For this you need to build two containers:
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
The whole project hosted in Azure DevOps: https://dev.azure.com/AlexMartyniuk/AltenChallenge
Both UI and API application hosted in App Services in the Microsoft Azure cloud. During a build process, docker images with services will be deployed to the Azure Container Registry. During a release, docker images will be injected into appropriate App Services and the App Services are restarted. Both build and release pipelines are started automatically after every code push into the master branch.

Build pipeline configurations you can find in:
* Web UI: https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesUI?path=%2Fazure-pipelines.yml&version=GBmaster
* Web API: https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI?path=%2Fazure-pipelines.yml&version=GBmaster

The deployment of an application to Azure performed via the next command. 
```
az webapp config container set -n AltenVehiclesAPI -g AltenChallenge-RG --docker-custom-image-name $(DockerCustomImageName) --docker-registry-server-url $(DockerRegistryServerUrl) --docker-registry-server-user $(DockerRegistryServerUser) --docker-registry-server-password $(DockerRegistryServerPassword)
```
This command just changes the configuration for the existing App Service. The command runs as a part of an automatic release pipeline that started after a successful build.

Build pipelines: https://dev.azure.com/AlexMartyniuk/AltenChallenge/_release)

Release pipelines: https://dev.azure.com/AlexMartyniuk/AltenChallenge/_build)

### Static Analyzing and Unit Testing
Web API part of the application has connected StyleCop rules check, that perform during each build. The check rules specified in [Rule checks](https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI?path=%2FStaticAnalysis.ruleset&version=GBmaster)

Also, you can find unit-tests in [Tests folder](https://dev.azure.com/AlexMartyniuk/AltenChallenge/_git/VehiclesAPI?path=%2FTests&version=GBmaster). 
Tests cover VehicleService as the main part of the functionality that works with storage. Tests execute as a part of the build pipeline.

### Data storage
As data storage is used SQLite database that automatically creates during the successful start of Web API application. The database consists of two tables: Customers and Vehicles that related as 1 : N. One customer can have several vehicles.
```
CREATE TABLE "Customers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT,
    "FullName" TEXT NULL,
    "Address" TEXT NULL
);
CREATE TABLE "Vehicles" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Vehicles" PRIMARY KEY AUTOINCREMENT,
    "VehicleId" TEXT NULL,
    "RegistrationNumber" TEXT NULL,
    "ConnectUpdated" TEXT NOT NULL,
    "CustomerId" INTEGER NOT NULL,
    CONSTRAINT "FK_Vehicles_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE
);
```

### Architecture
From an architectural point of view, the system is a 3-tier solution
* Presentation Tier is a Web UI Application.
* Application Tier is a Web API Application.
* Data Tier is SQLite local database connected via Entity Framework Core.

The whole application hosted on Microsoft Azure in App Services.

![Schema of architecture](https://dev.azure.com/AlexMartyniuk/73a1f480-1085-4872-94f9-7728e4b865bd/_apis/git/repositories/419e5f2b-0e33-4aa9-9d59-a14a416f162d/Items?path=%2FImages%2FSystemArchitecture.png&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=0&versionDescriptor%5Bversion%5D=master&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1)

### Logging and Monitoring
Web API application uses the Application Insights service from Azure. It allows log errors and monitor application under a load. For example, metrics of load testing of the application with 250 concurrent users.

![Load testing metrics](https://dev.azure.com/AlexMartyniuk/73a1f480-1085-4872-94f9-7728e4b865bd/_apis/git/repositories/419e5f2b-0e33-4aa9-9d59-a14a416f162d/Items?path=%2FImages%2FPerformanceTestResults.png&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=0&versionDescriptor%5Bversion%5D=master&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1)

### Serverless
Some parts of the application can be implemented as serverless. For example, Web API service can be hosted in Azure Function or AWS Lambda. But Web UI application can't be hosted as serverless because it uses .NET Core 3.0 that isn't supported by Function's or Lambda's runtimes.
Implementing Web API as a Function we need to add API proxy and HTTP trigger for this function to make it available from outside the cloud. 
Implementing Web API as an Azure Function gives us some advantages:
* it scales with demand automatically
* it reduces server cost because we don’t pay for idle
* it eliminates docker containers maintenance

But it also has own cons (for Azure Function and Consumption plan):
* cold start is significant for .NET based Functions
* we limited by 10 min of execution time
* we limited to the number of supported runtimes
* the serverless code has other runtime limitations (multithreading, etc.)

If we suppose that Connect endpoint will be requested very often and dashboard for connected vehicles will be built rarely we can build the Web API as four Functions and one Message Queue:
* function for handling HTTP queries for the Connect endpoint should save vehicle id into Queue for future processing
* function for handling new messages from the Queue and storing them into some persistent storage that can scale automatically
* function for getting vehicles data from persistent storage by HTTP request
* function for getting customers data from persistent storage by HTTP request

Taking into account the issue with the cold start that could freeze the Web UI application I recommend to implement the whole solution as a set of serverless function only if the number of connected vehicles will be significant.