using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTextController : MonoBehaviour {
    private TextMesh tm;
    private Color col;
    private bool possibilityOfNext;
    private bool possibilityOfPointedAction;
    private int index;
    // Use this for initialization
	void Start () {
        tm = GetComponent<TextMesh>();
        col = tm.color;
        possibilityOfNext = true;
        index = -1;
	}
    private void Update()
    {
        if (UserInputHandler.CheckAtomicStepIndex())
        { 
            possibilityOfNext = true;
        }
        else
        {
            possibilityOfNext = false;
        }

        if (UserInputHandler.CheckLaserPointer())
        {
            possibilityOfPointedAction = true;
        }
        else
        {
            possibilityOfPointedAction = false;
        }

        switch(index)
        {
            case 1:
                if (possibilityOfNext)
                {
                    tm.color = col;
                    tm.text = "Next step in trace";
                }
                else
                {
                    tm.color = Color.red;
                    tm.text = "Please wait for current events to finish";
                }
                break;
            case 2:
                if (possibilityOfNext)
                {
                    if (possibilityOfPointedAction)
                    {
                        tm.color = col;
                        tm.text = "Drop from pointed actor";
                    }
                    else
                    {
                        tm.color = Color.red;
                        tm.text = "Only possible when poiniting at an actor";
                    }
                }
                else
                {
                    tm.color = Color.red;
                    tm.text = "Please wait for current events to finish";
                }
                break;
            case 3:
                if (possibilityOfNext)
                {
                    if (possibilityOfPointedAction)
                    {
                        tm.color = col;
                        tm.text = "Receive from pointed actor";
                    }
                    else
                    {
                        tm.color = Color.red;
                        tm.text = "Only possible when poiniting at an actor";
                    }
                }
                else
                {
                    tm.color = Color.red;
                    tm.text = "Please wait for current events to finish";
                }
                break;
            case 4:
                if (possibilityOfPointedAction)
                {
                    tm.color = col;
                    tm.text = "Query state on / off";
                }
                else
                {
                    tm.color = Color.red;
                    tm.text = "Only possible when poiniting at an actor";
                }
                break;
            case 5:
                if (possibilityOfPointedAction)
                {
                    tm.color = col;
                    tm.text = "Tag / Untag pointed actor";
                }
                else
                {
                    tm.color = Color.red;
                    tm.text = "Only possible when poiniting at an actor";
                }
                break;
            case 6:
                if (possibilityOfPointedAction)
                {
                    tm.color = col;
                    tm.text = "Mark upcoming message";
                }
                else
                {
                    tm.color = Color.red;
                    tm.text = "Only possible when poiniting at an actor";
                }
                break;
        }
    }
    public void PausePlay()
    {
        index = 0;
        tm.text = "Pause / Play simulation";
    }

    public void Topography()
    {
        index = 0; 
        tm.text = "Get topography";
    }

    public void Exiting()
    {
        index = 0;
        tm.color = col;
        tm.text = "";        
    }

    public void OnOff()
    {
        index = 0;
        tm.text = "Start / Stop simulation";
    }

    public void Next()
    {
        index = 1;
    }

    public void Drop()
    {
        index = 2;
    }
    public void Receive()
    {
        index = 3;
    }
    public void State()
    {
        index = 4;
    }
    public void TagUntag()
    {
        index = 5;
    }
    public void Mark()
    {
        index = 6;
    }


}
