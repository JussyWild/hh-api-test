FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ApiTest/*.csproj ./ApiTest/
RUN cd ApiTest && dotnet restore -v minimal

# Copy everything else and build
COPY ApiTest/ ./ApiTest/
WORKDIR /app/ApiTest
ENTRYPOINT ["dotnet", "test", "--verbosity:normal"]
