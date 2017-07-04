//Attached to LogText and uses shared structures VisualizationHandler.logInfo

using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    static string prevText = "--No recent log--";
    static string colourTagTextEnd = "</color>";

    private List<GameObject> logs; //List of all logs generated

    public GameObject textPrefab;
    public int maxSize = 15; //Max size of log
    private void Start()
    {
        logs = new List<GameObject>();
    }


    public void NewLog(Log currLog)
    {
        
        string colourTagTextStart = "";
        
        switch(currLog.logType)
        {
            case 0: //DEBUG
                colourTagTextStart = "<color=white>";
                break;
            case 1: //INFO
                colourTagTextStart = "<color=yellow>";
                break;
            case 2: //WARNING
                colourTagTextStart = "<color=orange>";
                break;
            case 3: //ERROR
                colourTagTextStart = "<color=red>";
                break;
            default:
                Debug.LogError("Log type not identified");
                colourTagTextStart = "<color=white>";
                break;
        }
        string currText = colourTagTextStart+currLog.text+colourTagTextEnd;
        GameObject newLog = Instantiate(textPrefab);
        newLog.GetComponent<TextMesh>().text = currText;
        newLog.transform.parent = transform;
        newLog.transform.position = transform.position;
        AdjustPositionOfPreviousLogs(); //Adjust position of other logs
        logs.Add(newLog);
        AdjustSize(); //Make sure the log isn't longer than maxSize

        //Though the log may go on indefinitely, some scroll feature is required
    }
    private void AdjustPositionOfPreviousLogs()
    {
        foreach (GameObject obj in logs)
        {
            obj.transform.position -= new Vector3(0f, 0.2f, 0f); 
        }
    }
    private void AdjustSize()
    {
        if (logs.Count > maxSize)
        {
            Destroy(logs[0]);
            logs.RemoveAt(0);
        }
    }
}
