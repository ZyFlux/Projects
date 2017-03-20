using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceImplement : MonoBehaviour {

    public float timeDelay = 0.1f;

	// Starts after Awake()
	void Start ()
    {
        Debug.Log("We now begin implementing the trace");
        InvokeRepeating("ImplementNext", 0.0f, timeDelay);
    }

    void ImplementNext()
    {
        if (Trace.pointerToCurrEvent < Trace.allEvents.Count)
        {                   //index

            Trace.allEvents[Trace.pointerToCurrEvent].HandleVisualization();
            Trace.pointerToCurrEvent++; //Let's move to the next event
        }
    }

}
