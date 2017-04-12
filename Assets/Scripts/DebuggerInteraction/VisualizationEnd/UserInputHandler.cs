using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputHandler : MonoBehaviour
{
    public static Transform laserPointedActor; //set by SteamVR_LaserPointer and accessed by NetworkInterface during sends
    public static bool isPaused;

    private void Start()
    {
        isPaused = false;
    }
    public void TagUntagActor()
    {
        Debug.Log("About to tag/untag actor");
        //TagActorRequest tar = new TagActorRequest(laserPointedActor.name, true);
        //TODO
        //NetworkInterface.HandleTagUntagRequestToBeSent(tar);
        Debug.LogError("Not yet implemented");
    }

    public void ReceiveFromActor()
    {
        if (laserPointedActor != null && laserPointedActor.CompareTag("Actor"))
        {
            Debug.Log("About to receive from actor");
            ActionRequest rr = new ActionRequest("__NEXT__", laserPointedActor.name);
            NetworkInterface.HandleRequest(rr);
        }
    }

    public void DropFromActor()
    {
        if (laserPointedActor != null && laserPointedActor.CompareTag("Actor"))
        {
            Debug.Log("About to drop from actor");
            ActionRequest rr = new ActionRequest("__DROP__", laserPointedActor.name);
            NetworkInterface.HandleRequest(rr);
        }
    }

    public void QueryState()
    {
        if (laserPointedActor != null && laserPointedActor.CompareTag("Actor"))
        {

            bool toggle = (laserPointedActor.gameObject.GetComponent<ActorFunctionality>().getState) ? false : true;
            if (!toggle)
                laserPointedActor.gameObject.GetComponent<ActorFunctionality>().SwitchStateOff();
            Debug.Log("About to query state "+toggle.ToString());
            StateRequest sr = new StateRequest(laserPointedActor.name, toggle);
            NetworkInterface.HandleRequest(sr);
        }
    }
    public void PausePlay() //TODO- Hook up other scripts to this bool for real pause / play
    {
        isPaused = isPaused ? false : true;
        Debug.Log("Are we Paused? "+isPaused.ToString());
    }
    

    public void NextStep() //Go to the next step
    {
        Trace.pointerToCurrAtomicStep++; //Increment the atomic step counter
        //Send the next message to dispatcher
        ActionRequest ar = new ActionRequest("__NEXT__", ""); //TODO-Make things safe and clear so that the __NEXT__ is only possible at the correct moments
        NetworkInterface.HandleRequest(ar);
        Debug.Log("About to go to the next step");
    }
}