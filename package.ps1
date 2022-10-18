#!/usr/bin/env pwsh

dotnet restore
dotnet pack
dotnet tool install --global --add-source ./src/Exchange.Cli/nupkg Exchange.Cli
