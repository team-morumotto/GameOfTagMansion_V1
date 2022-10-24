using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GotoGameScene : MonoBehaviour
{
    public static int a;
    //タイトルの参加ボタンを押すとシーンが遷移
    public void ChangeScene(int n)
    {
        SceneManager.LoadScene("gameScene",LoadSceneMode.Single);
        a=n;
    }
}
