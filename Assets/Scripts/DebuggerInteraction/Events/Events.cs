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

[System.Serializable]
public class Log : ActorEvent
{
    public int type;
    /*
     * 0 = Debug
     * 1 = Info
     * 2 = Warning
     * 3 = Error
     * 
     */
    public string text; //To be displayed on the big screen
    public override void HandleVisualization()
    {
        VisualizationHandler.Handle(this);
    }
}

public class MessageDropped : ActorEvent
{
    public string receiverId;
    public string senderId;
    public string msg;
    public override void HandleVisualization() { VisualizationHandler.Handle(this); }
}