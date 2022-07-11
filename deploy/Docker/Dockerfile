ARG DOTNET_SDK_REPO=mcr.microsoft.com/dotnet/sdk:6.0
ARG DOTNET_RUNTIME_REPO=mcr.microsoft.com/dotnet/aspnet:6.0

FROM ${DOTNET_SDK_REPO} AS build

ARG RELEASE_VERSION

RUN mkdir -p /home/dockeruser/app
WORKDIR /home/dockeruser/app

COPY --chown=dockeruser:dockeruser PaymentGateway.sln PaymentGateway.sln

COPY --chown=dockeruser:dockeruser src/PaymentGateway.Api/PaymentGateway.Api.csproj src/PaymentGateway.Api/PaymentGateway.Api.csproj
COPY --chown=dockeruser:dockeruser src/PaymentGateway.Application/PaymentGateway.Application.csproj src/PaymentGateway.Application/PaymentGateway.Application.csproj
COPY --chown=dockeruser:dockeruser src/PaymentGateway.Domain/PaymentGateway.Domain.csproj src/PaymentGateway.Domain/PaymentGateway.Domain.csproj
COPY --chown=dockeruser:dockeruser src/PaymentGateway.Infrastructure/PaymentGateway.Infrastructure.csproj src/PaymentGateway.Infrastructure/PaymentGateway.Infrastructure.csproj

COPY --chown=dockeruser:dockeruser tests/Unit.Tests/PaymentGateway.Domain.Tests/PaymentGateway.Domain.Tests.csproj tests/Unit.Tests/PaymentGateway.Domain.Tests/PaymentGateway.Domain.Tests.csproj
COPY --chown=dockeruser:dockeruser tests/Unit.Tests/PaymentGateway.Application.Tests/PaymentGateway.Application.Tests.csproj tests/Unit.Tests/PaymentGateway.Application.Tests/PaymentGateway.Application.Tests.csproj
COPY --chown=dockeruser:dockeruser tests/Integration.Tests/PaymentGateway.Integration.Tests/PaymentGateway.Integration.Tests.csproj tests/Integration.Tests/PaymentGateway.Integration.Tests/PaymentGateway.Integration.Tests.csproj

RUN dotnet restore PaymentGateway.sln

COPY --chown=dockeruser:dockeruser src/. src/.
COPY --chown=dockeruser:dockeruser tests/. tests/.


RUN dotnet publish src/PaymentGateway.Api/PaymentGateway.Api.csproj -c Release -o /home/dockeruser/out --no-restore

RUN mkdir /home/dockeruser/out/logs

FROM ${DOTNET_SDK_REPO} AS unit-tests

COPY --chown=dockeruser:dockeruser --from=build /home/dockeruser/app /home/dockeruser/app

ENV ASPNETCORE_ENVIRONMENT=Docker
WORKDIR /home/dockeruser/app

CMD find ./tests/Unit.Tests -type f -name '*.Tests.csproj' | xargs -L1 dotnet test --configuration Release --logger:'trx' --results-directory '/home/dockeruser/reports'


FROM unit-tests AS integration-tests
ENV ASPNETCORE_ENVIRONMENT=Docker

CMD find ./tests/Integration.Tests -type f -name '*.Integration.Tests.csproj' | xargs -L1 dotnet test --configuration Release --logger:'trx' --results-directory '/home/dockeruser/reports'


FROM ${DOTNET_RUNTIME_REPO}

RUN mkdir -p /home/dockeruser/app

COPY --chown=dockeruser:dockeruser --from=build /home/dockeruser/out /home/dockeruser/app

WORKDIR /home/dockeruser/app
ENV ASPNETCORE_URLS=http://+:9501 \
    ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["./PaymentGateway.Api"]

