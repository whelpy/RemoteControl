// NodeMCU_Nokia_5510_Hello_nn
// microprocessor board: Lolin ESP8266 NodeMCU 
// display Nokia 5510
// prints "hello World! to screen
// January 13, 2021
// Floris Wouterlood
// public domain

#include <SPI.h>
#include <Adafruit_GFX.h>
#include <Adafruit_PCD8544.h>
#include <ArduinoJson.h>

#define CLK D4
#define DIN D3
#define DC  D2
#define CE  D1
#define RST D0

Adafruit_PCD8544 display = Adafruit_PCD8544 (CLK,DIN,DC,CE,RST);

#define LF          0x0A 
char json[255];
int idx;

void setup()   {
 
    Serial.begin (9600);
    Serial.println ();
    Serial.println ();
    Serial.println ("Hello World!");
    Serial.println ("on NodeMCU ESP8266 and Nokia 5510");
    Serial.println ();
    
    display.begin ();
    display.setContrast (60);                        
    display.clearDisplay ();                     
    display.setRotation (2);  
    display.setTextSize (1);
    display.setTextColor (BLACK);

    display.setCursor (0,0);
    display.println ("Remote Control");
    //display.setCursor (10,20);
    display.println ("connecting...");
    display.display ();       
}


void loop() 
{
    if (Serial.available() > 0) 
    {
      display.clearDisplay();
      
      json[idx] = Serial.read();
      if (json[idx] == LF) 
      {
        //Serial.print("Received new angle: ");
        json[idx-1] = 0;
        //Serial.println(json);

        DynamicJsonDocument doc(1024);
        deserializeJson(doc, json);

        // "{\"CpuLoad\":12,\"GpuLoad\":0,\"GpuFPS\":-1,\"MemLoad\":0}"

        int cpuLoad = doc["CpuLoad"];
        int gpuLoad = doc["GpuLoad"];
        int gpuFPS = doc["GpuFPS"];
        int memLoad = doc["MemLoad"];

        display.println ("CPU:" + String(cpuLoad) + " GPU:" + String(cpuLoad));
        display.println ("FPS:" + String(gpuFPS) + " MEM:" + String(memLoad));

        display.display ();      
        
        idx = -1;
      }
      idx++;
    }
}
