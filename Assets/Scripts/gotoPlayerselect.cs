using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gotoPlayerselect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerselectpanel;
    public GameObject titlemenupanel;
    void Start()
    {
        playerselectpanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void gotoplayerselect(){
        playerselectpanel.SetActive(true);
        titlemenupanel.SetActive(false);
    }
}
