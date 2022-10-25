using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class oni_sample : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine){
            return;
        }
        var speed = 5f;
        var vx = Input.GetAxis("Horizontal") * speed;
        var vz = Input.GetAxis("Vertical") * speed;

        rb.velocity = new Vector3(vx,rb.velocity.y,vz);

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
