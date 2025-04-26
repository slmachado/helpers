#!/bin/bash
set -e

# Caminhos e nomes
PROJECT_FILE="Helpers.csproj"
PROJECT_DIR="$(pwd)"
NUGET_OUTPUT="$PROJECT_DIR/bin/Release"
NUGET_LOCAL="$HOME/Code/nuget-local"

# 🔍 Extrai o PackageId e a Versão do .csproj
PACKAGE_ID=$(grep -oPm1 "(?<=<PackageId>)[^<]+" "$PROJECT_FILE")
VERSION=$(grep -oPm1 "(?<=<Version>)[^<]+" "$PROJECT_FILE")

echo "🧼 Limpando projeto..."
dotnet clean

echo "📦 Empacotando $PACKAGE_ID na versão $VERSION..."
dotnet pack -c Release

echo "📁 Garantindo repositório local: $NUGET_LOCAL"
mkdir -p "$NUGET_LOCAL"

echo "📤 Copiando pacote $PACKAGE_ID.$VERSION.nupkg para repositório local..."
cp "$NUGET_OUTPUT/$PACKAGE_ID.$VERSION.nupkg" "$NUGET_LOCAL/"

echo "📡 Verificando se o feed LocalNuget já está registrado..."
if ! dotnet nuget list source | grep -q "LocalNuget"; then
    echo "➕ Adicionando source LocalNuget"
    dotnet nuget add source "$NUGET_LOCAL" --name LocalNuget
else
    echo "✅ Source LocalNuget já registrado"
fi

echo "✅ Pacote $PACKAGE_ID v$VERSION disponível para uso!"
