

![[Untitled-2024-03-19-1436(1).png]]




The chosen architecture routes all traffic through an API gateway, such as YARP, GraphQL or Pingora. This gateway will handle things like load balancing, routing and TLS. The request will then be routed to the appropriate service. 

Most services expose an HTTP RESTful API. The services will be appropriately replicated and horizontally scaled using an orchestrator such as Kubernetes with the reverse proxy routing requests dynamically.

For some situations synchronous requests between services are required, such as with the 'Shopping Cart' service asserting that the cart's contents are valid. gRPC would be the most appropriate technology for this task.

An asynchronous message broker will be used to create a loosely coupled events system between services, such as 'Order Made' and 'Book Reserved'. Azure Service Bus would be an appropriate message broker technology. 

Services will use a variety of persistence solutions appropriate to their use case. But in all cases write operations will be executed within transactions. Databases such as SQL Server and Azure CosmosDb would be considered.

* The **Inventory** service requires high availability and responsiveness to serve the front end browsing and searching of book inventory. Therefore the datastore is replicated between Read and Write operations. This is due to a predictably high volume of front end requests for Book lists, stock etc. Whereas Book data is less likely to be updated, such as in response to shopping cart or purchase events, or an administrator updating stock manually - this would be the purpose of the 'Admin' API. The API is required to be robust in design and support flexible querying with pagination tokens and filtering.
	* The use of a memory cache such as Redis further reduces hits to the database for data that does not require real time precision, such as Book metadata.
	* The Database would be a relational database such as SQL Server. Although a document database may also serve appropriately. I imagine there may be some relation between book and author that may drive the decision for a relational database here, i.e. `select b.book from books b left join authors a on a.author = b.author where b.author = a.author`
	* A Content Delivery Network could be considered to sit in front of this service, to speed up the delivery of static content such as book images, sample text. 
* The **Shopping Cart** service leverages a high availability document NoSQL database with data replicated across instances. It is important to preserve the state of shoppings carts throughout a user session using cookie authentication, so that if a user leaves the shopping window or a moderate time passes then their shopping basket is not lost. However consideration needs to be taken not to eternally hold onto shopping carts. This service has a gRPC connection to the Inventory Service so that the validity of the cart can be asserted upon checkout.
* The **Payments** service uses a relational database, importantly with transactions, to record payments made. Otherwise I would imagine this service to be quite simple. Payments would obviously be handled by a third party provider such as Stripe, although I am not sure how viable Stripe is for larger businesses.
* The **Shipping** service provides an API that would retrieve saved shipping details for use in a customers checkout process and handle providing customers notifications for their order. Shipments themselves would be sourced from the event broker as a result of events published from the **Shopping Cart** service. It would then be the responsibility of this service to watch for shipping updates for in progress shipments and serve notifications to users about their delivery via external services, i.e. 'Your order has been dispatched and handed to the delivery company'.


## Scalability
Would be achieved with a combination of methods within each service as well overarching architectural decisions. With these approaches the system would be able to react to varying load and serve 'hot' content rapidly. Techniques such as:
* Load Balancer
* CDN
* Caching
* K8s Orchestrated Horizontal Scaling
* Segregated Databases
* Replicated Databases
* Limited Synchronous Inter-Service Communication and use of Message Broker

## Reliability
Would be achieved with robust API design and good authentication. Whilst a simple and well designed user interface can go far with ensuring a friendly user experience. Backend techniques can also help:
* Authentication at the service mesh level can help prevent malicious actors from causing issues to customer baskets or book inventory.
* Persistence in the Shopping Cart service ensures that a customers baskets are not lost during their session even if the user's session is temporarily lost.
* Certain relaxed design regarding state that takes into consideration the distributed nature of the application allows the user to have a less interrupted shopping experience. When checks are required, such as with the final stages of checkout, is the state asserted with the potential to block a user's action.

## Availability
Always on would be achieved primarily through the use of Kubernetes as an orchestrator to enable scaling. A minimum replica set for services along with the reverse proxy ensures that traffic always makes it to it's destination. In addition appropriate retry policies would be in place so that transient errors do not result in unavailability. Replicated databases provide increased availability at the data layer since workload is distributed between instances and account for downtime with individual instances.

## Performance
Is ensured using the following techniques:
* Content Delivery Network enables frequent large static data to be served with reduced network hops. The CDN would serve the **Inventory** service and provide rapid responses to Book images for example.
* Horizontal Scaling reduces the workload served to individual services by spreading traffic between instances. Therefore improving throughput. 

## Maintainability
Is a big one within distributed system design and the following techniques can be used to help operate and diagnose issues:
* Telemetry support throughout all services so that activity data can be piped to an Observability provider such as Honeycomb, where network requests can be traced and timings analysed, such as HTTP request duration, database fetches. A mature observability solution will help identify hot paths and provide an avenue for targeted optimisations.
* A bespoke logging solution and appropriate logging at the Service Mesh and Service layers, as well as specific domain event logging will help explain the behaviour of the system at a given point. In addition the propagation of correlation IDs allows traffic to be isolated per context.



# 2 - The Code Challenge

[[README]]