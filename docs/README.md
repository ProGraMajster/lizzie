# Lizzie Documentation

This directory contains reference material for the Lizzie scripting language.

- [Introduction and reference guide](introduction.md)

Additional examples and usage information are available in the [project README](../README.md).

The introduction covers core language features including typed variable
declarations using `var(@name, type, value)` and the host memory functions
`host-var`, `host-set` and `host-del` which allow values to persist across script
invocations.

## HTTP module

Lizzie ships with an HTTP module exposing simple helpers for making network
requests from scripts. The module provides `get`, `post`, `put` and `deleteUrl`
functions that perform HTTP GET, POST, PUT and DELETE operations respectively.

```lizzie
get("https://example.com")
post("https://example.com/api", "{ 'foo': 42 }")
put("https://example.com/api/1", "{ 'foo': 43 }")
deleteUrl("https://example.com/api/1")
```

Using these functions requires the runtime to allow `Capability.Network` and
to expose an `INetworkPolicy` implementation. The policy is consulted to ensure
requests are only sent to whitelisted origins.

Runtime profiles such as `RuntimeProfiles.ServerDefaults` accept an HTTP
whitelist and automatically register the HTTP module:

```csharp
var ctx = RuntimeProfiles.ServerDefaults(
    httpWhitelist: new[] { "https://example.com" });
// ctx exposes the HTTP functions and enforces the whitelist
```

Origins must be specified using the scheme and authority (e.g.
`https://example.com`). Requests to non-whitelisted origins will throw an
`UnauthorizedAccessException`.

