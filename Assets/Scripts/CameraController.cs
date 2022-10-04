using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerObject; //視点の対象
    public float rotateSpeed = 2.0f; //回転の速さ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3でX,Y方向の回転の度合いを定義
        Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed,Input.GetAxis("Mouse Y") * rotateSpeed, 0);
 
        //transform.RotateAround()をしようしてメインカメラを回転させる
        transform.RotateAround(playerObject.transform.position, Vector3.up, angle.x);
        transform.RotateAround(playerObject.transform.position, transform.right, angle.y);
    }
}
