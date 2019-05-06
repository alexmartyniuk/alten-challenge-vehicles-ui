FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.0.100-preview5-alpine
WORKDIR /VehiclesUI
COPY . .
RUN dotnet build --configuration Release
EXPOSE 80
CMD ["dotnet", "run", "--urls", "http://0.0.0.0:80"]
