# Remote Lock

## Run instructions:
- Run `dotnet restore` to restore the project
- Run `dotnet build` to build the project
- Run `dotnet publish -o ./publish`
- Run `dotnet test` to run the unit tests
- Run `docker-compose build`
- Run `docker-compose up`

## Approach:
The goal here is to allow users to open the doors remotely and show historical events to extend user experience beyond classical tags. So what it takes to have customers using such a cool feature? I have come up with a MVP which would allow the customers to login, view the doors they have access to and access them remotely either from Mobile/Laptop or equivalent gadgets. For quick & smoother onboarding of the customer, we can seed the data of the Users/Office/Door & their role details (In future we can have them via API's). Also we can make few of them as admins of the system among them to have access to the historical events of any users within their office(s).

Now we have successfully onboarded customer into our system, any user can login with their login credentials and should be able to view the doors they have access to and open the door remotely. Users can even provide comments if they are opening the door for someone else. As an admin will have a special access to look at the historical events of the user.

## Design:
System follows the typical clean architecture having Domain as the core, surrounded by Application layer which defines the functionalities of domain and sets the standard. Infrastructure does the job of conncecting to the outside world like Database, file system, logs. API provides an interface for clients to talk to the system.

### Domain Layer:
This will contain all entities and logic specific to the domain layer.

### Application Layer:
This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

#### Infrastructure Layer:
This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

#### API Layer:
REST layer which provides an interface to the clients to talk to the system via API's. This layer depends on both Application & Infrastructure layers, however dependency on the Infrastructure is only to support dependency injection. Therefore only Startup.cs should reference Infrastructure.

Few key acitivites which takes place within servie:
- Permission Middleware: Validates the users trying to access audit logs has necessary permission
- ErrorHandler Middleware: Global exception handler
- ValidationException: Exceptions on business validation errors

## Tech Stack:
- .Net Core
- C#
- EF Core
- SQL Server
- Elastic
- Kibana
- Gherkin

## Third party libraries:
- MediatR
- AutoMapper
- Swashbuckle
- NEST
- Serilog
- FluentValidation
- xUnit
- Specflow

## Table Mapping:
![image]()

## Testing:
Unit tests are written using xUnit and can be made part of CI
How can we test the product before getting into production? Simple! I have integrated the BDD scenarios which can completely replace the manual testing efforts and also can be made part of our CD. With this we can achieve one click deployment to production too.

## Advantages:
- Scalability
- Maintainability
- Testability
