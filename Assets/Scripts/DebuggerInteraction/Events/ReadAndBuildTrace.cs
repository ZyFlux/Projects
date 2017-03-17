//Now defunct because of network programming
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadAndBuildTrace : MonoBehaviour {
    public string jsonTraceFile; //file being read

	void Awake ()
    {
        //Initialization for the main program
        ActorCreated deadLetters = new ActorCreated("Actor[akka://sys/deadLetters]");
        Trace.allEvents.Add(deadLetters);

        string line; //Stores a line in the file
        int lineNumber = 1; //Functions as key to the dictionary
        // Read the file
        System.IO.StreamReader file = new System.IO.StreamReader(jsonTraceFile);
        while ((line = file.ReadLine()) != null)
        {

            ActorEvent ev = (JsonUtility.FromJson<ActorEvent>(line)); //Read into the event parent class
        
            //Now assign a specific class
            switch (ev.eventType)
            {
                case "ACTOR_CREATED":
                    ActorCreated specificActorCreatedEvent = (JsonUtility.FromJson<ActorCreated>(line));
                    Trace.allEvents.Add(specificActorCreatedEvent);
                    break;
                case "MESSAGE_SENT":
                    MessageSent specificMessageSentEvent = (JsonUtility.FromJson<MessageSent>(line));
                    Trace.allEvents.Add(specificMessageSentEvent);
                    break;
                case "MESSAGE_RECEIVED":
                    MessageReceived specificMessageReceivedEvent = (JsonUtility.FromJson<MessageReceived>(line));
                    Trace.allEvents.Add(specificMessageReceivedEvent);
                    break;
                case "ACTOR_DESTROYED":
                    ActorDestroyed specificActorDestroyedEvent = (JsonUtility.FromJson<ActorDestroyed>(line));
                    Trace.allEvents.Add(specificActorDestroyedEvent);
                    break;
                default :
                    //We don't know what this event is
                    Debug.LogError("Unknown event encountered in JSON");
                    break;
            }
            lineNumber++; //Increment lineNumber

        }
        Debug.Log("Trace built with " + Trace.allEvents.Count.ToString() + " events :)");
        file.Close();
    }

}
