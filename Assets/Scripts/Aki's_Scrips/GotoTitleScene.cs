using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoTitleScene : MonoBehaviour
{
    public void GotoTitle(){
        SceneManager.LoadScene("TitleScene",LoadSceneMode.Single);
    }
}
