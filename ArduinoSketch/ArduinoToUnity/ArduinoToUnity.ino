//This Script is designed to communicate with Unity via Serial communications over USB.
#include <SerialCommand.h>
SerialCommand sCmd;

//These variables are created for use in the main loop, 
//to demonstrate the way data can be collected and sent over Serial.
int milliseconds = 0;
int randomdata = 0;
String pos = "";


//This block of code runs once when the Arduino turns on and is 
//necessary in order for the Serial Communications to work properly.
//You can add or change messages the Arduino is listening for using
//the sCmd.addCommand("MessageArduinoIsListeningFor", <EventHandler>) 
//functionality demonstrated below. Take note that the handlers 
//(The thing after the commas) are created and programmed further below.
void setup() {
  Serial.begin(115200);
  while(!Serial);
  sCmd.addCommand("PING", pingHandler);
  sCmd.addCommand("ECHO", echoHandler);
  sCmd.addCommand("SEND", sendHandler);
  sCmd.addCommand("SENDALL", sendallHandler);
  sCmd.addDefaultHandler(unrecognized);
}

//This block of code runs forever, repeating again when it reaches the end.
//Most of this block is making up values to send over Serial; demonstrating
//how the script works. The sCmd.readSerial(); command is necessary however.
void loop() {
    sCmd.readSerial();

    //RELEVANT TO DEMO ONLY
    milliseconds = millis();
    randomdata = random(0,1000);
    pos = String(random(0,1000)) + "," + String(random(0,1000)) + "," + String(random(0,1000));
}

//This is a basic handler, in the case the Arduino gets a PING message sent to
//it over Serial. It returns "PING PONG", in response.
void pingHandler(){
  const char *command = sCmd.next();
  Serial.println("PING PONG");
}

//This is a slightly more detailed handler, in the case the Arduino gets an 
//ECHO message sent to it over Serial. It returns the message after the ECHO
//in response.
void echoHandler()
{
  char *arg;
  arg = sCmd.next();
  if(arg!=NULL)
  {
    Serial.println("ECHO " + String(arg));
    
  }else{
    Serial.println("Nothing To Echo"); 
  }
}

//This handler demonstrates meaningful data transfer. If the arduino
//gets a message in the form of SEND MILLIS/SECONDS/RANDOM/POS,
//the arduino will send the relevant information.
void sendHandler(){
  char *arg;
  arg = sCmd.next();
  
  if(arg!=NULL)
  {
    String str(arg);
    //Serial.println(str);
    if(str == "MILLIS")
    {
      Serial.println("Milliseconds: " + String(milliseconds));
    } else if(str == "SECONDS")
    {
      Serial.println("Seconds: " + String(milliseconds * 0.001));
    } else if(str == "RANDOM")
    {
      Serial.println("RandomData: " + String(randomdata));
    }
    else if(str == "POS")
    {
      Serial.println("Position: " + pos);
    }
  }
}

//This handler demonstrates data transfer as well. If the arduino
//gets the message SENDALL, it sends all the information it can.
void sendallHandler(){
  const char *command = sCmd.next();
  Serial.println("Milliseconds: " + String(milliseconds));
  Serial.println("Seconds: " + String(milliseconds * 0.001));
  Serial.println("RandomData: " + String(randomdata));
  Serial.println("Position: " + pos);
}

// This gets set as the default handler, and gets called when no other command matches.
void unrecognized(const char *command) {
  Serial.println("Unrecognized Command");
}
