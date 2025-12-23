# Don Damitz Portfolio Website

Professional portfolio website showcasing my skills, experience, and projects. Built with ASP.NET Core 8 and deployed to Microsoft Azure with a custom domain.

## 🚀 Live Demo

**Visit the live site:** [https://dondamitz.com](https://dondamitz.com)

**View source code:** [https://github.com/damit7/DonDamitzWebsite](https://github.com/damit7/DonDamitzWebsite)

> Built with ASP.NET Core 8, deployed to Microsoft Azure App Service with custom domain and SSL certificate

---

## ✨ Features

- **Professional Home Page** - Clean, modern introduction with professional branding
- **Resume Page** - Comprehensive work experience, education, and skills with downloadable PDF
- **Technologies Page** - Detailed showcase of technologies, frameworks, and development tools
- **About Me** - Personal background, interests, and professional journey
- **Contact Form** - Secure contact form with Azure SQL Database storage, rate limiting, and spam protection
- **Privacy Policy** - Comprehensive privacy policy covering data collection, security, and user rights

---

## 🛠️ Technologies Used

### Backend
- **Framework:** ASP.NET Core 8.0
- **Pattern:** Razor Pages (page-focused architecture)
- **Language:** C# 12
- **Database:** Azure SQL Database
- **Data Access:** ADO.NET with parameterized queries

### Frontend
- **UI Framework:** Bootstrap 5
- **Templating:** Razor syntax
- **Styling:** HTML5, CSS3
- **JavaScript:** jQuery (Bootstrap components)

### Cloud & DevOps
- **Hosting:** Microsoft Azure App Service
- **Database:** Azure SQL Database
- **Domain:** Custom domain (dondamitz.com) with DNS configuration
- **SSL/TLS:** Free Azure-managed SSL certificate with HTTPS enforcement
- **Version Control:** Git & GitHub

---

## 🏗️ Architecture

This project uses a **3-layer architecture** for clean separation of concerns:

### Presentation Layer
- **Location:** `Pages/` folder
- **Technology:** ASP.NET Core Razor Pages
- **Responsibility:** User interface, HTTP requests, view models

### Business Logic Layer
- **Location:** `Services/` folder
- **Responsibility:** 
  - Validation and business rules
  - Rate limiting (5 messages per 15 minutes per IP)
  - Spam detection and prevention
  - Input sanitization

### Data Access Layer
- **Location:** `Data/` folder
- **Technology:** ADO.NET
- **Responsibility:**
  - Database operations with parameterized queries
  - SQL injection prevention
  - Repository pattern implementation

---

## 🔐 Security Features

- ✅ **SQL Injection Prevention** - Parameterized queries using SqlParameter throughout all database operations
- ✅ **XSS Protection** - Automatic output encoding via ASP.NET Core Razor Pages
- ✅ **CSRF Protection** - Anti-forgery tokens automatically included in all form submissions
- ✅ **Rate Limiting** - Custom IP-based rate limiting: maximum 5 contact submissions per IP per 15 minutes
- ✅ **Input Validation** - Comprehensive client-side and server-side validation using Data Annotations
- ✅ **HTTPS Enforcement** - All traffic encrypted with SSL/TLS, HTTP automatically redirects to HTTPS
- ✅ **Secure Headers** - Security headers configured in production environment
- ✅ **Database Firewall** - Azure SQL Database firewall restricts access to Azure App Service only

---

## 📦 Project Structure
```
DonDamitzWebsite/
├── Data/                    # Data Access Layer
│   └── ContactRepository.cs # Repository pattern for database operations
├── Models/                  # Data models and view models
│   └── ContactMessage.cs    # Contact form data model
├── Pages/                   # Presentation Layer (Razor Pages)
│   ├── Index.cshtml         # Home page
│   ├── Resume.cshtml        # Resume page
│   ├── Technologies.cshtml  # Technologies showcase
│   ├── About.cshtml         # About me page
│   ├── Contact.cshtml       # Contact form
│   ├── Contact.cshtml.cs    # Contact form logic
│   ├── Privacy.cshtml       # Privacy policy
│   └── Shared/              # Shared layout and partials
│       └── _Layout.cshtml
├── Services/                # Business Logic Layer
│   └── ContactService.cs    # Contact form business logic and rate limiting
├── wwwroot/                 # Static files
│   ├── css/                 # Custom stylesheets
│   ├── js/                  # JavaScript files
│   ├── images/              # Images and media
│   └── files/               # Downloadable files
│       └── DonDamitz_Resume.pdf
├── appsettings.json         # Configuration (development)
├── appsettings.Production.json # Production configuration
├── Program.cs               # Application entry point
├── .gitignore               # Git ignore rules
├── README.md                # This file
└── SECURITY.md              # Security policy
```

---

## 💻 Local Development Setup

### Prerequisites

- **Visual Studio 2022** (Community, Professional, or Enterprise)
- **.NET 8.0 SDK** or later
- **SQL Server LocalDB** (included with Visual Studio)

### Steps to Run Locally

1. **Clone the repository:**
```bash
   git clone https://github.com/damit7/DonDamitzWebsite.git
   cd DonDamitzWebsite
```

2. **Set up the local database:**
   - Open SQL Server Object Explorer in Visual Studio
   - Connect to `(localdb)\mssqllocaldb`
   - Create a new database named `DonDamitzPortfolioDB`
   - Run the following SQL to create the ContactMessages table:
```sql
   CREATE TABLE ContactMessages (
       Id INT PRIMARY KEY IDENTITY(1,1),
       Name NVARCHAR(100) NOT NULL,
       Email NVARCHAR(100) NOT NULL,
       Subject NVARCHAR(200) NOT NULL,
       Message NVARCHAR(MAX) NOT NULL,
       IPAddress NVARCHAR(45),
       SubmittedOn DATETIME2 NOT NULL
   );
```

3. **Update connection string:**
   - Open `appsettings.json`
   - Verify the connection string points to your LocalDB:
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DonDamitzPortfolioDB;Trusted_Connection=True;"
   }
```

4. **Run the application:**
   - Press **F5** in Visual Studio, or
   - Run from command line: `dotnet run`
   - Navigate to: `https://localhost:[port]`

---

## 🚀 Deployment

This application is deployed to **Microsoft Azure** using the following services:

### Azure Resources
- **Azure App Service** (Basic B1 tier) - Web application hosting
- **Azure SQL Database** (Basic tier) - Production database
- **Azure DNS** - Custom domain configuration
- **Azure-managed SSL Certificate** - Free SSL with automatic renewal

### Deployment Process

1. **Build in Release mode**
2. **Right-click project** > Publish
3. **Select Azure App Service** as publish target
4. **Configure connection strings** in Azure Portal (App Service > Configuration > Connection strings)
5. **Set up custom domain** in Azure Portal (App Service > Custom domains)
6. **Enable HTTPS Only** in Azure Portal (App Service > TLS/SSL settings)
7. **Publish and verify** at https://dondamitz.com

### Environment Variables (Azure)

Connection strings are stored securely in Azure App Service Configuration, not in source code.

---

## 📧 Contact

**Don Damitz**

- 🌐 **Portfolio:** [https://dondamitz.com](https://dondamitz.com)
- 💼 **LinkedIn:** [https://www.linkedin.com/in/don-damitz-b15a8a23/](https://www.linkedin.com/in/don-damitz-b15a8a23/)
- 💻 **GitHub:** [https://github.com/damit7](https://github.com/damit7)
- 📧 **Email:** dondamitz@gmail.com

---

## 📄 License

This project is open source and available for educational and portfolio purposes.

---

## 🙏 Acknowledgments

This portfolio website was built as a practical learning project to demonstrate:
- Modern ASP.NET Core 8 development
- Cloud deployment to Microsoft Azure
- 3-layer architecture and clean code principles
- Comprehensive security best practices
- Professional web development workflows

**Special thanks** to the ASP.NET Core and Azure communities for excellent documentation and resources.

---

## 🔒 Security

For security vulnerability reporting, please see [SECURITY.md](SECURITY.md)

**Please do not report security vulnerabilities in public GitHub issues.**