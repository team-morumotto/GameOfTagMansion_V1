using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoTitleScene : MonoBehaviour
{
    public void GotoTitle(){
        SceneManager.LoadScene("TitleScene",LoadSceneMode.Single);
    }
    public void exeend(){
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
