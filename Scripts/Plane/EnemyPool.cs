using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;      
    List<GameObject> objectPool = new List<GameObject>();
    List<EnemyMove> objCtrlPool = new List<EnemyMove>();
    GameObject enemyPrefab;
    int currentCount;//记录显示存活数量
    int maxCount = 5;//可变化
    void Awake()
    {
        instance = this;
        enemyPrefab = Resources.Load<GameObject>("Plane/Enemy");
        GameObject emy;//初始化
        for (int i = 0; i < maxCount; i++)
        {
            emy = Instantiate(enemyPrefab, this.transform);
            emy.transform.SetParent(this.transform);
            objectPool.Add(emy);
            objCtrlPool.Add(emy.GetComponent<EnemyMove>());
            emy.SetActive(false);
        }
    }
    public void setMove(bool value)
    {
        foreach (EnemyMove obj in objCtrlPool)
        {
            obj.setMove(value);
        }
    }
    public void setInterval(float value)
    {
        foreach (EnemyMove obj in objCtrlPool)
        {
            obj.setInterval(value);
        }
    }
    public void Clear()
    {
        foreach (GameObject obj in objectPool)
        {
            if (obj.activeInHierarchy) obj.SetActive(false);
            currentCount = 0;
        }
    }
    public GameObject Get(float interval)//取物体=>主游戏调用,设置位置和速度
    {
        GameObject tmp = null;
        if (currentCount < maxCount)
        {
            for (int i = 0; i < objectPool.Count; i++)
            {
                if (!objectPool[i].activeInHierarchy)
                {
                    tmp = objectPool[i];
                    tmp.GetComponent<EnemyMove>().setInterval(interval);                  
                    ++currentCount;
                    return tmp;
                }
            }
        }
        tmp = Instantiate(enemyPrefab, this.transform);
        tmp.transform.SetParent(this.transform);
        tmp.GetComponent<EnemyMove>().setInterval(interval);
        tmp.SetActive(false);
        objectPool.Add(tmp);
        objCtrlPool.Add(tmp.GetComponent<EnemyMove>());       
        ++maxCount;
        ++currentCount;
        return tmp;
    }
    public void Put(GameObject obj)//放物体=>主游戏和怪物自身调
    {
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(false);       
        --currentCount;
    }
    IEnumerator Create()
    {
        yield return null;
    }
}
