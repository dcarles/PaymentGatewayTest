FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy csproj and restore as distinct layers

COPY ./src/PaymentGateway.Api/PaymentGateway.Api.csproj ./PaymentGateway.Api/
COPY ./src/PaymentGateway.PaymentsCore/PaymentGateway.PaymentsCore.csproj ./PaymentGateway.PaymentsCore/
COPY ./src/PaymentGateway.BankProcessor/PaymentGateway.BankProcessor.csproj ./PaymentGateway.BankProcessor/
COPY ./src/PaymentGateway.Data/PaymentGateway.Data.csproj ./PaymentGateway.Data/

RUN dotnet restore PaymentGateway.Api/PaymentGateway.Api.csproj

# copy everything else and build
COPY ./src/ ./
WORKDIR /PaymentGateway.Api/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine
WORKDIR /app
COPY --from=build /PaymentGateway.Api/out ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]