# System Requirements and Specifications Document
[Careblock]

![System overview diagram](link-to-system-overview-diagram.jpg) 
(If the image is too small you can view it [here](link-to-larger-image))

Here is the detailed documentation for each function in the CareBlock system as per the provided activity diagram:

## 1. Log in/Register

**Steps:**
1. **User enters registration information:**
    - **Description:** The user fills in the necessary information to create a new account including details such as username, password, email, and other required personal information.
    - **System Interaction:** The system collects and stores the registration data temporarily.
    - **Data Flow:** User input data is sent to the system's registration module for processing.

2. **User enters login information:**
    - **Description:** The user enters their username and password to log into the system.
    - **System Interaction:** The system compares the entered login credentials with the stored user data in the database.
    - **Data Flow:** Login credentials are sent to the authentication module.

3. **If the information is correct, the system grants access:**
    - **Description:** The system verifies the credentials. If they match the stored data, access is granted.
    - **System Interaction:** The system generates a session token for the user.
    - **Data Flow:** Authentication status and session token are returned to the user's interface.

4. **The user is redirected to the home page:**
    - **Description:** After successful login, the user is redirected to the system's home page.
    - **System Interaction:** The system loads the home page and personalizes it based on the user's profile.
    - **Data Flow:** Session data is used to display personalized information on the home page.

## 2. Manage Personal Information

**Steps:**
1. **User requests access to personal information:**
    - **Description:** The user navigates to the personal information management section.
    - **System Interaction:** The system retrieves the user's current personal data from the database.
    - **Data Flow:** User's request is processed and current personal data is fetched from the database.

2. **The system displays current information:**
    - **Description:** The system shows the user their current personal information.
    - **System Interaction:** The data is displayed in a user-friendly format for viewing and editing.
    - **Data Flow:** Retrieved data is sent to the user interface.

3. **User updates information:**
    - **Description:** The user edits their personal information, such as name, contact details, and other relevant data.
    - **System Interaction:** The system validates the new information and prepares it for updating.
    - **Data Flow:** Updated data is sent to the system's personal information module for validation.

4. **The system saves changes and updates information:**
    - **Description:** After validation, the system saves the updated information in the database.
    - **System Interaction:** The system confirms the update and stores the new data.
    - **Data Flow:** Updated data is stored in the database and confirmation is sent back to the user.

## 3. Make an Appointment (Patient)

**Steps:**
1. **Patients choose examination services:**
    - **Description:** The patient selects the type of medical services or examination they need.
    - **System Interaction:** The system displays available services based on the patient's selection.
    - **Data Flow:** Selected service data is sent to the appointment scheduling module.

2. **The system displays available time slots:**
    - **Description:** The system shows a list of available time slots for the chosen service.
    - **System Interaction:** Time slots are fetched from the database and displayed.
    - **Data Flow:** Available time slots are retrieved and sent to the user interface.

3. **Patients choose the appropriate time:**
    - **Description:** The patient selects a convenient time slot for their appointment.
    - **System Interaction:** The system temporarily reserves the chosen slot.
    - **Data Flow:** Selected time slot data is sent to the scheduling module.

4. **The patient confirms and saves the examination schedule:**
    - **Description:** The patient confirms their appointment details and the system finalizes the booking.
    - **System Interaction:** The system updates the schedule and confirms the appointment.
    - **Data Flow:** Appointment details are saved in the database and a confirmation is sent to the patient.

## 4. Work Schedule Allocation (Admin)

**Steps:**
1. **Admin accesses the scheduling interface:**
    - **Description:** The admin navigates to the scheduling section to manage doctors' work schedules.
    - **System Interaction:** The system loads the scheduling interface with relevant data.
    - **Data Flow:** Admin request is processed and scheduling data is retrieved.

2. **Admin chooses doctor and working hours:**
    - **Description:** The admin selects a doctor and assigns their working hours.
    - **System Interaction:** The system validates the schedule for conflicts.
    - **Data Flow:** Selected schedule data is sent to the scheduling module.

3. **The system updates working schedules for doctors:**
    - **Description:** The system updates the database with the new work schedule for the doctor.
    - **System Interaction:** The schedule is saved and conflicts are resolved.
    - **Data Flow:** Updated schedule data is stored in the database.

4. **Work schedules are stored and displayed:**
    - **Description:** The system displays the updated work schedule for the doctors.
    - **System Interaction:** The updated schedule is shown to the admin for verification.
    - **Data Flow:** Stored schedule data is retrieved and displayed.

## 5. Doctor Management (Admin)

**Steps:**
1. **Admin accesses the doctor management interface:**
    - **Description:** The admin navigates to the interface to manage doctor information.
    - **System Interaction:** The system loads the doctor management interface with current data.
    - **Data Flow:** Admin request is processed and doctor data is retrieved.

2. **Admin updates doctor information:**
    - **Description:** The admin edits or adds new information about doctors.
    - **System Interaction:** The system validates the new information.
    - **Data Flow:** Updated doctor data is sent to the doctor management module for validation.

3. **The system updates information in the database:**
    - **Description:** After validation, the system saves the updated information in the database.
    - **System Interaction:** The system confirms the update and stores the new data.
    - **Data Flow:** Updated doctor data is stored in the database.

4. **Doctor information is stored and displayed:**
    - **Description:** The system displays the updated doctor information.
    - **System Interaction:** The updated information is shown to the admin for verification.
    - **Data Flow:** Stored doctor data is retrieved and displayed.

## 6. Medication Management (Admin)

**Steps:**
1. **Admin accesses the medication management interface:**
    - **Description:** The admin navigates to the interface to manage medication information.
    - **System Interaction:** The system loads the medication management interface with current data.
    - **Data Flow:** Admin request is processed and medication data is retrieved.

2. **Admin updates medical information:**
    - **Description:** The admin edits or adds new information about medications.
    - **System Interaction:** The system validates the new information.
    - **Data Flow:** Updated medication data is sent to the medication management module for validation.

3. **The system updates information in the database:**
    - **Description:** After validation, the system saves the updated information in the database.
    - **System Interaction:** The system confirms the update and stores the new data.
    - **Data Flow:** Updated medication data is stored in the database.

4. **Drug information is stored and displayed:**
    - **Description:** The system displays the updated drug information.
    - **System Interaction:** The updated information is shown to the admin for verification.
    - **Data Flow:** Stored drug data is retrieved and displayed.

## 7. Manage Examination Packages (Admin)

**Steps:**
1. **Admin accesses the examination package management interface:**
    - **Description:** The admin navigates to the interface to manage examination packages.
    - **System Interaction:** The system loads the examination package management interface with current data.
    - **Data Flow:** Admin request is processed and examination package data is retrieved.

2. **Admin updates examination package information:**
    - **Description:** The admin edits or adds new information about examination packages.
    - **System Interaction:** The system validates the new information.
    - **Data Flow:** Updated examination package data is sent to the examination package management module for validation.

3. **The system updates information in the database:**
    - **Description:** After validation, the system saves the updated information in the database.
    - **System Interaction:** The system confirms the update and stores the new data.
    - **Data Flow:** Updated examination package data is stored in the database.

4. **Examination package information is stored and displayed:**
    - **Description:** The system displays the updated examination package information.
    - **System Interaction:** The updated information is shown to the admin for verification.
    - **Data Flow:** Stored examination package data is retrieved and displayed.

## 8. Bill Payment (Patient)

**Steps:**
1. **Patient accesses the invoice:**
    - **Description:** The patient navigates to the billing section to view their invoice.
    - **System Interaction:** The system retrieves and displays the patient's invoice.
    - **Data Flow:** Invoice data is retrieved from the database and displayed to the patient.

2. **Patient chooses payment method:**
    - **Description:** The patient selects their preferred payment method (e.g., credit card, PayPal).
    - **System Interaction:** The system processes the payment based on the selected method.
    - **Data Flow:** Payment method data is sent to the payment processing module.

3. **Payment processing system:**
    - **Description:** The system processes the payment and confirms the transaction.
    - **System Interaction:** The system interacts with the payment gateway to complete the transaction.
    - **Data Flow:** Payment details are sent to the payment gateway and confirmation is received.

4. **Invoice is confirmed and stored:**
    - **Description:** The system confirms the payment and updates the invoice status in the database.
    - **System Interaction:** The system stores the confirmed payment details and marks the invoice as paid.
    - **Data Flow:** Payment confirmation is stored in the database and the invoice status is updated.

## 9. Statistics of Medical Examination History (Patient, Doctor, Admin)

**Steps:**
1. **Users access medical history:**
    - **Description:** Users (patients, doctors, admin) navigate to the medical history section.
    - **System Interaction:** The system retrieves the relevant medical history based on user access rights.
    - **Data Flow:** Medical history data is fetched from the database and displayed.

2. **The system displays medical history based on access rights:**
    - **Description:** The system shows the medical history data according to the user's role and permissions.
    - **System Interaction:** Data is filtered and presented based on user access levels.
    - **Data Flow:** Retrieved medical history data is sent to the user interface.

3. **Users view detailed information:**
    - **Description:** Users can view detailed information about past medical examinations.
    - **System Interaction:** The system provides detailed records for the user to review.
    - **Data Flow:** Detailed medical history data is displayed.

4. **The system saves views:**
    - **Description:** The system logs the access and viewing of medical history for auditing purposes.
    - **System Interaction:** The system records the user's access to medical history.
    - **Data Flow:** Viewing logs are stored in the database.

## 10. Prepare Medical Examination Results (Doctor)

**Steps:**
1. **The doctor accesses the interface for preparing examination results:**
    - **Description:** The doctor navigates to the section to input and prepare examination results.
    - **System Interaction:** The system loads the interface for preparing examination results.
    - **Data Flow:** Doctor's request is processed and relevant data is retrieved.

2. **The doctor enters the examination results:**
    - **Description:** The doctor inputs the examination findings and results into the system.
    - **System Interaction:** The system validates and processes the entered results.
    - **Data Flow:** Examination results data is sent to the results preparation module.

3. **The system stores the results in the database:**
    - **Description:** The system saves the examination results in the patient's medical records.
    - **System Interaction:** The system confirms and stores the results.
    - **Data Flow:** Examination results are stored in the database.

4. **The doctor exports the examination results report (export):**
    - **Description:** The doctor exports the results into a report format (e.g., PDF).
    - **System Interaction:** The system generates and exports the report.
    - **Data Flow:** Exported report file is created.

5. **The system sends notifications and examination results to patients via email or text message system:**
    - **Description:** The system sends the examination results to the patient through the chosen communication method.
    - **System Interaction:** Notifications are sent to the patient's email or phone.
    - **Data Flow:** Notification and results data are sent to the communication module.

6. **Patients receive and view examination results:**
    - **Description:** Patients receive the notification and access the examination results.
    - **System Interaction:** The system provides access to the results for the patient to view.
    - **Data Flow:** Examination results are displayed to the patient.

This detailed documentation outlines the step-by-step process and interactions for each function within the CareBlock system, ensuring a comprehensive understanding of the workflow.