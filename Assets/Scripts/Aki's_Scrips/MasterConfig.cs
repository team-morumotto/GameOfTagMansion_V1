using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace MasterConfig{
    public class MasterConfig : MonoBehaviourPunCallbacks
    {
        public GameObject[] SpawnPoint;//キャラクターのステージスポーンポイント
        public float currentTime;
        public GameObject[] list = {null,null,null,null};
        public Text Text;
        public static bool GameTimeStartFlg = false;

        public int j = 0;

        //残り人数生やす用
        public Text peopletext;
        private bool Maxpeople = false;
        
        void Update(){
            if(!photonView.IsMine){
                return;
            }

            Numberofpeopleleft();
        }

        //残り人数の反映
        private void Numberofpeopleleft(){
            if(PhotonNetwork.PlayerList.Length == 4){
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
}