
/*
    2022/10/18
    Atsuki Kobayashi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Character : MonoBehaviourPunCallbacks
{
    public float speed = 5f;
    void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            var y = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            transform.Translate(x, 0, y);
            string gomi = PhotonNetwork.LocalPlayer.UserId.ToString();
            Debug.Log("asa");
        }
    }
}
