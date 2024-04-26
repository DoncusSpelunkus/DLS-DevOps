import requests
import streamlit as st
import json

def get_location_from_ip():
    res = requests.request("GET", 'https://ipinfo.io/')
    data = res.json()
    country = data['country']
    return country

country = get_location_from_ip()

inDenmark = False

ssn = st.text_input("Enter your social security number")
diastolic = st.number_input("Enter your Diastolic Blood pressure")
systolic = st.number_input("Enter your Systolic Blood pressure")


if(country == "DK"):
    inDenmark = True
else:
    inDenmark = False

def sendInfo(data):
    try:
        url = "http://dls-devops-MeasurementService-1:8082/Measurement/Create"

        headers = {
            'Content-Type': 'application/json',
            "country": country
        }

        response = requests.request("POST", url, headers=headers, data=json.dumps(data))
        print(response.text)
        return response.text
    except:
        return "Error sending measurement to the server"


if(inDenmark):
    if st.button("Send measurement"):
        measurementDto = {
            "PatientSsn": str(ssn),  # Convert to string
            "diastolic": int(diastolic),  # Convert to integer
            "systolic": int(systolic)      # Convert to integer
        }
        response = sendInfo(measurementDto)
        print("Response:", response)  # Print out the response received from the server
        print("Measurement sent")
else:
    st.write("You are not in Denmark, you cannot send measurements")


