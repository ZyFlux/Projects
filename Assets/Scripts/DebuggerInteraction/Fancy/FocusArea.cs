using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusArea : MonoBehaviour {
    public GameObject controllerWithLaser; //Set in the inspector
    private Vector3 scale;

    private void Awake()
    {
        if (controllerWithLaser == null)
            Debug.LogError("The controller with laser pointer isn't appropriately defined for the focus area.");
    }


    void Start () {
        scale = transform.localScale;


        controllerWithLaser.GetComponent<VRTK.VRTK_ControllerEvents>().TriggerClicked += new VRTK.ControllerInteractionEventHandler(SnapHere); //Listen to trigger event
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SnapHere(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if(UserInputHandler.laserPointedActor!= null)
            if (UserInputHandler.laserPointedActor.CompareTag("Actor"))
            {
                //Code for snapping
                UserInputHandler.laserPointedActor.position = transform.position + new Vector3(Random.Range(-scale.x, scale.x)/2, 1.7f, Random.Range(-scale.y, scale.y)/2);
                Debug.Log("Snapping " + UserInputHandler.laserPointedActor.name + " into focus area.");
            }
    }
}
