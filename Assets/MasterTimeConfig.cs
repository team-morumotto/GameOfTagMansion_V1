using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MasterTimeConfig : MonoBehaviourPunCallbacks
{
    public Text Text;
    // Start is called before the first frame update
    void Start()
    {
        Text = GameObject.Find("/Canvas").transform.Find("Time").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(RandomMatchMaker.GameStartFlg){
            Debug.Log("GameStart");
            photonView.RPC(nameof(Time),RpcTarget.All);
        }
    }
}
