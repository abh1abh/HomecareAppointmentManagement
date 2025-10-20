# HomecareAppointmentManagement

## How to Run

```bash
dotnet restore
dotnet build
dotnet run
```

**Runs on**: localhost:5191

**Node version**: 22.11.0

**.NET version**: 8.0

## Login, Authentication and Authorization

We have limited the MVP with only these six users and no registration.

Below are all the pre-seeded user accounts created by the database initializer (`DBInit`).

> **Note:** The database resets on every run.

---

#### Admin Login

| Role  | Email                    | Password      |
| ----- | ------------------------ | ------------- |
| Admin | **admin@homecare.local** | **Admin123!** |

---

#### Clients Logins

| Name        | Email                   | Password       | Role   |
| ----------- | ----------------------- | -------------- | ------ |
| John Doe    | **john@homecare.local** | **Client123!** | Client |
| Jane Smith  | **jane@homecare.local** | **Client123!** | Client |
| Bob Johnson | **bob@homecare.local**  | **Client123!** | Client |

---

#### Healthcare Workers Logins

| Name         | Email                    | Password      | Role             |
| ------------ | ------------------------ | ------------- | ---------------- |
| Alice Brown  | **alice@homecare.local** | **Nurse123!** | HealthcareWorker |
| David Wilson | **david@homecare.local** | **Nurse123!** | HealthcareWorker |

---
