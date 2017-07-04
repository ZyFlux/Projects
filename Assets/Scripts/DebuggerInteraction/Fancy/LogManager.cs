//Attached to LogText and uses shared structures VisualizationHandler.logInfo

using UnityEngine;

public class LogManager : MonoBehaviour
{
    static TextMesh tm;
    static string prevText = "--No recent log--";
    static string colourTagTextEnd = "</color>";

    public GameObject textPrefab;

    private void Start()
    {
        tm = GetComponent<TextMesh>();
        tm.text = prevText;
    }


    public static void NewLog(Log currLog)
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
        prevText = currText + "\n" + prevText;

        tm.text = prevText;

        //Though the log may go on indefinitely, some scroll feature is required
    } 
}
