FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Bank.API/Bank.API.csproj", "Bank.API/"]
RUN dotnet restore "Bank.API/Bank.API.csproj"
COPY . .
WORKDIR "/src/Bank.API"
RUN dotnet build "Bank.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.API.dll"]