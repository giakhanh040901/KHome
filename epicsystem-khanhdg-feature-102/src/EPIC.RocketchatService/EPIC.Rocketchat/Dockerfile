#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EPIC.RocketchatService/EPIC.Rocketchat/EPIC.Rocketchat.csproj", "EPIC.RocketchatService/EPIC.Rocketchat/"]
RUN dotnet restore "EPIC.RocketchatService/EPIC.Rocketchat/EPIC.Rocketchat.csproj"
COPY . .
WORKDIR "/src/EPIC.RocketchatService/EPIC.Rocketchat"
RUN dotnet build "EPIC.Rocketchat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPIC.Rocketchat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPIC.Rocketchat.dll"]