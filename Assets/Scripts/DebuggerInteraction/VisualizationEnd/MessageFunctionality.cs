using UnityEngine;
using System.Collections;

public class MessageFunctionality : MonoBehaviour
{
    //Set by ActorFunctionality
    public string msg; //The message carried
    public GameObject sender; //who sent me?
    public GameObject recipient; //who receives me?

    public GameObject prefabLink3DText; //A reference to the prefab that is used for 3D info text
    public GameObject infoText; //Is used by other scripts to enable or disable
    

    public int durationOfLineInSteps = 5; //Number of steps after linerenderer is destroyed

    public bool isActive = false; //Activity state of the message



    //Internal usage for curve drawing
    public int bezierPointResolution = 50; //Number of points in the trajectory

    private int arrayCountKeeper = 0; //Where are we
    private float t = 0.0f;
    private int stepsOnStart;
    private GameObject lineRenderer; //Reference to LineRenderer set in Start()

    void Start()
    {
        lineRenderer = transform.GetChild(0).gameObject; //This is the LineRenderer. There is also an option to getbyname but this one chose for performance
        
        if (prefabLink3DText != null)
        {
            infoText = Instantiate(prefabLink3DText); //Instantiate the infoText
            infoText.transform.parent = transform; //Who's the daddy?
            infoText.transform.position = transform.position; //+ new Vector3(0, 0.1f, 0);
            infoText.SetActive(false);

            infoText.GetComponent<TextMesh>().text = "Sent by " + sender.gameObject.name;
        }
        else
            Debug.LogError("Error! Prefab not found!");

        stepsOnStart = Trace.numOfStepsElapsed;

        //Set message string
        if(msg ==  null)
        {
            Debug.Log("The message had no msg field.");
        }
        else
        {
            infoText.GetComponent<TextMesh>().text = msg;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == recipient)
        {
            isActive = false;

            this.gameObject.GetComponent<SphereCollider>().isTrigger = false; //We need this no more
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;   //Rigidbody is a pain
                                      //There has been a collision, now to reset the sender status and make more dynamic changes
            
            transform.DetachChildren(); //Detach the trail renderer- we do some fancy stuff to show the queue
  
            transform.rotation = recipient.transform.rotation; //Make sure the message faces the same way as the recipient block

            //Now, we put this somewhere special                                                                                        //To add a little more leeway
            transform.position = new Vector3(recipient.transform.position.x, recipient.transform.position.y + (transform.localScale.y * 2.1f * recipient.GetComponent<ActorFunctionality>().messageQueue.Count), recipient.transform.position.z);
            rb.useGravity = true;

        }

    }
    void Update()
    {
        if (isActive)
        {
            if (arrayCountKeeper <= bezierPointResolution && t < 1.0f)
            {
                arrayCountKeeper++;
                t = arrayCountKeeper * 1.0f / bezierPointResolution;
                transform.position = GetBezierPoint(sender.transform.position, new Vector3((sender.transform.position.x + recipient.transform.position.x) / 2, 3f, (sender.transform.position.z + recipient.transform.position.z) / 2), recipient.transform.position, t);
                //The overflow of arrayCountKeeper is handled by the MessageSphere
            }
        }
        //If not active, wait

        if((stepsOnStart + durationOfLineInSteps) == Trace.numOfStepsElapsed) //Destroy linerenderer after durationOfLineInSteps
        { Destroy(lineRenderer); }
    }

    public void ToggleState()
    {
        Debug.Log("About to toggle message on/off");
        if (infoText.activeSelf)
        {
            infoText.SetActive(false);
        }
        else
            infoText.SetActive(true);
    }

    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) //Thanks to Wikipedia & catlikecoding
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * p0 +
            2f * oneMinusT * t * p1 +
            t * t * p2;
    }
}
