using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCol : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMove>() != null)
        {
            //Debug.Log("±»Åö×²£¡");
            //EnemyPool.instance.Put(collision.gameObject);
            collision.GetComponent<EnemyMove>().startFlash();
            transform.parent.SendMessage("setCollide", true);
        }      
    }
}
