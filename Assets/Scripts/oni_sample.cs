using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class oni_sample : MonoBehaviourPunCallbacks
{
    void FixedUpdate(){
        if(photonView.IsMine){
            PhotonNetwork.LocalPlayer.NickName = SetName.NAME;   // 名前をセット(名前入力後にオブジェクト生成のため)
        }
    }
    void OnCollisionEnter(Collision col){
        if(col.gameObject.GetComponent<PhotonView>() == false){
            return;
        }
        //photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, p);

    }

    [PunRPC]
    private void RpcSendMessage(string message) {
        Debug.Log(message);
    }
}
