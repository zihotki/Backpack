# Backpack
This project is a try to provide a common solution to implicitly pass various contextual 
information useful for servicing purposes (logging, tracing, calculating metrics) through the call chain. 

# Integration
The initial goal of the project is to have integration of both Zipkin and Backpack frameworks with [Serilog](https://serilog.net/) logging library and [EasyNetQ](http://easynetq.com/) (because who uses raw RabbitMQ anyway?). Zipkin implementation supports only json transport (and as a result ElasticSearch) and 64-bit SpanIds. It's implemented in accordance with [Open Zipkin documentation](http://zipkin.io/pages/instrumenting.html)

# References
Zipkin integration is highly based on https://github.com/d-collab/zipkin.net project with many core modifications.
