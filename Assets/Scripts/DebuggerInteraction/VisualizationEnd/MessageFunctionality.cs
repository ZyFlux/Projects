using UnityEngine;
using System.Collections;

public class MessageFunctionality : MonoBehaviour
{
    public string msg; //The message carried
    public GameObject sender; //who sent me?
    public GameObject recipient; //who receives me?

    public GameObject prefabLink3DText; //A reference to the prefab that is used for 3D info text
    public GameObject infoText; //Is used by other scripts to enable or disable

    public bool isActive = false; //Activity state of the message

    //Internal usage for curve drawing
    public int bezierPointResolution = 500; //Number of points in the trajectory
    private int arrayCountKeeper = 0; //Where are we
    private float t = 0.0f;


    void Start()
    {
        if (prefabLink3DText != null)
        {
            infoText = Instantiate(prefabLink3DText); //Instantiate the infoText
            infoText.transform.parent = this.transform; //Who's the daddy?
            infoText.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
            infoText.SetActive(false);

            infoText.GetComponent<TextMesh>().text = "Sent by " + sender.gameObject.name;
        }
        else
            Debug.LogError("Error! Prefab not found!");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == recipient)
        {
            isActive = false;

            this.gameObject.GetComponent<SphereCollider>().isTrigger = false; //We need this no more
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false; //Rigidbody is a pain
                                                                           //There has been a collision, now to reset the sender status and make more dynamic changes

            transform.rotation = recipient.transform.rotation; //Make sure the message faces the same way as the recipient block
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
    }

    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) //Thanks to Wikipedia & catlikecoding internet tutorial 
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * p0 +
            2f * oneMinusT * t * p1 +
            t * t * p2;
    }
}
