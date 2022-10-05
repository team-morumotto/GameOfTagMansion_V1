using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManeger : MonoBehaviour
{
    public GameObject sb;
    private bool DebugPanel = false;
    public GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        sb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L)&&DebugPanel == false) {
                sb.SetActive(true);
                DebugPanel = true;
            }
        else if(Input.GetKey(KeyCode.L)&&DebugPanel == true){
                sb.SetActive(false);
                DebugPanel = false;
        }            
    }
    public void DebugCamera(){
        Camera.GetComponent<CameraController>().DebugCameramode();
    }
}
