FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Klc.Mutabix.Api/Klc.Mutabix.Api.csproj", "Klc.Mutabix.Api/"]
COPY ["Klc.Mutabix.Application/Klc.Mutabix.Application.csproj", "Klc.Mutabix.Application/"]
COPY ["Klc.Mutabix.Domain/Klc.Mutabix.Domain.csproj", "Klc.Mutabix.Domain/"]
COPY ["Klc.Mutabix.Infrastructure/Klc.Mutabix.Infrastructure.csproj", "Klc.Mutabix.Infrastructure/"]
RUN dotnet restore "Klc.Mutabix.Api/Klc.Mutabix.Api.csproj"

COPY . .
WORKDIR "/src/Klc.Mutabix.Api"
RUN dotnet build "Klc.Mutabix.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Klc.Mutabix.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Klc.Mutabix.Api.dll"]
