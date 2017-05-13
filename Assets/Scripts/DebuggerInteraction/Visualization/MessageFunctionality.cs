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
    private float deltaChange;
    public bool isActive = false; //Activity state of the message



    //Internal usage for curve drawing
    public int bezierPointResolution; //Number of points in the trajectory

    private int arrayCountKeeper = 0; //Where are we?
    private float t = 0.0f;
    private int stepsOnStart;
    private GameObject lineRenderer; //Reference to LineRenderer set in Start()

    void Start()
    {
        lineRenderer = transform.GetChild(0).gameObject; //This is the LineRenderer. There is also an option to getbyname but this one chose for performance
        

        stepsOnStart = Trace.numOfStepsElapsed;

        //Set message string
        if(msg ==  null)
        {
            Debug.Log("The message had no msg field.");
        }


        deltaChange = 1.0f / durationOfLineInSteps;
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
    public void NewTraceStep() //Because a message is broadcasted by TraceImplement -> Used for step-by-step transparency
    {
        if (lineRenderer != null) //lineRenderer hasn't been destroyed yet
        {
            if (((stepsOnStart + durationOfLineInSteps) <= Trace.numOfStepsElapsed)) //Destroy linerenderer after durationOfLineInSteps
            { Destroy(lineRenderer); }
           
            else
            {
                Color tempCol;
                tempCol = lineRenderer.GetComponent<Renderer>().material.color;
                tempCol.a -= deltaChange; //reduce alpha by a delta amount

                lineRenderer.GetComponent<Renderer>().material.color = tempCol;
            }
            
        }
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

    void OnDestroy() //Delete the line renderer when the message is destroyed
    {
        Debug.Log("Deleting the trail renderer to " + recipient.name + " as message has been consumed");
        Destroy(lineRenderer);
    }
}
