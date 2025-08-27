//消息中心
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : Singleton<MessageCenter>
{
    //委托消息传递
    public delegate void DelMsgDelivery(ParameterData pa);
    //消息中心缓存集合<string: 数据大的分类，DelMsgDelivery：数据执行委托>
    public static Dictionary<string, DelMsgDelivery> dicMsgs = new Dictionary<string, DelMsgDelivery>();
    /// <summary>
    /// 添加消息监听者
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
    /// 移除消息监听者
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
    /// 取消所有指定消息的监听
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
//传递数据类
public class ParameterData
{
    public object data { get; set; }
    public ParameterData(object da)
    {
        data = da;
    }
}
