# CareBlock Service

**CareBlock Service** is the backend component of the CareBlock project, a decentralized Electronic Health Record (EHR) system funded by Project Catalyst. This service manages secure medical data storage, authentication, and blockchain integration, enabling patients to have ownership and control over their health records across various healthcare providers.

## 🧩 Project Overview

CareBlock aims to revolutionize healthcare data management by:  
- **Empowering Patients**: Providing individuals with immutable and comprehensive access to their medical histories.  
- **Ensuring Interoperability**: Facilitating seamless data sharing across different healthcare systems and providers.  
- **Leveraging Blockchain**: Utilizing blockchain technology to guarantee data integrity, security, and transparency.

## 🛠️ Tech Stack

- **Language**: C#  
- **Framework**: ASP.NET Core  
- **Database**: Microsoft SQL Server  
- **Blockchain Integration**: Web3j (Java-based Ethereum library)

## 📂 Repository Structure

```
Careblock-service/
├── Scripts/             # Database scripts and migrations
├── Careblock-service/   # Core backend service code
├── Documents/           # Project documentation
├── .gitignore           # Git ignore file
└── README.md            # Project overview and setup instructions
```

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher)  
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- [Node.js](https://nodejs.org/) (for frontend integration, if applicable)

### Installation

1. **Clone the repository:**

```bash
git clone https://github.com/Careblock/Careblock-service.git
cd Careblock-service
```

2. **Set up the database:**

- Create a new SQL Server database.  
- Update the connection string in `appsettings.json` with your database credentials.

3. **Apply migrations:**

```bash
dotnet ef database update
```

4. **Run the application:**

```bash
dotnet run
```

The service will start on `http://localhost:5000` by default.

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

*This project is part of the CareBlock initiative, aiming to bring decentralized solutions to healthcare data management.*
