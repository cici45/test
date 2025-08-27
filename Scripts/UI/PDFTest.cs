using UnityEngine;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

public class PDFTest : MonoBehaviour
{
    public static PDFTest test;
    // Use this for initialization
    void Start()
    {
        test = this;
        //PrintPDF();
    }

    void PrintPDF()
    {
        Document document = new Document();
        try
        {
            //���ɵ�λ��C:\Users\123\Desktop\�ļ�
            PdfWriter.GetInstance(document, new FileStream(@"C://Users//123//Desktop//ѵ�������.pdf", FileMode.Create));

            document.Open();
            //document.SetPageSize(PageSize.A4);
            //document.PageCount = 2;
            //��������
            BaseFont bftitle = BaseFont.CreateFont(@"c:\windows\fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(bftitle, 35);


            BaseFont bf1 = BaseFont.CreateFont(@"c:\windows\fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(bf1, 20);
            iTextSharp.text.Font fonttitle10 = new iTextSharp.text.Font(bf1, 18);
            //д����
            Paragraph title = new Paragraph("ѵ������", fonttitle);
            //�������
            title.Alignment = Rectangle.ALIGN_CENTER;
            document.Add(title);
            Paragraph nullLine = new Paragraph(" ", font1);
            document.Add(nullLine);
            //д��һ������, Paragraph
            string UserInfo = "     ����������    ���䣺66    �Ա���";
            string UserInfo2 = "     ���飺111    ҽԺ��000";
            document.Add(new Paragraph(UserInfo, font1));
            document.Add(new Paragraph(UserInfo2, font1));
            document.Add(nullLine);
            //����һ�����У��Էֿ���������
            Paragraph nullp = new Paragraph(" ", fonttitle);
            nullp.Leading = 10;
            document.Add(nullp);
            //���һ��4��
            PdfPTable table = new PdfPTable(4);
            PdfPCell cellname = new PdfPCell(new Phrase("ѵ��ģʽ", fonttitle10));
            //cellname.setMinimumHeight(15)
            PdfPCell celltxtname = new PdfPCell(new Phrase("�ȳ�ѵ��", fonttitle10));
            PdfPCell cellno = new PdfPCell(new Phrase("ʱ��", fonttitle10));
            PdfPCell celltxtno = new PdfPCell(new Phrase("5����", fonttitle10));
            PdfPCell celltime = new PdfPCell(new Phrase("�Ѷ�", fonttitle10));
            PdfPCell celltxttime = new PdfPCell(new Phrase("һ��", fonttitle10));
            PdfPCell cellgrade = new PdfPCell(new Phrase("��ɶ�", fonttitle10));
            PdfPCell celltxtgrade = new PdfPCell(new Phrase("95%", fonttitle10));
            //��Ԫ��ռ������
            //celltxttotaltime.Colspan = 2;
            //���뵥Ԫ������
            table.AddCell(cellname);
            table.AddCell(cellno);
            table.AddCell(celltime);
            table.AddCell(cellgrade);
            table.AddCell(celltxtname);
            table.AddCell(celltxtno);
            table.AddCell(celltxttime);
            table.AddCell(celltxtgrade);

            document.Add(table);
            document.Close();
            Debug.Log("pdf�ļ�д��ɹ�");
        }
        catch (DocumentException de)
        {
            Debug.Log(de.Message);
        }
        catch (IOException io)
        {
            Debug.Log(io.Message);
        }

    }


    public void OnClick(string[] strArr)
    {
        if (strArr.Length == 0 || strArr == null) return;
        Document document = new Document();
        try
        {
            string[] tempArr = UserInfoData.GetUserInfo();
            if (tempArr == null) return;
            //���ɵ�λ��C:\Users\123\Desktop\�ļ�
            PdfWriter.GetInstance(document, new FileStream(@"C://Users//123//Desktop//�ļ�//" + tempArr[1]+"ѵ�������.pdf", FileMode.Create));

            document.Open();
            //document.SetPageSize(PageSize.A4);
            //document.PageCount = 2;
            //��������
            BaseFont bftitle = BaseFont.CreateFont(@"c:\windows\fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(bftitle, 35);


            BaseFont bf1 = BaseFont.CreateFont(@"c:\windows\fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(bf1, 20);
            iTextSharp.text.Font fonttitle10 = new iTextSharp.text.Font(bf1, 12);
            //д��һ������, Paragraph
            document.Add(new Paragraph("��ã� PDFhhhhh !", font1));
            //д����
            Paragraph title = new Paragraph("ѵ������", fonttitle);
            //�������
            title.Alignment = Rectangle.ALIGN_CENTER;
            document.Add(title);

            //����һ�����У��Էֿ���������
            Paragraph nullp = new Paragraph(" ", fonttitle);
            nullp.Leading = 10;
            document.Add(nullp);
            //���һ��4��
            PdfPTable table = new PdfPTable(2);
            PdfPCell cellname = new PdfPCell(new Phrase("����", fonttitle10));
            PdfPCell celltxtname = new PdfPCell(new Phrase(tempArr[0], fonttitle10));
            PdfPCell cellno = new PdfPCell(new Phrase("ID", fonttitle10));
            PdfPCell celltxtno = new PdfPCell(new Phrase(tempArr[1], fonttitle10));
            PdfPCell celltime = new PdfPCell(new Phrase("����", fonttitle10));
            PdfPCell celltxttime = new PdfPCell(new Phrase(tempArr[2], fonttitle10));
            PdfPCell cellgrade = new PdfPCell(new Phrase("�Ա�", fonttitle10));
            PdfPCell celltxtgrade = new PdfPCell(new Phrase(tempArr[3], fonttitle10));
            PdfPCell celltotaltime = new PdfPCell(new Phrase("ʱ��", fonttitle10));
            PdfPCell celltxttotaltime = new PdfPCell(new Phrase("2023-4-20", fonttitle10));
            PdfPCell Text_1 = new PdfPCell(new Phrase("����˶��Ƕ�:", fonttitle10));
            PdfPCell Data_1 = new PdfPCell(new Phrase(strArr[0], fonttitle10));
            PdfPCell Text_2 = new PdfPCell(new Phrase("��һ������˶��Ƕ� ��", fonttitle10));
            PdfPCell Data_2 = new PdfPCell(new Phrase(strArr[1], fonttitle10));
            PdfPCell Text_3 = new PdfPCell(new Phrase("����˶��ٶȣ�", fonttitle10));
            PdfPCell Data_3 = new PdfPCell(new Phrase(strArr[2], fonttitle10));
            PdfPCell Text_4 = new PdfPCell(new Phrase("��һ������˶��ٶȣ�", fonttitle10));
            PdfPCell Data_4 = new PdfPCell(new Phrase(strArr[3], fonttitle10));
            PdfPCell Text_5 = new PdfPCell(new Phrase("�����ٶȣ�", fonttitle10));
            PdfPCell Data_5 = new PdfPCell(new Phrase(strArr[4], fonttitle10));
            PdfPCell Text_6 = new PdfPCell(new Phrase("��һ�������ٶȣ�", fonttitle10));
            PdfPCell Data_6 = new PdfPCell(new Phrase(strArr[5], fonttitle10));
            PdfPCell Text_7 = new PdfPCell(new Phrase("������ɶȣ�", fonttitle10));
            PdfPCell Data_7 = new PdfPCell(new Phrase(strArr[6], fonttitle10));
            PdfPCell Text_8 = new PdfPCell(new Phrase("��һ��������ɶȣ�", fonttitle10));
            PdfPCell Data_8 = new PdfPCell(new Phrase(strArr[7], fonttitle10));
            PdfPCell Text_9 = new PdfPCell(new Phrase("�ۺ�����ֵ��", fonttitle10));
            PdfPCell Data_9 = new PdfPCell(new Phrase(strArr[8], fonttitle10));
            PdfPCell Text_10 = new PdfPCell(new Phrase("��һ���ۺ�����ֵ��", fonttitle10));
            PdfPCell Data_10 = new PdfPCell(new Phrase(strArr[9], fonttitle10));
            //��Ԫ��ռ������
            //celltxttotaltime.Colspan = 2;
            //���뵥Ԫ������
            table.AddCell(cellname);
            table.AddCell(celltxtname);
            table.AddCell(cellno);
            table.AddCell(celltxtno);
            table.AddCell(celltime);
            table.AddCell(celltxttime);
            table.AddCell(cellgrade);
            table.AddCell(celltxtgrade);
            table.AddCell(celltotaltime);
            table.AddCell(celltxttotaltime);
            table.AddCell(Text_1);
            table.AddCell(Data_1);
            table.AddCell(Text_2);
            table.AddCell(Data_2);
            table.AddCell(Text_3);
            table.AddCell(Data_3);
            table.AddCell(Text_4);
            table.AddCell(Data_4);
            table.AddCell(Text_5);
            table.AddCell(Data_5);
            table.AddCell(Text_6);
            table.AddCell(Data_6);
            table.AddCell(Text_7);
            table.AddCell(Data_7);
            table.AddCell(Text_8);
            table.AddCell(Data_8);
            table.AddCell(Text_9);
            table.AddCell(Data_9);
            table.AddCell(Text_10);
            table.AddCell(Data_10);

            ////ͬ��ʽ��չ���
            //for (int i = 0; i < 14; i++)
            //{

            //    table.AddCell(new PdfPCell(new Phrase("��" + i + "������ڵ�", fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    PdfPCell pic = new PdfPCell(new Phrase("��" + i + "������ڵ�Ľ�ͼ", fonttitle10));
            //    pic.Colspan = 2;
            //    table.AddCell(pic);
            //}
            //for (int i = 0; i < 14; i++)
            //{

            //    table.AddCell(new PdfPCell(new Phrase("��" + i + "������ڵ�", fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    PdfPCell pic = new PdfPCell(new Phrase("��" + i + "������ڵ�Ľ�ͼ", fonttitle10));
            //    pic.Colspan = 2;
            //    table.AddCell(pic);
            //}
            document.Add(table);
            document.Close();
            Debug.Log("pdf�ļ�д��ɹ�");
            //Debug.Log(document.PageNumber);
            //Debug.Log(document.PageSize);
            //document.OpenDocument();
        }
        catch (DocumentException de)
        {
            Debug.Log(de.Message);
        }
        catch (IOException io)
        {
            Debug.Log(io.Message);
        }


    }

}