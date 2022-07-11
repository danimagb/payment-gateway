# Payment Gateway API

Payment gateway API giving merchants the ability to process payments for their shoppers

# Technologies

- [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
- [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)

# Assumptions

1. The API should be authententicated thus Authentication via API Key is provided
2. The API must provide idempotent payment requests to avoid duplicated payments. There is the possibility for merchants to povide a RequestId when creating payments.
3. Merchants can only see their payments requests
4. Acquiring Bank retrieves a response in an acceptable timeframe

# Overview

The project structure is inspired in the Clean Architecture & CQS

## PaymentGateway.Api

This is the entrypoint of client requests. In this layer is defined the Rest API.

## PaymentGateway.Domain

This layer contains all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

## PaymentGateway.Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

## PaymentGateway.Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

# PaymentGateway.Api

The content-type is application/json.
There are two endpoints available:

- `POST /payments` - To create a payment.
- `GET /payments/{id}` - To retrieve previously created payment details.

Both endpoints are secured by API Key authentication that must be passed in a `x-api-key` header on every request.
There are already two API Keys defined in the appsettings.json that can be used to make requests.

### `POST /payments`

Define the `x-api-key` header : `mTbC4r1Eh7wvXrXE1UDl18NGH1fRzcrRz`

**Happy Path**

Request:

```json
{
  "requestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 10,
  "currency": "USD",
  "cardDetails": {
    "holder": "Daniel",
    "number": "4263982640269299",
    "cvv": "1234",
    "expiryMonth": 1,
    "expiryYear": 2020
  }
}
```

Response:

```json
{
  "id": "a034c7c2-d07b-4212-b935-86ea17375b6e",
  "requestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "createdAt": "2022-07-11T21:04:57.6828763Z",
  "processedAt": "2022-07-11T21:04:57.8022098Z",
  "status": "Accepted",
  "message": "Payment successfully executed"
}
```

**Duplicated Payment**

If the same merchant tries to create two requests with the same `requestId`, the api will assume that the second request is a duplicated payment.

Request:

```json
{
  "requestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 10,
  "currency": "USD",
  "cardDetails": {
    "holder": "Some Card Holder",
    "number": "4263982640269299",
    "cvv": "1234",
    "expiryMonth": 1,
    "expiryYear": 2020
  }
}
```

Response:

`Status Code: 409-Conflict`

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
  "title": "Conflict",
  "status": 409,
  "detail": "A payment with RequestId 3fa85f64-5717-4562-b3fc-2c963f66afa6 has already been created. Check the Payment Details for the payment Id:a034c7c2-d07b-4212-b935-86ea17375b6e"
}
```

**Failed Payment**

The Acquiring bank implementation is set to Decline payments with an amount above `100000` regardless of the currency.

Request:

```json
{
  "requestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 100001,
  "currency": "USD",
  "cardDetails": {
    "holder": "Some Card Holder",
    "number": "4263982640269299",
    "cvv": "1234",
    "expiryMonth": 1,
    "expiryYear": 2020
  }
}
```

Response:

`Status Code: 201-Created`

```json
{
  "id": "50ea4d0d-dcc8-4cde-8adb-8aaf2ba14572",
  "requestId": "1fa85f64-5717-4562-b3fc-2c963f66afa6",
  "createdAt": "2022-07-11T21:12:53.7876182Z",
  "processedAt": "2022-07-11T21:12:53.9096618Z",
  "status": "Declined",
  "message": "The payment amount is too high"
}
```

**Failed Payment validation**

If some of the properties in the payment failed to be validated by the domain.

Request:

```json
{
  "requestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 100001,
  "currency": "TEST",
  "cardDetails": {
    "holder": "Some Card Holder",
    "number": "4263982640269299",
    "cvv": "1234",
    "expiryMonth": 1,
    "expiryYear": 2020
  }
}
```

Response:

`Status Code: 400-Bad Request`

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Currency 'TEST' is unsupported"
}
```

### `GET /payments/{id}`

Define the `x-api-key` header : `mTbC4r1Eh7wvXrXE1UDl18NGH1fRzcrRz`

**Happy Path**

Response:

`Status Code: 200-Ok`

```
{
  "id": "a2bd0bfe-167e-4ae4-8f8c-1933b48d9c94",
  "requestId": "2fa85f64-5717-4542-b3fc-2c963f66afa6",
  "amount": 1000,
  "currency": "USD",
  "cardDetails": {
    "holder": "Daniel",
    "number": "426398******9299",
    "cvv": "1234",
    "expiryMonth": 1,
    "expiryYear": 2020
  },
  "createdAt": "2022-07-11T21:23:22.785946Z",
  "processedAt": "2022-07-11T21:23:22.901978Z",
  "status": "Accepted",
  "message": "Payment successfully executed"
}
```

**Payment Not found**

Happens if the given payment Id doesn't exist in the database, or even if it exists but associated to another Merchant

Response:

`Status Code: 404-Not Found`

```
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "traceId": "00-4aef0ad55f5335f9abceb812231cf94a-ca22d4d01311b644-00"
}
```

# Setup

## Pre-requisites

- .NET Core 6.0
- Docker, if you wish to run with docker
- MakeFile, if you wish to run using make commands

You can run the application using:

1. Visual Studio. Set `PaymentGateway.Api` as the default startup project.
2. Run `docker-compose build` followed by `docker-compose up` and open `http://localhost:9501/swagger` in a browser of your choice
3. Run `make run` and open `http://localhost:9501/swagger` in a browser of your choice

In swagger, use the `Authorize` button and insert one of the API keys defined in `appsettings.json`:

`mTbC4r1Eh7wvXrXE1UDl18NGH1fRzcrRz`

`mCRvA1t9Fp0QhMeLu6IGs11LVH3gTjmnEY`

Tests can also be executed via `docker-compose` or `make` command containers:

```bash
make unit-tests
```

```bash
make integration-tests
```

# Extras

## Authentication

All the endpoints are secured by API Key Authentication.
You can find the available keys in the appsettings.json.

## Logging

`SeriLog` for structured file logging.

Log files can be found in the /logs folder

## Conatinerized

The service, database, unit tests and integration tests are containerized using docker.

# Improvements

- More unit and integration tests can be added in the future. Due to time constraints I just added tests to cover the main functionality and to showcase how testing could be done with the current project structure.
- Resilience mechanisms when communicating with the database or Acquiring Bank. Transient errors when communicating with Acquiring Bank coul be retried.
- Async communication with Acquiring Bank. This way, we would be able to retrieve a fast response to the merchant and implement some sort of webhook integration when a response from the Acquiring bank is received.
- Client SDK to communicate with the API.
- Implement CQRS in order to separate reads and writes into different databases. For now the solution architecture is simply using CQS.
- CI to be added with github actions. This should be straightforward as we already have images being built in the `docker-compose` to run unit and integration tests
