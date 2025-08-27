using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font=iTextSharp.text.Font;
using Image=iTextSharp.text.Image;
using UnityEngine.Networking;
using UnityEngine;
using System.Data;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class WritePDF: MonoBehaviour
{
    #region 创建文件所需
    public static iTextSharp.text.pdf.BaseFont heiBaseFont;/*基础字体*/
    public static Font  titleFont;/*报告字体样式*/
    public static Font firstTitleFont;/*大标题字体样式*/
    public static Font secondTitleFont;/*小标题字体样式*/
    public static Font contenFont;/*内容字体样式*/
    public static iTextSharp.text.Document pdfdocument;/*文档*/
    public static string newFontpath;/*资源路径*/
    static string titlepath;
    static string textpath;
    #endregion

    #region 测试用数据
    static List<string> testlist = new List<string>() {"20230317","张三","男","正常","康复","2023.03.17" };/*患者基本信息*/
    static List<string> datatestlist = new List<string>() { "位置稳定性", "最大速度", "速度稳定性", "最小速度", 
        "加速稳定性", "最大加速度", "变化幅值", "最小加速度", "加速变化幅值", "速度面积比", "加速面积比" };/*评估具体数据*/
    static List<string> ss = new List<string>() { "欢乐厨房", "  15s", "    初级", "    95%完成度", "    95分" };/*训练内容*/
    #endregion

    private void Awake()
    {
        #region pdf文件路径与所用字体路径
        titlepath = Application.streamingAssetsPath + "/Fonts/SIMKAI.TTF";/*字体路径*/
        textpath = Application.streamingAssetsPath + "/Fonts/SIMKAI.TTF";/*字体路径*/
        #endregion
    }
    void Start()
    {
        //WritesPDF(testlist,ss, datatestlist);

    }

    public static void TestDebugPDF()
    {
        
    }


    #region 创建pdf文件并写入数据内容
    /// <summary>
    /// 创建pdf数据表
    /// </summary>
    /// <param name="filepath">PDF文件路径</param>
    /// <param name="titleFontpath">标题字体路径</param>
    /// <param name="textFontpath">正文字体路径</param>
    /// <param name="title">标题内容</param>
    /// <param name="tableContent">正文内容</param>
    public static void WritesPDF(List<string> infos, List<string> tableContent, List<string> datalist)
    {
        string pdfpath = Application.streamingAssetsPath + "/"+ infos[1] + "推胸拉背训练报告" + infos[5]+".pdf";
        if (File.Exists(pdfpath))
        {
            File.Delete(pdfpath);
        }
        try
        {
            #region pdf文件初始化
            pdfdocument = new Document(PageSize.A4);
            string dirpath = Path.GetDirectoryName(pdfpath);
            Directory.CreateDirectory(dirpath);
            FileStream os = new FileStream(pdfpath, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfdocument, os);
            pdfWriter.SetEncryption(null,null,PdfWriter.AllowCopy|PdfWriter.AllowPrinting,true);
            pdfdocument.Open();
            #endregion

            #region pdf文件字体创建与设置 titleFontpath（字体文件路径）
            BaseFont bf_Title = BaseFont.CreateFont(titlepath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);            
            Font font_Title = new Font(bf_Title, 30, (int)FontStyle.Bold);
            Font font_subhead = new Font(bf_Title, 20, (int)FontStyle.Bold);

            BaseFont bf = BaseFont.CreateFont(textpath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 16);/*字体设置*/

            string titleName = "推胸拉背训练报告";
            Paragraph paragraph_title = new Paragraph(titleName, font_Title);/*写入内容字体设置*/
            paragraph_title.Alignment = Rectangle.ALIGN_CENTER;/*写入内容对齐方式设置*/
            #endregion


            pdfdocument.Add(paragraph_title);/*数据大标题写入pdf文件中*/
            Paragraph nullp = new Paragraph(" ", font_Title);/*空白行*/
            nullp.Leading = 15;/*空白行大小*/
            pdfdocument.Add(nullp);

            PdfPTable paragraph_infos = new PdfPTable(3);
            paragraph_infos.HorizontalAlignment= Rectangle.ALIGN_CENTER;
            PdfPCell id = new PdfPCell(new Phrase("患者ID", font));
            PdfPCell name = new PdfPCell(new Phrase("姓名", font));
            PdfPCell sex = new PdfPCell(new Phrase("性别", font));
            PdfPCell bq = new PdfPCell(new Phrase("基本病情", font));
            PdfPCell ks = new PdfPCell(new Phrase("医院", font));
            PdfPCell sj = new PdfPCell(new Phrase("报告时间", font));
            PdfPCell _id = new PdfPCell(new Phrase(infos[0], font));
            PdfPCell _name = new PdfPCell(new Phrase(infos[1], font));
            PdfPCell _sex = new PdfPCell(new Phrase(infos[2], font));
            PdfPCell _bq = new PdfPCell(new Phrase(infos[3], font));
            PdfPCell _ks = new PdfPCell(new Phrase(infos[4], font));
            PdfPCell _sj = new PdfPCell(new Phrase(infos[5], font));
            paragraph_infos.AddCell(id);
            paragraph_infos.AddCell(name);
            paragraph_infos.AddCell(sex);
            paragraph_infos.AddCell(_id);
            paragraph_infos.AddCell(_name);
            paragraph_infos.AddCell(_sex);
            paragraph_infos.AddCell(bq);
            paragraph_infos.AddCell(ks);
            paragraph_infos.AddCell(sj);
            paragraph_infos.AddCell(_bq);
            paragraph_infos.AddCell(_ks);
            paragraph_infos.AddCell(_sj);
            pdfdocument.Add(paragraph_infos);/*填入患者基本信息*/
            pdfdocument.Add(nullp);/*填入空白行作为分割*/
            PdfPTable table = new PdfPTable(5);/*表格列数*/
            PdfPTable datatable = new PdfPTable(1);
            PlanContent(tableContent, nullp, table,font, font_subhead);
            pdfdocument.Add(nullp);
            DataEvaluation(datalist, datatable, font, font_subhead, nullp);
            pdfdocument.Add(nullp);
            ImageLoad(Application.streamingAssetsPath + "/Screenshot/ScreenShot_ScreenCapture.png", font_subhead);
            pdfdocument.Add(nullp);
            ImageLoad(Application.streamingAssetsPath + "/Screenshot/ScreenShot_ReadPixelsWithCamera.png", font_subhead);
            pdfdocument.Add(nullp);
            pdfdocument.Close();
            PrintImage(pdfpath);
        }
       catch (DocumentException de) { Debug.LogError(de.Message); }
       catch(IOException io) { Debug.LogError(io.Message); }
    }
    #endregion
    public static void PrintImage(string path)
    {
        if (File.Exists(path))
        {
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = path;
            process.StartInfo.Verb = "print";
            process.Start();
            TipsPage_1.tipsPage.OpenThisPage("PDF打印成功！");
        }
        else
        {
            TipsPage_1.tipsPage.OpenThisPage("文件不存在！");
        }
    }




    #region 字符长度判断填充
    /// <summary>
    /// 字符串长度判断
    /// </summary>
    /// <param name="str">填入的字符串</param>
    /// <returns></returns>
    public static string GetStringLength(string str,string strlength)
    {
        string mylength = strlength;
        if (str.Length < mylength.Length)
        {
            for (int i = str.Length; i < mylength.Length; i++)
            {
                str += " ";
            }
        }
        return str;
    }
    #endregion

    #region 训练内容填写
    /// <summary>
    /// 训练内容填写
    /// </summary>
    /// <param name="planContent">训练数据</param>
    /// <param name="nullp">空白行</param>
    /// <param name="table">表单</param>
    /// <param name="font">正文字体样式</param>
    /// <param name="headfont">小标题字体样式</param>
    public static void PlanContent(List<string> planContent, Paragraph nullp, PdfPTable table, Font font, Font headfont)
    {
        string planhead = "   训练内容：";
        Paragraph paragraph_head = new Paragraph(planhead, headfont);
        paragraph_head.Alignment = Rectangle.ALIGN_LEFT;
        pdfdocument.Add(paragraph_head);
        PdfPCell cellname = new PdfPCell(new Phrase("游戏名称", font));
        PdfPCell cellno = new PdfPCell(new Phrase("训练时间", font));
        PdfPCell celltime = new PdfPCell(new Phrase("难度", font));
        PdfPCell cellgrade = new PdfPCell(new Phrase("任务完成度", font));
        PdfPCell celltotaltime = new PdfPCell(new Phrase("得分", font));
        PdfPCell celltxtname = new PdfPCell(new Phrase(planContent[0], font));
        PdfPCell celltxtno = new PdfPCell(new Phrase(planContent[1], font));
        PdfPCell celltxttime = new PdfPCell(new Phrase(planContent[2], font));
        PdfPCell celltxtgrade = new PdfPCell(new Phrase(planContent[3], font));
        PdfPCell celltxttotaltime = new PdfPCell(new Phrase(planContent[4], font));

        table.AddCell(cellname);
        table.AddCell(cellno);
        table.AddCell(celltime);
        table.AddCell(cellgrade);
        table.AddCell(celltotaltime);
        table.AddCell(celltxtname);
        table.AddCell(celltxtno);
        table.AddCell(celltxttime);
        table.AddCell(celltxtgrade);
        table.AddCell(celltxttotaltime);
        table.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        table.WidthPercentage = 90F;
        pdfdocument.Add(nullp);
        pdfdocument.Add(table);
        pdfdocument.Add(nullp);
        pdfdocument.Add(nullp);
    }
    #endregion

    #region 评估数据填写

    /// <summary>
    /// 医疗数据填写
    /// </summary>
    /// <param name="planContent">训练数据</param>
    /// <param name="nullp">空白行</param>
    /// <param name="table">表单</param>
    /// <param name="font">正文字体样式</param>
    /// <param name="headfont">小标题字体样式</param>
    public static void DataContent(List<string> planContent, Paragraph nullp, PdfPTable table, Font font, Font headfont)
    {
        string planhead = "   医疗数据：";
        Paragraph paragraph_head = new Paragraph(planhead, headfont);
        paragraph_head.Alignment = Rectangle.ALIGN_LEFT;
        pdfdocument.Add(paragraph_head);
        PdfPCell cellname = new PdfPCell(new Phrase("关节活动度:", font));
        PdfPCell cellno = new PdfPCell(new Phrase("肌力 ：", font));
        PdfPCell celltime = new PdfPCell(new Phrase("肌耐力：", font));
        PdfPCell celltxtname = new PdfPCell(new Phrase(planContent[0], font));
        PdfPCell celltxtno = new PdfPCell(new Phrase(planContent[1], font));
        PdfPCell celltxttime = new PdfPCell(new Phrase(planContent[2], font));

        table.AddCell(cellname);
        table.AddCell(cellno);
        table.AddCell(celltime);
        table.AddCell(celltxtname);
        table.AddCell(celltxtno);
        table.AddCell(celltxttime);
        table.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        table.WidthPercentage = 90F;
        pdfdocument.Add(nullp);
        pdfdocument.Add(table);
        pdfdocument.Add(nullp);
        pdfdocument.Add(nullp);
    }

    /// <summary>
    /// 评估数据填入
    /// </summary>
    /// <param name="Datalist">填入的数据</param>
    /// <param name="table">pdf表单</param>
    /// <param name="font">正文字体样式</param>
    /// <param name="headfont">小标题字体样式</param>
    public static void DataEvaluation(List<string> Datalist,  PdfPTable table, Font font, Font headfont, Paragraph nullp)
    {
        string datahead = "   数据评估："+ "\n";
        Paragraph paragraph_head = new Paragraph(datahead, headfont);
        paragraph_head.Alignment = Rectangle.ALIGN_LEFT;
        pdfdocument.Add(paragraph_head);
        string dataeva = "\n" +
                "最大运动角度（°）:" + GetStringLength(Datalist[0], "         ") + "最小运动角度（°） ：" + GetStringLength(Datalist[1], "      ") + "\n" + "\n" + "\n" +
                "平均角速度（°/s）：" + GetStringLength(Datalist[2], "        ") + "最大角速度（°/s）：" + GetStringLength(Datalist[3], "       ") + "\n" + "\n" + "\n" +
                "平均角加速度(°/s²)：" + GetStringLength(Datalist[4], "       ") + " 肌力：" + GetStringLength(Datalist[5], "                    ") + "\n" + "\n" + "\n" +
                "肌力稳定性：" + GetStringLength(Datalist[6], "                                                                                   ") + "\n" + "\n" + "\n" +
                "疲劳时间（s）：" + GetStringLength(Datalist[7], "                                                                                ") + "\n" + "\n" + "\n" +
                "耐力：" + GetStringLength(Datalist[8], "                      ");
        Paragraph paragraph_content = new Paragraph(dataeva + "\n", font);
        paragraph_head.Alignment = Rectangle.ALIGN_CENTER;
        PdfPCell cell = new PdfPCell(paragraph_content);
        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        table.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        table.AddCell(cell);
        table.WidthPercentage = 90F;
        pdfdocument.Add(nullp);
        pdfdocument.Add(table);

    }
    #endregion

    public static void ImageLoad(string _path,Font _font, float width = 525,float height = 252)
    {
        string planhead = "   训练报表：";
        Paragraph paragraph_head = new Paragraph(planhead, _font);
        if (!File.Exists(_path))
        {
            Debug.LogWarning("该路径下不存在指定图片，请检测路径是否正确！");
            return;
        }
        Image image = Image.GetInstance(_path);
        image.ScaleToFit(width, height);
        image.Alignment = Element.ALIGN_JUSTIFIED;
        pdfdocument.Add(image);
    }

    #region 字符串转字节格式
    /// <summary>
    /// 字符串转字节
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] StringToByte(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return new byte[0];
        }
        string s = str.Replace(" ", "");
        int count = s.Length / 2;
        var result = new byte[count];
        for(int i = 0; i < count; i++)
        {
            var tempBytes = Byte.Parse(s.Substring(2 * i, 2), System.Globalization.NumberStyles.HexNumber);
            result[i] = tempBytes;
        }
        return result;
    }
    #endregion
}
