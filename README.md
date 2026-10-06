# Society_hub

## Project Description

**Society_hub** is a comprehensive, web-based housing society and apartment management portal built with ASP.NET Core MVC and Entity Framework Core. The application streamlines and digitizes day-to-day residential community operations by providing dedicated workflows for society administrators, residents, and gate security guards. From flat and resident record keeping, complaint handling, and maintenance billing to scheduled visitor tracking and community event registrations, Society_hub centralizes all society interactions into an intuitive, role-governed platform.

---

## Features

- **Role-Based Access Control**: Strict multi-role authentication and authorization using ASP.NET Core Identity for Admins, Residents, and Security Guards.
- **Apartment & Flat Management**: Complete tracking of apartment blocks/wings and individual flats with occupancy status.
- **Resident Administration**: Resident profile management with flat assignments, family size tracking, and personal accounts.
- **Staff & Security Guard Management**: Admin onboarding of security guards with assigned shifts and credentials.
- **Security Guard Gate Operations**: Real-time gate dashboard for security guards with live expected visitors, checked-in tracking, and instant check-in / check-out actions.
- **Resident Self-Service Dashboard**: Personal resident portal with real-time counters, profile overview, quick service actions, and billing statements.
- **Visitor Pre-Registration & Gate Log**: Residents can pre-register incoming visitors, while security guards verify and stamp entry and exit times at the gate.
- **Complaint Grievance Tracking**: Resident ticket filing, administrative assignment, workflow transitions (*Pending* → *In Progress* → *Resolved*).
- **Maintenance Billing & Payment Records**: Generation of monthly maintenance dues, marking bills as paid with official receipt numbers, resident payment history, and printable payment receipts.
- **Community Notices Board**: Society-wide circular and announcement board published by administrators and readable by residents.
- **Event Scheduling & Privacy-Compliant RSVP**: Society events with registration and cancellation features. Full attendee roster is visible only to Admins, while residents view only their own registration status.
- **Responsive Bootstrap UI**: Modern, clean, and mobile-friendly interface across all dashboards, forms, and data tables.

---

## User Roles

### Admin
- **Society Management**: Manage apartment blocks, wings, and towers.
- **Flat Management**: Create and configure flats, floor numbers, and track vacant vs. occupied units.
- **Resident Management**: Manage resident profiles, contact details, and flat allocations.
- **Security Guard Management**: Onboard security guards, assign duty shifts (Morning, Evening, Night), and manage staff credentials.
- **Complaint Management**: Review all complaints across the society, advance tickets through workflow stages (*Start Work*, *Resolve*), and track issue resolution.
- **Visitor Management**: Oversee the comprehensive society visitor history and security log.
- **Maintenance Bills**: Generate monthly maintenance bills, set due dates and amounts, mark payments as received, and generate receipt numbers.
- **Notices**: Author, edit, publish, and delete official society notices and circulars.
- **Events**: Schedule community events and inspect attendee registrations.
- **Dashboard**: High-level statistical overview of total residents, total flats, occupancy counts, pending complaints, today's visitors, and upcoming events.

### Resident
- **Dashboard**: Overview of personal profile, flat assignment, pending complaints, upcoming visitors, notice counts, and upcoming events.
- **Complaints**: Submit new maintenance complaints and track their status in real time.
- **Visitors**: Pre-register expected guests and track scheduled visitor arrival and gate logs.
- **Maintenance Bills**: View monthly maintenance statements and current payment status (*Pending* or *Paid*).
- **Payment History**: Review settled maintenance payments.
- **Receipt**: View and print official electronic payment receipts for paid bills.
- **Notices**: Read official announcements, circulars, and updates published by the management committee.
- **Events**: Browse scheduled community events, register attendance, or cancel registrations.

### Security Guard
- **Dashboard**: Dedicated gate operations center displaying guard profile, assigned duty shift, expected visitors count, and active inside visitors count.
- **Visitor Management**: View expected visitors list and active visitors currently inside the society premises.
- **Check-In**: Record arrival of expected guests with timestamped entry.
- **Check-Out**: Record departure of visitors with timestamped exit.
- **Profile**: View staff profile details, contact information, and assigned shift.

---

## Technologies Used

- **C#**: Primary backend programming language.
- **ASP.NET Core MVC (.NET 10)**: Web application framework and MVC architecture.
- **Entity Framework Core**: Object-Relational Mapper (ORM) for database querying and persistence.
- **SQL Server (LocalDB / Express)**: Relational database management system.
- **ASP.NET Core Identity**: User authentication, role management, password hashing, and cookie authorization.
- **Bootstrap 5**: Modern, responsive CSS framework for components, cards, tables, and modal layouts.

---

## Authentication and Authorization

The application uses ASP.NET Core Identity for secure authentication with role-based authorization attributes (`[Authorize(Roles = "...")]`):

- **Admin Role**: Possesses comprehensive management permissions across society blocks, flats, residents, security staff, billing, notices, and events.
- **Resident Role**: Scoped strictly to the authenticated resident's personal data. Residents cannot access administrative controls or view other residents' personal complaints, bills, or event RSVPs.
- **SecurityGuard Role**: Authorized specifically for gate visitor operations, check-in, check-out, and guard profile viewing.
- **Privacy Protections**: Event registration details are restricted so that residents can only see their own attendance status, while only Admins see the full society attendee list. Unauthorized access attempts are safely redirected to an Access Denied view or the login page.

---

## Database

The database consists of the following primary entities and relationships managed via Entity Framework Core:

- **ApartmentBlock ↔ Flat**: One-to-Many (`ApartmentBlock.Flats`). An apartment block contains multiple flats.
- **Flat ↔ Resident**: One-to-Many (`Flat.Residents`). A flat can have one or more registered residents.
- **ApplicationUser ↔ Resident**: One-to-One / Foreign Key (`Resident.ApplicationUserId`). Links the Identity account to the resident's profile.
- **ApplicationUser ↔ SecurityGuard**: One-to-One / Foreign Key (`SecurityGuard.ApplicationUserId`). Links the Identity account to the security guard's staff profile.
- **Resident ↔ Visitor**: One-to-Many (`Resident.Visitors`). Visitors are registered under the hosting resident.
- **Resident ↔ Complaint**: One-to-Many (`Resident.Complaints`). Complaints are submitted by and associated with specific residents.
- **Resident ↔ MaintenanceBill**: One-to-Many (`Resident.MaintenanceBills`). Maintenance bills are issued to individual residents.
- **Event ↔ EventRegistration ↔ Resident**: Many-to-Many junction relationship (`Event.EventRegistrations` & `Resident.EventRegistrations`). Tracks which residents have registered for particular community events.
- **Notice**: Standalone circulars published by society administrators for all residents.

---

## How to Run

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server or SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)
- Visual Studio 2022 (v17.12+) or Visual Studio Code

### Steps to Run

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Chaudhari-Dhruv/Society_Hub.git
   cd Society_Hub
   ```

2. **Configure Database Connection**:
   Open `Society_hub/appsettings.json` and verify or update the connection string to match your SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SocietyHubDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Apply Database Migrations**:
   Run the following command from the project root to create and update the database schema:
   ```bash
   dotnet ef database update --project Society_hub/Society_hub.csproj
   ```

4. **Build and Run the Application**:
   ```bash
   dotnet run --project Society_hub/Society_hub.csproj
   ```

5. **Access the Application**:
   Open your browser and navigate to the local URL displayed in the terminal (typically `https://localhost:7198` or `http://localhost:5249`).

6. **Default Roles**:
   Upon first launch, `IdentitySeeder` automatically creates the default roles: `Admin`, `Resident`, and `SecurityGuard`. Register an account via the Register page to get started.

---

## Future Enhancements

- **QR Code Visitor Verification**: Generate digital QR gate passes for instant verification at the society security checkpoint.
- **Payment Gateway Integration**: Online payment processing via Stripe, Razorpay, or UPI for real-time maintenance settlement.
- **Automated Notifications**: Email and SMS alerts for notice board publications, visitor arrivals, and overdue bills.
- **Document Management**: Resident parking permit and lease agreement document uploads.
- **Facility Booking**: Reservation system for clubhouse, swimming pool, and community hall amenities.
