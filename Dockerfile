FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy only the project files needed for the API
COPY ["src/QuantityMeasurementApp.Api/QuantityMeasurementApp.Api.csproj", "src/QuantityMeasurementApp.Api/"]
COPY ["src/QuantityMeasurementApp.Business/QuantityMeasurementApp.Business.csproj", "src/QuantityMeasurementApp.Business/"]
COPY ["src/QuantityMeasurementApp.Models/QuantityMeasurementApp.Models.csproj", "src/QuantityMeasurementApp.Models/"]
COPY ["src/QuantityMeasurementApp.Repository/QuantityMeasurementApp.Repository.csproj", "src/QuantityMeasurementApp.Repository/"]

# Restore dependencies for the API project only
RUN dotnet restore "src/QuantityMeasurementApp.Api/QuantityMeasurementApp.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR "/source/src/QuantityMeasurementApp.Api"
RUN dotnet build "QuantityMeasurementApp.Api.csproj" -c Release -o /app/build
RUN dotnet publish "QuantityMeasurementApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuantityMeasurementApp.Api.dll"]
