using UnityEngine;

public class LogDisplayer : MonoBehaviour
{
    TextMesh tm;
    private void Start()
    {
        tm = GetComponent<TextMesh>();
    }
    // Update is called once per frame
    void Update()
    {
        if (VisualizationHandler.logInfo != null) //TODO: Optimize this to only be executed when there is a new log received
        {
            tm.text = VisualizationHandler.logInfo.text;
            switch (VisualizationHandler.logInfo.type)
            {
                case 0: //DEBUG
                    tm.color = Color.white;
                    break;
                case 1: //INFO
                    tm.color = Color.gray;
                    break;
                case 2: //WARNING
                    tm.color = Color.yellow;
                    break;
                case 3: //ERROR
                    tm.color = Color.red;
                    break;
                default:
                    Debug.LogError("Log type not identified");
                    break;
            }

        }
    } 
}
