#!/usr/bin/env.python
import spidev
import os
import math
import time
import pigpio
import math
import Adafruit_DHT
import json
import paho.mqtt.publish as publicare

MQTT_HOST = 'mqtt.beia-telemetrie.ro'
MQTT_TOPIC = 'odsi/mari-anais'

DHT_SENSOR = Adafruit_DHT.DHT22
DHT_PIN = 4

spi=spidev.SpiDev()
spi.open(0,0)
spi.max_speed_hz=1000000


def ReadInput(channel):
    adc = spi.xfer2([6|(channel&4)>>2, (channel&3)<<6,0])
    data = ((adc[1]&15)<<8)+adc[2]
    return data

def ConvertVolts (data, places):
    volts = (data*3.3)/float(4095)
    volts = round(volts,places)
    return volts

while True:
    #temp = ReadInput(0)
    #print(ReadInput(0)) 
    #print('Temp = ' + str(ConvertVolts(temp,4)))
    
    humidity, temperature = Adafruit_DHT.read_retry(DHT_SENSOR, DHT_PIN)
    light=ConvertVolts(ReadInput(1),4)
    #sound from here
    cnt=0
    cntr=0
    nr_perioade = 0
    contor = 0                          
    t=time.time()
    while time.time()-t < 5:
        cnt=cnt+1
        sound = ReadInput(0)
        if sound < 3500:
            cntr=cntr+1      
            
    if cntr > cnt/1000:
        print ('zgomot')
        sound = 1
         
    else:
        print ('liniste')
        sound = 0
    #to here
    print("Temp={0:0.1f}*C  Humidity={1:0.1f}%".format(temperature, humidity))
    print("Light={0:0.1f} lux".format(light*1000))
    
    payload_dict={"Temperature_in_degrees":temperature,
                  "Humidity_level":humidity,
                  "Light_level":(light*1000),
                  "Sound_detection":sound}
    try:
        publicare.single(MQTT_TOPIC,qos = 1,hostname = MQTT_HOST,payload = json.dumps(payload_dict))
    except:
        time.sleep(0.01)
    time.sleep(2)
    
     
