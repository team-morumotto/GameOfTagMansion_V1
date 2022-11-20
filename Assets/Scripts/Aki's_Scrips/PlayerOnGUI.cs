using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGUI : MonoBehaviour
{
    private GUIStyle style;

	void Start()
	{
		style = new GUIStyle();
		style.fontSize = 30;
	}

    void OnGUI(){
        GUI.Label(new Rect(10,700,500,400),"左スティックで移動",style);
        GUI.Label(new Rect(510,700,500,400),"右スティックでカメラ操作",style);
    }
}
