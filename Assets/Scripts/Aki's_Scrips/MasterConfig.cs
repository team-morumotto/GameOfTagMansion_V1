using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MasterConfig : MonoBehaviourPunCallbacks
{
    public static GameObject SpawnPoint;
    public float currentTime;
    public Text Text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameStart();
        Debug.Log(currentTime);

        if(RandomMatchMaker.GameStartFlg == true){
            GameStart();
        }
    }

    void GameStart(){
        currentTime = PhotonNetwork.ServerTimestamp;
        Text.text = currentTime.ToString();
    }
}
