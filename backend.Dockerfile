FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/ backend/

RUN dotnet restore backend/CrossReview.Web/CrossReview.Web.csproj

RUN dotnet publish backend/CrossReview.Web/CrossReview.Web.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# КРИТИЧНО: заставляем слушать все интерфейсы
ENV ASPNETCORE_URLS=http://0.0.0.0:5171

EXPOSE 5171
ENTRYPOINT ["dotnet", "CrossReview.Web.dll"]