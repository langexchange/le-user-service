#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG NUGET_GITHUB_PASSWORD
WORKDIR /src
COPY ["LE.UserService/NuGet.config", "LE.UserService/"]
RUN sed -i "s|</configuration>|<packageSourceCredentials><github><add key=\\\"Username\\\" value=\\\"thuongnguyen1508\\\" /><add key=\\\"ClearTextPassword\\\" value=\\\"${NUGET_GITHUB_PASSWORD}\\\" /></github></packageSourceCredentials></configuration>|" "LE.UserService/NuGet.config"
COPY ["LE.UserService/*.csproj", "LE.UserService/"]
COPY ["LE.UserService.Infrastructure/*.csproj", "LE.UserService.Infrastructure/"]
RUN dotnet restore "LE.UserService/LE.UserService.csproj" --configfile ./LE.UserService/NuGet.config
COPY ./ ./
WORKDIR "/src/LE.UserService"
RUN dotnet build "LE.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LE.UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG ASPNETCORE_ENVIRONMENT              
ARG CONSUL_URL
ARG DB_CONNECTION_STRING
ARG NEO4J_DATABASE              
ARG NEO4J_HOST
ARG NEO4J_USER
ARG NEO4J_PASSWORD
ARG SECRET_KEY
ARG CHAT_DOMAIN

ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ENV CONSUL_URL=$CONSUL_URL
ENV DB_CONNECTION_STRING=$DB_CONNECTION_STRING
ENV NEO4J_DATABASE=$NEO4J_DATABASE
ENV NEO4J_HOST=$NEO4J_HOST
ENV NEO4J_USER=$NEO4J_USER
ENV NEO4J_PASSWORD=$NEO4J_PASSWORD
ENV SECRET_KEY=$SECRET_KEY
ENV CHAT_DOMAIN=$CHAT_DOMAIN

ENTRYPOINT ["dotnet", "LE.UserService.dll"]

# docker build 
#             --build-arg ASPNETCORE_ENVIRONMENT=Develop
#             --build-arg MINIO_HOST
#             --build-arg CONSUL_URL
#             --build-arg S3_BUCKET_NAME
#             --build-arg AWS_ACCESS_KEY_ID
#             --build-arg AWS_SECRET_ACCESS_KEY
#             -t uploader .