using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GoToChooseChara : MonoBehaviour {
    public static int PlayMode = 1;
    public static int Characters = 0;
    private int choose = 0;
    public GameObject NextObject = null;

    //タイトルの参加ボタンを押すとシーンが遷移
    public void ChangeScene(int choose) {
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }
    public void isChoosePlayMode(int choose) {
        PlayMode = choose;
        NextObject.SetActive(true);
    }

    public void isChooseCharacter(int choose) {
        Characters = choose;
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }
}
