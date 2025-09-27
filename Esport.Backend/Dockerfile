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

# Install font packages
RUN echo "deb http://deb.debian.org/debian/ bookworm main contrib" > /etc/apt/sources.list && \
    echo "deb-src http://deb.debian.org/debian/ bookworm main contrib" >> /etc/apt/sources.list && \
    echo "deb http://security.debian.org/ bookworm-security main contrib" >> /etc/apt/sources.list && \
    echo "deb-src http://security.debian.org/ bookworm-security main contrib" >> /etc/apt/sources.list
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update; apt-get install -y ttf-mscorefonts-installer fontconfig

# Refresh font cache
RUN fc-cache -f -v

# Sets the working directory
WORKDIR /app

# Copies files from the build step to the final image
COPY --from=build-env /app/out .

# Exposes the port used by the application
EXPOSE 80

# Defines the input command to start the application
ENTRYPOINT ["dotnet", "Esport.Backend.dll"]
