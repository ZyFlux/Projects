//We have the concept of an atomic step, which is a list of events. We need to cycle through each atomic step and each event within this step.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceImplement : MonoBehaviour {

    public float timeDelay = 0.5f;
    private AudioSource audioS;
  
    void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }
	void Start ()
    {
        //Debug.Log("We now begin implementing the trace");
        InvokeRepeating("ImplementNext", 0.0f, timeDelay);
    }

    public void ImplementNext() //Implement the next thing
    {
        if (Trace.NewStepPossible())
        {
            audioS.Play(); //Play a sound
            Trace.allEvents[Trace.pointerToCurrAtomicStep][Trace.pointerToCurrEvent].HandleVisualization();
            Trace.IncrementPointer(); //Let's move to the next event
        }
        else
            Debug.Log("End of trace reached.");

    }

}
