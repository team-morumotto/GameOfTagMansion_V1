using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gotoPlayerselect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayerSelectPanel;
    public GameObject TitleMenuPanel;
    void Start() {
        PlayerSelectPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }
    public void gotoplayerselect(){
        PlayerSelectPanel.SetActive(true);
        TitleMenuPanel.SetActive(false);
    }
}
