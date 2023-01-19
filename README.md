# eShop with Microservices
UNDER DEVELOPMENT
eShop done with microservices with NET Core.

## Catalog.API
Microservice created to populate Catalog by API RESTful.
Database used: MongoDB

## Basket.API 
Microservice created to populate Basket or Cart.
Memory database used: Redis 

## Discount.Grpc
GRPC Microservice for checking discounts when Checkout order is triggered.
Database used: PostgreSQL

## Ordering.API 
Microservice for ordering. Basket.API send a message into RabbitMQ queue and Ordering.API consumes this message and inserts in database.
Database used: SQL Server.

##Ocelot API Gateway
Gateway for routing from one microservice.
