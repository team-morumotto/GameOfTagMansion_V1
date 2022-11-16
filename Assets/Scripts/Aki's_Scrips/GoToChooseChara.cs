using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GoToChooseChara : MonoBehaviourPunCallbacks {
    public static int PlayMode = 1;
    public static int Characters = 0;
    private int choose = 0;
    public GameObject BackObject = null;
    public GameObject NextObject = null;
    public static bool OnEndEditFlg = false;
    public static string Name;

    //タイトルの参加ボタンを押すとシーンが遷移
    public void ChangeScene(int choose) {
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }
    public void isChoosePlayMode(int choose) {
        PlayMode = choose;
        //ボタンで移動させないと初期ボタンの指定ができないので一旦無効化
        //NextObject.SetActive(true);
    }
    public void isBackCharaToPlayMode() {
        NextObject.SetActive(false);
        BackObject.SetActive(true);
    }
    public void isChooseCharacter(int choose) {
        Characters = choose;
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
        OnEndEditFlg = true;//キャラクター選択ボタンが押されたときにサーバーに接続する
    }

    [PunRPC]
    void Oni_Button_Off(){
        GameObject.Find("OniButton").SetActive(false);
    }
}
