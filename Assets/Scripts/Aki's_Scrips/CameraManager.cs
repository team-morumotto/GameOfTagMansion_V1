using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void OnTrigerEnter(Collider c)
    {
        if(c.gameObject.tag == "WALL")
        {
            Debug.Log("WALLEnter");
        }
    }

    void OnTrigerExit(Collider c)
    {
        if(c.gameObject.tag == "WALL")
        {
            Debug.Log("WALLExit");
        }
    }
}
