FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IdentityServer5.csproj", "."]
RUN dotnet restore "IdentityServer5.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "IdentityServer5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer5.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer5.dll"]
