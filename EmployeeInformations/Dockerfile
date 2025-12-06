# Base image for running the app (.NET 9 ASP.NET runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Build image (.NET 9 SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the csproj first (for faster restore)
COPY EmployeeInformations.Web.csproj ./
RUN dotnet restore "EmployeeInformations.Web.csproj"

# Copy all source files
COPY . .

# Publish the project
RUN dotnet publish "EmployeeInformations.Web.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeeInformations.Web.dll"]
