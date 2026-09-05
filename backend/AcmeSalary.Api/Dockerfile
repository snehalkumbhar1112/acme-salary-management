FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AcmeSalary.Api.csproj", "./"]
RUN dotnet restore "AcmeSalary.Api.csproj"

COPY . .
RUN dotnet publish "AcmeSalary.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

CMD ["sh", "-c", "dotnet AcmeSalary.Api.dll --urls http://0.0.0.0:$PORT"]
