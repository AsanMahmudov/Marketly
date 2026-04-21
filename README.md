# Marketly - Professional Classified Ads Platform

## 📌 Project Overview
Marketly is a comprehensive ASP.NET Core web application designed for a modern classifieds marketplace. The platform enables users to list items, manage interest via watchlists, and communicate through a real-time messaging system.

### 🔍 How it Works
Marketly acts as a bridge between buyers and sellers through a streamlined, secure workflow:

* **Ad Discovery:** Users can browse a paginated list of advertisements, utilizing a dynamic search and category filtering system to find specific items.
* **User Engagement:** Authenticated users can "Watch" ads to track them in their personal dashboard or initiate direct communication with sellers via an integrated messaging system.
* **Ad Lifecycle:** Sellers can create, edit, and soft-delete their listings. The system ensures data integrity by preventing the permanent deletion of records that have associated message history.
* **Admin Oversight:** Administrators have access to a secure dashboard to manage the marketplace categories, equipped with safety checks to ensure no category is removed while it still contains active listings.

## 🏗️ Architecture & Design Decisions
The project is built following the **3-Layer Architecture** pattern to ensure a clean separation of concerns and high maintainability:

* **Marketly.Web (Presentation Layer):** Handles the UI logic, Routing, and Area management (including a secure Admin Area for category management).
* **Marketly.Core (Domain/Business Layer):** Contains all service logic, interfaces, and view models. This layer is independent of the database implementation to ensure high testability.
* **Marketly.Infrastructure (Data Layer):** Manages the database context, repository implementations, and data seeding.

### Applied SOLID Principles & Patterns
* **Dependency Inversion:** All services are injected via interfaces (`IAdService`, `ICategoryService`, etc.), ensuring the system is decoupled.
* **Repository Pattern:** We utilize an `IApplicationRepository` to abstract Entity Framework Core, keeping business logic agnostic of the data source.
* **Soft Delete:** To maintain data integrity and message history, ads are "Soft Deleted" using an `IsActive` flag instead of being removed from the database.
* **Safety Guards:** The `CategoryService` includes logic to block the deletion of categories that still contain active ads, preventing orphaned records.

## 🛠️ Key Features
* **Advanced Pagination:** Server-side pagination is handled in the `AdService` to ensure high performance even with large datasets.
* **Conversation Threading:** Messages are grouped by participants and Ad ID to provide a seamless chat experience.
* **Secure Admin Area:** A dedicated area for administrators to manage categories, featuring custom Bootstrap modal confirmations for dangerous actions.
* **Zero-Configuration Setup:** The project is configured to automatically create and seed the database on the first run using **LocalDB**.

## 🚀 How to Run
1.  **Open** the solution in Visual Studio 2022.
2.  **Restore** NuGet packages.
3.  **Ensure** SQL Server LocalDB is running.
4.  **Press F5:** The application will automatically apply migrations and seed initial data.

### 🧪 Test Credentials
| Role | Email | Password |
| :--- | :--- | :--- |
| **Administrator** | `admin@marketly.com` | `Admin123!` |
| **Regular User** | `user@marketly.com` | `User123!` |

## 🧪 Testing Strategy
* **Unit Testing:** The project includes a robust test suite using **NUnit** and **Moq**.
* **Async Mocking:** We utilized `MockQueryable` to simulate complex asynchronous EF Core operations like `ToListAsync()` and `CountAsync()` in a unit test environment.
* **Coverage:** Business logic services (Ads, Categories, Messages, and Users) maintain high code coverage.
