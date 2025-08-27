
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawTableTool : MonoBehaviour
{
    public GridLayoutGroup layout;
    public static DrawTableTool I;
    List<TableBase> allTable = new List<TableBase>();
    // Start is called before the first frame update
    void Start()
    {
        I = this;
       // DrawTable(3,2,200,80,3,Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawTable(float row, float column,float width,float height,float outline,Color color) 
    {
        for (int i = 0; i < row; i++)
        {
            for (int k = 0; k < column; k++)
            {
                GameObject obj = Resources.Load<GameObject>("Game/TablePanle");
                var objs=  Instantiate(obj,transform);
                objs.transform.GetComponent<TableBase>().ColorChange(color,outline,width,height);
                allTable.Add(objs.GetComponent<TableBase>());
                if (k==0)
                {

                    objs.transform.Find("Image_h_Left").GetComponent<RectTransform>().anchoredPosition=new Vector2(-width/2+outline/2,0 );
                }
                if (k==column-1)
                {
                    objs.transform.Find("Image_h_Right").GetComponent<RectTransform>().anchoredPosition = new Vector2(width / 2-outline / 2, 0);
                }
            }
          
        }
        //≈≈–Ú
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = (int)column;
        layout.cellSize = new Vector2(width,height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform.GetComponent<RectTransform>());
    }

    public void ShowText(string [] strs) 
    {
        if (strs.Length!=allTable.Count)
        {
            return;
        }
        for (int i = 0; i < allTable.Count; i++)
        {
            allTable[i].ShowText(strs[i]);
        }
    }

    public void DeleteTable() 
    {
        allTable.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    
    }
}
