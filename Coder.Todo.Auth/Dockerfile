FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Coder.Todo.Auth/Coder.Todo.Auth.csproj", "Coder.Todo.Auth/"]
RUN dotnet restore "Coder.Todo.Auth/Coder.Todo.Auth.csproj"
COPY . .
WORKDIR "/src/Coder.Todo.Auth"
RUN dotnet build "Coder.Todo.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Coder.Todo.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coder.Todo.Auth.dll"]
