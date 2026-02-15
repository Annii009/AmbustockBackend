# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["AmbustockBackend/AmbustockBackend.csproj", "AmbustockBackend/"]
RUN dotnet restore "AmbustockBackend/AmbustockBackend.csproj"

# Copiar el resto del código y compilar
COPY AmbustockBackend/. AmbustockBackend/
WORKDIR "/src/AmbustockBackend"
RUN dotnet build "AmbustockBackend.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "AmbustockBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmbustockBackend.dll"]
