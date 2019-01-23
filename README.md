# UnityArduinoConnector
This GitHub Repository has been developed to facilitate communication between an Arduino Board and a Unity Program. The communication is run through the computers Serial Ports. 

This Repository consists of several folders. 

# ArduinoSketch
The ArduinoSketch folder contains the Arduino Sketch file that drives the Arduino Boards Reading and Writing from/to the Unity Program. 

ArduinoToUnity has one dependency:
<SerialCommand.h> - This dependency enables communications over serial ports. 

Global Variables:
sCmd - Is of type SerialCommand, this variable parses and tokenises commands recieved over a serial port. 
milliseconds - An integer value that is used in the main loop, its purpose is to aid in the demonstration of how data can be sent and recieved over serial ports. 
randomData - An integer Value, again to demonstrate of data can be sent Recieved and Manipulated.
pos - A string that gets parsed to determine to position of a tree for Unity. 

Functions:

void Setup(): This code is run once when the Arduino turns on. This is required for the Serial Communications to function correctly. 
Messages can be added or modified using the sCmd.addCommand("messageToListenFor", EventHandler) call. Handlers must be implemented in this sketch. 

void Loop(): This function loops continuously, This function generates demo values to show how the data can be sent across serial communications. The sCmd.readSerial() call is needed.

void pingHandler(): This is an example of an event Handler. This event Handler tells the Arduino Board to reply with a "PING PONG" message, after recieving a "PING" message from the PC. 

void echoHandler(): A more complicated event handler, The arduino receives an ECHO message over the serial connection. THe Arduino returns the message after the ECHO. Otherwise returns a "Nothing to Echo" message.

void sendHandler(): An event handler which highlights meaningful data transfer. If the Arduino gets a message in the form of SEND/MILLIS/SECONDS/RANDOM/POS, then the Arduino will send the required information. 

void sendallHandler(): This event handler also transfers data, but sends all available data upon request. 

void unrecognized(): Error handler, returns "Unrecognized Command" if there are no commands that match.

# OSC Test Project
This Unity Project is a test project for the reading and writing of data to/from an Arduino Board. There are two main C# scripts for this project. The ArduinoConnector, and the Arduino Data. 

  # Arduino Connector
  Is a C# script to connect, send and receive data from an Arduino Board. To use this script, Attach to an empty game object. The   start() function calls the Open() function first to open a connection to the Arduino board. Then call any functions needed by your game/project, finally close() the connection. 
  
  Global Variables:
  port: String that represents the Serial Port that the Arduino is connected to. 
  
  baudrate: The rate at which information can be transferred on the port. bits/second. Represented as an integer value.
  
  timesToLoop: Number of times to loop (has just been used to for testing positive writes). 
  
  stream: The serial ports IO data stream. Is of type SerialPort.
  
  d: Varaible containing Arduino Data. Is of type ArduinoData.
  
  Functions: 
  void Open(): Opens the Serial Port Connection, timesout after 50 milliseconds. This function iterates over the Serial Ports and gets the open ones. This enables multiple Arduino Boards to be connected to the PC and communicated with. 
  
  void WriteToArduino(string message): Writes to the Arduino Board, then flushes the IO stream. Writes the contents of the message to the arduino board. 
  
  string ReadFromArduino(int timeout = 10): Reads output from the Arduino Board, returns it as a string, which can then be parsed an manipulated as needed. If nothing is read by the script then the connection timesout after the 100ms. 
  
  IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity):
  This function asynchronously reads from the arduino board. By utilising this rather than the previously implemented ReadFromArduino() function we cal reduce large amounts of system lag, if we require rapid calls. 
  
  Close(): Closes the IO stream. 
  
  Start(): Standard unity Start function. This calls the Open() function, Starts the Coroutine for Async.Reading from Arduino, then Writes two messages to the Arduino board for troubleshooting. 
  
  Update(): Standard Unity Update function, is called every frame and checks for changes. This update function also contains basic testing keyboard commands. If the 'space' key is pressed, then the command SEND MILLIS is sent to the arduino board and the Arduino will reply. If 'F' is pressed then the ECHO command is issued, 'D' issues the SENDALL command. 'T' issues the SENDPOS command. 
  
  OnApplicationQuit(): Upon quitting the application, the IO Stream closes. 
  
  # ArduinoData
  Parses and uses Arduino Data, in the test project this data is used to populate the scene with tree prefabs based off of the position data. 
  
  List<string> incoming: A list of strings that constitute the response from the Arduino Board. 
  GameObject tree: An empty gameobject that will be used to populate the scene with trees.
  
  ConvertToInt(): Converts characters in the list to integers.
  
  ConvertToVector3(): Converts strings into a List of Vector3 Coordinates. Used in the placement of trees. 
