# eShop with Microservices
eShop done with microservices with NET Core and Aggregator pattern.


## Catalog.API
ASP.NET Core Web API application
REST API principles, CRUD operations
MongoDB database connection and containerization
Repository Pattern Implementation
Swagger Open API implementation

## Basket.API 
ASP.NET Web API application
REST API principles, CRUD operations
Redis database connection and containerization
Consume Discount gRPC Service for inter-service sync communication to calculate product final price
Publish BasketCheckout Queue with using MassTransit and RabbitMQ

## Discount.Grpc
ASP.NET gRPC Server application
Build a Highly Performant inter-service gRPC Communication with Basket Microservice
Exposing gRPC Services with creating Protobuf messages
Using Dapper for micro-orm implementation to simplify data access and ensure high performance
PostgreSQL database connection and containerization

## Ordering.API 
Implementing DDD, CQRS, and Clean Architecture with using Best Practices
Developing CQRS with using MediatR, FluentValidation and AutoMapper packages
Consuming RabbitMQ BasketCheckout event queue with using MassTransit-RabbitMQ Configuration
SqlServer database connection and containerization
Using Entity Framework Core ORM and auto migrate to SqlServer when application startup

## Ocelot API Gateway
Gateway for routing from one microservice.

## Microservices Communication
Sync inter-service gRPC Communication
Async Microservices Communication with RabbitMQ Message-Broker Service
Using RabbitMQ Publish/Subscribe Topic Exchange Model
Using MassTransit for abstraction over RabbitMQ Message-Broker system
Publishing BasketCheckout event queue from Basket microservices and Subscribing this event from Ordering microservices
Create RabbitMQ EventBus.Messages library and add references Microservices

## Web App
WebUI ShoppingApp Microservice
ASP.NET Core Web Application with Bootstrap 4 and Razor template
Call Ocelot APIs with HttpClientFactory
ASPNET Core Razor Tools — View Components, partial Views, Tag Helpers, Model Bindings and Validations, Razor Sections etc.
