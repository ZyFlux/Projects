//Contains interfaces called on by the network connection
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkInterface
{
	
    public static void HandleResponseReceived(string jsonResponse)
    {
        Debug.Log("Debugger sent us " + jsonResponse);
        QueryResponse currResponse = (JsonUtility.FromJson<QueryResponse>(jsonResponse));
        switch (currResponse.responseType)
        {
            case "ACTION_RESPONSE":
                //Debug.Log("Received an action response");
                ActionResponse curr = (JsonUtility.FromJson<ActionResponse>(jsonResponse));
                //One ActionResponse includes a list of events for one atomic step
                List<ActorEvent> tempList = new List<ActorEvent>();
                foreach (string ev in curr.events)
                    tempList.Add(EventUnwrapper(ev));
                if(tempList.Count > 0)
                    Trace.allEvents.Add(tempList);

                foreach (State st in curr.states)
                    StateUnwrapper(st);
               
                break;

            case "TOPOGRAPHY_RESPONSE":
                TopographyResponse tr = (JsonUtility.FromJson<TopographyResponse>(jsonResponse));
                //One TopographyResponse includes the type and ordered list of actors, which need to be unwrapped
                TopographyUnwrapper(tr);
                break;
            default:
                Debug.LogError("Unable to resolve to a particular class");
                break;
        }

    }

    private static ActorEvent EventUnwrapper(string line)
    {
        ActorEvent ev = (JsonUtility.FromJson<ActorEvent>(line)); //Read into the event parent class
        //Now assign a specific class
        switch (ev.eventType) //Go through all possible event types
        {
            case "ACTOR_CREATED":
                ActorCreated specificActorCreatedEvent = (JsonUtility.FromJson<ActorCreated>(line));
                ev = specificActorCreatedEvent;
                break;
            case "MESSAGE_SENT":
                MessageSent specificMessageSentEvent = (JsonUtility.FromJson<MessageSent>(line));
                ev = specificMessageSentEvent;
                break;
            case "MESSAGE_RECEIVED":
                MessageReceived specificMessageReceivedEvent = (JsonUtility.FromJson<MessageReceived>(line));
                ev = specificMessageReceivedEvent;
                break;
            case "ACTOR_DESTROYED":
                ActorDestroyed specificActorDestroyedEvent = (JsonUtility.FromJson<ActorDestroyed>(line));
                ev = specificActorDestroyedEvent;
                break;
            case "LOG":
                Debug.Log("Log received");
                Log newLog = (JsonUtility.FromJson<Log>(line));
                ev = newLog;
                break;
            case "MESSAGE_DROPPED":
                MessageDropped specificMessageDroppedEvent = (JsonUtility.FromJson<MessageDropped>(line));
                ev = specificMessageDroppedEvent;
                break;
            default:
                //We don't know what this event is
                Debug.LogError("Unknown event encountered in JSON");
                break;
        }
        return ev;
    }
    private static void StateUnwrapper(State st)
    {
        //Send the state to Actor
        GameObject actorConcerned = Actors.allActors[st.actorId];
        //Make the send message threadsafe
        SendMessageContext context = new SendMessageContext(actorConcerned, "NewStateReceived", st, SendMessageOptions.RequireReceiver);
        SendMessageHelper.RegisterSendMessage(context);
    }


    private static void TopographyUnwrapper(TopographyResponse tr)
    {

        VisualizationHandler.Handle(tr);
    }

    public static void HandleTagUntagRequestToBeSent(bool toggle, string actorId)
    {
        TagActorRequest curr = new TagActorRequest(actorId, toggle);
        string toSend = JsonUtility.ToJson(curr);
        AsynchronousClient.Send(AsynchronousClient.client, toSend);
        Debug.Log("We sent a tag actor request");
    }

    public static void HandleRequest(StateRequest curr)
    {
        string toSend = JsonUtility.ToJson(curr);
        AsynchronousClient.Send(AsynchronousClient.client, toSend);
    }

    public static void HandleRequest(ActionRequest curr)
    {
        string toSend = JsonUtility.ToJson(curr);
        AsynchronousClient.Send(AsynchronousClient.client, toSend);
    }
    public static void HandleRequest(TopographyRequest curr)
    {
        string toSend = JsonUtility.ToJson(curr);
        AsynchronousClient.Send(AsynchronousClient.client, toSend);
    }

}

