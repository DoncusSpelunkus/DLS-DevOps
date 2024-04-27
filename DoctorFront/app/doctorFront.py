import streamlit as st
import requests
import json

patients = []

# Functions
def sendInfo(data):
    print("Registering patient")
    try:
        url = "http://dls-devops-PatientService-1:8081/Patient/Create"

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("POST", url, headers=headers, data=json.dumps(data))
        st.write(response)
        return response
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
        st.write(response)
        return response
    except:
        return "Error sending measurement to the server"
    
    
def deletePatient(ssn):
    print("Deleting patient")
    try:
        url = "http://dls-devops-PatientService-1:8081/Patient/Delete/" + str(ssn)

        headers = {
            'Content-Type': 'application/json',
        }
        response = requests.request("DELETE", url, headers=headers)
        st.write(response)
        return response
    except:
        return "Error sending measurement to the server"
    
    
# Visual elements
name = st.text_input("Enter patient name")

ssn = st.text_input("Enter patient SSN")

mail = st.text_input("Enter patient E-mail")  
    
if st.button("Register patient"):
    measurementDto = {
        "Ssn": str(ssn),  # Convert to string
        "Mail": str(mail),  # Convert to integer
        "Name": str(name)      # Convert to integer
    }
    response = sendInfo(measurementDto)
    if(response.status_code == 200):
        print(response.text)
        st.success("Measurement sent successfully")
    else:
        print(response.text)
        st.error("An error occurred while sending the measurement")


if st.button("Get all patients"):
    response = getPatients()
    if(response.status_code == 200):
        print(response.text)
        patients = response.json()
        st.success("Patients retrieved successfully")
    else:
        print(response.text)
        st.error("An error occurred while retrieving the patients")
        

delete = st.text_input("Delete a patient by entering the SSN")



if st.button("Delete patient"):
    response = deletePatient(delete)
    if(response.status_code == 200 or response.status_code == 204):
        print(response.text)
        st.success("Patient deleted successfully")
    else:
        print(response.text)
        st.error("An error occurred while deleting the patient")


table = st.table(patients)

getPatients()