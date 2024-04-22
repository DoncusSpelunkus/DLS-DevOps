# Use the .NET Core SDK image to build the test project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["XunitTest/XunitTest.csproj", "XunitTest/"]
RUN dotnet restore "XunitTest/XunitTest.csproj"
COPY . .
WORKDIR "/src/XunitTest"

RUN build as Publish
RUN dotnet publish "XunitTest.csproj" -c Release -o /app/publish

FROM build as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "XunitTest.dll"]
