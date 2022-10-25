using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class oni_sample : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    public float speed = 5f;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if(photonView.IsMine){
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
            var h = Input.GetAxis ("Horizontal") * speed;				// 入力デバイスの水平軸をhで定義
            var v = Input.GetAxis ("Vertical") * speed;				// 入力デバイスの垂直軸をvで定義
            
            rb.velocity = new Vector3(h, rb.velocity.y, v);
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
