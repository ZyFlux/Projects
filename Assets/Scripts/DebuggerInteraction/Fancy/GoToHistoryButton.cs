using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToHistoryButton : MonoBehaviour {

    public int indexOfDispatcher; //index of the dispatcher


    public void HandleClick()
    {
                                        //We decrement by 1 to get the correct id
        StepRequest sr = new StepRequest(indexOfDispatcher-1);
        NetworkInterface.HandleRequest(sr);
    }
}
