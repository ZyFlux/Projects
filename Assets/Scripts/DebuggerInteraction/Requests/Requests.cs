//These are sent by the program to us

using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QueryRequest
{
    public string requestType;

}

public class ActionRequest : QueryRequest
{
    public string receiverId;
    public string actionType;


    public ActionRequest(string action, string actorInvolved)
    {
        if (action != null) //Pass the actionId
            actionType = action;
        else
            actionType = "";
        receiverId = actorInvolved;
        requestType = "ACTION_REQUEST";
    }

}

public class StateRequest : QueryRequest
{
    public string actorId;
    public bool toGet; //Is it toggle on or toggle off?
    public StateRequest (string id, bool onOff)
    {
        requestType = "STATE_REQUEST";
        actorId = id;
        toGet = onOff;
    }
}

public class TagActorRequest : QueryRequest
{
    public string actorId;
    public bool toTag; //True- Tag, False- Untag
    public TagActorRequest (string id, bool flag)
    {
        requestType = "TAGACTOR_REQUEST";
        actorId = id;
        toTag = flag;
    }
}


//New architecture
[System.Serializable]
public class State
{
    public string actorId;
    public Color behavior;
    public string vars; //All variables separated by new line
}


[System.Serializable]
public class QueryResponse
{
    public string responseType;
    public virtual void HandleThis()
    { Debug.LogError("Control has passed to the base QueryResponse class"); }
}

[System.Serializable]
public class ActionResponse : QueryResponse
{
    public List<string> events;
    public List<State> states;
     
    public override void HandleThis()
    { Debug.Log("Receive Response class"); }
}
