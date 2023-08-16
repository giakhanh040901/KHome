#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EPIC.InvestService/EPIC.InvestAPI/EPIC.InvestAPI.csproj", "EPIC.InvestService/EPIC.InvestAPI/"]
COPY ["EPIC.InvestService/EPIC.InvestDomain/EPIC.InvestDomain.csproj", "EPIC.InvestService/EPIC.InvestDomain/"]
COPY ["EPIC.BondService/EPIC.Entities/EPIC.Entities.csproj", "EPIC.BondService/EPIC.Entities/"]
COPY ["EPIC.BondService/EPIC.Utils/EPIC.Utils.csproj", "EPIC.BondService/EPIC.Utils/"]
COPY ["EPIC.InvestService/EPIC.InvestEntities/EPIC.InvestEntities.csproj", "EPIC.InvestService/EPIC.InvestEntities/"]
COPY ["EPIC.InvestService/EPIC.InvestRepositories/EPIC.InvestRepositories.csproj", "EPIC.InvestService/EPIC.InvestRepositories/"]
RUN dotnet restore "EPIC.InvestService/EPIC.InvestAPI/EPIC.InvestAPI.csproj"
COPY . .
WORKDIR "/src/EPIC.InvestService/EPIC.InvestAPI"
RUN dotnet build "EPIC.InvestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPIC.InvestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPIC.InvestAPI.dll"]