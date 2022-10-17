using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ネット動作よくわからないので実体ポップできるようにコントローラー式にしてます
    [HideInInspector] public GameObject Player; //追う対象 場合によってはfindで必要なもの探してください
    public GameObject player; //プレハブであってほしい
    private Rigidbody rb;
    private GameObject stateM;

    [SerializeField] private float initSpeed = 0.1f;
    private float speed;
    private Vector3 rv; //入力値の格納用
    void Start () {
        stateM = GameObject.Find("StateManeger");
        Player = Instantiate(player);
        rb = Player.GetComponent<Rigidbody>();
        //Player = GameObject.Find("Player");
        speed = initSpeed;
    }

    void Update () {
        rv = new Vector3();        
        if(stateM.GetComponent<StateManeger>().isSpeedUp == true){
            speed = initSpeed * 2;
            Debug.Log("ぬぬぬ");
        }
        
        //WASD押したら方向を代入
        if (Input.GetKey(KeyCode.A)) {
                rv.x = -speed;
                //Player.transform.Rotate(0, -rotatespeed, 0);
                Player.transform.position += Player.transform.forward * speed;
            }
        if (Input.GetKey(KeyCode.D)) {
                rv.x = speed;
                //Player.transform.Rotate(0, rotatespeed, 0);
                Player.transform.position += Player.transform.forward * speed;
            }
        if (Input.GetKey(KeyCode.W)) {
                rv.z = speed;
                Player.transform.position += Player.transform.forward * speed;
            }
        if (Input.GetKey(KeyCode.S)) {
                rv.z = -speed;
                Player.transform.position += Player.transform.forward * speed;
            }
    }

    void FixedUpdate(){
        //カメラのxとzのベクトルを抽出
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * rv.z + Camera.main.transform.right * rv.x;
 
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * speed + new Vector3(0, rb.velocity.y, 0);
 
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) {
            Player.transform.rotation = Quaternion.LookRotation(moveForward);
        }

    }
}