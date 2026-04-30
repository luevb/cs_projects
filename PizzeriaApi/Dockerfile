FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PizzeriaApi.csproj", "."]
RUN dotnet restore "PizzeriaApi.csproj"

COPY . .
RUN dotnet build "PizzeriaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PizzeriaApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PizzeriaApi.dll"]
