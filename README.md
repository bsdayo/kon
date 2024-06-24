# Kon

A ultra simple "centralized configuration server" for distributed systems using .NET ecosystem.

Scans configuration files in directory and serve them as a single flattened JSON object.

Currently, supports `.json`, `.toml`, and `.ini` files.

## Getting started

### Basic usage

Prepare some configuration files in a directory, like, hello.json:

```json
{
  "Hello": "World!",
  "ConnectionStrings": {
    "MyDatabase": "Some=Thing; Here=There;"
  }
}
```

Serve all configuration files in current working directory:

```shell
$ kon
```

Curl it and check the response:

```shell
$ curl -s localhost:5000 | jq
{
  "Hello": "World!",
  "ConnectionStrings:MyDatabase": "Some=Thing; Here=There;"
}
```

### Authentication

Add `--token` option (or set the `TOKEN` environment variable) to enable authentication:

```shell
$ kon --token myauthtoken
```

Then provide the token in `Authorization` header:

```shell
$ curl -s localhost:5000 -H "Authorization: Bearer myauthtoken" | jq
```

### Change listening port

Specify listener prefix:

```shell
$ kon --urls http://127.0.0.1:80
```

## License

[MIT](LICENSE)