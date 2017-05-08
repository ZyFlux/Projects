//Has the 'Log' shared resource

//TODO- Get Actor descriptor as prefab from Resources
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationHandler : MonoBehaviour
{
    public static bool logCreateForEvent = true;
    public static float outlineTime = 1.0f;
    public static void Handle (ActorCreated currEvent)
    {
        //TODO: Optimize this by putting it in a prefab
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Actor";
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        go.transform.position = new Vector3(Random.Range(0.5f, 3.5f), Random.Range(1.25f, 1.9f), Random.Range(-1.0f, 1.0f));
        go.transform.name = currEvent.actorId;
        go.transform.parent = TraceImplement.rootOfActors.transform;
        go.AddComponent<ActorFunctionality>(); //Add the script for actor functionality

        //Add this to the dictionary
        Actors.allActors.Add(currEvent.actorId, go);
        if (logCreateForEvent)
        {
            //Create a Log of it
            Log newLog = new Log(0, "Actor created : " + currEvent.actorId);
            Handle(newLog);
        }
        Actors.allActors[currEvent.actorId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.magenta, outlineTime/2);
    }
    public static void Handle (ActorDestroyed currEvent)
    {
       
        GameObject.Destroy(Actors.allActors[currEvent.actorId]);
        Actors.allActors.Remove(currEvent.actorId);

        if (logCreateForEvent)
        {
            //Create a Log of it
            Log newLog = new Log(0, "Actor destroyed : " + currEvent.actorId);
            Handle(newLog);
        }
    }
    public static void Handle(MessageSent currEvent)
    {
        GameObject senderGO = Actors.allActors[currEvent.senderId];
        //Use dictionary of actors to do this
        ActorFunctionality af = senderGO.GetComponent<ActorFunctionality>();
        af.GenerateMessage(Actors.allActors[currEvent.receiverId], currEvent.msg);

        if (logCreateForEvent)
        {
            //Create a Log of it
            Log newLog = new Log(0, "Message sent : " + currEvent.senderId + " to " + currEvent.receiverId);
            Handle(newLog);
        }

    }
    public static void Handle(MessageReceived currEvent)
    {
        GameObject recGO = Actors.allActors[currEvent.receiverId];
        //Use dictionary of actors to do this
        ActorFunctionality af = recGO.GetComponent<ActorFunctionality>(); //TODO- Problem here!
        af.ReceiveMessageFromQueue(Actors.allActors[currEvent.senderId]);

        if (logCreateForEvent)
        {
            //Create a Log of it
            Log newLog = new Log(0, "Message received : " + currEvent.receiverId);
            Handle(newLog);
        }
    }
    
    public static void Handle(Log currEvent)
    {
        //Maybe also play an error sound?
        //Send message to the main screen to change the text
        LogDisplayer.NewLog(currEvent);
     }

    public static void Handle(MessageDropped currEvent)
    {
        //Do some animation to show disappearing message
        //Currently, this is visualized like MessageReceived
        ActorFunctionality af = Actors.allActors[currEvent.receiverId].GetComponent<ActorFunctionality>();
        af.ReceiveMessageFromQueue();

        if (logCreateForEvent)
        {
            //Create a Log of it
            Log newLog = new Log(0, "Message dropped : " + currEvent.receiverId);
            Handle(newLog);
        }
    }


    //-------------------Outlining functions
    public static void Outline(ActorCreated currEvent)
    {
        //Currently called from the Handler function
    }
    public static void Outline(ActorDestroyed currEvent)
    {
        Actors.allActors[currEvent.actorId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.red, outlineTime);

    }
    public static void Outline(MessageSent currEvent)
    {
        Actors.allActors[currEvent.receiverId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.magenta, outlineTime);
        Actors.allActors[currEvent.senderId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.magenta, outlineTime);
    }
    public static void Outline(MessageReceived currEvent)
    {
        Actors.allActors[currEvent.receiverId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.magenta, outlineTime);
        Actors.allActors[currEvent.senderId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.magenta, outlineTime);
    }

    public static void Outline(Log currEvent)
    {
        //Undefined what really needs to be done
    }

    public static void Outline(MessageDropped currEvent)
    {
        Actors.allActors[currEvent.receiverId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.red, outlineTime);
        Actors.allActors[currEvent.senderId].GetComponent<ActorFunctionality>().MomentaryOutline(Color.red, outlineTime);
    }

}
