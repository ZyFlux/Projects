//Has the 'Log' shared resource

//TODO- Get Actor descriptor as prefab from Resources
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualizationHandler
{
    //Some shared data 
    public static Log logInfo;

    public static void Handle (ActorCreated currEvent)
    {
        //TODO: Optimize this by putting it in a prefab
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Actor";
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        go.transform.position = new Vector3(Random.Range(-1.5f, 2.5f), Random.Range(1.25f, 1.9f), Random.Range(-2.0f, 2.0f));
        go.transform.name = currEvent.actorId;
        go.AddComponent<ActorFunctionality>(); //Add the script for actor functionality

        //Add this to the dictionary
        Actors.allActors.Add(currEvent.actorId, go);
    }
    public static void Handle (ActorDestroyed currEvent)
    {
       
        GameObject.Destroy(Actors.allActors[currEvent.actorId]);
        Actors.allActors.Remove(currEvent.actorId);

    }
    public static void Handle(MessageSent currEvent)
    {
        GameObject senderGO = Actors.allActors[currEvent.senderId];
        //Use dictionary of actors to do this
        ActorFunctionality af = senderGO.GetComponent<ActorFunctionality>();
        af.GenerateMessage(Actors.allActors[currEvent.receiverId], currEvent.msg);
      
    }
    public static void Handle(MessageReceived currEvent)
    {
        GameObject recGO = Actors.allActors[currEvent.receiverId];
        //Use dictionary of actors to do this
        ActorFunctionality af = recGO.GetComponent<ActorFunctionality>(); //TODO- Problem here!
        af.ReceiveMessageFromQueue(Actors.allActors[currEvent.senderId]);
    }
    
    public static void Handle(Log currEvent)
    {
        //Maybe also play an error sound?
        //Send message to the main screen to change the text
        logInfo = currEvent;
     }

    public static void Handle(MessageDropped currEvent)
    {
        //Do some animation to show disappearing message
        //Currently, this is visualized like MessageReceived
        ActorFunctionality af = Actors.allActors[currEvent.receiverId].GetComponent<ActorFunctionality>();
        af.ReceiveMessageFromQueue();
    }
}
