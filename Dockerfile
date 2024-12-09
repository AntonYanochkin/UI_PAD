# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application files and publish
COPY . .
RUN dotnet publish -c Release -o /out

# Stage 2: Serve the application
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copy published files from the build stage
COPY --from=build /out/wwwroot .

# Expose the port the app runs on
EXPOSE 80
