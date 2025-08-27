using UnityEngine;

using UnityEditor;

using System.Collections;

public class ScreenShot : MonoBehaviour
{
    public static ScreenShot instantiate;
    Camera pdfCam;

    //public int w;
    //public int h;

    private void Awake()
    {
        instantiate = this;
        pdfCam = GameObject.Find("PDFCamera").GetComponent<Camera>();
    }

    //void Update()
    //{

    //    //����Ҽ����£��������ֽ�ͼ��ʽ�ĵ���

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        //ScreenShot_ScreenCapture();

    //        //StartCoroutine(ScreenShot_ReadPixels());

    //        //targetCamera.gameObject.SetActive(true);

    //        ScreenShot_ReadPixelsWithCamera();

    //        //targetCamera.gameObject.SetActive(false);

    //        //ˢ�¹���Ŀ¼����ʾ�����ͼƬ��������Բ�Ҫ����Ϊ�ֶ�����

    //        AssetDatabase.Refresh();

    //    }

    //}

    //Unity�Դ��Ľ�ͼ����

    public void OnStartScreenShot()
    {
        StartCoroutine(ScreenShot_ReadPixels());
    }

    private void ScreenShot_ScreenCapture()
    {

        //����������

        ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot/ScreenShot_ScreenCapture.png");

    }

    //��ȡ��Ļ���ؽ��н�ͼ

    private IEnumerator ScreenShot_ReadPixels()
    {

        yield return new WaitForEndOfFrame();

        //��ȡ����

        Texture2D tex = new Texture2D(750, 500);

        tex.ReadPixels(new Rect(670, 225, tex.width, tex.height), 0, 0);

        tex.Apply();

        //�����ȡ�Ľ��

        string path = Application.streamingAssetsPath + "/Screenshot/ScreenShot_ScreenCapture.png";

        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());

    }

    //��ȡָ�������Ⱦ������

    public void ScreenShot_ReadPixelsWithCamera()
    {

        //��ָ��������� RenderTexture

        RenderTexture renTex = new RenderTexture(Screen.width, Screen.height, 16);

        pdfCam.targetTexture = renTex;

        pdfCam.Render();

        RenderTexture.active = renTex;

        //��ȡ����

        Texture2D tex = new Texture2D(750, 500);

        tex.ReadPixels(new Rect(620, 300, tex.width, tex.height), 0, 0);

        tex.Apply();

        //��ȡĿ��������ؽ�������Ⱦ�ָ�ԭ�ȵķ�ʽ

        pdfCam.targetTexture = null;

        RenderTexture.active = null;

        Destroy(renTex);

        //�����ȡ�Ľ��

        string path = Application.streamingAssetsPath + "/Screenshot/ScreenShot_ReadPixelsWithCamera.png";

        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());

    }

}