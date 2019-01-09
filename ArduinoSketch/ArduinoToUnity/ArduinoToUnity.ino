

#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand sCmd;

int ledPin = 10;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  while(!Serial);
  sCmd.addCommand("PING", pingHandler);
  sCmd.addCommand("ECHO", echoHandler);
  //sCmd.setDefaultHandler(errorHandler);
  pinMode(ledPin, OUTPUT);  
  
}

void loop() {
  // put your main code here, to run repeatedly:
  
  
  if(Serial.available() >0){
    sCmd.readSerial();
    delay(50);
  }
}

void pingHandler(){
  const char *command = sCmd.next();
  Serial.println("PONG");
  digitalWrite(ledPin,HIGH);
  delay(100);
  digitalWrite(ledPin,LOW);
  delay(50);
}

void echoHandler()
{
  char *arg;
  arg = sCmd.next();
  if(arg!=NULL)
  {
    Serial.println(arg);
    
  }else{
    Serial.println("Nothing To Echo"); 
  }
}
