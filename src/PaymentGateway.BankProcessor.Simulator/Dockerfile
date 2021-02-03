FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy csproj and restore as distinct layers

COPY ./src/PaymentGateway.BankProcessor.Simulator/PaymentGateway.BankProcessor.Simulator.csproj ./PaymentGateway.BankProcessor.Simulator/

RUN dotnet restore PaymentGateway.BankProcessor.Simulator/PaymentGateway.BankProcessor.Simulator.csproj

# copy everything else and build
COPY ./src/ ./
WORKDIR /PaymentGateway.BankProcessor.Simulator/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine
WORKDIR /app
COPY --from=build /PaymentGateway.BankProcessor.Simulator/out ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "PaymentGateway.BankProcessor.Simulator.dll"]