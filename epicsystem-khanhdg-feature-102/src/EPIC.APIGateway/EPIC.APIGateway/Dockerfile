#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EPIC.APIGateway/EPIC.APIGateway/EPIC.APIGateway.csproj", "EPIC.APIGateway/EPIC.APIGateway/"]
RUN dotnet restore "EPIC.APIGateway/EPIC.APIGateway/EPIC.APIGateway.csproj"
COPY . .
WORKDIR "/src/EPIC.APIGateway/EPIC.APIGateway"
RUN dotnet build "EPIC.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPIC.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPIC.APIGateway.dll", "--environment=Production"]