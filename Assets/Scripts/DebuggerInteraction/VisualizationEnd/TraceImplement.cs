using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceImplement : MonoBehaviour {

    public float timeDelay = 0.1f;
    private AudioSource audioS;
	void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }
	void Start ()
    {
        Debug.Log("We now begin implementing the trace");
        InvokeRepeating("ImplementNext", 0.0f, timeDelay);
    }

    void ImplementNext() //Implement the next thing
    {
        if (Trace.pointerToCurrAtomicStep != Trace.allEvents.Count)
        {
            if (Trace.pointerToCurrEvent < Trace.allEvents[Trace.pointerToCurrAtomicStep].Count)
            {                   //index

                Trace.allEvents[Trace.pointerToCurrAtomicStep][Trace.pointerToCurrEvent].HandleVisualization();
                Trace.pointerToCurrEvent++; //Let's move to the next event
            }
            else
            {
                if (Trace.pointerToCurrAtomicStep < Trace.allEvents.Count)
                {
                    Debug.Log("Moving to the next atomic step");
                    Trace.pointerToCurrAtomicStep++;
                    //Make sound
                    audioS.Play();
                }
            }
        }
    }

}
