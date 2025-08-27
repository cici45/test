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
        // ����JSON���л����������������
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        // �ڴ���������ʱ�洢���л�����ֽ�����
        MemoryStream stream = new MemoryStream();
        serializer.WriteObject(stream, obj);//���л����ڴ���
        // ���ڴ����е��ֽ�����ת��ΪUTF8�ַ�����JSON��ʽ��
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
        #region ���Post ����
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
        //��ȡ��Ӧ����
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }

    public string Post(string url, string content)
    {
        string result = "";
        // 1. ����һ��HTTP���������д��ݵ�����ַ��url��
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";//���󷽷���GET��POST��POST�ʺϷ��ʹ�������
        req.ContentType = "application/json";//������������JSON�����ߺ�˰������ʽ����

        #region ���Post ����
        //׼����Ҫ���͵İ���
        byte[] data = Encoding.UTF8.GetBytes(content);//���ַ������л�Ϊ�ֽ�����
        req.ContentLength = data.Length;//string���ͻ�Ҫ�洢����
        //��ȡ������
        //using()�ڴ����ִ����Ϻ��Զ�������Դ�� Dispose() �����ͷ���Դ
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);//д�����ݣ��������ݣ���ȡ����ʼ����λ�ã����ݳ��ȣ�
            reqStream.Close();//�ر���,˫�ر���
        }
        #endregion
        //�������󲢵ȴ���˻ظ�
        //����ֵΪ WebResponse ���ͣ�ͨ����Ҫǿ��ת��Ϊ HttpWebResponse��req��url
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //��ȡ��Ӧ����
        
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();//��ȡ����
        }
        return result;//��������
    }

}

/*����ģ����,�൱�ڿ�ݵ�ģ�壬��˸���ģ����н���*/
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
/// �����ֻ��Ų���
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
    public bool hasPrescription; // �Ƿ񿪾߼����Ԥ����
    public bool paid; // �Ƿ�ɷ�
    public bool deviceCorrect; // �豸�Ƿ���ȷ
    public int trainingType; // ѵ�����ͣ�0 ��ʾ���ѵ����1 ��ʾ��Ԥѵ��
    public string value;//��ʾ����
    public string gear;//��λ

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