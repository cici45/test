using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
public class PDFReport : IDisposable
{
    BaseFont heiBaseFont;//��������
    public Font titleFont;//����������ʽ
    public Font firstTitleFont;//�����������ʽ
    public Font secondTitleFont;//С����������ʽ
    public Font contentFont;//����������ʽ
    public Document document;//�ĵ�
    string newFontPath;

    public static IEnumerator ������Դ����д·��(string Oldpath, string newPath)
    {
        if (File.Exists(newPath))
        {
            yield break;
        }
        //Uri uri = new Uri(Oldpath);
        //using (UnityWebRequest request = UnityWebRequest.Get(uri))
        //{
        //    yield return request.SendWebRequest();
        //    if (string.IsNullOrEmpty(request.error))
        //    {
        //        yield return File.WriteAllBytesAsync(newPath, request.downloadHandler.data);
        //    }
        //    else
        //    {
        //        Debug.LogError(request.error);
        //    }
        //}
    }

    public IEnumerator ��ʼ��(string filePath)
    {
        document = new Document(PageSize.A4);
        string dirPath = Path.GetDirectoryName(filePath);
        Directory.CreateDirectory(dirPath);
        //Ϊ��Document����һ��Writerʵ����
        FileStream os = new FileStream(filePath, FileMode.Create);
        PdfWriter.GetInstance(document, os);
        //���ĵ�
        document.Open();
        string oldPath = Application.streamingAssetsPath + "/SourceHanSansSC-Medium.otf";
        newFontPath = Application.persistentDataPath + "/SourceHanSansSC-Medium.otf";
        yield return ������Դ����д·��(oldPath, newFontPath);
        //��������
        heiBaseFont = BaseFont.CreateFont(newFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        titleFont = new Font(heiBaseFont, 26, 1);
        firstTitleFont = new Font(heiBaseFont, 20, 1);
        secondTitleFont = new Font(heiBaseFont, 13, 1);
        contentFont = new Font(heiBaseFont, 11, Font.NORMAL);
    }

    public void ���PDF���(DataTable dt)
    {
        List<float> columns = new List<float>();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            columns.Add(1);
        }
        ���PDF���(dt, columns.ToArray());
    }
    public void ���PDF���(DataTable dt, float[] columnW)
    {

        List<string> list = new List<string>();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            string s = dt.Columns[i].ColumnName;
            list.Add(s);
        }
        //����
        foreach (DataRow row in dt.Rows)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string s = row[i].ToString();
                list.Add(s);
            }
        }
        AddTable(columnW, list.ToArray());
    }
    /// <summary>
    /// ���ӱ��
    /// </summary>
    /// <param name="column">������ȱ���</param>
    /// <param name="content">����</param>
    public void AddTable(float[] columns, string[] content)
    {
        PdfPTable table = new PdfPTable(columns);
        table.WidthPercentage = 100;
        //table.SetTotalWidth(new float[] {10,10,10,10,10,10,10,20 });
        for (int i = 0; i < content.Length; i++)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content[i], contentFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            table.AddCell(cell);
        }
        document.Add(table);
    }


    /// <summary>
    /// �ո�
    /// ������У���������������
    /// </summary>
    public void AddNullLine()
    {
        Paragraph nullLine = new Paragraph(" ", secondTitleFont);
        nullLine.Leading = 5;
        document.Add(nullLine);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="titleStr">��������</param>
    /// <param name="font">�������壬��Ϊһ������Ͷ�������</param>
    /// <param name="alignmentType">�����ʽ,0Ϊ�����,1Ϊ����</param>
    public void AddTitle(string titleStr, int alignmentType = 0)
    {
        Paragraph contentP = new Paragraph(new Chunk(titleStr, titleFont));
        contentP.Alignment = alignmentType;
        document.Add(contentP);
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="content">����</param>
    /// <param name="alignmentType">�����ʽ,0Ϊ�����,1Ϊ����</param>
    public void AddContent(string content, int alignmentType = 0)
    {
        Paragraph contentP = new Paragraph(new Chunk(content, contentFont));
        contentP.Alignment = alignmentType;
        document.Add(contentP);
    }

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="imagePath"></param>
    /// <param name="scale"></param>
    public void AddImage(string imagePath, int width = 475, int height = 325)
    {
        if (!File.Exists(imagePath))
        {
            Debug.LogWarning("��·���²�����ָ��ͼƬ������·���Ƿ���ȷ��");
            return;
        }
        Image image = Image.GetInstance(imagePath);
        image.ScaleToFit(width, height);
        image.Alignment = Element.ALIGN_JUSTIFIED;
        document.Add(image);
    }

    /// <summary>
    /// �ر��ĵ�
    /// </summary>
    public void Dispose()
    {
        document.Close();
    }
}

