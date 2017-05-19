using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageQueueFunctionality : MonoBehaviour
{
    //3D text and other prefabs that actor nodes own the asses of
    private static GameObject prefabNameText; //A reference to the prefab that is used for each individual actor
    private GameObject contentText; //Reference to the varScreen

    //The queue of current messages
    public Queue<GameObject> messageQueue = new Queue<GameObject>(); //Init queue

    private Material mat; //Holds the material
    public Vector3 originalPosition; //Used to revert to original position after the actor has snapped into focus area once

    void Awake()
    {
        if (!prefabNameText)
            prefabNameText = Resources.Load("NameText") as GameObject;

        mat = GetComponent<Material>();
        mat = new Material(Resources.Load("White") as Material);

        GetComponent<Renderer>().enabled = false;
    }
    
    // Use this for initialization
    void Start()
    {
        Debug.Log("Created message queue " + transform.name);
        if (prefabNameText != null)
        {
            contentText = Instantiate(prefabNameText);
            contentText.transform.parent = this.transform; 
            contentText.transform.position = this.transform.position - new Vector3(0, 0.3f, 0); //With a small offset
            contentText.SetActive(false); //Initially not visible
        }
        else
            Debug.LogError("Error! Prefab not found!");

        //Set material
        this.GetComponent<MeshRenderer>().material = mat;

        originalPosition = transform.position; //Set the original position
        Debug.Log(originalPosition);
    } 

    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //Auxiliary functions

    public void EnqueueToMsgQueue(GameObject msgObj)
    {
        messageQueue.Enqueue(msgObj);
        GetComponent<Renderer>().enabled = true;
    }

    public GameObject DequeueFromMsgQueue()
    {
        GameObject msg = messageQueue.Dequeue();
        if(messageQueue.Count == 0)
        {
            GetComponent<Renderer>().enabled = false;
        }
        return msg;
    }

    public void ToggleMsgQueueInfo()
    {
        if(contentText.activeSelf)
        {
            contentText.SetActive(false);
        } else
        {
            string headMsgText = messageQueue.Peek().GetComponent<MessageFunctionality>().msg;
            contentText.GetComponent<TextMesh>().text = "Msg count: " + messageQueue.Count + " \nPeek msg: \n" + headMsgText;
            contentText.SetActive(true);
        }

    }

}



