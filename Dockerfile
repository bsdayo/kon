FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS builder
RUN apk add --no-cache clang build-base zlib-dev
WORKDIR /repo
COPY . .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
COPY --from=builder /publish/kon /bin/
WORKDIR /config
ENTRYPOINT ["/bin/kon"]