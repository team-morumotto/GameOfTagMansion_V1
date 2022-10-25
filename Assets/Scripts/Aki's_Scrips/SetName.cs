/*

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SetName : MonoBehaviourPunCallbacks
{
    public GameObject SetNamePanel;
    public static string NAME;
    public InputField SetNameIF;
    public GameObject Connect;
    public static bool onEndEditFLG;
    void Start(){
        SetNamePanel.SetActive(true);//名前入力パネルON
        SetNameIF.onEndEdit.AddListener(delegate {SetNameAfter(SetNameIF.text);});//InputField入力後にSetNameAfter関数を実行
    }
    void SetNameAfter(string name){
        onEndEditFLG = true;//名前入力が行われたらRandomMatchMakerスクリプトのConnect関数を実行するためのフラグ
        NAME = name;//サーバーに接続した後に生成されるキャラクターに名前を付けるために名前を受け取る(UnityChanのデフォルトRgidスクリプトで取得)
        Connect.SetActive(true);//netWorkMakerオブジェクトをtrue
        SetNamePanel.SetActive(false);//名前入力パネルOFF
    }
}