FROM microsoft/dotnet:2.2-sdk AS build

WORKDIR /app

EXPOSE 80/tcp

COPY . ./
RUN dotnet restore RestCashflowWebApi/*.csproj
RUN dotnet publish RestCashflowWebApi/*.csproj -c Debug -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app

COPY --from=build /app/RestCashflowWebApi/out ./
ENTRYPOINT ["dotnet", "RestCashflowWebApi.dll"]