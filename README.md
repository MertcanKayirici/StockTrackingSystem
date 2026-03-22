# 🚀 Stock Tracking System

> ⚡ A production-ready stock management system with advanced filtering, export capabilities, and a real-time admin dashboard

A scalable and production-style **stock tracking and management system** built with **ASP.NET Core MVC, Entity Framework Core, and SQL Server**.

The application simulates a real-world business environment, enabling full control over **inventory, suppliers, categories, and stock movements** through a modern and responsive **admin dashboard**.

It focuses on **data-driven decision making**, **clean architecture**, and **high usability**.

---

## 🎬 Demo GIFs

> Overview of system features and real-time interactions

| 🖥️ System Overview |
|-------------------|
| ![Application Overview](Screenshots/app_overview.gif) |

| ⚡ AJAX Live Updates | 🔄 Stock Movement |
|---------------------|------------------|
| ![AJAX Live Update](Screenshots/ajax_live_update.gif) | ![Stock Movement Create](Screenshots/stock_movement_create.gif) |

---

## ✨ Key Features

- 📦 Product Management (CRUD)  
- 🏢 Supplier Management (CRUD + status toggle)  
- 🗂️ Category Management  
- 🔄 Stock In / Stock Out System  
- 📊 Real-time Dashboard Statistics  
- 📥 Advanced Export System (Excel / PDF with dynamic filtering support)  
- ⚡ AJAX-based Status Updates (Active / Passive toggle)  
- 🧾 Audit Log System (tracking all actions)  
- 🔍 Advanced Filtering & Search  
- 📅 Date-based filtering & sorting  
- 🎨 Modern UI (custom CSS + Bootstrap 5)  

---

## 🛠️ Tech Stack

- ASP.NET Core MVC  
- Entity Framework Core  
- Microsoft SQL Server  
- Bootstrap 5  
- JavaScript (AJAX, Fetch API)  
- HTML5 / CSS3  

---

## 🎥 Feature Demonstrations

### 📥 Export Feature (Excel / PDF)

- Export filtered data directly from the admin panel  
- Supports Excel (.xlsx) and PDF (.pdf) formats  
- Preserves active filters during export operations  

![Export Demo](Screenshots/export_demo.gif)

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
- Controllers → MVC Controllers  
- Models → Entity Framework Models  
- Views → Razor Views  
- Database → SQL Scripts  
- Screenshots → Images & GIFs  

---

## 📌 Architecture Highlights
- Layered MVC architecture
- Separation of concerns (Controller / Service / Data layers)
- Entity relationships with strong constraints
- Reusable UI components
- AJAX-driven dynamic interactions
- Centralized logging (Audit Log system)
- 
---

## ⭐ Project Purpose

This project was developed to simulate a real-world stock tracking system, focusing on:

- Clean architecture
- Scalable database design
- Admin panel usability
- Real-time data management

---

- GitHub: https://github.com/MertcanKayirici
- LinkedIn: https://www.linkedin.com/in/mertcankayirici

---

## 💡 Project Purpose

This project was built to demonstrate **real-world backend development skills**, including:

- Data modeling & relational design  
- Business logic implementation  
- Admin panel architecture  
- Reporting & export systems  

It is part of my professional portfolio.

---

## 👨‍💻 Developer

**Mertcan Kayırıcı**  
Backend-focused Full Stack Developer
ASP.NET Core & SQL Server

--- 

## ⭐️ Support

If you like this project, don't forget to star ⭐ the repository.

---
