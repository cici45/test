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
            //生成的位置C:\Users\123\Desktop\文件
            PdfWriter.GetInstance(document, new FileStream(@"C://Users//123//Desktop//训练结果单.pdf", FileMode.Create));

            document.Open();
            //document.SetPageSize(PageSize.A4);
            //document.PageCount = 2;
            //标题字体
            BaseFont bftitle = BaseFont.CreateFont(@"c:\windows\fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(bftitle, 35);


            BaseFont bf1 = BaseFont.CreateFont(@"c:\windows\fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(bf1, 20);
            iTextSharp.text.Font fonttitle10 = new iTextSharp.text.Font(bf1, 18);
            //写标题
            Paragraph title = new Paragraph("训练报表", fonttitle);
            //标题居中
            title.Alignment = Rectangle.ALIGN_CENTER;
            document.Add(title);
            Paragraph nullLine = new Paragraph(" ", font1);
            document.Add(nullLine);
            //写入一个段落, Paragraph
            string UserInfo = "     姓名：张三    年龄：66    性别：男";
            string UserInfo2 = "     病情：111    医院：000";
            document.Add(new Paragraph(UserInfo, font1));
            document.Add(new Paragraph(UserInfo2, font1));
            document.Add(nullLine);
            //输入一个空行，以分开标题与表格
            Paragraph nullp = new Paragraph(" ", fonttitle);
            nullp.Leading = 10;
            document.Add(nullp);
            //表格一共4列
            PdfPTable table = new PdfPTable(4);
            PdfPCell cellname = new PdfPCell(new Phrase("训练模式", fonttitle10));
            //cellname.setMinimumHeight(15)
            PdfPCell celltxtname = new PdfPCell(new Phrase("等长训练", fonttitle10));
            PdfPCell cellno = new PdfPCell(new Phrase("时长", fonttitle10));
            PdfPCell celltxtno = new PdfPCell(new Phrase("5分钟", fonttitle10));
            PdfPCell celltime = new PdfPCell(new Phrase("难度", fonttitle10));
            PdfPCell celltxttime = new PdfPCell(new Phrase("一般", fonttitle10));
            PdfPCell cellgrade = new PdfPCell(new Phrase("完成度", fonttitle10));
            PdfPCell celltxtgrade = new PdfPCell(new Phrase("95%", fonttitle10));
            //单元格占用两格
            //celltxttotaltime.Colspan = 2;
            //填入单元格内容
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
            Debug.Log("pdf文件写入成功");
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
            //生成的位置C:\Users\123\Desktop\文件
            PdfWriter.GetInstance(document, new FileStream(@"C://Users//123//Desktop//文件//" + tempArr[1]+"训练结果单.pdf", FileMode.Create));

            document.Open();
            //document.SetPageSize(PageSize.A4);
            //document.PageCount = 2;
            //标题字体
            BaseFont bftitle = BaseFont.CreateFont(@"c:\windows\fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(bftitle, 35);


            BaseFont bf1 = BaseFont.CreateFont(@"c:\windows\fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font1 = new iTextSharp.text.Font(bf1, 20);
            iTextSharp.text.Font fonttitle10 = new iTextSharp.text.Font(bf1, 12);
            //写入一个段落, Paragraph
            document.Add(new Paragraph("你好， PDFhhhhh !", font1));
            //写标题
            Paragraph title = new Paragraph("训练报表", fonttitle);
            //标题居中
            title.Alignment = Rectangle.ALIGN_CENTER;
            document.Add(title);

            //输入一个空行，以分开标题与表格
            Paragraph nullp = new Paragraph(" ", fonttitle);
            nullp.Leading = 10;
            document.Add(nullp);
            //表格一共4列
            PdfPTable table = new PdfPTable(2);
            PdfPCell cellname = new PdfPCell(new Phrase("姓名", fonttitle10));
            PdfPCell celltxtname = new PdfPCell(new Phrase(tempArr[0], fonttitle10));
            PdfPCell cellno = new PdfPCell(new Phrase("ID", fonttitle10));
            PdfPCell celltxtno = new PdfPCell(new Phrase(tempArr[1], fonttitle10));
            PdfPCell celltime = new PdfPCell(new Phrase("年龄", fonttitle10));
            PdfPCell celltxttime = new PdfPCell(new Phrase(tempArr[2], fonttitle10));
            PdfPCell cellgrade = new PdfPCell(new Phrase("性别", fonttitle10));
            PdfPCell celltxtgrade = new PdfPCell(new Phrase(tempArr[3], fonttitle10));
            PdfPCell celltotaltime = new PdfPCell(new Phrase("时间", fonttitle10));
            PdfPCell celltxttotaltime = new PdfPCell(new Phrase("2023-4-20", fonttitle10));
            PdfPCell Text_1 = new PdfPCell(new Phrase("最大运动角度:", fonttitle10));
            PdfPCell Data_1 = new PdfPCell(new Phrase(strArr[0], fonttitle10));
            PdfPCell Text_2 = new PdfPCell(new Phrase("第一次最大运动角度 ：", fonttitle10));
            PdfPCell Data_2 = new PdfPCell(new Phrase(strArr[1], fonttitle10));
            PdfPCell Text_3 = new PdfPCell(new Phrase("最大运动速度：", fonttitle10));
            PdfPCell Data_3 = new PdfPCell(new Phrase(strArr[2], fonttitle10));
            PdfPCell Text_4 = new PdfPCell(new Phrase("第一次最大运动速度：", fonttitle10));
            PdfPCell Data_4 = new PdfPCell(new Phrase(strArr[3], fonttitle10));
            PdfPCell Text_5 = new PdfPCell(new Phrase("最大加速度：", fonttitle10));
            PdfPCell Data_5 = new PdfPCell(new Phrase(strArr[4], fonttitle10));
            PdfPCell Text_6 = new PdfPCell(new Phrase("第一次最大加速度：", fonttitle10));
            PdfPCell Data_6 = new PdfPCell(new Phrase(strArr[5], fonttitle10));
            PdfPCell Text_7 = new PdfPCell(new Phrase("任务完成度：", fonttitle10));
            PdfPCell Data_7 = new PdfPCell(new Phrase(strArr[6], fonttitle10));
            PdfPCell Text_8 = new PdfPCell(new Phrase("第一次任务完成度：", fonttitle10));
            PdfPCell Data_8 = new PdfPCell(new Phrase(strArr[7], fonttitle10));
            PdfPCell Text_9 = new PdfPCell(new Phrase("综合评定值：", fonttitle10));
            PdfPCell Data_9 = new PdfPCell(new Phrase(strArr[8], fonttitle10));
            PdfPCell Text_10 = new PdfPCell(new Phrase("第一次综合评定值：", fonttitle10));
            PdfPCell Data_10 = new PdfPCell(new Phrase(strArr[9], fonttitle10));
            //单元格占用两格
            //celltxttotaltime.Colspan = 2;
            //填入单元格内容
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

            ////同格式拓展添加
            //for (int i = 0; i < 14; i++)
            //{

            //    table.AddCell(new PdfPCell(new Phrase("第" + i + "个任务节点", fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    PdfPCell pic = new PdfPCell(new Phrase("第" + i + "个任务节点的截图", fonttitle10));
            //    pic.Colspan = 2;
            //    table.AddCell(pic);
            //}
            //for (int i = 0; i < 14; i++)
            //{

            //    table.AddCell(new PdfPCell(new Phrase("第" + i + "个任务节点", fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    table.AddCell(new PdfPCell(new Phrase(0.ToString(), fonttitle10)));
            //    PdfPCell pic = new PdfPCell(new Phrase("第" + i + "个任务节点的截图", fonttitle10));
            //    pic.Colspan = 2;
            //    table.AddCell(pic);
            //}
            document.Add(table);
            document.Close();
            Debug.Log("pdf文件写入成功");
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