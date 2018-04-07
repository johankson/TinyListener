# TinyListener

A simple tool for listening to your applications. There is a shared server that anyone could use, but you should set up your own for security reasons.

## How to use it?

1. Make up a channel name
2. Install the [nuget package](https://www.nuget.org/packages/TinyListener/) in your .net project and configure it with that channel name
3. Log stuff using the client
4. View stuff using the web client 


### Client

Create a TinyListener client and start logging stuff. If you don't pass in a httpClient (with a base url set) the client will default to https://tinylistener.azurewebsites.net which is totally OPEN for anyone to view at the moment.

```csharp
// The current version demands an HttpClient to be passed. Future versions will most likely only take an URL.
var httpClient = CreateHttpClient();
var client = new TinyListener(httpClient);
await client.Send("test-channel", "Ducks are cute");
```

### Server

There is a web project that you should set up yourself and then configure your client with that URL.

If you just want to test it out, there is a server available at https://tinylistener.azurewebsites.net


## Why?

The main reason is to dump text to a web view. Most likely best to use during the early development or debugging of a mobile app.
These logs are not persisted. They are only visible a short while.

## Roadmap

* Think more about security... We need to make sure that the traffic and viewing of the logs are safe.

