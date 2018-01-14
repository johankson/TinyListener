# TinyListener

A simple tool for listening to your applications. There is a shared server that anyone could use, but you should set up your own for security reasons.

## How to use it?

1. Make up a channel name
2. Install the (soon to be) nuget package in your .net project and configure it with that channel name
3. Log stuff using the client
4. View stuff using the web client

## Why?

The main reason is to dump text to a web view. Most likely best to use during the early development or debugging of a mobile app.
These logs are not persisted. They are only visible a short while.

## Roadmap

* Think more about security... We need to make sure that the traffic and viewing of the logs are safe.

