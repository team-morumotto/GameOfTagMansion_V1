using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item01 : MonoBehaviour
{
    private GameObject stateM;
    // Start is called before the first frame update
    void Start()
    {
        stateM = GameObject.Find("StateManeger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        stateM.GetComponent<StateManeger>().kinokoOn();
        Destroy(gameObject);
    }
}
