//We have the concept of an atomic step, which is a list of events. We need to cycle through each atomic step and each event within this step.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceImplement : MonoBehaviour {

    public float timeDelay = 2.5f; //time delay (in addition to what happens in ImplementNext)
    public static GameObject rootOfActors;
    private AudioSource audioS;
  
    void Awake()
    {
        audioS = GetComponent<AudioSource>();
        rootOfActors = GameObject.Find("RootActors"); //The parent to everything
    }
    IEnumerator Start()
    {
        while (true) //Execute indefinitely
        {
            yield return new WaitForSeconds(timeDelay); //Time delay for each event visualization
            if (!UserInputHandler.isPaused)
            {
                if (Trace.NewStepPossible())
                {
                    audioS.Play(); //Play a sound
                    Trace.allEvents[Trace.pointerToCurrAtomicStep][Trace.pointerToCurrEvent].HandleOutline();    //Do the outlining
                    yield return new WaitForSeconds(1f); //Time delay for actual visualization
                    Trace.allEvents[Trace.pointerToCurrAtomicStep][Trace.pointerToCurrEvent].HandleVisualization();
                    rootOfActors.BroadcastMessage("NewTraceStep", SendMessageOptions.DontRequireReceiver);

                    Trace.IncrementPointer(); //Let's move to the next event
                }

            }
        }
    }
}
