# C4 MODEL ARCHITECTURE
## [Careblock]

### I. System Context Diagram

![System Context Diagram](path/to/image1.png)

#### 1.1. CAREBLOCK [Software System]
CAREBLOCK is the primary software system for medical records management. It interacts with various users and external systems.

#### 1.2. Persons
- **Super Admin**
  - **Role**: Senior system administrator.
  - **Function**: Manage doctors and drugs in the system.
- **Patient**
  - **Role**: Patient web user.
  - **Function**: Manage personal profile and schedule appointments.
- **Doctor**
  - **Role**: Patient web user.
  - **Function**: Manage personal profile and schedule appointments.
- **Admin**
  - **Role**: System administrator.
  - **Function**: Manage doctors and arrange work.

#### 1.3.External Systems
CAREBLOCK interacts with external systems to store and process data:
- **Github**
  - **Function**: Public source code repository and version control.
  - **Relationship**: CAREBLOCK retrieves information from GitHub to ensure code consistency and updates.
- **Cardano nodes [Blockchain System]**
  - **Function**: Blockchain system for data storage.
  - **Relationship**: CAREBLOCK uploads data to Cardano nodes to ensure the integrity and security of medical data.
- **IPFS [Store System]**
  - **Function**: Distributed data storage system.
  - **Relationship**: CAREBLOCK uploads data to IPFS for fast and efficient data storage and retrieval.
- **Eternl Wallet [Blockchain System]**
  - **Function**: Blockchain wallet for data storage.
  - **Relationship**: CAREBLOCK uses the API (JSON/HTTPS) to interact with and store data in the Eternl Wallet.

#### 1.4. Detailed interactions
- **1.4.1. Super Admin**
  - **Clinic Management**:
    - Add new clinic to the system: Enter detailed clinic information.
    - Edit clinic information: Update clinic details, specialties, or other relevant information.
    - Delete clinic: Remove the clinic from the system when necessary.
  - **Medicine Management**:
    - Add new medicine to the catalog: Enter the name, ingredients, uses, and dosage of the medicine.
    - Update medicine information: Edit medicine details such as changing the dosage or adding notes.
    - Remove medicine from the catalog: Delete medicines no longer in use from the system.
- **1.4.2. Patient**
  - **Personal Record Management**:
    - Update personal information: Edit details such as name, address, phone number, and email.
    - View medical history: Access and view medical notes, past medical history, and previous tests.
  - **Appointment Scheduling**:
    - Search for doctors: Based on specialty, name, or location.
    - Choose appointment date and time: Select a suitable time based on the doctor's schedule.
    - Confirm appointment: Receive confirmation and reminders about the appointment via email or SMS.
- **1.4.3. Doctor**
  - **Patient Record Management**:
    - Access patient records: View detailed information about medical history, test results, and medical notes.
    - Update medical notes: Add or edit notes related to diagnosis and treatment.
    - Record treatment history: Update treatment processes and measures taken.
  - **Appointment Management**:
    - View appointments: Check the list of patient appointments.
    - Edit appointments: Change the time, cancel, or reschedule appointments.
    - Confirm and remind patients: Send confirmation and reminders about appointments.
- **1.4.4. Admin**
  - **Doctor Management**:
    - Add new doctor: Enter information and register the doctor in the system.
    - Update doctor information: Edit information related to the doctors.
    - Remove doctor: Remove doctors who are no longer working from the system.
  - **Work Scheduling**:
    - Schedule work shifts: Assign daily or weekly work shifts for doctors.
    - Adjust work schedules: Change or adjust work schedules as requested.
    - Manage clinics: Ensure clinics and equipment are ready for appointments.

### II. Container Diagram

![Container Diagram](path/to/image2.png)

#### 2.1. Account Component
The User Account Management component handles the account information of various user types such as patients, doctors, and administrators. It ensures the integrity and security of user data while supporting user authentication during system login and managing access rights based on roles. This component interacts with the Web Application to receive requests for registration, updates, deletion, and login from the web interface and sends responses regarding the request status. Additionally, it provides account information related to doctors' schedules to the Schedule Component and appointment information to the Appointment Component.

#### 2.2. Appointment Component
The Appointment Management component handles scheduling appointments between patients and doctors. It coordinates and manages the prioritization of appointments, ensuring that appointments do not conflict with doctors' schedules. The Appointment Component receives requests to create, update, and delete appointments from the Web Application, sends responses about appointment statuses, and receives account information related to appointments from the Account Component. Additionally, it sends diagnostic results related to appointments to the Diagnostic Component and selects medications related to appointments from the Medicine Component. All appointment information is stored in a Relational Database.

#### 2.3. Diagnostic Component
The Diagnostic Management component handles patient diagnostic data. It stores and manages diagnostic results, provides an interface for doctors to input and view results, and analyzes diagnostic data to assist doctors in making medical decisions. The Diagnostic Component also generates diagnostic reports for storage and sharing with patients and other doctors. This component receives diagnostic results from the Appointment Component, uploads diagnostic data to Elastic Search for storage and display, and uploads diagnostic metadata to Cardano nodes. Additionally, it stores diagnostic data in a Relational Database.

#### 2.4. Schedule Component
The Doctor Schedule Management component allows the creation of work schedules based on doctors' requests and availability, modifies schedules when necessary, and ensures that appointments do not conflict with work schedules. The Schedule Component receives requests to create and update work schedules from doctors' accounts via the Account Component and stores the schedule information in a Relational Database.

#### 2.5. Medicine Component
The Medicine Information Management component handles the creation, updating, and deletion of information about medicines, including dosages and usage instructions. It provides medication information for appointments and diagnoses. The Medicine Component receives requests to select medications related to appointments from the Appointment Component and stores medication information in a Relational Database.

#### 2.6. Relational Database
The Relational Database stores all data for the CAREBLOCK system, including accounts, appointments, diagnoses, schedules, and medications. It ensures data safety and security as well as data integrity through validation and backup mechanisms. The Relational Database interacts with all other components to store and retrieve necessary data.

#### 2.7. External Systems
The CAREBLOCK system interacts with several external systems to securely and distributedly store medical data. The Eternl Wallet stores medical data on the blockchain and receives data from the Account Component via API. Cardano nodes store diagnostic metadata from the Diagnostic Component. Elastic Search stores and displays diagnostic data receiving data from the Diagnostic Component. Finally, IPFS stores distributed data and receives data from the Account Component via API.

### III. Component Diagram
#### 3.1. Account Component

![Account Component](path/to/image3.1.png)

##### 3.1.1. Web Application
- **Function**: Provides a web-based interactive interface built using ReactJS. Allows users (patients, doctors, administrators) to manage accounts, schedule appointments, and handle diagnostic information.
- **Interaction**: Sends API requests (JSON/HTTPS) to various components within the system.

##### 3.1.2. Account Controller
- **Function**: Handles user account-related requests: creation, registration, validation, personal information updates, and account deletions.
- **Interaction**: Receives requests from the Web Application. Uses the Permission Controller to verify access rights. Interacts with the Account Database to store and retrieve account information.

##### 3.1.3. Patient Controller
- **Function**: Manages patient information and activities: viewing and updating medical records, scheduling appointments, and managing personal information.
- **Interaction**: Interacts with the Account Controller for account validation. Uses the Permission Controller to check access rights.

##### 3.1.4. Doctor Controller
- **Function**: Manages doctor information and activities: patient record management, work schedules, and medical diagnoses.
- **Interaction**: Interacts with the Account Controller for account validation. Uses the Permission Controller to check access rights.

##### 3.1.5. Admin Controller
- **Function**: Manages administrator information and activities: user, doctor, and patient management, and system monitoring.
- **Interaction**: Interacts with the Account Controller for account validation. Uses the Permission Controller to ensure proper access rights.

##### 3.1.6. Permission Controller
- **Function**: Manages and verifies user access rights within the system based on user roles (patient, doctor, administrator).
- **Interaction**: Interacts with all other controllers to verify and ensure valid access rights.

##### 3.1.7. Organization Controller
- **Function**: Manages information and activities of medical organizations: managing organization details, doctors, and patients.
- **Interaction**: Interacts with the Admin Controller for information validation and access rights management.

##### 3.1.8. Account Database
- **Function**: Stores all user account information: personal details, contact information, and access rights.
- **Interaction**: Uses SQL Server for secure data storage. Interacts with all other controllers for storing and retrieving information.

##### 3.1.9. Eternl Wallet
- **Function**: Blockchain-based data storage. Stores encrypted medical data for security and integrity.
- **Interaction**: Interacts with the Account Controller via API (JSON/HTTPS) to store and retrieve encrypted data.

#### 3.2. Appointment Component

![Appointment Component](path/to/image3.2.png)

##### 3.2.1. Appointment Controller
- **Function**: Manages appointment scheduling: creating, updating, and canceling appointments between patients and doctors.
- **Interaction**: Receives requests from the Web Application via API. Interacts with the Account Database and Relational Database for appointment information.

##### 3.2.2. Appointment Database
- **Function**: Stores all appointment-related data: patient appointments, doctor schedules, and appointment statuses.
- **Interaction**: Uses SQL Server for secure data storage. Interacts with the Appointment Controller for storing and retrieving data.

##### 3.2.3. Notification Controller
- **Function**: Manages notifications: sending appointment confirmations, reminders, and updates to patients and doctors.
- **Interaction**: Interacts with the Appointment Controller to gather appointment data. Uses external services (SMS, email) to send notifications.

#### 3.3. Diagnostic Component

![Diagnostic Component](path/to/image3.3.png)

##### 3.3.1. Diagnostic Controller
- **Function**: Manages diagnostic data: storing diagnostic results, analyzing data, and generating reports.
- **Interaction**: Receives diagnostic data from the Appointment Controller. Interacts with the Diagnostic Database and Elastic Search.

##### 3.3.2. Diagnostic Database
- **Function**: Stores all diagnostic data: results, analysis, and reports.
- **Interaction**: Uses SQL Server for secure data storage. Interacts with the Diagnostic Controller for storing and retrieving data.

##### 3.3.3. Elastic Search
- **Function**: Stores and displays diagnostic data for analysis.
- **Interaction**: Receives diagnostic data from the Diagnostic Controller for storage and analysis.

##### 3.3.4. Cardano Nodes
- **Function**: Blockchain-based data storage for diagnostic metadata.
- **Interaction**: Receives diagnostic metadata from the Diagnostic Controller for secure and distributed storage.

#### 3.4. Schedule Component

![Schedule Component](path/to/image3.4.png)

##### 3.4.1. Schedule Controller
- **Function**: Manages doctor schedules: creating, updating, and ensuring no conflicts with appointments.
- **Interaction**: Receives requests from the Web Application via API. Interacts with the Schedule Database for schedule information.

##### 3.4.2. Schedule Database
- **Function**: Stores all schedule-related data: doctor work schedules and availability.
- **Interaction**: Uses SQL Server for secure data storage. Interacts with the Schedule Controller for storing and retrieving data.

#### 3.5. Medicine Component

![Medicine Component](path/to/image3.5.png)

##### 3.5.1. Medicine Controller
- **Function**: Manages medicine information: creating, updating, and deleting medicine details.
- **Interaction**: Receives requests from the Web Application via API. Interacts with the Medicine Database for medicine information.

##### 3.5.2. Medicine Database
- **Function**: Stores all medicine-related data: names, ingredients, uses, and dosages.
- **Interaction**: Uses SQL Server for secure data storage. Interacts with the Medicine Controller for storing and retrieving data.

#### 3.6. Relational Database

![Relational Database](path/to/image3.6.png)

##### 3.6.1. Relational Database
- **Function**: Centralized storage for all system data: accounts, appointments, diagnoses, schedules, and medications.
- **Interaction**: Uses SQL Server for secure and validated data storage. Interacts with all other components for storing and retrieving data.

### IV. Code Component Diagram
#### 4.1. Account Component Code Structure

![Account Component Code](path/to/image4.1.png)

- **AccountService**: Contains business logic for managing user accounts, including registration, updates, and deletion.
- **AccountRepository**: Handles database interactions for account data using SQL queries and ORM.
- **AccountController**: Manages HTTP requests related to accounts, interacts with the AccountService for processing.

#### 4.2. Appointment Component Code Structure

![Appointment Component Code](path/to/image4.2.png)

- **AppointmentService**: Contains business logic for managing appointments, including creation, updates, and cancellations.
- **AppointmentRepository**: Handles database interactions for appointment data using SQL queries and ORM.
- **AppointmentController**: Manages HTTP requests related to appointments, interacts with the AppointmentService for processing.

#### 4.3. Diagnostic Component Code Structure

![Diagnostic Component Code](path/to/image4.3.png)

- **DiagnosticService**: Contains business logic for managing diagnostic data, including storing results and generating reports.
- **DiagnosticRepository**: Handles database interactions for diagnostic data using SQL queries and ORM.
- **DiagnosticController**: Manages HTTP requests related to diagnostics, interacts with the DiagnosticService for processing.

#### 4.4. Schedule Component Code Structure

![Schedule Component Code](path/to/image4.4.png)

- **ScheduleService**: Contains business logic for managing doctor schedules, including creation and updates.
- **ScheduleRepository**: Handles database interactions for schedule data using SQL queries and ORM.
- **ScheduleController**: Manages HTTP requests related to schedules, interacts with the ScheduleService for processing.

#### 4.5. Medicine Component Code Structure

![Medicine Component Code](path/to/image4.5.png)

- **MedicineService**: Contains business logic for managing medicine information, including creation, updates, and deletion.
- **MedicineRepository**: Handles database interactions for medicine data using SQL queries and ORM.
- **MedicineController**: Manages HTTP requests related to medicines, interacts with the MedicineService for processing.

---

### Diagrams
- **System Context Diagram**: Provides an overview of the entire system, showing the interactions between users and external systems.
- **Container Diagram**: Shows the system's high-level structure, illustrating the main containers/components and their interactions.
- **Component Diagram**: Details the internal structure of each container/component, showing the major components and their relationships.
- **Code Component Diagram**: Represents the code-level structure for each main component, detailing the classes, methods, and their interactions.