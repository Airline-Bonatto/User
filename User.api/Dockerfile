# ==== ESTÁGIO 1: BUILD ====
# Imagem base para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivo .csproj e restaurar dependências
# Substitua "User.csproj" pelo nome do seu arquivo de projeto
COPY ["User.csproj", "./"]
RUN dotnet restore "User.csproj"

# Copiar todo o código fonte
COPY . .

# Compilar a aplicação em modo Release
RUN dotnet build "User.csproj" -c Release -o /app/build

# ==== ESTÁGIO 2: PUBLISH ====
# Publicar a aplicação (gerar arquivos para produção)
FROM build AS publish
RUN dotnet publish "User.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==== ESTÁGIO 3: RUNTIME ====
# Imagem base menor apenas para executar
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar arquivos publicados do estágio anterior
COPY --from=publish /app/publish .

# Porta padrão do ASP.NET Core
EXPOSE 8080
EXPOSE 8081

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "User.dll"]