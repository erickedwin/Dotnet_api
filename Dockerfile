# Dockerfile
#FROM mcr.microsoft.com/dotnet/core/sdk:6.0 AS test-api
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS test-api
WORKDIR /apinetcore5
#Copy csprj and restore as distinct layers
COPY . .
RUN dotnet restore "./apinetcore5.csproj" --disable-parallel
RUN dotnet publish "./apinetcore5.csproj" -c release -o /app --no-restore

#Server Stage
#Copy everything else and build
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=test-api /app ./

EXPOSE 5000


ENTRYPOINT ["dotnet", "apinetcore5.dll"]




#Copy everything else and build
#COPY . ./
#RUN dotnet publish -c Release -o out
#ENTRYPOINT [ "dotnet", "aspnetcoreapp.dll" ]
#Build runtime image
#FROM mcr.microsoft.com/dotnet/core/aspnet:6.0
#WORKDIR /apinetcore5
#COPY --from=test-api /apinetcore5/out .
#ENTRYPOINT [ "dotnet", "aspnetcoreapp.dll" ]

