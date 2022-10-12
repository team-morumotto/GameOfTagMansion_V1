using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject targetObj;
    private Vector3 targetPos;
    [SerializeField]private float rotatespeed = 400f;
    private bool DebugCameraflag = false; //デバッグ用カメラ
    private Vector3 initCameraposition;
    void Start () {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
        initCameraposition = transform.position;
    }
 
    void Update() {
        if(DebugCameraflag == false){
            // targetの移動量分、自分（カメラ）も移動する
            transform.position += targetObj.transform.position - targetPos;
            targetPos = targetObj.transform.position;
            camerarotate();
        }
        else if(DebugCameraflag == true){
            camerarotate();
            if(Input.GetKey(KeyCode.UpArrow)){
                transform.position += transform.forward;
            }
            if(Input.GetKey(KeyCode.DownArrow)){
                transform.position -= transform.forward;
            }
            if(Input.GetKey(KeyCode.RightArrow)){
                transform.position += transform.right;
            }
            if(Input.GetKey(KeyCode.LeftArrow)){
                transform.position -= transform.right;
            }
        }
    }   
    private void camerarotate(){
        // マウスの右クリックを押している間
        if (Input.GetMouseButton(0)) {
            // マウスの移動量
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            // targetの位置のY軸を中心に、回転（公転）する
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * rotatespeed);
            // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
            transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * rotatespeed);
        }
    } 
    public void DebugCameramode(){
        var pc = GameObject.Find("PlayerController");
        DebugCameraflag = !DebugCameraflag;
        if(DebugCameraflag == false){
            pc.GetComponent<PlayerController>().enabled = true;
        }
        else if(DebugCameraflag == true){
            pc.GetComponent<PlayerController>().enabled = false;
            transform.position = targetObj.transform.position+initCameraposition;
        }
    }
}
