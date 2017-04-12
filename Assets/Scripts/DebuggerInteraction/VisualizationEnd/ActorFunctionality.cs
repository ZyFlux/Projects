using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorFunctionality : MonoBehaviour
{
    //3D text and other prefabs that actor nodes own the asses of

    private static GameObject prefabNameText; //A reference to the prefab that is used for each individual actor
    public GameObject nameText; //Is used by other scripts to enable or disable


    //The queue of current messages
    public Queue<GameObject> messageQueue = new Queue<GameObject>(); //Init queue

    private static GameObject prefabMessageSphereInstance;


    private Material mat; //Holds the material

    public bool getState = false; //Initially, weshow no state

    private static GameObject prefabVarScreen;
    private GameObject varScreen;

    void Awake()
    {

        //Initialize all prefabs and important stuff
        if(!prefabNameText)
            prefabNameText = Resources.Load("NameText") as GameObject;
        if(!prefabMessageSphereInstance)
            prefabMessageSphereInstance = Resources.Load("Message") as GameObject;
        if (!mat)
            mat = new Material(Resources.Load("White") as Material); //mat must be a new material- each actor has its own
        if(!prefabVarScreen)
            prefabVarScreen = Resources.Load("VarsScreen") as GameObject;
        
        
        //Set up stuff for grab capability
        VRTK.VRTK_InteractableObject vrio = gameObject.AddComponent<VRTK.VRTK_InteractableObject>();//Make it grabbable for drag drop
        vrio.isGrabbable = true;


        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = 100.0f;
        rb.drag = 100.0f;

    }

    // Use this for initialization
    void Start()
    {

        //My name is..
        if (prefabNameText != null)
        {
            nameText = Instantiate(prefabNameText);
            nameText.GetComponent<TextMesh>().text = this.gameObject.name; //say my name..
            nameText.transform.parent = this.transform; //Who's the daddy?
            nameText.transform.position = this.transform.position; //With no offset
        }
        else
            Debug.LogError("Error! Prefab not found!");

        varScreen = Instantiate(prefabVarScreen);
        varScreen.transform.parent = this.transform; //Who's the daddy?
        varScreen.transform.position = this.transform.position - new Vector3(0, 0.3f, 0); //With a small offset

        //Set material
        this.GetComponent<MeshRenderer>().material = mat;

    } 

    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //Auxiliary functions

   
    public void GenerateMessage(GameObject recipient)
    {
        GameObject MessageSphereInstance = Instantiate(prefabMessageSphereInstance, transform.position, transform.rotation) as GameObject; //Instantiate message sphere prefab
        recipient.GetComponent<ActorFunctionality>().messageQueue.Enqueue(MessageSphereInstance);
        MessageSphereInstance.transform.parent = recipient.transform;
        
        MessageFunctionality mf = MessageSphereInstance.GetComponent<MessageFunctionality>();
        mf.sender = this.gameObject; //Tell the message who the sender is
        mf.recipient = recipient;
        Debug.Log("New instance of message created from " + this.gameObject.ToString() + " for " + recipient.gameObject.ToString());
        mf.isActive = true;
        
    }

    public void ReceiveMessageFromQueue() //NO strict queue consistency checks
    {
       
        GameObject consumedMessage = messageQueue.Dequeue(); //Consume message from queue
        
        Debug.Log("Message " + consumedMessage.ToString() + " accepted by " + this.gameObject.ToString());
        Destroy(consumedMessage);
    }
    public void ReceiveMessageFromQueue(GameObject sender)
    {
        GameObject consumedMessage = messageQueue.Dequeue(); //Consume message from queue
        Debug.Log("Message " + consumedMessage.ToString() + " accepted by " + this.gameObject.ToString());
        Destroy(consumedMessage);
    }


    public void ChangeColour(Color colour)
    {
        Debug.Log("About to change colour");
        mat.color = colour;
    }

    public void UpdateState(State st)
    {
        ChangeColour(st.behavior); //change colour of the actor

        foreach (Transform child in transform) //re-enable the renderers (if they aren't already enabled)
        {
            if (child.CompareTag("State"))
            {
                transform.GetComponent<MeshRenderer>().enabled = true; //Turn off MeshRenderer
            }
        }
    }

    public void SwitchStateOff() //Turn off getting state for actor
    {
        getState = false;
        ChangeColour(Color.white); //Set colour to white
        //Turn off MeshRenderer for all children 
        foreach (Transform child in transform)
        {
            if(child.CompareTag("State"))
            {
                transform.GetComponent<MeshRenderer>().enabled = false; //Turn off MeshRenderer
            }
        }
    }

    public void NewStateReceived(State st) //Broadcast a message about change of state
    {
        BroadcastMessage("UpdateState", st, SendMessageOptions.RequireReceiver);
    }

}



