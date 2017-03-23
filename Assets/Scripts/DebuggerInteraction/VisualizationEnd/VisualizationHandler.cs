//TODO- Get Actor descriptor as prefab from Resources
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualizationHandler
{
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
        //Use dictionary of actors to do this
        ActorFunctionality af = Actors.allActors[currEvent.senderId].GetComponent<ActorFunctionality>();
        af.GenerateMessage(Actors.allActors[currEvent.receiverId]);
      
    }
    public static void Handle(MessageReceived currEvent)
    {
        //Use dictionary of actors to do this
        ActorFunctionality af = Actors.allActors[currEvent.receiverId].GetComponent<ActorFunctionality>();
        af.ReceiveMessageFromQueue(Actors.allActors[currEvent.senderId]);
    }
    
    public static void Handle(Log currEvent)
    {
        //Send message to the light to change colour for error
        //Maybe also play an error sound?
        //Send message to the main screen to change the text
    }
}
