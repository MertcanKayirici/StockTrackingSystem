# 🚀 Stock Tracking System

A modern and fully dynamic **stock tracking and management system** built with **ASP.NET Core MVC, Entity Framework Core, and SQL Server**.

This project provides a complete solution for managing **products, suppliers, categories, and stock movements** through a powerful **admin dashboard**, while delivering a clean and responsive management experience.

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

## ✨ Features

<<<<<<< HEAD
### 📦 Product Management
- Full CRUD operations  
- Category-based organization  
- Stock quantity tracking  
- Active / Passive status system  

### 🏢 Supplier Management
- Supplier CRUD operations  
- Status toggle (Active / Passive)  
- Contact & company information tracking  
- Advanced filtering & search  

### 🔄 Stock Movement System
- Stock In / Stock Out operations  
- Movement history tracking  
- Real-time stock updates  
- Data consistency with relational structure  

### 📊 Dashboard & Analytics
- Real-time system statistics  
- Product, supplier and stock insights  
- Clean and modern dashboard UI  

### ⚡ System Features
- AJAX-based dynamic updates  
- Audit log system (action tracking)  
- Date-based filtering & sorting  
- Responsive and modern UI (Bootstrap 5)  
=======
- 📦 Product Management (CRUD)  
- 🏢 Supplier Management (CRUD + status toggle)  
- 🗂️ Category Management  
- 🔄 Stock In / Stock Out System  
- 📊 Real-time Dashboard Statistics  
- 📥 Advanced Export System (Excel / PDF with dynamic filtering support)
- ⚡ AJAX-based Status Updates (Active / Passive toggle)  
- 🧾 Audit Log System (tracking all actions)  
- 🔍 Advanced Filtering & Search (supplier panel)  
- 📅 Date-based filtering & sorting  
- 🎨 Modern UI (custom CSS + Bootstrap 5)  
>>>>>>> acfb166 (Enhance README with export feature, architecture improvements and professional descriptions)

---

## 🛠️ Tech Stack

- ASP.NET Core MVC  
- Entity Framework Core  
- Microsoft SQL Server  
- Bootstrap 5  
- JavaScript (AJAX, Fetch API)  
- HTML5 / CSS3  

---

## 📸 Screenshots

<<<<<<< HEAD
### 📊 Dashboard
=======
### 📥 Export Feature (Excel / PDF)

- Export filtered data directly from the admin panel  
- Supports Excel (.xlsx) and PDF (.pdf) formats  
- Maintains active filters during export  

![Export Demo](Screenshots/export-demo.gif)

---

### 📦 Product & Stock Management
>>>>>>> acfb166 (Enhance README with export feature, architecture improvements and professional descriptions)

![Dashboard Overview](Screenshots/dashboard.png)

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

### 🧠 Database Diagram

![Database Diagram](Screenshots/database_diagram.png)

---

## 🚀 Key Highlights

- Fully dynamic stock management system  
- Real-time AJAX interactions  
- Clean and scalable database design  
- Audit logging system for tracking actions  
- Modern admin panel UI  

---

## 🏗️ Architecture

This project follows a **layered MVC architecture**:

- Controllers → Handle application flow  
- Models → Represent database entities (Entity Framework Core)  
- Views → Razor-based UI  
- Database → SQL Server relational structure  

The system is designed with **clean architecture principles and scalability in mind**.

---

## 🔄 How It Works

### 👤 User / Admin Flow
1. Admin accesses the dashboard  
2. Performs CRUD operations (products, suppliers, categories)  
3. Stock movements update quantities in real-time  
4. Changes are reflected instantly via AJAX  

### 🔄 Stock Logic
- Stock In → increases quantity  
- Stock Out → decreases quantity  
- All actions are logged in the system  

---

## ⚙️ Installation

### 1. Clone the repository
```bash
git clone https://github.com/MertcanKayirici/StockManagementSystem.git
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

<<<<<<< HEAD
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
=======
## 📌 Architecture Highlights
- Layered MVC architecture
- Separation of concerns (Controller / Service / Data layers)
- Entity relationships with strong constraints
- Reusable UI components
- AJAX-driven dynamic interactions
- Centralized logging (Audit Log system)
>>>>>>> acfb166 (Enhance README with export feature, architecture improvements and professional descriptions)

---

## 👨‍💻 Developer

**Mertcan Kayırıcı**  
Backend-Focused Full Stack Developer

<<<<<<< HEAD
Backend-focused Full Stack Developer
ASP.NET Core & SQL Server

---

## ⭐ Project Purpose

This project was developed to simulate a real-world stock tracking system, focusing on:

- Clean architecture
- Scalable database design
- Admin panel usability
- Real-time data management

---
=======
GitHub: https://github.com/MertcanKayirici
LinkedIn: https://www.linkedin.com/in/mertcankayirici

---

## 💡 Project Purpose

This project was built to demonstrate **real-world backend development skills**, including:

- Data modeling & relational design  
- Business logic implementation  
- Admin panel architecture  
- Reporting & export systems  

It is part of my professional portfolio.

--- 

## ⭐️ Support

If you like this project, don't forget to star ⭐ the repository.


>>>>>>> acfb166 (Enhance README with export feature, architecture improvements and professional descriptions)
