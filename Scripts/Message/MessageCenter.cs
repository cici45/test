//��Ϣ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : Singleton<MessageCenter>
{
    //ί����Ϣ����
    public delegate void DelMsgDelivery(ParameterData pa);
    //��Ϣ���Ļ��漯��<string: ���ݴ�ķ��࣬DelMsgDelivery������ִ��ί��>
    public static Dictionary<string, DelMsgDelivery> dicMsgs = new Dictionary<string, DelMsgDelivery>();
    /// <summary>
    /// �����Ϣ������
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="handler"></param>
    public static void AddMsgListener(string msgType, DelMsgDelivery handler)
    {
        if (!dicMsgs.ContainsKey(msgType))
        {
            dicMsgs.Add(msgType, null);
        }
        dicMsgs[msgType] += handler;
    }
    /// <summary>
    /// �Ƴ���Ϣ������
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="handler"></param>
    public static void RemoveMsgListener(string msgType, DelMsgDelivery handler)
    {
        if (dicMsgs.ContainsKey(msgType))
        {
            dicMsgs[msgType] -= handler;
        }
    }
    /// <summary>
    /// ȡ������ָ����Ϣ�ļ���
    /// </summary>
    public static void ClearAllMsgListener()
    {
        if (dicMsgs != null)
        {
            dicMsgs.Clear();
        }
    }
    public static void SendMsg(string msgType, ParameterData pa)
    {
        DelMsgDelivery del;
        if (dicMsgs.TryGetValue(msgType, out del))
        {
            if (del != null)
            {
                del(pa);
            }
        }
    }
    public static void SendMsg(string msgType)
    {
        SendMsg(msgType, null);
    }
    public static void SendMsg(string msgType, bool v)
    {
        SendMsg(msgType, new ParameterData(v));
    }
}
//����������
public class ParameterData
{
    public object data { get; set; }
    public ParameterData(object da)
    {
        data = da;
    }
}
