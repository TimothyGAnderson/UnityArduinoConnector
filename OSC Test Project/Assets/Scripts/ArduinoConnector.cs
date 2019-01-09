using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;


/// <summary>
/// C# based connector to an Arduino Board. Usable in Unity, Attach this to a game object and call the open function first, then either read/write or 
/// the asynchronous function. When finished close the stream with the Close() function.
/// </summary>
public class ArduinoConnector : MonoBehaviour
{
    //Serial Port where the Arduino is connected, this changes depending on what COM value the computer assigns the board
    //Change this to function that crawls COM ports and finds the one that returns PONG (or required data), then use that specific COM Port
    public string port = "COM4";
    //Baudrate of the port, this can change depending on the baudrate of the arduino
    public int baudrate = 9600;
    //Time for Write to Arduino to loop (this is for just testing positive writes to the arduino board)
    public int timesToLoop = 1;

    //SerialPort IO stream
    private SerialPort stream;
    

    /// <summary>
    /// Opens a Serial Port, timesout after 50 milliseconds
    /// </summary>
    public void Open()
    {
        //Open Serial Port
        Debug.Log("Open Serial Port");
        stream = new SerialPort(port, baudrate);
        stream.ReadTimeout = 50;
        stream.Open();

    }

    /// <summary>
    /// writes to arduino boards, then flushes the stream
    /// </summary>
    /// <param name="message">The Message to be sent</param>
    public void WriteToArduino(string message)
    {
        //Send a Read Request
        Debug.Log("Write to Arduino: " + message);
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    /// <summary>
    /// Reads output from the Arduino boards and returns it as a string that can then be parsedin Unity. 
    /// </summary>
    /// <param name="timeout">If there is nothing to be read from the Arduino Board then timeout the function call.</param>
    /// <returns>Data from the Arduino board as a string format</returns>
    public string ReadFromArduino(int timeout = 10)
    {
        stream.ReadTimeout = timeout;
        try
        {
            Debug.Log(stream.ReadLine());
            return stream.ReadLine();

        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    /// <summary>
    /// Asynchronous Read from Arduino board function, By reading from the arduino board using this Enumerable coroutine, then we can avoid 
    /// lots of system lag
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="fail"></param>
    /// <param name="timeout"></param>
    /// <returns>Returns an enumerable set of data returned from the Arduino board</returns>
    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            //Single Read Attempt
            try
            {
                
                dataString = stream.ReadLine();
                

            }
            catch (TimeoutException)
            {
                dataString = null;

            }
            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);

            }

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;
        }
        while (diff.Milliseconds < timeout);

        if (fail != null)
        {
            fail();
        }
        yield return null;

    }


    /// <summary>
    /// Closes the IO Stream
    /// </summary>
    public void Close()
    {
        stream.Close();
    }
    /// <summary>
    /// Standard Unity start function
    /// </summary>
    public void Start()
    {
        Open();

        for (int i = 0; i < timesToLoop; i++)
        {
            WriteToArduino("PING");
        }
        //ReadFromArduino();
        
        StartCoroutine(AsynchronousReadFromArduino((string s) => Debug.Log(s), () => Debug.LogError("Error"), 10000f));
        //Close();
    }
}


