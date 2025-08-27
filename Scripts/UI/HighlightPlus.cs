
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightPlus : MonoBehaviour
{

   public Color color;
   Image image;
   void Start()
   {
       image = transform.GetComponent<Image>();
   }

   // Update is called once per frame
   void Update()
   {
       float a = Mathf.PingPong(Time.time, 1);
       color.a = a;
       image.color = color;
   }


}
