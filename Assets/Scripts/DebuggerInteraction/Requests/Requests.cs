using System.Collections.Generic;

[System.Serializable]
public class QueryRequest
{
    public string requestType;

}

public class ReceiveRequest : QueryRequest
{
    int receiverId;
}

public class StateRequest : QueryRequest
{
    int actorId;
}

public class TagActorRequest : QueryRequest
{
    int actorId;
    bool toTag; //True- Tag, False- Untag
}


[System.Serializable]
public class QueryResponse
{
    public string responseType;
}

public class StateResponse : QueryResponse
{
    string state;
}
public class ReceiveResponse : QueryResponse
{
    List<ActorEvent> events;
}

