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
        if (Trace.pointerToCurrAtomicStep != Trace.allEvents.Count)
        {

            if (Trace.pointerToCurrEvent < Trace.allEvents[Trace.pointerToCurrAtomicStep].Count)
            {                   //index
                audioS.Play(); //Play a sound
                Trace.allEvents[Trace.pointerToCurrAtomicStep][Trace.pointerToCurrEvent].HandleVisualization();
                Trace.pointerToCurrEvent++; //Let's move to the next event
            }

        }
    }

}
