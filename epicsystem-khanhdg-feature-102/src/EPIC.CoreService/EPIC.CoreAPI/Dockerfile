#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EPIC.CoreService/EPIC.CoreAPI/EPIC.CoreAPI.csproj", "EPIC.CoreService/EPIC.CoreAPI/"]
COPY ["EPIC.BondService/EPIC.Entities/EPIC.Entities.csproj", "EPIC.BondService/EPIC.Entities/"]
COPY ["EPIC.BondService/EPIC.Utils/EPIC.Utils.csproj", "EPIC.BondService/EPIC.Utils/"]
COPY ["EPIC.CoreService/EPIC.CoreDomain/EPIC.CoreDomain.csproj", "EPIC.CoreServices/EPIC.CoreDomain/"]
COPY ["EPIC.CoreService/EPIC.CoreRepositories/EPIC.CoreRepositories.csproj", "EPIC.CoreService/EPIC.CoreRepositories/"]
RUN dotnet restore "EPIC.CoreService/EPIC.CoreAPI/EPIC.CoreAPI.csproj"
COPY . .
WORKDIR "/src/EPIC.CoreService/EPIC.CoreAPI"
RUN dotnet build "EPIC.CoreAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPIC.CoreAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPIC.CoreAPI.dll"]