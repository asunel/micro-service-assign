FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY .  "JaegerTest/"
RUN dotnet restore "JaegerTest/JaegerTest.csproj"
COPY . .
WORKDIR "/src/JaegerTest"
RUN dotnet build "JaegerTest.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "JaegerTest.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JaegerTest.dll"] 