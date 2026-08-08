# eireneysabelkarunungan-lgtm-IT_ELECTIVE_2_MIDTERM_EXAM_1_KarununganEirene
# Vehicle Service Monitoring System

An ASP.NET Core MVC (.NET 8) lab exercise solution. Service advisors register,
log in, and monitor customer vehicles from check-in until release.


Front desk staff can:
- Create an account and log in
- Register a vehicle when a customer brings it in for service
- View a list of all vehicles currently being serviced
- Search for a specific vehicle by service number, customer name, 
  plate number, or vehicle model
- Edit a service job's details or update its status
- Release a vehicle once the service is done, which records the 
  release date and time


## Authentication

The app uses Cookie Authentication to manage login sessions. Once a 
user logs in, a secure cookie is issued so they don't need to log in 
again on every page. Pages related to vehicle service tracking are 
protected and can only be accessed by logged-in users.


## TECHNOLOGIES USED
- ASP.NET Core MVC (.NET 8)
- C#
- Razor Views (.cshtml)
- Bootstrap for styling
- Data Annotation Validation
- Repository Pattern (in-memory storage)

## RUN the System

* **1. Download ZIP:** In the GitHub repo, click "Code" and choose **"Download ZIP"**.
* **2. Extract All:** Select the downloaded .zip file and extract.
* **3. Find file location:** Open the folder then open **VehileServiceMonitoringSystem**.
* **4. Open POSApp:** Find **VehileServiceMonitoringSystem.slnx** and open with Visual Studio.
* **5. Run:** Click **Run (http)** in Visual studio or press **F5**.
* **6. Open in web:** If the localhost doesn't open automatically, take note of the localhost port in the console and open it in your preferred browser.
