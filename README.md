# Electronic Medical Record

*Live Deployement Link:* https://electronic-medical-record-web-app-678-ezdxbub2gjddfrdp.eastasia-01.azurewebsites.net/ 

A full-stack Electronic Medical Record (EMR) web application developed as a senior software development project by a team of college students.

The application provides healthcare staff with tools for managing patients, appointments, medical information, practitioner charting, employee communication, and employee administration. The application uses **Blazor Server** for the user interface and a service-oriented architecture to separate business logic and database operations from the presentation layer.

The project was developed locally using **PostgreSQL** and deployed to **Azure SQL** for the production environment.

> **Note:** This application is an educational project and uses sample/test data. It is not intended for use with real patient information.

---

## Features

### Patient Management

* Patient demographic information
* Patient medical profiles
* Patient listing and search
* Practitioner patient charting
* Historical chart notes

### Scheduling

* Employee/practitioner scheduling
* Appointment creation and management
* Practitioner-specific schedules

### Charting

* Practitioner-created chart notes
* Chart history
* Author-based editing permissions
* Staff access to patient charts
* Practitioner-specific chart management

### Employee Management

* Employee accounts
* Role-based access
* Employee administration
* Administrative employee creation

### Messaging

* Internal employee messaging
* Chat functionality
* Real-time communication using SignalR

### Authentication & Authorization

* Secure employee login
* Role-based authorization
* Protected pages and operations
* Administrative access controls
* Practitioner-specific functionality

---

## Technologies

The application was developed using the following technologies:

* **C#** – Primary programming language
* **.NET 10** – Application runtime
* **ASP.NET Core** – Web application framework
* **Blazor Server** – Interactive web UI
* **Entity Framework Core** – ORM and database access
* **PostgreSQL** – Local development database
* **Azure SQL** – Production database
* **ASP.NET Core Identity** – Authentication and authorization
* **SignalR** – Real-time messaging
* **HTTP Endpoints** – Authentication-related requests
* **Git & GitHub** – Version control and collaborative development

---

## Architecture

The application follows a **service-oriented architecture** within a Blazor Server application.

The primary application flow is:

```text
Blazor Pages & Components
          ↓
       Services
          ↓
 Entity Framework Core
          ↓
 PostgreSQL / Azure SQL
```

### Project Structure

* **Pages/Components** – User interface and application interaction
* **Services** – Business logic, CRUD operations, and application workflows
* **Models** – Database entities and application models
* **DTOs** – Data transfer between application layers
* **Data** – Entity Framework Core database configuration
* **Identity** – Authentication, authorization, and role management

This separation allows UI components to remain focused on presentation and user interaction while application logic and database operations are handled by dedicated services.

---

## Roles & Permissions

The application supports three primary employee roles.

| Role              | Primary Responsibilities                                            |
| ----------------- | ------------------------------------------------------------------- |
| **Administrator** | Employee management and administrative functionality                |
| **Practitioner**  | Patient care, scheduling, and charting                              |
| **Employee**      | General staff functionality, including patient access and messaging |

### Chart Permissions

Patient charts use author-based permissions for editing.

* All authorized staff members can view patient charts.
* Practitioners can create chart notes.
* Only the practitioner who created a chart note can edit that note.
* Administrative and employee functionality is controlled through role-based authorization.

---

## Database

The database provides persistent storage for the application's primary data.

It includes:

* Patient information
* Patient medical profiles
* Appointments
* Practitioner chart notes
* Chart history
* Employee information
* Identity/user information
* Chat and messaging data

**PostgreSQL** was used during local development, while **Azure SQL** was used for the deployed production environment.

Entity Framework Core is responsible for database access and object-relational mapping.

---

## Authentication & Authorization

The application uses **ASP.NET Core Identity** for authentication and role-based authorization.

Authentication and authorization features include:

* Employee authentication
* Role-based access control
* Protected pages and operations
* Practitioner-specific permissions
* Administrative access controls
* Identity-managed user accounts

Authorization is applied at both the page and operation levels where appropriate.

---

## Team Contributions

This application was developed collaboratively as a senior software development project.

### Brandon Richards

**Team Lead / Software Developer**

* Global CSS configuration
* Database connection and database logic
* Initial sample data seeder and continued seeding development
* Authentication and authorization using ASP.NET Core Identity
* Login page
* Patient service
* Charting page, service, models, and components
* Scheduling page, service, models, and components
* Azure deployment

### Alexandre Paul Kouame

**Software Developer**

* Navigation menu
* Responsive page frontend
* Patient models
* Employee models and employee service
* Messaging page, service, models, and components
* Messaging frontend and backend
* Contribution to sample data seeding
* Chat and message sample data
* Employee sample data
* SignalR client configuration

### Haringanji Roike

**Software Developer**

* Patient listing page
* Patient dialog frontend
* Administration page frontend
* Employee management frontend
* Employee management dialogs
* Sample patient data

---

## Getting Started

### Prerequisites

Before running the application locally, install:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* PostgreSQL
* Git

### Setup

1. Clone the repository.

2. Configure the local PostgreSQL database connection.

3. Configure the required application secrets and environment-specific settings.

4. Restore the project dependencies:

```bash
dotnet restore
```

5. Apply the Entity Framework Core migrations:

```bash
dotnet ef database update
```

6. Run the application:

```bash
dotnet run
```

The application can then be accessed through the local development URL provided by ASP.NET Core.

---

## Deployment

The application was developed locally using PostgreSQL and deployed using **Azure SQL** for the production environment.

Environment-specific configuration is used to keep database connection information and other sensitive configuration outside of source control.

---

## Project Highlights

This project provided experience developing and maintaining a multi-user web application from development through deployment.

Key areas of experience include:

* Full-stack C#/.NET development
* Blazor Server application development
* Service-oriented architecture
* Entity Framework Core
* Relational database design and management
* Authentication and role-based authorization
* Real-time communication with SignalR
* CRUD application development
* Component-based UI development
* Git and GitHub collaboration
* Database migrations
* Azure deployment
* Collaborative software development and code integration

---

## Disclaimer

This project was developed for educational purposes as a senior software development project.

All patient, employee, appointment, and medical information contained within the application is fictional/sample data. The application is **not intended for use with real patient information or as a production healthcare system**.
