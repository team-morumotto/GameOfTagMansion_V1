using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class oni_sample : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    public float speed = 5f;
    private float inputHorizontal;
    float inputVertical;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if(!photonView.IsMine){
            return;
        }
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
            var h = Input.GetAxis ("Horizontal") * speed;				// 入力デバイスの水平軸をhで定義
            var v = Input.GetAxis ("Vertical") * speed;				// 入力デバイスの垂直軸をvで定義
            
            //rb.velocity = new Vector3(h, rb.velocity.y, v);
        
    }
    void FixedUpdate(){
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
    
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
    
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * speed + new Vector3(0, rb.velocity.y, 0);
    
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }
    void OnCollisionEnter(Collision col){
        if(col.gameObject.GetComponent<PhotonView>() == false){
            return;
        }
        //photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, p);
    }

    /*[PunRPC]
    private void RpcSendMessage(string message) {
        Debug.Log(message);
    }*/
}
