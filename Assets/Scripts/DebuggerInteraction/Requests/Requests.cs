using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QueryRequest
{
    public string requestType;

}

public class ReceiveRequest : QueryRequest
{
    string receiverId;
    public ReceiveRequest(string actorToReceive)
    {
        receiverId = actorToReceive;
        requestType = "RECEIVE_REQUEST";
    }

}

public class StateRequest : QueryRequest
{
    string actorId;
    public StateRequest (string id)
    {
        requestType = "STATE_REQUEST";
        actorId = id;
    }
}

public class TagActorRequest : QueryRequest
{
    string actorId;
    bool toTag; //True- Tag, False- Untag
    public TagActorRequest (string id, bool flag)
    {
        requestType = "TAGACTOR_REQUEST";
        actorId = id;
        toTag = flag;
    }
}


[System.Serializable]
public class QueryResponse
{
    public string responseType;
    public virtual void HandleThis()
    { Debug.LogError("Control has passed to the base QueryResponse class"); }
}

public class StateResponse : QueryResponse
{
    public string state;
    public override void HandleThis()
    { Debug.Log("State Response class"); }
}
public class ReceiveResponse : QueryResponse
{
    public List<string> eventJsons;
    public override void HandleThis()
    { Debug.Log("Receive Response class"); }
}

