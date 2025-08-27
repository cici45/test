using System;
using System.Data.SqlTypes;
using System.IO.Ports;
using System.Resources;
using System.Xml;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Xml_Read
{
    public int Senser_Type=0;
    public string Com_ID = null;
 
    public int Encorder_ID=0;
    public int Baudrate=0;
    public int Parity_Set = 0;//效验位
    public int Data_Bits = 8;//数据位
    public int Stop_Bits = 1;//停止位

    public long Encorder_Puls = 0;// the total puls counts of one circle
    public int Sampling_Time_ms=50;

    public double Position_Offset=0;
    public int Position_Dir=0;
    public int MutiCircle_Select;

    public int Sensor_1_ID, Sensor_2_ID, Sensor_3_ID, Sensor_4_ID;
    public double Sensor_1_Offset, Sensor_2_Offset, Sensor_3_Offset, Sensor_4_Offset, Force_Ratio,
                  Base_Board_Length, Base_Board_Width, Sensor_1_x, Sensor_1_y, 
                  Sensor_2_x, Sensor_2_y, Sensor_3_x, Sensor_3_y, Sensor_4_x, Sensor_4_y;



    public Xml_Read(string Xml_Name_Read )
	{
        XmlDocument Xml_File = new XmlDocument();
        Xml_File.Load( Xml_Name_Read );
        //Xml_File.LoadXml(Xml_Name_Read.Trim());// Xml_Name_Read);

        //XmlNodeList nodeList = document.SelectSingleNode("Com_Setting").ChildNodes;
        //XmlElement element_read = (XmlElement)nodeList[0];

        //XmlElement element_single = element_read.GetElementsByTagName("");
        //
        // TODO: Add constructor logic here
        //
        XmlNodeList Node_Read = Xml_File.GetElementsByTagName("Com_ID");
        Com_ID = Node_Read[0].InnerText;

         Node_Read = Xml_File.GetElementsByTagName("Senser_Type");
        int.TryParse(Node_Read[0].InnerText, out Senser_Type);


        Node_Read = Xml_File.GetElementsByTagName("Baudrate");
        int.TryParse(Node_Read[0].InnerText, out Baudrate);

        Node_Read = Xml_File.GetElementsByTagName("Data_Bits");
        int.TryParse(Node_Read[0].InnerText, out Data_Bits);

        Node_Read = Xml_File.GetElementsByTagName("Parity_Set");
        int.TryParse(Node_Read[0].InnerText, out Parity_Set);

        Node_Read = Xml_File.GetElementsByTagName("Parity_Set");
        int.TryParse(Node_Read[0].InnerText, out Parity_Set);

        Node_Read = Xml_File.GetElementsByTagName("Stop_Bits");
        int.TryParse(Node_Read[0].InnerText, out Stop_Bits);

        Node_Read = Xml_File.GetElementsByTagName("Sampling_Time_ms");
        int.TryParse(Node_Read[0].InnerText, out Sampling_Time_ms);

        Node_Read = Xml_File.GetElementsByTagName("Encorder_Puls");
        long.TryParse(Node_Read[0].InnerText, out Encorder_Puls);

        Node_Read = Xml_File.GetElementsByTagName("Encorder_ID");
        int.TryParse(Node_Read[0].InnerText, out Encorder_ID);


        Node_Read = Xml_File.GetElementsByTagName("Position_Dir");
        int.TryParse(Node_Read[0].InnerText, out Position_Dir);

        Node_Read = Xml_File.GetElementsByTagName("Position_Offset");
        double.TryParse(Node_Read[0].InnerText, out Position_Offset);

         Node_Read = Xml_File.GetElementsByTagName("MutiCircle_Select");
        int.TryParse(Node_Read[0].InnerText, out MutiCircle_Select);

        Node_Read = Xml_File.GetElementsByTagName("Sensor_1_ID");
        int.TryParse(Node_Read[0].InnerText, out Sensor_1_ID);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_2_ID");
        int.TryParse(Node_Read[0].InnerText, out Sensor_2_ID);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_3_ID");
        int.TryParse(Node_Read[0].InnerText, out Sensor_3_ID);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_4_ID");
        int.TryParse(Node_Read[0].InnerText, out Sensor_4_ID);


        Node_Read = Xml_File.GetElementsByTagName("Sensor_1_Offset");
        double.TryParse(Node_Read[0].InnerText, out Sensor_1_Offset);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_2_Offset");
        double.TryParse(Node_Read[0].InnerText, out Sensor_2_Offset);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_3_Offset");
        double.TryParse(Node_Read[0].InnerText, out Sensor_3_Offset);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_4_Offset");
        double.TryParse(Node_Read[0].InnerText, out Sensor_4_Offset);

        Node_Read = Xml_File.GetElementsByTagName("Force_Ratio");
        double.TryParse(Node_Read[0].InnerText, out Force_Ratio);
        Node_Read = Xml_File.GetElementsByTagName("Base_Board_Length");
        double.TryParse(Node_Read[0].InnerText, out Base_Board_Length);
        Node_Read = Xml_File.GetElementsByTagName("Base_Board_Width");
        double.TryParse(Node_Read[0].InnerText, out Base_Board_Width);


        Node_Read = Xml_File.GetElementsByTagName("Sensor_1_x");
        double.TryParse(Node_Read[0].InnerText, out Sensor_1_x);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_2_x");
        double.TryParse(Node_Read[0].InnerText, out Sensor_2_x);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_3_x");
        double.TryParse(Node_Read[0].InnerText, out Sensor_3_x);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_4_x");
        double.TryParse(Node_Read[0].InnerText, out Sensor_4_x);

        Node_Read = Xml_File.GetElementsByTagName("Sensor_1_y");
        double.TryParse(Node_Read[0].InnerText, out Sensor_1_y);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_2_y");
        double.TryParse(Node_Read[0].InnerText, out Sensor_2_y);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_3_y");
        double.TryParse(Node_Read[0].InnerText, out Sensor_3_y);
        Node_Read = Xml_File.GetElementsByTagName("Sensor_4_y");
        double.TryParse(Node_Read[0].InnerText, out Sensor_4_y);



    }
}
