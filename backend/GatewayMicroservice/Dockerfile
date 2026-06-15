FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["GatewayMicroservice.csproj", "./"]
RUN dotnet restore "GatewayMicroservice.csproj"

COPY . .
RUN dotnet publish "GatewayMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "GatewayMicroservice.dll"]
