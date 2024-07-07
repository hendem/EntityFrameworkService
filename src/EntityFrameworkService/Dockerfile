#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging. 
#Note: This doker file is for executing and debugging with docker from visual studio and may not work if built form the command line.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EntityFrameworkService/EntityFrameworkService.csproj", "EntityFrameworkService/"]
RUN dotnet restore "EntityFrameworkService/EntityFrameworkService.csproj"
COPY . .
WORKDIR "/src/EntityFrameworkService"
RUN dotnet build "EntityFrameworkService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EntityFrameworkService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EntityFrameworkService.dll"]