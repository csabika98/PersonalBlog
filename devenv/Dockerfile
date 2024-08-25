# Using SDK8.0 image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore any dependencies
COPY ["PersonalBlogCsabaSallai.csproj", "./"]
RUN dotnet restore "./PersonalBlogCsabaSallai.csproj"

# Copy the remaining source code and build the project
COPY . .
RUN dotnet build "PersonalBlogCsabaSallai.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "PersonalBlogCsabaSallai.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "PersonalBlogCsabaSallai.dll"]
