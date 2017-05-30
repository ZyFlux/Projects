using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QueryResponse
{
    public string responseType;
    public virtual void HandleThis()
    { Debug.LogError("Control has passed to the base QueryResponse class"); }
}


public class ActionResponse : QueryResponse
{
    public List<string> events;
    public List<State> states;

    public override void HandleThis()
    { Debug.Log("Receive Response class"); }
}


public class TopographyResponse : QueryResponse //Special, as this executes immediately vis a vis ActionResponse (events added to Trace)
{
    public string topographyType; //currently only "RING"
    public List<string> orderedActorIds;

    public override void HandleThis()
    { Debug.Log("Receive Response class"); }
}
