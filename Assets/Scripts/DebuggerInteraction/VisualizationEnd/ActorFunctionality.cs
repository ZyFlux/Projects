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



    void Awake()
    {
        //Initialize all prefabs and important stuff
        prefabNameText = Resources.Load("NameText") as GameObject;

        prefabMessageSphereInstance = Resources.Load("Message") as GameObject;

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
            nameText.transform.position = this.transform.position - new Vector3(0, 0.1f, 0); //With a small offset
        }
        else
            Debug.LogError("Error! Prefab not found!");

    } 

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


    public void ChangeColour(string colour)
    {
        Debug.Log("About to change colour");
        MeshRenderer goRenderer = GetComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Standard")); //Use the standard shader 
        //TODO: optimize

        switch (colour)
        {
            case "RED":
                mat.color = Color.red;
                break;
            case "GREEN":
                mat.color = Color.green;
                break;
            case "BLUE":
                mat.color = Color.blue;
                break;
            default:
                mat.color = Color.gray;
                break;
        }

        goRenderer.material = mat;
    }
}



