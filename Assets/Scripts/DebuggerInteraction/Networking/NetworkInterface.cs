//Contains interfaces called on by the network connection
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkInterface
{
	
    public static void HandleResponseReceived(string jsonResponse)
    {
        QueryResponse currResponse = (JsonUtility.FromJson<QueryResponse>(jsonResponse));
        switch (currResponse.responseType)
        {
            case "STATE_RESPONSE":
                //Send message to the concerned actor
                StateResponse curr = (JsonUtility.FromJson<StateResponse>(jsonResponse));
                GameObject go = Actors.allActors[curr.actorId];
                Debug.Log("Sending a message to " + curr.actorId + " to change colour to " + curr.state);
                SendMessageContext context = new SendMessageContext(go, "ChangeColour", curr.state, SendMessageOptions.RequireReceiver);
                SendMessageHelper.RegisterSendMessage(context);
                break;
            case "RECEIVE_RESPONSE":
                ReceiveResponse currReceive = (JsonUtility.FromJson<ReceiveResponse>(jsonResponse));
                foreach (string ev in currReceive.events)
                    EventUnwrapper(ev);
                break;
            default:
                Debug.LogError("Unable to response to a particular class");
                break;
        }

    }

    private static void EventUnwrapper(string line)
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
            default:
                //We don't know what this event is
                Debug.LogError("Unknown event encountered in JSON");
                break;
        }
    }

    public static void HandleTagUntagRequestToBeSent()
    {

    }

    public static void HandleRequest(StateRequest curr)
    {
        string toSend = JsonUtility.ToJson(curr);
        AsynchronousClient.Send(AsynchronousClient.client, toSend);
    }

    public static void HandleRequest(ReceiveRequest curr)
    {
        string toSend = JsonUtility.ToJson(curr);
        AsynchronousClient.Send(AsynchronousClient.client, toSend);
    }
}

