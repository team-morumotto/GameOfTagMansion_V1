/*### CREATOR #####
2022/10/18
Atsuki Kobayashi

2022/10/18 - Edit and modify by Rikuto Kashiwaya.
##### ｺｺﾏﾃﾞCREATOR ###*/

/*### README #####
PhotonNetwork.LocalPlayer.NickName  ... ローカルプレイヤーのニックネーム
PhotonNetwork.NickName              ... プレイヤーのニックネーム情報格納
photonView.Owner.NickName           ... プレイヤーのニックネームを表示

Powered by トマシープが学ぶ Unity/VR/AR が好きなミーハー人間のメモ
https://bibinbaleo.hatenablog.com/entry/2019/09/06/131024
##### ｺｺﾏﾃﾞREADME ###*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class DisplayName : MonoBehaviourPunCallbacks {
    public static TextMeshProUGUI NickName;
    string Name;
    void Start(){
        NickName = GetComponentInChildren<TextMeshProUGUI>();
        NickName.text = $"{"Player"}({photonView.OwnerActorNr})";//PhotonNetwork.LocalPlayer.NickNameを自分のオブジェクトの子になっているtextMeshオブジェクトに入れる
    }

    void Update() {
        GetComponent<RectTransform>().transform.LookAt(Camera.main.transform);//誰から見ても常に自分の名前が正面に見えるようにする
    }
}