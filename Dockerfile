#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LegionWebApp/LegionWebApp.csproj", "LegionWebApp/"]
RUN dotnet restore "LegionWebApp/LegionWebApp.csproj"
COPY . .
WORKDIR "/src/LegionWebApp"
RUN dotnet build "LegionWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LegionWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegionWebApp.dll"]