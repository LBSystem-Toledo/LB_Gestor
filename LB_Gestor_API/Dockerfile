#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LB_Gestor_API/LB_Gestor_API.csproj", "LB_Gestor_API/"]
RUN dotnet restore "LB_Gestor_API/LB_Gestor_API.csproj"
COPY . .
WORKDIR "/src/LB_Gestor_API"
RUN dotnet build "LB_Gestor_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LB_Gestor_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LB_Gestor_API.dll"]