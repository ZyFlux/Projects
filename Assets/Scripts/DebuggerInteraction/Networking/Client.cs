﻿//Initialized by init
//The __INIT__ message to the server is also sent from here


//Inspired from https://msdn.microsoft.com/en-us/library/bew39x2a(v=vs.80).aspx (minor changes)
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

// State object for receiving data from remote device.
public class StateObject
{
    // Client socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 8000; //Currently, our size is 8000 bytes, as we don't check for a complete read
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public static class AsynchronousClient
{
    // The port number for the remote device.
    private const int port = 2772;

    // ManualResetEvent instances signal completion.
    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private static ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    // The response from the remote device.
    private static String response = String.Empty;
    
    public static Socket client;
    public static void StartClient()
    {
        // Connect to a remote device.
        try
        {
            // Establish the remote endpoint for the socket.
            IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(/*"139.19.183.9"*/"127.0.0.1"), port);

            // Create a TCP/IP socket.
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //We create a socket here


            // Connect to the remote endpoint.
            client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), client);
            
            
            connectDone.WaitOne();
            //WaitOne causes the block

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public static void SendInitMessage()
    {
        //Send an initial query request
        ActionRequest initialRequest = new ActionRequest("__INIT__", "");
        string initialRequestString = JsonUtility.ToJson(initialRequest);


        // Send test data to the remote device.
        Send(client, initialRequestString);
        Debug.Log("We sent " + initialRequestString);
    } 

    public static void FreeSocket()
    {
        // Release the socket.
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }


    }

    public static void Receive(Socket client)
    {
        try
        {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
       
        try
        {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                /*
                // Get the rest of the data.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            
            else
            {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                    
                }
                // Signal that all bytes have been received.
                receiveDone.Set();
                
            }*/
                NetworkInterface.HandleResponseReceived(state.sb.ToString()); //Perform appropriate action on response received
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        // Signal that all bytes have been received.


    }

    public static void Send(Socket client, String data)
    {
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = client.EndSend(ar);
            //Debug.Log("Sent  " + bytesSent.ToString() + " bytes to server." );

            // Signal that all bytes have been sent.
            sendDone.Set();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        Receive(client); //After each send, we receive something
    }

}