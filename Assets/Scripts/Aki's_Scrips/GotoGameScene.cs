using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GotoGameScene_Oni : MonoBehaviour
{
    public static int b=1;
    //タイトルの参加ボタンを押すとシーンが遷移
    public void ChangeScene()
    {
        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }
}
