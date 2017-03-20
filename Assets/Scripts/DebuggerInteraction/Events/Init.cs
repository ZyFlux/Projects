//Initializes the visualization and starts the TCP chain

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {
    public bool isActive;
	// Use this for initialization
	void Awake () {
        if (isActive)
        {
            ActorCreated deadLetters = new ActorCreated("Actor[akka://sys/deadLetters]");
            Trace.allEvents.Add(deadLetters);

            Debug.Log("About to start the client..");
            AsynchronousClient.StartClient();
        }
    }

    private void OnApplicationQuit()
    {
        AsynchronousClient.FreeSocket();
    }


}
