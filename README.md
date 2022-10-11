# booking-app-server

Aplicação de gerenciamento de locações.

## Build and Publish

```sh
docker buildx build -t maxandriani/booking-app-server:0.0.0-rc1 -f ./src/BookingApp.RestServer/Dockerfile --platform linux/amd64,linux/arm64 --push .
```