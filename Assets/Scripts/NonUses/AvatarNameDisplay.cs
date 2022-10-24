/*
    2022/10/18
    Atsuki Kobayashi
*/

using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarNameDisplay : MonoBehaviourPunCallbacks
{
    GameObject Player;
    private void Start() {
        var nameLabel = GetComponent<Text>();
        // プレイヤー名とプレイヤーIDを表示する
        nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
    }
    void Update(){
        Player = transform.root.gameObject;
        if(Player.GetComponent<PhotonView>().IsMine){
            transform.position = Player.transform.position + new Vector3(0, 1.5f, 0);
        }
    }
}