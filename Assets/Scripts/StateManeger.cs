using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManeger : MonoBehaviour
{
    public bool isSpeedUp = false;
    private float WaitTime = 0;
    void Start() {
        Application.targetFrameRate = 60; //60FPSに固定する
    }

    // Update is called once per frame
    void Update() {
        Application.targetFrameRate = 60; //60FPSに固定する
    }

    public void kinokoOn(){
        StartCoroutine(UpSpeed());
    }

    IEnumerator UpSpeed(){
        isSpeedUp = true;
        WaitTime = 10f;
        yield return new WaitForSeconds(WaitTime);
        isSpeedUp = false;
    }
}
