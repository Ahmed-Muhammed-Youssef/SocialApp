# Set the base image to use for the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) and restore dependencies 
COPY *.csproj ./
RUN dotnet restore

# Copy remaining code
COPY . ./

# Restore as distinct layers
RUN dotnet restore

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release -o out --no-restore

# Build runtime image
# Set the base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# Set the working directory inside the container
WORKDIR /app
# Copy the published output from the build stage
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]