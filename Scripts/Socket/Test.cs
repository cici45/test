using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public struct model
    {
        public int count;
        public string name;
        public model(int count, string name)
        {
            this.count = count;
            this.name = name;
        }
    }

    List<model> list=new List<model>();
    List<int> list2=new List<int>();
    // Start is called before the first frame update
    void Start()
    { float a=0;
        for (int i = 0; i < 4; i++)
        {
            list.Add(new model(i, i.ToString() + "0"));
            list2.Add(i);
        }
        DOTween.To(()=>a,e=>a= e,2,2);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            
        //    Debug.LogError(list.Find((e) => e.name == "30").count);
        //    Debug.LogError(list2.Find((e) => e == 0));
        //    Debug.LogError(list.FindAll((e) => e.name == "30")[0].count);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        Debug.LogError(list[i].name);
        //        Debug.LogError(list[i].count);

        //    }
        //}
    }
}
