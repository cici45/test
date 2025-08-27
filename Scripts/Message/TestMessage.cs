//≤‚ ‘
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessageCenter.AddMsgListener("ExplosionEvent", Explosin);
    }
    void Explosin(ParameterData pa)
    {
        bool value = (bool)pa.data;
        Debug.Log(value);
        //Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        MessageCenter.RemoveMsgListener("ExplosionEvent", Explosin);
    }
}



