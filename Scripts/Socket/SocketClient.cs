using UnityEngine.UI;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine.SceneManagement;

public class SocketClient : MonoBehaviour
{
    //���������ť���ڷ������ݺ͹ر�����
    public Button sen;
    public Button col;
    //�����ı�
    public InputField inputText;
    // Start is called before the first frame update
    void Start()
    {
        ConnectServer();
        sen.onClick.AddListener(send_smg);//����
        col.onClick.AddListener(close_btnClick);//�ر�
    }

    bool ISoPEN = false;
    /// <summary>
    /// �򿪿ͻ�������
    /// </summary>
    public void send_smg()
    {
        Send(inputText.text);
        if (!ISoPEN)
        {
            ISoPEN = true;
            MessageCenter.SendMsg("ReceiveData", true);
        }else if (ISoPEN)
        {
            MessageCenter.SendMsg("ReceiveData", false);
        }
        
        //Debug.Log("pppp");
    }
    /// <summary>
    /// �رտͻ�������
    /// </summary>
    public void close_btnClick()
    {
        //MessageCenter.SendMsg("ReceiveData", true);
        //close();
    }

    /// <summary>
    /// ���ӷ�����
    /// </summary>
    static Socket socket_client;
    public static void ConnectServer()
    {
        try
        {
            IPAddress pAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint pEndPoint = new IPEndPoint(pAddress, 52315);
            socket_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket_client.Connect(pEndPoint);
            Debug.Log("���ӳɹ�");
            //�����̣߳�ִ�ж�ȡ��������Ϣ
            Thread c_thread = new Thread(Received);
            c_thread.IsBackground = true;
            c_thread.Start();
        }
        catch (System.Exception)
        {

            Debug.LogError("IP�˿ںŴ�����߷�����δ����");
        }
    }

    /// <summary>
    /// ��ȡ��������Ϣ
    /// </summary>
    public static void Received()
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int len = socket_client.Receive(buffer);
                if (len == 0) break;
                string str = Encoding.UTF8.GetString(buffer, 0, len);
                //Debug.Log("�ͻ��˴�ӡ������������Ϣ��" + socket_client.RemoteEndPoint + ":" + str);
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="msg"></param>
    public static void Send(string msg)
    {
        try
        {
            byte[] buffer = new byte[1024];
            buffer = Encoding.UTF8.GetBytes(msg);
            socket_client.Send(buffer);
        }
        catch (System.Exception)
        {

            Debug.Log("δ����");
        }
    }
    /// <summary>
    /// �ر�����
    /// </summary>
    public static void close()
    {
        try
        {
            socket_client.Close();
            Debug.Log("�رտͻ�������");
        }
        catch (System.Exception)
        {
            Debug.LogError("δ����");
        }
    }
}