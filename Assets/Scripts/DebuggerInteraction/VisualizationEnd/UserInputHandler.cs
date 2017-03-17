using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInputHandler
{
    public static Transform laserPointedActor; //set by SteamVR_LaserPointer and accessed by NetworkInterface during sends
    public static void HandleRightRadial (int buttonId)
    {
        switch(buttonId)
        {
            case 0:
                PausePlay();
                break;
            case 2:
                QueryState();
                break;
            case 3:
                TagUntagActor();
                break;
            default:
                Debug.LogError("Illegal button clicked");
                break;
        }
    }
    static void TagUntagActor()
    {
        Debug.Log("About to tag/untag actor");
        //TagActorRequest tar = new TagActorRequest(laserPointedActor.name, true);
        //TODO
        //NetworkInterface.HandleTagUntagRequestToBeSent(tar);
        Debug.LogError("Not yet implemented");
    }

    static void QueryState()
    {
        Debug.Log("About to query state");
        StateRequest sr = new StateRequest(laserPointedActor.name);
        NetworkInterface.HandleStateRequestToBeSent(sr);
    }
    static void PausePlay() //TODO
    {
        Debug.Log("About to Pause / Play");
        //Broadcast a Pause?
    }
    
}