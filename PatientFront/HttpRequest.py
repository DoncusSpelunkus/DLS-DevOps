import requests
import json


def send_measurement(data):
    try:
        url = "http://measurement:8082/measurement/create"

        headers = {
            'Content-Type': 'application/json'
        }

        response = requests.request("POST", url, headers=headers, data=json.dumps(data))
        return response.text
    except:
        return "Error sending measurement to the server"
    