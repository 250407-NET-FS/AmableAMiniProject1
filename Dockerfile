FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY MiniProject.Api/*.csproj ./MiniProject.Api/
COPY MiniProject.Data/*.csproj ./MiniProject.Data/
COPY MiniProject.Models/*.csproj ./MiniProject.Models/
COPY MiniProject.Services/*.csproj ./MiniProject.Services/
COPY MiniProject.Tests/*.csproj ./MiniProject.Tests/
RUN dotnet restore

COPY . .
WORKDIR /src/MiniProject.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /src
COPY --from=build /src/MiniProject.Api/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "MiniProject.Api.dll"]