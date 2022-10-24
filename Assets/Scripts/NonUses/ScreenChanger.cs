/*
    2022/10/18
    Atsuki Kobayashi
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChanger : MonoBehaviour
{
    public InputField inputField;
    public CanvasScaler canvasScaler;
public void ScreenChange()
    {
        switch (inputField.text)
        {
            case "a":
                Screen.SetResolution(1280, 720, false , 60);
                Debug.Log("unnko");
                break;
            case "b":
                Screen.SetResolution(1920, 1080, false , 60);
                break;
            case "c":
                Screen.SetResolution(2880, 1620, false , 60);
                break;
        }
    }
}
