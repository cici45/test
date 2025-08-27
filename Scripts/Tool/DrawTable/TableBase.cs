using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableBase : MonoBehaviour
{
    // Start is called before the first frame update
    private Color color;
    private float outline;//√Ë±ﬂ
    private float width, height;
    InputField inputField;
    void Start()
    {
      
    }

    public void SetPositonAndSize(float outline, float width, float height) 
    {
        transform.Find("Image_w_up").GetComponent<RectTransform>().sizeDelta = new Vector2(width, outline);
        transform.Find("Image_w_Down").GetComponent<RectTransform>().sizeDelta = new Vector2(width, outline);
        transform.Find("Image_h_Left").GetComponent<RectTransform>().sizeDelta = new Vector2(height, outline);
        transform.Find("Image_h_Right").GetComponent<RectTransform>().sizeDelta = new Vector2(height, outline);
        transform.Find("InputField").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        //transform.Find("Image_w_up").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height/2-outline/2);
        //transform.Find("Image_w_Down").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(height/2-outline/2));
        transform.Find("Image_w_up").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height/2);
        transform.Find("Image_w_Down").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(height/2));
        transform.Find("Image_h_Left").GetComponent<RectTransform>().anchoredPosition = new Vector2(-width/2, 0);
        transform.Find("Image_h_Right").GetComponent<RectTransform>().anchoredPosition = new Vector2(width/2, 0);
    }

    public void SetFristAndEndPosition() 
    {
        transform.Find("Image_w_up").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height / 2 - outline / 2);
        transform.Find("Image_w_Down").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(height / 2 - outline / 2));
    }
    /// <summary>
    /// —’…´¥¶¿Ì
    /// </summary>

    public void ColorChange(Color color,float outline,float width,float height) 
    {
        this.color = color;
        this.outline = outline;
        this.width = width;
        this.height = height;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color=color;
        }
        SetPositonAndSize(outline,width,height);
    }
    public void ShowText(string text) 
    {
        transform.Find("InputField").GetComponent<InputField>().text=text;
    }
}
