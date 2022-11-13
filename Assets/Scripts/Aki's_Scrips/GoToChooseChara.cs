using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GoToChooseChara : MonoBehaviour {
    public static int PlayMode = 1;
    public static int Characters = 0;
    private int choose = 0;
    public static bool onEndEditFLG;
    public GameObject BackObject = null;
    public GameObject NextObject = null;

    //タイトルの参加ボタンを押すとシーンが遷移
    public void ChangeScene(int choose) {
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }
    public void isChoosePlayMode(int choose) {
        PlayMode = choose;
        NextObject.SetActive(true);
    }
    public void isBackCharaToPlayMode() {
        NextObject.SetActive(false);
        BackObject.SetActive(true);
    }
    public void isChooseCharacter(int choose) {
        Characters = choose;
        onEndEditFLG = true;//名前入力が行われたらRandomMatchMakerスクリプトのConnect関数を実行するためのフラグ
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }
}
