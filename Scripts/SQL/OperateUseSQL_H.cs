using iTextSharp.text.pdf.parser.clipper;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OperateUseSQL_H
{
   // private OperateUseSQL instance;
    //public OperateUseSQL Instance { get { return instance; } }
    static SqlHelper_H db;
    static string dbpath = Application.streamingAssetsPath + "/UserInfo2.db";
    
    public OperateUseSQL_H(string connectionString)
    {
       
    }

    public static void OpenSQL()
    {
        string path = "data source=" + dbpath;
        db = new SqlHelper_H(path);
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <param name="colName">字段名</param>
    /// <param name="dataValues">数据</param>
    public static void Add_Data(string TableName, string[] colName, List<string[]> dataValues)
    {
        
        
        try
        {
            if (IsTbleExist(TableName))
            {
                for(int dataIndex = 0; dataIndex < dataValues.Count; dataIndex++)
                {
                    db.InsertValues(TableName, dataValues[dataIndex]);
                }
            }
            else if (!IsTbleExist(TableName))
            {
                string[] coltype = new string[] { };
                ArrayList type = new ArrayList(coltype.ToList());
                for (int colindex = 0; colindex < colName.Length; colindex++)
                {
                    type.Add("TEXT");
                }
                coltype = (string[])type.ToArray(typeof(string));
                db.CreateTable(TableName, colName, coltype);
                for (int dataIndex = 0; dataIndex < dataValues.Count; dataIndex++)
                {
                    db.InsertValues(TableName, dataValues[dataIndex]);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }

        //db.CloseConnection();
    }
   
    /// <summary>
    /// 读取表格数据
    /// </summary>
    /// <param name="TableName"></param>
    /// <param name="data"></param>
    /// <param name="n"></param>
    public static void Read_Data(string TableName, out List<string[]> data,out int n)
    {
        data = new List<string[]>();
        n = 0;
        //string path = "data source=" + dbpath;
        //db = new SqlHelper_H(path);

        try
        {
            string[] inser = new string[] { };
            ArrayList inserarrayList = new ArrayList(inser.ToList());
            if (IsTbleExist(TableName))
            {
                n = 0;
                SqliteDataReader sqreader = db.ReadFullTable(TableName);
                while (sqreader.Read())
                {
                    n++;
                    for (int index = 0; index < sqreader.FieldCount; index++)
                    {
                        inserarrayList.Add(sqreader.GetFieldValue<String>(index));
                    }
                    inser = (string[])inserarrayList.ToArray(typeof(string));
                    data.Add(inser);
                    inser.Clone();
                    inserarrayList.Clear();
                }
            }
            else
            {
                n = 0;
                data = null;
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }

        //db.CloseConnection();

    }
    /// <summary>
    /// 单个数据更新
    /// </summary>
    /// <param name="TableName"></param>
    /// <param name="colname"></param>
    /// <param name="newValues"></param>
    /// <param name="ID"></param>
    public static void UpdataData(string tableName, string selectKey, string selectValue, string selectKey1, string selectValue1, string colNames, string colValues)
    {
        db.UpdateOneData(tableName, selectKey, selectValue, selectKey1, selectValue1, colNames, colValues);
    }

    public static void Updata_Data(string TableName, string[] colname, string[] newValues, string ID)
    {

        if (IsTbleExist(TableName))
        {
            db.UpdateValues(TableName, colname, newValues, "UserID", ID);

        }
        else
        {
            return;
        }

    }
    
    /// <summary>
    /// 单行数据删除
    /// </summary>
    /// <param name="TableName"></param>
    /// <param name="colname"></param>
    /// <param name="Values"></param>
    public static void Delete_DataValues(string TableName,string[] colname, string[] Values)
    {
        //string path = "data source=" + dbpath;
        //db = new SqlHelper_H(path);
        string[] op = new string[] { "= " };
        if (IsTbleExist(TableName))
        {
            db.DeleteValuesOR(TableName, colname, op, Values);
        }
        else
        {
            return;
        }
        
    }

    public static void Delete_DataAllValues(string TableName)
    {
        //string path = "data source=" + dbpath;
        //db = new SqlHelper_H(path);
        if (IsTbleExist(TableName))
        {
            db.DeleteTable(TableName);
            
            return;
        }
        else
        {
            Debug.Log("没有该表格");
            return;
        }
            
        
    }

    /// <summary>
    /// 整个数据表删除（连同表的定义）
    /// </summary>
    /// <param name="TableName"></param>
    public static void Delete_Table(string TableName)
    {
        
        //new WaitForSeconds(1f);
        db.CloseConnection();
        string path = "data source=" + dbpath;
        db = new SqlHelper_H(path);
        db.DropTable(TableName);
        Debug.Log("删除成功");
        db.CloseConnection();
        //string path = "data source=" + dbpath;
        db = new SqlHelper_H(path);
    }

    /// <summary>
    /// 判断表格是否存在
    /// </summary>
    /// <param name="tablename"></param>
    /// <returns></returns>
    public static bool IsTbleExist(string tablename)
    {
        //string path = "data source=" + dbpath;
        //db = new SqlHelper_H(path);
        string queryString = "select name from sqlite_master where type='table'";
        var a = db.ExecuteQuery(queryString);
        while (a.Read())
        {
            if (a.GetString(0) == tablename)
            {
                return true;
            }
        }
        //db.CloseConnection();
        return false;
    }

    public static bool IsHavaData(string id, string tableName)
    {
        string queryString = "Select " + "Identifying" + " From " + tableName + " Where " + "Identifying" + " = '" + id + "'";
        //string comm = "Select " + "Identifying" + " From " + tableName + " Where " + "Identifying" + " = '" + id + "'";
        var a = db.ExecuteQuery(queryString);
        string data = a.GetValue(0).ToString();
        //Debug.Log("读取的数据："+data);
        if (data == id)
        {
            //db.CloseConnection();
            return true;
        }
        else
        {
            
            //db.CloseConnection();
            return false;
        }
    }

}
