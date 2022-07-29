#build phase
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS BUILD
WORKDIR /source
COPY . .
RUN dotnet restore "./Hays.API/Hays.API.csproj" --disable-parallel
RUN dotnet publish "./Hays.API/Hays.API.csproj" -c release -o /app --no-restore

#Save stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal

WORKDIR /app
COPY --from=build /app ./

EXPOSE 80

ENTRYPOINT [ "dotnet","Hays.API.dll" ]