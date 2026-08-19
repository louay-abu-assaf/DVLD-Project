# DVLD Project (Driving & Vehicle License Department)

A desktop system for managing driver licensing workflows, built with a classic **3-tier architecture** using **C# / .NET Framework 4.8**, **Windows Forms**, and **SQL Server**.

## Overview

DVLD Project is designed to digitalize core licensing operations inside a driving and vehicle license department.  
It supports end-to-end flows from person registration to license issuance, renewal, replacement, and detainment handling.

## Core Functional Areas

- **People Management**
  - Register, edit, and search people records.
- **User & Access Management**
  - User administration, login, session handling, and password changes.
- **Driver Management**
  - Link people to driver profiles and manage driver records.
- **Application Management**
  - Create and track licensing applications and statuses.
- **Local Driving License Workflow**
  - New local license applications, test scheduling/tracking, and first-time issuance.
- **International License Workflow**
  - Issue and manage international licenses.
- **License Services**
  - Renew licenses.
  - Replace lost or damaged licenses.
  - Detain and release licenses.
- **Configuration Modules**
  - Manage application types.
  - Manage test types.

## Architecture

The solution follows a layered structure:

1. **Presentation Tier** (`DVLD Project/`)
   - Windows Forms UI, screens, user controls, and navigation logic.
2. **Business Tier** (`DVLD Buisness Tier/`)
   - Domain entities and business rules (`clsApplications`, `clsLicense`, `clsTest`, etc.).
3. **Data Access Tier** (`DVLD DataAccess Tier/`)
   - SQL Server communication and CRUD operations through ADO.NET.

## Solution Structure

```text
DVLD-Project/
├── DVLD Project/               # Presentation Tier (WinForms)
├── DVLD Buisness Tier/         # Business Logic Tier
├── DVLD DataAccess Tier/       # Data Access Tier
└── Images/                     # Project images/screenshots
```

## Technology Stack

- **Language:** C#
- **Framework:** .NET Framework 4.8
- **UI:** Windows Forms
- **Database:** Microsoft SQL Server
- **Data Access:** ADO.NET (`System.Data.SqlClient`)
- **IDE:** Visual Studio 2022 (solution format v17)

## Dependencies (UI Libraries)

The presentation project references several UI component libraries, including:

- Telerik WinForms components
- Guna.UI2
- Bunifu UI
- SunnyUI
- Mook.UI
- Hamekoz.UI.WinForm

> Note: Some references in the project file use local machine paths. You may need to adjust package/library locations before building.

## Getting Started

### 1) Prerequisites

- Windows OS
- Visual Studio 2022 (or compatible with .NET Framework 4.8 projects)
- SQL Server instance with a prepared **DVLD** database schema and seed data

### 2) Configure Database Connection

Update the connection string in:

`DVLD DataAccess Tier/DataAccessSettings.cs`

```csharp
public static string connectionString = "server=.;Database=DVLD;user ID=sa;******;";
```

Replace it with your SQL Server credentials/environment values.

### 3) Open and Build

1. Open solution file:
   - `DVLD Project/DVLD Project.sln`
2. Restore dependencies (if needed).
3. Build in **Debug** or **Release** mode.
4. Run the **DVLD Project Presentation Tier** startup project.

## Notes

- This repository currently focuses on application source code and assets.
- Database setup scripts are not fully documented in the root of the repository, so you should ensure the DVLD database structure is already available before first run.

## Author

**Louay Abu-assaf**  
(As referenced in the application UI)
