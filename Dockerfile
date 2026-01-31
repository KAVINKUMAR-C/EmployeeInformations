# Stage 1: Build all projects
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy solution file first
COPY *.sln ./

# Copy csproj files
COPY EmployeeInformations/*.csproj ./EmployeeInformations/
COPY EmployeeInformations.Business/*.csproj ./EmployeeInformations.Business/
COPY EmployeeInformations.Common/*.csproj ./EmployeeInformations.Common/
COPY EmployeeInformations.CoreModels/*.csproj ./EmployeeInformations.CoreModels/
COPY EmployeeInformations.Data/*.csproj ./EmployeeInformations.Data/
COPY EmployeeInformations.DI/*.csproj ./EmployeeInformations.DI/
COPY EmployeeInformations.Model/*.csproj ./EmployeeInformations.Model/

# Restore all projects
RUN dotnet restore

# Copy everything else
COPY . .

# Build and publish MVC project
WORKDIR /src/EmployeeInformations
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EmployeeInformations.dll"]
