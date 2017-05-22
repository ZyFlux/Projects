using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorFunctionality : MonoBehaviour
{
    //3D text and other prefabs that actor nodes own the asses of
    private static GameObject prefabNameText; //A reference to the prefab that is used for each individual actor
    public GameObject nameText; //Is used by other scripts to enable or disable
    
    private static GameObject prefabMessageSphereInstance;
    private static GameObject prefabMessageQueueBoxInstance;

    private Material mat; //Holds the material
    private VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter outliner;

    public bool getState = false; //Is the state shown (or not)?
    public bool getTag = false;//Is the actor tagged (or not)?

    public Vector3 originalPosition; //Used to revert to original position after the actor has snapped into focus area once

    public GameObject messageQueueBox;
    private static GameObject prefabVarScreen;
    private GameObject varScreen; //Reference to the varScreen

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
        if (!prefabMessageQueueBoxInstance)
            prefabMessageQueueBoxInstance = Resources.Load("MessageQueue") as GameObject;

        messageQueueBox = Instantiate(prefabMessageQueueBoxInstance);
        messageQueueBox.transform.parent = this.transform; //Who's the daddy?
        messageQueueBox.transform.position = this.transform.position + new Vector3 (0f,transform.localScale.y/2, 0f); //With no offset

        //Set up stuff for grab capability
        VRTK.VRTK_InteractableObject vrio = gameObject.AddComponent<VRTK.VRTK_InteractableObject>();//Make it grabbable for drag drop
        vrio.isGrabbable = true;
        vrio.touchHighlightColor = Color.grey;

        //Set up stuff for Outlining
        outliner = gameObject.AddComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>();
        outliner.Initialise(); //Initialize with an outline colour
 

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.freezeRotation = true; //Allow rotation only along Y-axis
        rb.useGravity = false;
        rb.angularDrag = 100.0f;
        rb.drag = 100.0f;

    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Created actor called " + transform.name);
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
        varScreen.SetActive(false); //Initially not visible

        //Set material
        this.GetComponent<MeshRenderer>().material = mat;

        originalPosition = transform.position; //Set the original position
    
    } 

    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //Auxiliary functions


    //Outlining
   public void MomentaryOutline(Color outlineC, float t)
    {
        outliner.Highlight(outlineC);
        StartCoroutine(RemoveOutlineAfterTime(t));
    }

    public IEnumerator RemoveOutlineAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        outliner.Unhighlight();
    }


    //Messaging
    public void GenerateMessage(GameObject recipient, string text)
    {
        GameObject MessageSphereInstance = Instantiate(prefabMessageSphereInstance, transform.position, transform.rotation) as GameObject; //Instantiate message sphere prefab
        recipient.GetComponent<ActorFunctionality>().messageQueueBox.GetComponent<MessageQueueFunctionality>().EnqueueToMsgQueue(MessageSphereInstance);
        MessageSphereInstance.transform.parent = recipient.transform;
        
        MessageFunctionality mf = MessageSphereInstance.GetComponent<MessageFunctionality>();
        mf.sender = this.gameObject; //Tell the message who the sender is
        mf.recipient = recipient;
        mf.msg = text;
        Debug.Log("New instance of message created from " + this.gameObject.ToString() + " for " + recipient.gameObject.ToString());
        mf.isActive = true; 
    }

    public void ReceiveMessageFromQueue() //Used for MessageDroppped
    {
        GameObject consumedMessage = messageQueueBox.GetComponent<MessageQueueFunctionality>().DequeueFromMsgQueue(); //Consume message from queuesssss
        Debug.Log("Message " + consumedMessage.ToString() + " dropped by " + this.gameObject.ToString());
        Destroy(consumedMessage);
    }


    public void ReceiveMessageFromQueue(GameObject sender)
    {
        GameObject consumedMessage = messageQueueBox.GetComponent<MessageQueueFunctionality>().DequeueFromMsgQueue(); //Consume message from queue
        Debug.Log("Message " + consumedMessage.ToString() + " accepted by " + this.gameObject.ToString());
        Destroy(consumedMessage);
    }

    //Coloring- state changes
    public void ChangeColour(Color colour)
    {
        Debug.Log("About to change colour");
        mat.color = colour;
    }

    public void UpdateState(State st)
    {
        ChangeColour(st.behavior); //change colour of the actor
    }

    public void NewStateReceived(State st) //Broadcast a message about change of state
    {
        if (getState)
        {
            gameObject.BroadcastMessage("UpdateState", st);
            Debug.Log("Updating state on " + transform.name);
        }
        else
            Debug.LogError("Updated state received even though the getState bool is false");
    }


    //-----------------------Variable switches
    public bool ToggleState()
    {
        if (getState)
        {
            getState = false;
            varScreen.SetActive(false); //Disable the var screen
            ChangeColour(Color.white); //Set colour to white- generic
            return false;
        }
        else
        {
            getState = true;
            varScreen.SetActive(true);
            return true;
        }
    }

    public bool ToggleTag()
    {
        if (getTag)
        {
            getTag = false;
            return false;
        }
        else
        {
            getTag = true;
            return true;
        }
    }
}



