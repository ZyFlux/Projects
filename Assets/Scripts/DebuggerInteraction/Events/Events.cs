using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorEvent
{
    public string eventType;
    public virtual void HandleVisualization() { Debug.LogError("Event parent cannot be visualized"); }
}


[System.Serializable]
public class ActorCreated : ActorEvent
{
    public string actorId;
    public override void HandleVisualization()
    { VisualizationHandler.Handle(this); }
    public ActorCreated(string id)
    { actorId = id; }
}
[System.Serializable]
public class ActorDestroyed : ActorEvent
{
    public string actorId;
    public override void HandleVisualization()
    { VisualizationHandler.Handle(this); }
}
[System.Serializable]
public class MessageSent : ActorEvent
{
    public string receiverId;
    public string senderId;
    public string msg;
    public override void HandleVisualization() { VisualizationHandler.Handle(this); }
}
[System.Serializable]
public class MessageReceived : ActorEvent
{
    public string receiverId;
    public string senderId;
    public string msg;
    public override void HandleVisualization() { VisualizationHandler.Handle(this); }
}