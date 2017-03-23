using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVarsText : MonoBehaviour {

    TextMesh tm;

    // Use this for initialization
    void Start () {

        tm = GetComponent<TextMesh>();
        tm.text = "Waiting for initial state..";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateState(State st)
    {
        tm.text = st.vars;
        Debug.Log("Successfully changed state");
    }
}
