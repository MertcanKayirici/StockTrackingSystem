# 🚀 Stock Tracking System

A modern and fully dynamic **stock tracking and management system** built with **ASP.NET Core MVC, Entity Framework Core, and SQL Server**.

This project provides a complete solution for managing **products, suppliers, categories, and stock movements** through a powerful **admin dashboard**, while delivering a clean and responsive management experience.

---

### 🎬 Demo GIFs

| 🖥️ System Overview |
|-------------------|
| ![](Screenshots/app_overview.gif) |

| ⚡ AJAX Live Updates | 🔄 Stock Movement |
|---------------------|------------------|
| ![](Screenshots/ajax_live_update.gif) | ![](Screenshots/stock_movement_create.gif) |

---

## ✨ Features

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

### 📊 Dashboard

![](Screenshots/dashboard.png)

---

### 📦 Product Management

| List | Create | Edit |
|------|--------|------|
| ![](Screenshots/product_list.png) | ![](Screenshots/product_create.png) | ![](Screenshots/product_edit.png) |

---

### 🔄 Stock Movement

| List | Create | Details |
|------|--------|---------|
| ![](Screenshots/stock_movement_list.png) | ![](Screenshots/stock_movement_create.png) | ![](Screenshots/stock_movement_details.png) |

---

### 🏢 Supplier Management

| List | Create | Edit |
|------|--------|------|
| ![](Screenshots/supplier_list.png) | ![](Screenshots/supplier_create.png) | ![](Screenshots/supplier_edit.png) |

---

### 🗂️ Category Management

| List | Create | Edit |
|------|--------|------|
| ![](Screenshots/category_list.png) | ![](Screenshots/category_create.png) | ![](Screenshots/category_edit.png) |

---

### 🧠 Database Diagram

![](Screenshots/database_diagram.png)

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
```xml
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=StockTrackingDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
> ⚠️ You may need to adjust the server name depending on your local SQL Server configuration.

### 6. Run the project
dotnet run

---

## 📌 Important Notes
- Ensure SQL Server is running
- Update the connection string if needed
- Do not share sensitive credentials
- 
---

## 📂 Project Structure
Controllers → MVC Controllers
Models → Entity Framework Models
Views → Razor Views
Database → SQL Scripts
Screenshots → Images & GIFs

---

## 👨‍💻 Developer

Mertcan Kayırıcı

Backend-focused Full Stack Developer
ASP.NET Core & SQL Server

---

## ⭐ Project Purpose

This project was developed to simulate a real-world stock tracking system, focusing on:

- Clean architecture
- Scalable database design
- Admin panel usability
- Real-time data management
