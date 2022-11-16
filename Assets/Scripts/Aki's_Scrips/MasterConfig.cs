using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MasterConfig : MonoBehaviourPunCallbacks {
    //## Unity オブジェクトリスト ##//
    public GameObject[] SpawnPoint;                     //キャラクターのステージスポーンポイント
    public float currentTime;
    public GameObject[] list = {null,null,null,null};
    public Text Text;

    //## ワールド等外部的変数 ##//
    public static bool GameTimeStartFlg = false;
    public int j = 0;                                   //残り人数生やす用？
    public Text peopletext;
    private bool Maxpeople = false;
    public static float GameTimer = 60.0f;             // カウントダウンの時間(ローカル時間が引かれるため可変)

    public static string SpawnWorld = "/Mansion";
    void Start() {
        Application.targetFrameRate = 60; // 60FPSに固定する
    }

    void Update(){
        Application.targetFrameRate = 60; // 60FPSに固定する
        if(!photonView.IsMine){
            return;
        }

        photonView.RPC(nameof(Numberofpeopleleft),RpcTarget.All);
    }

    //残り人数の反映
    [PunRPC]
    private void Numberofpeopleleft(){
        if(PhotonNetwork.PlayerList.Length == 2){
            Maxpeople = true;
        }
        if(Maxpeople){
            peopletext.text = "残り人数:" + (PhotonNetwork.PlayerList.Length - 1)+"人";
        }
        else{
            peopletext.text = "残り人数:" + (PhotonNetwork.PlayerList.Length)+"人";
        }
    }
}