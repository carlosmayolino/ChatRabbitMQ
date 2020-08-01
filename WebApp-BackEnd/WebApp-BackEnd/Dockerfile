FROM microsoft/dotnet:2.2-sdk AS img-build

WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Debug -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app

COPY --from=img-build /app/out .
ENTRYPOINT ["dotnet", "WebApp-BackEnd.dll"]
