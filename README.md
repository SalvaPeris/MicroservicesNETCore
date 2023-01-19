# eShop with Microservices
UNDER DEVELOPMENT
eShop done with microservices with NET Core.

1. ## Catalog.API : Microservice created to populate Catalog by API RESTful.
  Database used: MongoDB

2. ## Basket.API : Microservice created to populate Basket or Cart.
  Memory database used: Redis 

3. ## Discount.Grpc : GRPC Microservice for checking discounts when Checkout order is triggered.
  Database used: PostgreSQL

4. ## Ordering.API : Microservice for ordering. Basket.API send a message into RabbitMQ queue and Ordering.API consumes this message and inserts in database.
  Database used: SQL Server.
