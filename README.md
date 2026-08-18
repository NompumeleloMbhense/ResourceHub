# ResourceHub API

ResourceHub is a full-stack resource booking and management system built with **Blazor WebAssembly, 
ASP.NET Core 10 Web API and Entity Framework Core**.

The application enables organizations to manage shared resources such as meeting rooms, equipment and workspaces
while providing a streamlined booking experience with authentication, filtering, dashboards and booking management capabilities.

---

## Key Features

**Resource Management**
- Create, edit, view, and delete resources
- Track resource availability
- Capacity management
- Search resources by name and location
- Filter resources by availability and capacity range
- Paginated resource listings

**Booking Management**
- Create, edit, view, and delete bookings
- Move bookings between resources
- View booking history for individual resources
- Search bookings by keyword or user
- Filter bookings by date range
- Paginated booking listings

**Dashboard**
- Total resources overview
- Available resources overview
- Total bookings overview
- Upcoming bookings overview
- Upcoming bookings table for quick visibility into scheduled reservations

**Authentication & Security**
- JWT-based authentication
- Protected routes using authorization
- Secure API communication between client and server

---

## Tech Stack

- **C#**
- **.NET 10**
- **Entity Framework Core**
- **Blazor WebAssembly**
- **SQL Server**
- **AutoMapper**
- **HTML & CSS**
- **JWT Authentication**

---

## System Architecture

The application follows a layered architecture across multiple projects:

**ResourceHub.Api (Backend)**
ASP.NET Core Web API responsible for:

- Business logic
- Validation
- Repository implementations
- Entity Framework Core database access
- Authentication and authorization

**ResourceHub.Client (Frontend)**
Blazor WebAssembly application responsible for:

- User interface
- API communication
- State management
- Resource and booking management workflows

**ResourceHub.Core**
Contains:

- Domain entities
- Repository interfaces
- Query parameter models
- Shared business contracts

**ResourceHub.Shared**
Contains:

- DTOs (Data Transfer Objects)
- Form models
- Shared validation models

**ResourceHub.Infrastructure**
Responsible for:

- Data persistence
- Repository implementations
- Database configuration
- Entity Framework Core integration

---

## Getting Started

**Prerequisites**
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

**Clone Repository**

    git clone https://github.com/NompumeleloMbhense/ResourceHub.git

**Navigate To Project**

    cd ResourceHub

**Update Database Connection**

Update the connection string in:

    appsettings.json

example:

    "ConnectionStrings": {
          "DefaultConnection": "YOUR_CONNECTION_STRING_HERE"
    }

**Apply Migrations**

    dotnet ef database update
    
**Run API**

    dotnet run --project ResourceHub.Api

**Run Client**

    dotnet run --project ResourceHub.Client

---

## Challenges and Solutions

**1. Booking Validation**

Challenge

Users could create bookings where the end time occurred before the start time, resulting in invalid reservations.

Solution

Implemented both client-side and server-side validation to ensure:
- End time must be later than start time
- Invalid bookings are rejected before being saved
- Clear validation messages are displayed to users

**2. Error Handling & Standardised Exception Design**

Challenge

The API initially handled exceptions using a large switch statement in middleware to map different exception types to HTTP status codes. As the number of domain exceptions grew, this approach became harder to maintain and violated the Open/Closed Principle.

Solution

Refactored the error-handling system to use a standardised base exception pattern, removing the need for switch-based mapping:
- Introduced a base exception class (e.g. AppException) containing a reusable HTTP status code property
- All domain-specific exceptions (e.g. BookingConflictException, ResourceNotFoundException) inherit from this base class
- Updated global exception middleware to handle all custom exceptions in a single unified block
- Eliminated large switch statements for cleaner and more maintainable mapping
- Ensured each exception is self-describing and carries its own HTTP status responsibility


**3. Booking Relocation Feature**

Challenge

Users needed a way to move existing bookings to a different resource without deleting and recreating them.

Solution

Implemented a booking relocation workflow:
- Users can select an existing booking
- Choose a new resource
- Update the booking while preserving booking details

---

## Future Improvements

- Future Improvements
- Role-based authorization
- Booking conflict calendar view
- Email notifications
- Resource images
- Analytics dashboard
- Dark mode
- Database auditing
- Real-time booking updates with SignalR

---
## Images

**Home Page**
<img width="1898" height="867" alt="Homepage" src="https://github.com/user-attachments/assets/69b21cbf-66e6-4327-9dde-85206e1c8e73" />

**Login Page**
<img width="1891" height="860" alt="LoginPage" src="https://github.com/user-attachments/assets/fe0a0bbb-75a0-4263-b6a8-5068d10247d7" />


**Resources**

**Dashboard**
<img width="800" height="366" alt="Dashboard" src="https://github.com/user-attachments/assets/be5ab8d1-04f1-4e0c-9858-96bbaeb7c797" />


**Resources List**
<img width="800" height="363" alt="ResourcesList" src="https://github.com/user-attachments/assets/947bc223-4d83-4774-b669-b4db3d35122b" />


**Resource Details**
<img width="800" height="362" alt="ResourceDetails" src="https://github.com/user-attachments/assets/112c39e5-51ce-4ca3-85ef-0f640672b218" />


**Resource Search**
<img width="800" height="366" alt="ResourcesSearch" src="https://github.com/user-attachments/assets/14442cc0-c2a4-47d1-83f9-c7ea090ffe8f" />


**Create Resource**
<img width="800" height="364" alt="CreateResource" src="https://github.com/user-attachments/assets/716ca454-5e5c-4e76-9495-f7cdffe95594" />


**Update Resource**
<img width="800" height="365" alt="UpdateResource" src="https://github.com/user-attachments/assets/42f27765-d584-486d-8948-0ede7d2b9875" />


**Delete Resource**
<img width="800" height="365" alt="DeleteResource" src="https://github.com/user-attachments/assets/c97b7c1b-eb40-42fe-ba39-8e038e7e0d1e" />

**Bookings**


**Bookings List**
<img width="800" height="364" alt="BookingsList" src="https://github.com/user-attachments/assets/6c713a3c-44d7-4f4a-ae26-31a3497f1da6" />


**Bookings Details**
<img width="800" height="365" alt="BookingDetails" src="https://github.com/user-attachments/assets/6cdbf67f-754a-4967-9f0f-0e75fa841be3" />


**Create Booking**
<img width="800" height="361" alt="CreateBooking" src="https://github.com/user-attachments/assets/54be2669-d6e8-44c2-90d8-638d3a562132" />


**Booking Conflict**
<img width="800" height="366" alt="BookingConflict" src="https://github.com/user-attachments/assets/ce524273-7893-48d2-b576-4f118cc3287b" />


**End Time must be greater than start time**
<img width="1897" height="868" alt="TimeConflict" src="https://github.com/user-attachments/assets/f7ec7934-8e31-4d0f-9062-727877bc52c8" />


**Booking Calendar**
<img width="1897" height="862" alt="BookingCalendar" src="https://github.com/user-attachments/assets/a919fecb-bb1c-4f25-ad5d-b8cbad4ebe80" />




---

## Author

**Nompumelelo Mbhense**
Software Developer focused on the Microsoft ecosystem, building projects with C#, .NET, ASP.NET Core, Blazor and SQL Server.

---

## License
This project is licensed under the MIT License.
