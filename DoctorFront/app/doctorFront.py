import streamlit as st
import requests
import json
import pandas as pd

# Functions [HTTP requests]
def createPatient(data):
    print("Registering patient")
    try:
        url = "http://dls-devops-PatientService-1:8081/Patient/Create"

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("POST", url, headers=headers, data=json.dumps(data))
        st.write(response)
        if(response.status_code == 200):
            st.success("Patient registered successfully")
        else:
            st.error("An error occurred while registering the patient")
    except:
        return "Error sending measurement to the server"


def getPatients():
    print("Getting patients")
    try:
        url = "http://dls-devops-PatientService-1:8081/Patient/GetAllPatients"

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("GET", url, headers=headers)
        patients = pd.DataFrame(response.json())
        st.session_state.patients = patients
        if(response.status_code == 200):
            st.success("Patients retrieved successfully")
        else:
            st.error("An error occurred while retrieving the patients")
    except:
        st.error("An error occurred while retrieving the patients")
    
    
def deletePatient(ssn):
    print("Deleting patient")
    try:
        url = "http://dls-devops-PatientService-1:8081/Patient/Delete/" + str(ssn)

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("DELETE", url, headers=headers)
        st.write(response)
        response = deletePatient(delete)
        if(response.status_code == 200 or response.status_code == 204):
            st.success("Patient deleted successfully")
        else:
            st.error("An error occurred while deleting the patient")
        return response
    except:
        return "Error sending measurement to the server"
    
def getMeasurements(ssn):
    print("Getting measurements")
    try:
        url = "http://dls-devops-MeasurementService-1:8082/Measurement/GetAll/" + str(ssn)

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("GET", url, headers=headers)
        if(response.status_code == 200):
            measurements = pd.DataFrame(response.json())
            st.session_state.measurements = measurements
            print(st.session_state.measurements)
            st.success("Measurements retrieved successfully")
        else:
            st.error("An error occurred while retrieving the measurements")
        st.write(response)
        return response
    except:
        return "Error sending measurement to the server"
    
def updateMeasurement(id, data):
    print("Updating measurement")
    try:
        st.write(id)
        url = "http://dls-devops-MeasurementService-1:8082/Measurement/Update/" + str(id)

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("PUT", url, headers=headers, data=json.dumps(data))
        st.write(response)
        if(response.status_code == 200):
            st.success("Measurements updated successfully")
        else:
            st.error("An error occurred while updating the measurements")
        return response
    except:
        return "Error sending measurement to the server"
    
#instantiating data
if 'patients' not in st.session_state:
    response = getPatients()
    

if 'measurements' not in st.session_state:
    st.session_state.measurements = pd.DataFrame()

# Visual elements
st.title("Doctor Frontend")

st.header("Patient Management", divider="rainbow")
          
st.subheader("Register a patient", divider="gray")

name = st.text_input("Enter patient name")

ssn = st.text_input("Enter patient SSN")

mail = st.text_input("Enter patient E-mail")  
    
if st.button("Register patient"):
    measurementDto = {
        "Ssn": str(ssn),  # Convert to string
        "Mail": str(mail),  # Convert to integer
        "Name": str(name)      # Convert to integer
    }
    createPatient(measurementDto)

st.subheader("Get all patients", divider="gray")

if st.button("Get all patients"):
    getPatients()
    
table = st.table(st.session_state.patients)
        
st.subheader("Delete a patient", divider="gray")

delete = st.text_input("Delete a patient by entering the SSN")

if st.button("Delete patient"):
    deletePatient(delete)

st.header("Measurement Management", divider="rainbow")

st.subheader("Get measurements for a patient", divider="gray")

patientSnn = st.text_input("Enter patient SSN to get measurements")
          
if st.button("Get measurements"):
    getMeasurements(patientSnn)
    

if st.session_state.measurements.empty:
    st.write("No measurements found")
else:
    st.subheader("Edit measurements:", divider="gray")
    
    measurements = st.session_state.measurements
    
    selected = st.selectbox("Select a measurement id to edit", measurements["id"])
    
    selected_measurement_row = measurements.loc[measurements["id"] == selected]
    
    # Extract the selected measurement value
    selected_measurement = selected_measurement_row.iloc[0] if not selected_measurement_row.empty else None
    
    st.subheader("Selected measurement:", divider="gray")
    st.write("Date of measurement " + selected_measurement["date"])
    st.write("Diastolic " + str(selected_measurement["diastolic"]))
    st.write("Systolic " + str(selected_measurement["systolic"]))
    
    st.subheader("Update the measurement:", divider="gray")
    newDiastolic = st.text_input("Enter new Diastolic", value=selected_measurement["diastolic"])
    newSystolic = st.text_input("Enter new Systolic", value=selected_measurement["systolic"])
    
    if st.button("Update measurement"):
        updateMeasurement(selected_measurement["id"], {
            "Diastolic": int(newDiastolic),
            "Systolic": int(newSystolic),
            "PatientSsn": (selected_measurement["patientSsn"])
        })
        getMeasurements(selected_measurement["patientSsn"])
    
    

# instantiate data
