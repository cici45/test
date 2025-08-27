using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    Image image;
    void Start()
    {
       image= transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float a= Mathf.PingPong(Time.time,1);
        image.color = new Color(1,1,1,a);
    }

}
