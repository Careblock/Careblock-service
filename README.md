# Careblock

## Introduction

Careblock is an innovative healthcare application leveraging the security and transparency of the Cardano blockchain. Built with ASP.NET 6.0, Careblock aims to enhance healthcare data management by providing a decentralized, secure, and efficient platform for healthcare providers and patients.

## Table of Contents

1. [Features](#features)
2. [Installation](#installation)
3. [Configuration](#configuration)
4. [Usage](#usage)
5. [System Requirements and Specifications](#system-requirements-and-specifications)
6. [Architectural Design C4 Model](#architectural-design-c4-model)
7. [Contributing](#contributing)
8. [License](#license)

## Features

- **Decentralized Data Management**: Store healthcare data (EHR) securely on the Cardano blockchain.
- **Patient and Provider Portals**: Separate interfaces for patients and healthcare providers with custome and management features for each healthcare provider.
- **Secure Access**: Ensure data privacy and integrity using blockchain technology.
- **Scalability**: Built with ASP.NET 6.0 for high performance and scalability.

## Installation

### Prerequisites

- .NET 6.0 SDK
- Docker
- MSSQL 2019

### Steps

1. **Clone the repository**:
    ```bash
    git clone [https://github.com/Careblock/careblock.git](https://github.com/Careblock/Careblock-service.git)
    cd careblock
    ```

2. **Install backend dependencies**:
    ```bash
    dotnet restore
    ```

3. **Set up the database**:
    - Create a PostgreSQL database.
    - Update the connection string in `appsettings.json`.
    - Node: Currently, database is located in Azure cloud so we need connection string to connect. In the future, we will create a Docker Container to simply run the Database locally 

4. **Start the application**:
    ```bash
    dotnet run
    ```

## Configuration

Update the following settings in `appsettings.json`:

- **ConnectionStrings**: Set your PostgreSQL connection string.
- **JWTSettings**: Update JWT token settings for authentication.
- **PinataSettings**: Update API Key, ApiSecret for Pinata.
- **CloudinarySettings**: Update API Key, ApiSecret for Cloudinary.

```json
{
  "AppSettings": {
    "Secret": "",
    "RefreshTokenTTL": 2
  },
  "ConnectionString": "",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DatabaseConfig": {
    "TimeoutTime": 30,
    "DetailedError": true,
    "SensitiveDataLogging": true
  },
  "PinataConfig": {
    "ApiKey": "",
    "ApiSecret": "",
    "ApiUrl": "",
    "Bearer": ""
  },
  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

## Usage

### Running the Application

- **Development**:
    ```bash
    dotnet watch run
    ```

- **Production**:
    ```bash
    dotnet publish -c Release -o ./publish
    cd publish
    dotnet careblock.dll
    ```
## System Requirements and Specifications

Document details are written at this [link](https://github.com/Careblock/Careblock-service/blob/main/documents/system-requirements-and-specifications.md).

Summary of system requirements includes:

- **Login/Register**
- **Manage Personal Information**
- **Make an Appointment (Patient)**
- **Work Schedule Allocation (Admin)**
- **Doctor Management (Admin)**
- **Medication Management (Admin)**
- **Manage Examination Packages (Admin)**
- **Bill Payment (Patient)**
- **Statistics of Medical Examination History**
- **Export Examination Results (Doctor)**

## Architectural Design C4 Model

The system is designed according to the C4 model. Details at this [link](https://github.com/Careblock/Careblock-service/blob/main/documents/c4-model.md).

System design summary includes the following components:

- **Account Component**
- **Appointment Component**
- **Schedule Component**
- **Medicine Component**
- **Diagnostic Component**

## Contributing

We welcome contributions to Careblock. To contribute:

1. **Fork the repository**:
    - Click the "Fork" button on the top right corner of the repository page on GitHub.

2. **Create a feature branch**:
    ```bash
    git checkout -b feature/your-feature-name
    ```

3. **Commit your changes**:
    - Make your changes and commit them with a descriptive message.
    ```bash
    git add .
    git commit -m "Add a detailed description of your changes"
    ```

4. **Push the branch**:
    ```bash
    git push origin feature/your-feature-name
    ```

5. **Open a pull request**:
    - Go to your forked repository on GitHub.
    - Click on the "Compare & pull request" button.
    - Provide a detailed description of your changes and submit the pull request.

## License

This project is licensed under the MIT License.

---

For any questions or support, please contact [careblock.io@gmail.com](mailto:careblock.io@gmail.com).


