For this coding challenge i will choose to develop the Inventory Service.

For the sake of time a few corners have been cut with regards to the proposed architecture in [[1 - The Design Challenge]] 

1. No form of message broker is used. RabbitMQ would have been the choice for ease of setup and integration test support. And flexible support for message broker technology
2. Authentication/Authorisation is not enabled for the `admin` endpoints. Auth0 JWT bearer would be sufficient for authentication.
3. An in memory cache is used for caching HTTP responses. Redis would have been the logical choice.
4. Due to local issues with my postgres where Migrations were not being executed, an in memory database adapter is used for Entity Framework. 
5. The Inventory Validation gRPC service is effectively mock implemented

In places where corners have been cut, code to achieve such as been left but commented out. 

There is a skeleton CRUD support for Books. Using CQRS from REST to handlers, this would enable query splitting for a real database.

The following endpoints are enabled. Viewable with Swagger.

## Public API 
Authentication would be enabled in the service mesh layer

<font  color="purple">HTTP GET</font> `/book/{id}`
Enables fetching a single book by ID. 

<font  color="purple">HTTP GET</font> `/books/?offset={offset}&pageSize={pageSize}`
Enables fetching multiple books. This endpoint supports paging, however due to time, filtering is not supported.


## Admin API
Authentication would be enabled in the application layer

<font  color="green">HTTP POST</font> `/admin/books`
Enables creation of a book

<font  color="orange">HTTP PUT</font> `/admin/books/{id}`
Enables the modification of an existing book by ID. If the record exists, the request body is used to modify it.

<font  color="red">HTTP DELETE</font> `/admin/books/{id}`
Enables the deletion of a record by ID


## Inventory Service
This a gRPC service used for the Shopping Cart service to validate stock prior to checkout. 

#### ValidateInventory
This RPC takes a list of Inventory items which are composed of `{ ID, Required_Quantity }`. The response is the received items, with their stock status set as a 50/50 chance of being `InStock` or `OutOfStock`



# Testing
Attached is my insomnia collection. which i used for local testing the HTTP client support. 

There is a skeleton for an Integration Test framework, with a working example of an integration test for the `InventoryService.ValidateInventory