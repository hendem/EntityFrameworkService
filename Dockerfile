# ####
# create a simple asp.net application - requires a base SDK image
# ####
FROM  mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /


ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS="http://*:8080;https://*:8443"


# copy a csproj you developed or create a new webapp and restore dependencies
# use different image to perform building, unit tests, sonarscans, and blackduckscans 
# ####
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 

WORKDIR /

ARG appversion
ARG appname
ARG buildnumber
ARG branchname

ENV app_version=$appversion
ENV app_name=$appname
ENV app_build_number=$buildnumber
ENV target_path=$targetCodePath


# copy source code
COPY *.sln ./
COPY ./src ./src
COPY ./tests ./tests
COPY *.props ./ 



# restore dependencies and build
RUN dotnet restore . 
RUN echo "${target_path}"

# build the dotnet build
RUN dotnet build .
#/${target_path}
RUN dotnet test .
#/${target_path} --logger:"trx" --settings coverlet.runsettings /p:CoverletOutput="src/"


# Install openssl
RUN apt-get update && \
    apt-get install -y openssl

# Generate a self-signed certificate with OpenSSL
RUN openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365 -nodes -subj '/CN=localhost'

# Combine the private key and the certificate into a .pfx file
RUN openssl pkcs12 -export -out aspnetapp.pfx -inkey key.pem -in cert.pem -passout pass:YourPassword


FROM build AS publish

RUN dotnet publish . -c Release -o /publish

# ####
# generate a minimal runtime image
# ####
FROM base AS final 
EXPOSE 8443
EXPOSE 8080

COPY --from=build /aspnetapp.pfx /https/aspnetapp.pfx

# Set environment variables to configure .NET to use the certificate
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="YourPassword"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

WORKDIR /app
COPY --from=publish /publish . 


ENTRYPOINT [ "/app/EntityFrameworkService" ]
HEALTHCHECK CMD curl --fail -k https://localhost/api/health || exit 1