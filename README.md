# Hsm-Server

This project is a hospital management system project and Made for BIM441 lesson

As a scenario, the user registers, searches for an appointment, confirms the appointment and is informed with the necessary e-mail notifications.

## Project Architecture and Design Patterns

The project was designed in accordance with Onion Architecture and modern software development principles to provide a modular, testable and extensible structure.

- Onion Architecture (Soğan Mimarisi) : The project is developed with an onion architecture where dependencies are routed from outer layers to inner layers
- Domain-Driven Design (DDD) : Proje, iş mantığını ve alan bilgisini merkezde tutan bir yaklaşım olan Domain-Driven Design (DDD) prensiplerine uygun olarak geliştirilmiştir.
- CQRS and SOA : In the project, Command (Write) and Query (Read) operations are clearly separated and the Project has been developed in accordance with a service-oriented architectural structure.
- Options Pattern : The project uses the Options Pattern to make configuration settings readable and manageable.
- Builder Pattern : By using the Builder Pattern in the project, complex objects were created step by step and flexibly.

## Features

- Patients can book, update or cancel appointments online using their unique credentials..

- Integrated email keeps patients informed about upcoming appointments or updates

- It provides private and secure access for different roles such as administrator, doctor and patient.

## Library Used

- **EventFlux**

  - **Explanation**: It is an event-based event handler library with multiple event triggering features.

  - **Usage Example**: You can give an assembly reference and then can inject:
    ```csharp
    services.AddEventBus(AssemblyReference.Assemblies);
    ```

- **EfCore.Repository and Base.Repository**

  - **Explanation**: It is a general library that can work on all SQL-based databases..

  - **Usage Example**: An assembly reference can be given and the model interface can be assigned for additional marking.:
    ```csharp
    services.EfCoreRepositoryServiceRegistration<IBaseEntity, HsmDbContext>(ServiceLifetime.Scoped, assembly);
    ```

- **FlowValidate**

  - **Explanation**: This library aims to subject models to a validation phase..

  - **Usage Example**: An assembly reference can be given and then the rules required for validation can be written.:
    ```csharp
    services.FluentVal(AssemblyReference.Assembly);
    ```

## Layers

#### 1. **Shared**

- This section has been developed to perform map operations on models in some places.

#### 2. **Test**

- This layer contains unit and integration tests. Mostly unit tests are written.

## Technologies Used
- Visual Studio
- Docker
- Nginx
- Postgresql
- ASP.Net Core API
- Entity Framework Core
- .Net Core 8
- Entity Framework Core
- Swagger
- EventFlux
- EfCore.Repository
- FlowValidate
- SMTP
- CQRS
- PowerShell
