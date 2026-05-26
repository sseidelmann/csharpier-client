# ==========================================
# Stage 1: Code direkt aus Git klonen
# ==========================================
FROM alpine/git:latest AS repo-env
WORKDIR /src

# Das Repository klonen und direkt zum Tag 1.2.6 springen
RUN git clone --depth 1 --branch 1.2.6 https://github.com/belav/csharpier.git .

# ==========================================
# Stage 2: Die App mit dem .NET SDK bauen
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /src

RUN set -uex \
    && apt-get update \
    && apt-get install -y ca-certificates curl gnupg \
    && mkdir -p /etc/apt/keyrings \
    && curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key \
     | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg \
    && NODE_MAJOR=18 \
    && echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" \
     | tee /etc/apt/sources.list.d/nodesource.list \
    && apt-get update \
    && apt-get install nodejs -y;

# Kopiere den gesamten Code aus der Git-Stage
COPY --from=repo-env /src .

# Restore & Publish des Playgrounds
RUN dotnet restore Src/CSharpier.Playground/CSharpier.Playground.csproj

WORKDIR /src/Src/CSharpier.Playground/ClientApp
RUN npm ci
RUN npm run build

WORKDIR /src
RUN dotnet publish Src/CSharpier.Playground/CSharpier.Playground.csproj -c Release -o /app/publish

# ==========================================
# Stage 3: Das finale, minimale Laufzeit-Image
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Nur die fertigen Build-Dateien kopieren (kein Quellcode im finalen Image!)
COPY --from=build-env /app/publish .

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "CSharpier.Playground.dll"]