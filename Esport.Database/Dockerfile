# Step 1: Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

# Sets the working directory
WORKDIR /app

# Copies project files and restores dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all remaining files and build the application
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Step 2: Run Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Sets the working directory
WORKDIR /app

# Copies files from the build step to the final image
COPY --from=build-env /app/out .

# Defines the input command to start the application
ENTRYPOINT ["dotnet", "Esport.Database.dll"]
