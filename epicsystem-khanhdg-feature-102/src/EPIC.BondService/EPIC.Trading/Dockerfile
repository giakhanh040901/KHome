#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EPIC.BondService/EPIC.Trading/EPIC.BondAPI.csproj", "EPIC.BondService/EPIC.Trading/"]
COPY ["EPIC.BondService/EPIC.Utils/EPIC.Utils.csproj", "EPIC.BondService/EPIC.Utils/"]
COPY ["EPIC.BondService/EPIC.Services/EPIC.BondDomain.csproj", "EPIC.BondService/EPIC.Services/"]
COPY ["EPIC.BondService/EPIC.Entities/EPIC.Entities.csproj", "EPIC.BondService/EPIC.Entities/"]
COPY ["EPIC.BondService/EPIC.BondRepositories/EPIC.BondRepositories.csproj", "EPIC.BondService/EPIC.BondRepositories/"]
COPY ["EPIC.BondService/EPIC.DataAccess/EPIC.DataAccess.csproj", "EPIC.BondService/EPIC.DataAccess/"]
RUN dotnet restore "EPIC.BondService/EPIC.Trading/EPIC.BondAPI.csproj"
COPY . .
WORKDIR "/src/EPIC.BondService/EPIC.Trading"
RUN dotnet build "EPIC.BondAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPIC.BondAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPIC.BondAPI.dll"]
