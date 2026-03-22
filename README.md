# 🚀 Stock Tracking System

> ⚡ Developed with modern admin panel architecture and real-world stock management logic

A fully dynamic **stock tracking and management system** built with **ASP.NET Core MVC, Entity Framework Core, and SQL Server**.

This project provides a complete solution for managing **products, suppliers, categories, and stock movements** through a powerful and user-friendly **admin dashboard**.

---

## 🎬 Demo

> Full system overview (dashboard, stock flow, CRUD operations)

![App Demo](Screenshots/app_overview.gif)

### ⚡ AJAX Live Update
![AJAX Demo](Screenshots/ajax_live_update.gif)

### 🔄 Stock Movement Create
![Stock Movement](Screenshots/stock_movement_create.gif)

---

## ✨ Key Features

- 📦 Product Management (CRUD)  
- 🏢 Supplier Management (CRUD + status toggle)  
- 🗂️ Category Management  
- 🔄 Stock In / Stock Out System  
- 📊 Real-time Dashboard Statistics  
- ⚡ AJAX-based Status Updates (Active / Passive toggle)  
- 🧾 Audit Log System (tracking all actions)  
- 🔍 Advanced Filtering & Search (supplier panel)  
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

### 📦 Product & Stock Management

| Product List | Product Create |
|--------------|----------------|
| ![](Screenshots/product_list.png) | ![](Screenshots/product_create.png) |

| Product Details | Product Edit |
|-----------------|--------------|
| ![](Screenshots/product_details.png) | ![](Screenshots/product_edit.png) |

---

### 🔄 Stock Movement System

| Stock List | Create Movement |
|------------|-----------------|
| ![](Screenshots/stock_movement_list.png) | ![](Screenshots/stock_movement_create.png) |

| Details | Edit |
|---------|------|
| ![](Screenshots/stock_movement_details.png) | ![](Screenshots/stock_movement_edit.png) |

---

### 🏢 Supplier Management

| Supplier List | Create |
|---------------|--------|
| ![](Screenshots/supplier_list.png) | ![](Screenshots/supplier_create.png) |

| Details | Edit |
|---------|------|
| ![](Screenshots/supplier_details.png) | ![](Screenshots/supplier_edit.png) |

---

### 🗂️ Category Management

| Category List | Create |
|---------------|--------|
| ![](Screenshots/category_list.png) | ![](Screenshots/category_create.png) |

| Details | Edit |
|---------|------|
| ![](Screenshots/category_details.png) | ![](Screenshots/category_edit.png) |

---

### 📊 Dashboard Overview

![Dashboard](Screenshots/dashboard.png)

---

## 🧠 Database Design

> Relational database structure designed for scalability

![Database Diagram](Screenshots/database_diagram.png)

---

## ⚙️ Installation

```bash
git clone https://github.com/MertcanKayirici/StockManagementSystem.git
```
1️⃣ Database Setup

Run SQL script:

/Database/StockTrackingDb.sql
2️⃣ Configure Connection String
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=StockTrackingDb;Trusted_Connection=True;"
}
3️⃣ Run Project
dotnet run

---

## 📌 Architecture Highlights
- Layered MVC structure
- Entity relationships (Foreign Keys & constraints)
- Clean UI component system
- AJAX-driven interactions
- Logging & tracking system

---

## 🚀 Future Improvements
📊 Chart.js dashboard (monthly / yearly stats)
🔔 Notification system (low stock alerts)
📱 Mobile-first UI improvements
🔐 Role-based authentication
📡 RESTful API layer

---

## 👨‍💻 Author

Mertcan Kayırıcı

GitHub: https://github.com/MertcanKayirici
LinkedIn: https://www.linkedin.com/in/mertcankayirici
⭐️ Support

If you like this project, don't forget to star ⭐ the repository.
