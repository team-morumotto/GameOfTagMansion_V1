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
    void Start(){
        SetNamePanel.SetActive(true);
        SetNameIF.onEndEdit.AddListener(delegate {SetNameAfter(SetNameIF.text);});
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.F)){
        SetNamePanel.SetActive(true);
        }
    }
    void SetNameAfter(string name){
        NAME = name;
        Connect.SetActive(true);
        SetNamePanel.SetActive(false);
    }
}