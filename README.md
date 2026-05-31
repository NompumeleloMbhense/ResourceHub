# ResourceHub API

A modern resource booking and management system built with **ASP.NET Core, Blazor WebAssembly and Entity Framework Core**.

ResourceHub helps organizations manage shared resources such as meeting rooms, equipment, workspaces and other bookable assets through a clean web interface.

---

## Features

**Dashboard**
- View total resources
- View available resources
- View total bookings
- View upcoming bookings
- Quick access to common actions
- Upcoming bookings overview

**Resource Management**
- Create resources
- Edit resources
- Delete resources
- View resource details
- Track availability status
- Capacity management
- Search and filter resources

**Booking Management**
- Create bookings
- Edit bookings
- Delete bookings
- Move bookings between resources
- View booking details
- Search and filter bookings
- Date range filtering
- Pagination support

**Authentication & Authorization**
- Secure login system
- Protected pages using authorization
- JWT authentication

---

## Tech Stack

- **.NET 10 / ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **Swagger (Swashbuckle)**

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
    "DefaultConnection": "Server=.;Database=ResourceHubDb;Trusted_Connection=True;"
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

**2. Resource Availability Filtering**
Challenge
As the number of resources increased, finding available resources became difficult.

Solution
Added filtering and search functionality that allows users to:
- Search by resource name
- Filter by location
- Filter by availability
- Filter by capacity range


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

## Author

**Nompumelelo Mbhense**
Software Developer focused on the Microsoft ecosystem, building projects with C#, .NET, ASP.NET Core, Blazor and SQL Server.

---

## License
This project is licensed under the MIT License.
