using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SE : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] buttonSE;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>戻るボタンを押したときに戻るSEを鳴らす</summary>
    public void Button_Back_SE(){
        audioSource.PlayOneShot(buttonSE[0]);
    }

    /// <summary>選ぶ(画面が進む)ボタンを押したときのSEを鳴らす</summary>
    public void Button_Select_SE(){
        audioSource.PlayOneShot(buttonSE[1]);
    }

    public void Button_Member_Select_SE(){
        audioSource.PlayOneShot(buttonSE[2]);
    }
}
