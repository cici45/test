using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyMove : MonoBehaviour
{
    bool isMove;
    float interval = 10;  
    float durTime;
    bool isShow;
    Image img;
    void Awake()
    {
        img = transform.GetComponent<Image>();
    }
    void OnEnable()
    {
        StartCoroutine("Move");
    }
    void OnDisable()
    {       
        StopCoroutine("Move");
        StopCoroutine("Flash");
        img.color = new Color(img.color.r, img.color.g, img.color.b, 255);
    }
    public void startFlash()
    {
        StartCoroutine("Flash");
    }
    public void setInterval(float value)
    {
        interval = value;
    }
    public void setMove(bool value)
    {
        isMove = value;
    }
    IEnumerator Move()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + Vector3.left * 1500;
        while (true)
        {
            if (isMove)
            {
                if (transform.localPosition.x <= -1640) EnemyPool.instance.Put(this.transform.gameObject);
                transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition - Vector3.right * 400 * interval, Time.deltaTime);

                durTime += Time.deltaTime;
                if (durTime < interval)
                {
                    transform.localPosition = Vector3.Lerp(startPos, endPos, durTime / interval);
                }
                else
                {
                    durTime = 0;
                    EnemyPool.instance.Put(this.transform.gameObject);
                    yield break;
                }
            }            
            yield return null;
        }
    }
    IEnumerator Flash()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (isMove)
            {
                if (img.color.a == 0) isShow = false;
                if (img.color.a == 255) isShow = true;
                if (isShow) img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                else
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 255);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        yield break;
    }
}
