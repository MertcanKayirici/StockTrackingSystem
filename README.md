# 🚀 Stock Tracking System

<p align="center">
  <img src="https://img.shields.io/badge/.NET-Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET-Core-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL-Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Entity-Framework-512BD4?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Bootstrap-5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" />
  <img src="https://img.shields.io/badge/Chart.js-FF6384?style=for-the-badge&logo=chartdotjs&logoColor=white" />
</p>

---

> ⚡ A modern stock management system with advanced filtering, export capabilities, and a real-time admin dashboard

A scalable and production-style **stock tracking and management system** built with **ASP.NET Core MVC, Entity Framework Core, and SQL Server**.

The application simulates a real-world business environment, enabling full control over **inventory, suppliers, categories, and stock movements** through a modern and responsive **admin dashboard**.

It focuses on **data-driven decision making**, **clean and maintainable architecture**, and **high usability**.

---

## 🎬 Demo GIFs

> Overview of system features and real-time interactions

<p align="center">
  <img src="Screenshots/app_overview.gif" width="100%" />
</p>

<p align="center">
  <img src="Screenshots/ajax_live_update.gif" width="49%" />
  <img src="Screenshots/stock_movement_create.gif" width="49%" />
</p>

---

## ✨ Key Features

- 📦 Product Management (CRUD)
- 🏢 Supplier Management (CRUD + status toggle)
- 🗂️ Category Management
- 🔄 Stock In / Stock Out System
- 📊 Real-time Dashboard Statistics
- 📥 Export System (Excel / PDF with filtering support)
- ⚡ AJAX-based Status Updates (Active / Passive toggle)
- 🧾 Audit Log System (tracks all critical actions)
- 🔍 Advanced Filtering & Search
- 📅 Date-based filtering & sorting
- 🎨 Modern UI (Bootstrap 5 + custom styling)
- 📈 Stock analytics & insights

---

## 🚀 Key Highlights

- Real-world business logic implementation  
- Advanced filtering & reporting system  
- Export functionality (Excel & PDF)  
- Audit logging for traceability  
- AJAX-powered dynamic UI  

---

## 🛠️ Tech Stack

- ASP.NET Core MVC (Backend)
- Entity Framework Core (ORM)
- SQL Server (Database)
- Bootstrap 5 (UI Framework)
- JavaScript (AJAX, Fetch API)
- HTML5 / CSS3 (Frontend structure)

---

## 🎥 Feature Demonstrations

### 📥 Export Feature (Excel / PDF)

- Export filtered data directly from the admin panel  
- Supports Excel (.xlsx) and PDF (.pdf) formats  
- Preserves active filters during export operations  

<p align="center">
  <img src="Screenshots/export_demo.gif" width="100%" />
</p>

---

### 📊 Dashboard Overview

![Dashboard](Screenshots/dashboard.png)

---

### 📦 Product Management

| List | Create | Edit |
|------|--------|------|
| ![Product List](Screenshots/product_list.png) | ![Product Create](Screenshots/product_create.png) | ![Product Edit](Screenshots/product_edit.png) |

---

### 🔄 Stock Movement

| List | Create | Details |
|------|--------|---------|
| ![Stock Movement List](Screenshots/stock_movement_list.png) | ![Stock Movement Create](Screenshots/stock_movement_create.png) | ![Stock Movement Details](Screenshots/stock_movement_details.png) |

---

### 🏢 Supplier Management

| List | Create | Edit |
|------|--------|------|
| ![Supplier List](Screenshots/supplier_list.png) | ![Supplier Create](Screenshots/supplier_create.png) | ![Supplier Edit](Screenshots/supplier_edit.png) |

---

### 🗂️ Category Management

| List | Create | Edit |
|------|--------|------|
| ![Category List](Screenshots/category_list.png) | ![Category Create](Screenshots/category_create.png) | ![Category Edit](Screenshots/category_edit.png) |

---

## 🧠 Database Design

> Relational database structure designed for scalability

![Database Diagram](Screenshots/database_diagram.png)

---

## ⚙️ Installation

### 1. Clone the repository
```bash
git clone https://github.com/MertcanKayirici/StockTrackingSystem.git
```
### 2. Open the project

Open the project using Visual Studio / VS Code

### 3. Create database

Create a database named:
```bash
StockTrackingDb
```
### 4. Run SQL script

Execute:
```bash
/Database/StockTrackingDb.sql
```
### 5. Configure connection string

Update your appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=StockTrackingDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
> ⚠️ You may need to adjust the server name depending on your local SQL Server configuration.

### 6. Run the project
Run the project using **Visual Studio (F5)** 🚀

---

## 📌 Important Notes
- Ensure SQL Server is running
- Update the connection string if needed
- Do not share sensitive credentials
 
---

## 📂 Project Structure
- Controllers → Handle HTTP requests and business flow  
- Models → Entity Framework data models  
- Views → Razor UI components  
- Database → SQL scripts and schema  
- Screenshots → Project visuals & demos  

---

## 📌 Architecture Highlights
- Layered MVC architecture
- Separation of concerns (Controller / Service / Data layers)
- Clear request flow (Controller → Service → Data access)
- Relational database design with constraints
- AJAX-driven dynamic UI
- Centralized Audit Log system
- Reusable UI components
- Business logic handled through structured layers
  
---

## ⭐ Project Purpose

This project was built to simulate a real-world inventory management system and demonstrate:

- Scalable backend architecture  
- Relational database design  
- Business logic implementation  
- Admin panel development  

---

## 💡 Why This Project Matters

This project demonstrates the ability to build a real-world business application, not just a simple CRUD system.

It includes:

- Business logic handling (stock movements)
- Data consistency with relational database design
- Audit logging for traceability
- Dynamic UI with AJAX
- Reporting and export capabilities

This makes it closer to a real production system used in companies.

---

## 👨‍💻 Developer

**Mertcan Kayırıcı**  
Backend-focused Full Stack Developer
ASP.NET Core & SQL Server

- GitHub: https://github.com/MertcanKayirici
- LinkedIn: https://www.linkedin.com/in/mertcankayirici

--- 

## ⭐️ Support

If you like this project, don't forget to star ⭐ the repository.

---
