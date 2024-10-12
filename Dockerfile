FROM mcr.microsoft.com/dotnet/sdk:8.0 AS debug

WORKDIR /work
COPY . .
RUN dotnet restore
# this is not for debug for final image
# RUN dotnet publish -c Release -o /work/publish /p:UseAppHost=false

RUN apt-get update
RUN apt-get install -y curl
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg

# follow this https://stackoverflow.com/questions/76498526/http-ports-when-running-asp-net-8-container-in-aks
# if want to change port
# ENV ASPNETCORE_HTTP_PORTS=5156
EXPOSE 8080
CMD ["dotnet", "run", "--urls", "http://+:8080"]

# Start new image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS prod
EXPOSE 8080
WORKDIR /app
COPY --from=debug /work/publish .
ENTRYPOINT ["dotnet", "HelloWorldApi.dll"]