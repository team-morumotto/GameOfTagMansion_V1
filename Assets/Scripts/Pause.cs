using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update

private bool flag = false;

    [SerializeField] private GameObject PauseM;
    void Start()
    {
        flag = false;
        PauseM.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && flag == false){
            PauseM.SetActive(true);
            flag = true;
            Debug.Log(flag);
        }

        else if (Input.GetKey(KeyCode.Space) && flag == true){
            PauseM.SetActive(false);
            flag = false;
        }
    }
}
