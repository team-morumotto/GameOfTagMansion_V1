using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class item : MonoBehaviourPunCallbacks
{
    void Update(){
        transform.Rotate(0, 1, 0);
    }
    void OnTriggerEnter(Collider collision){
        if(!photonView.IsMine) return;
        if(collision.gameObject.tag == "Nigeru" || collision.gameObject.tag == "Oni"){
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
