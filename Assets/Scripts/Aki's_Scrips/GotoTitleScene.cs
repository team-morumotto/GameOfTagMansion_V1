using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoTitleScene : MonoBehaviour
{
    [SerializeField] public GameObject ThanksPanel;
    public void GotoTitle(){
        SceneManager.LoadScene("TitleScene",LoadSceneMode.Single);
    }

    /// <summary>ヨコアリくん祭りにおける「ありがとう」の表示</summary>
    public void Go_To_Thanks_Panel(){
        ThanksPanel = GameObject.Find("Canvas").transform.Find("Result_PanelLite").gameObject;
        ThanksPanel.SetActive(true);
    }

    public static void exeend(){
        //エディタの場合
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        //ビルドの場合
        #else
            Application.Quit();//ゲームプレイ終了
        //なんかあったとき
        #endif
    }
}