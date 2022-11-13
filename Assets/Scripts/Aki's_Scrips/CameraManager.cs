using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Wall")
        {
            Debug.Log("WALLEnter");
        }
    }

    void OnTriggerStay(Collider c)
    {
        if(c.gameObject.tag == "Wall")
        {
            Debug.Log("WALLStay");
        }
    }

    void OnTriggerExit(Collider c)
    {
        if(c.gameObject.tag == "Wall")
        {
            Debug.Log("WALLExit");
        }
    }
}
