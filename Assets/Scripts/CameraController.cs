using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*public GameObject playerObject; //視点の対象
    public GameObject plyerController;
    public float rotateSpeed = 2.0f; //回転の速さ
    private bool DebugCameraflag = false; //デバッグ用カメラ
    private Vector3 initCameraposition;
    private Vector3 targetpos;
    private Vector3 targetrotate;
    
    // Start is called before the first frame update
    void Start()
    {
        initCameraposition = transform.position;
        transform.position = playerObject.transform.position+initCameraposition;
    }

    // Update is called once per frame
    void Update()
    {
        
        //通常のカメラモード
        if(DebugCameraflag == false){
            //プレイヤー追尾
            //transform.position = playerObject.transform.position+initCameraposition;
            //Vector3でX,Y方向の回転の度合いを定義
            Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed,Input.GetAxis("Mouse Y") * rotateSpeed, 0);
    
            //transform.RotateAround()を使用してメインカメラを回転させる
            transform.RotateAround(transform.position, Vector3.up, angle.x);
            transform.RotateAround(transform.position, transform.right, angle.y);

            //カメラの方向にプレイヤーを向かせたかった（遺言）
            //playerObject.transform.RotateAround(playerObject.transform.position, Vector3.up, angle.x);
            transform.position += playerObject.transform.position - targetpos;
            targetpos = playerObject.transform.position;
            transform.RotateAround(targetpos, Vector3.up, angle.x * Time.deltaTime);
        }
        else if(DebugCameraflag == true){
            //Vector3でX,Y方向の回転の度合いを定義
            Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed,Input.GetAxis("Mouse Y") * rotateSpeed, 0);
    
            //transform.RotateAround()を利用してメインカメラを回転させる
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
    
    }*/
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
