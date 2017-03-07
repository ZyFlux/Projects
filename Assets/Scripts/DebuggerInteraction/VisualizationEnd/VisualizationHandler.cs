using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualizationHandler
{
    public static void Handle (ActorCreated currEvent)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        go.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(1.75f, 2.0f), Random.Range(-2.0f, 2.0f));
        go.transform.name = currEvent.actorId;
        go.AddComponent<ActorFunctionality>(); //Add the script for actor functionality
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = 100.0f;
        rb.drag = 5.0f;
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

}
