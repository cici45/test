using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

public class DataUpload : MonoBehaviour
{
    public static DataUpload loadData;

    private void Start()
    {
        loadData = this;
    }

    public string ObjectToJson(object obj)
    {
        // 创建JSON序列化器，传入对象类型
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        // 内存流用于临时存储序列化后的字节数据
        MemoryStream stream = new MemoryStream();
        serializer.WriteObject(stream, obj);//序列化到内存流
        // 将内存流中的字节数据转换为UTF8字符串（JSON格式）
        byte[] dataBytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(dataBytes, 0, (int)stream.Length);
        return Encoding.UTF8.GetString(dataBytes);
    }


    public string Post(string url, object obj)
    {
        string result = "";
        string content = ObjectToJson(obj);
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/json";
        Debug.Log(content);
        #region 添加Post 参数
        byte[] data = Encoding.UTF8.GetBytes(content);
        req.ContentLength = data.Length;
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }
        #endregion

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取响应内容
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }

    public string Post(string url, string content)
    {
        string result = "";
        // 1. 创建一个HTTP请求对象，填写快递单（地址是url）
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";//请求方法有GET和POST，POST适合发送大量数据
        req.ContentType = "application/json";//设置内容类型JSON，告诉后端按这个格式解析

        #region 添加Post 参数
        //准备好要发送的包裹
        byte[] data = Encoding.UTF8.GetBytes(content);//将字符串序列化为字节数组
        req.ContentLength = data.Length;//string类型还要存储长度
        //获取请求流
        //using()在代码块执行完毕后，自动调用资源的 Dispose() 方法释放资源
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);//写入数据（数据内容，读取的起始索引位置，数据长度）
            reqStream.Close();//关闭流,双重保障
        }
        #endregion
        //发送请求并等待后端回复
        //返回值为 WebResponse 类型，通常需要强制转换为 HttpWebResponse，req是url
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取响应内容
        
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();//读取内容
        }
        return result;//返回内容
    }

}

/*数据模型类,相当于快递单模板，后端根据模板进行解析*/
public class PostData_1
{
    public string communityDoctorId;
    public string patientId;
    public string deviceNum;
}
public class PostData_5
{
    //private string deviceNumber;
    public string userId;
}
public class PostData_7
{
    public string name;
    public string userId;
}
[System.Serializable]
public class PostData_6
{
    public string deviceNumber;
    public string userId;
}
[System.Serializable]
public class PostData_6_1
{
    public string deviceNumber;
    public string phone;
}
[System.Serializable]
/// <summary>
/// 名字手机号查找
/// </summary>
public class PostData_3
{
    public string name;

}
[System.Serializable]
public class PostData_3_1
{
    public string phone;

}
public class PostData_2
{
    public string deviceNum;
    public string patientId;
}

[System.Serializable]
public class UserData
{
    public string code;
    public string msg;
    public Data[] data;
}
[System.Serializable]
public class UserData5
{
    public string code;
    public string msg;
    public Data data;
}
[System.Serializable]
public class UserData3
{
    public string code;
    public string msg;
    public Data1 data;

}


[System.Serializable]
public class UserData1
{
    public string code;
    public string msg;
    public string data;
}
[System.Serializable]

public class Data
{
    public string userId;
    public string communityDoctorId;
    public string num;
    public string name;
    public string age;
    public string sex;
    public string phone;
    public string status;
}

[System.Serializable]
public class Data1
{
    public bool hasPrescription; // 是否开具检查或干预方案
    public bool paid; // 是否缴费
    public bool deviceCorrect; // 设备是否正确
    public int trainingType; // 训练类型，0 表示检查训练，1 表示干预训练
    public string value;//提示内容
    public string gear;//档位

}

[System.Serializable]
public class TrainData
{
    public string code;
    public string msg;
    public Data_1[] data;
}

[System.Serializable]
public class Data_1
{
    public string gameName;
    public string equipmentDuration;
    public string difficultyLevel;
    public string maxMontionAngle;
    public string minMontionAngle;
    public string movementTime;
    public string equipment;
    public string type;
}