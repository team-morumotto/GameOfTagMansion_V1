using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerObject; //視点の対象
    public GameObject plyerController;
    public float rotateSpeed = 2.0f; //回転の速さ
    private bool DebugCameraflag = false; //デバッグ用カメラ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //通常のカメラモード
        if(DebugCameraflag == false){
            //Vector3でX,Y方向の回転の度合いを定義
            Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed,Input.GetAxis("Mouse Y") * rotateSpeed, 0);
    
            //transform.RotateAround()をしようしてメインカメラを回転させる
            transform.RotateAround(playerObject.transform.position, Vector3.up, angle.x);
            transform.RotateAround(playerObject.transform.position, transform.right, angle.y);
        }
        else if(DebugCameraflag == true){
            //Vector3でX,Y方向の回転の度合いを定義
            Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed,Input.GetAxis("Mouse Y") * rotateSpeed, 0);
    
            //transform.RotateAround()をしようしてメインカメラを回転させる
            transform.RotateAround(transform.position, Vector3.up, angle.x);
            transform.RotateAround(transform.position, transform.right, angle.y);
            
            if (Input.GetKey(KeyCode.LeftArrow)) {
                this.transform.Translate (-0.1f,0.0f,0.0f);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                this.transform.Translate (0.1f,0.0f,0.0f);
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                this.transform.Translate (0.0f,0.0f,0.1f);
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                this.transform.Translate (0.0f,0.0f,-0.1f);
            }
            if (Input.GetKey(KeyCode.Q)) {
                this.transform.Translate (0.0f,1.0f,0.0f);
            }
            if (Input.GetKey(KeyCode.E)) {
                this.transform.Translate (0.0f,-1.0f,0.0f);
            }
        }
    }
    public void DebugCameramode(){
        DebugCameraflag = !DebugCameraflag;
        if(DebugCameraflag == false){
            plyerController.GetComponent<PlayerController>().enabled = true;
        }
        else if(DebugCameraflag == true){
            plyerController.GetComponent<PlayerController>().enabled = false;
        }
    }
}
