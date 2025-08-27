using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAngle : MonoBehaviour
{
    Transform rightLeg,leftLeg;
    List<Transform> skeleton = new List<Transform>();
    Vector3[] values = new Vector3[2];//int i = 0;//测试
    float variance;//总变化角度

    Transform Scapula_L;
    Transform Shoulder_L;
    Transform Elbow_L;
    Transform Scapula_R;
    Transform Shoulder_R;
    Transform Elbow_R;
    Transform Hip_R;
    Transform Hip_L;
    Transform Knee_R;
    Transform Knee_L;
    Transform Ankle_L;
    Transform Ankle_R;
    float Hip_RAngle;
    float Hip_LAngle;
    float Knee_RAngle;
    float Knee_LAngle;
    float Ankle_LAngle;
    float Ankle_RAngle;
    Vector3 Scapula_L_Angel;
    Vector3 Shoulder_L_Angel;
    Vector3 Elbow_L_Angel;
    Vector3 Scapula_R_Angel;
    Vector3 Shoulder_R_Angel;
    Vector3 Elbow_R_Angel;

    void Awake()
    {
        #region 添加物体
        //skeleton.Add(Scapula_L);
        //skeleton.Add(Shoulder_L);
        //skeleton.Add(Elbow_L);
        //skeleton.Add(Shoulder_L);
        //skeleton.Add(Scapula_L);
        //skeleton.Add(Shoulder_L);
        //skeleton.Add(Scapula_L);
        //skeleton.Add(Shoulder_L);
        //skeleton.Add(Scapula_L);
        //skeleton.Add(Shoulder_L);
        #endregion

        #region 初始角度值获取
        //for (int i = 0; i < values.Length; i++)
        //{
        //    values[i] = skeleton[i].localEulerAngles;
        //}
        #endregion
        Hip_R = GameObjFind(transform, "Hip_R");
        Hip_L = GameObjFind(transform, "Hip_L");
        Knee_R = GameObjFind(transform, "Knee_R");
        Knee_L = GameObjFind(transform, "Knee_L");
        Ankle_L = GameObjFind(transform, "Ankle_L");
        Ankle_R = GameObjFind(transform, "Ankle_R");
        Hip_RAngle = Hip_R.localEulerAngles.z;
        Hip_LAngle = Hip_L.localEulerAngles.z;
        Knee_RAngle = Knee_R.localEulerAngles.z;
        Knee_LAngle = Knee_L.localEulerAngles.z;
        Ankle_LAngle = Ankle_L.localEulerAngles.z;
        Ankle_RAngle = Ankle_R.localEulerAngles.z;
    }

    private void Start()
    {
       
    }
    int i = 0;
    //void Update()//测试
    //{
    //    SetModelAngle(i++);
    //    if (Input.GetKey(KeyCode.A)) InitAngle();
    //}
    public void ModeAngleUpdate(float angle)
    {
        Hip_R.localEulerAngles=new Vector3( Hip_R.localEulerAngles.x, Hip_R.localEulerAngles.y, Hip_RAngle+angle*30);
        Hip_L.localEulerAngles=new Vector3(Hip_L.localEulerAngles.x, Hip_L.localEulerAngles.y, Hip_LAngle + angle*30);
        Knee_R.localEulerAngles=new Vector3(Knee_R.localEulerAngles.x, Knee_R.localEulerAngles.y, Knee_RAngle-angle*50);
        Knee_L.localEulerAngles=new Vector3(Knee_L.localEulerAngles.x, Knee_L.localEulerAngles.y, Knee_LAngle - angle*50);
        Ankle_L.localEulerAngles=new Vector3(Ankle_L.localEulerAngles.x, Ankle_L.localEulerAngles.y, Ankle_LAngle + angle*30);
        Ankle_R.localEulerAngles=new Vector3(Ankle_R.localEulerAngles.x, Ankle_R.localEulerAngles.y, Ankle_RAngle + angle*30);
    }
    public void SetModelAngle(float angle)
    {
        //Debug.Log("动作重现");
        Scapula_L = GameObjFind(transform, "Scapula_L");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/
        Shoulder_L = GameObjFind(transform, "Shoulder_L");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/
        Elbow_L = GameObjFind(transform, "Elbow_L");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/


        Scapula_R = GameObjFind(transform, "Scapula_R");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/
        Shoulder_R = GameObjFind(transform, "Shoulder_R");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/
        Elbow_R = GameObjFind(transform, "Elbow_R");
        if (angle != variance && angle < 60)
        {
            Debug.Log(UserInfoData.CurrentAngle);

            Scapula_L.localEulerAngles = new Vector3(Scapula_L.localEulerAngles.x, Scapula_L.localEulerAngles.y, Scapula_L.localEulerAngles.z - (angle - Scapula_LCurrent_Angel));

            Scapula_R.localEulerAngles = new Vector3(Scapula_R.localEulerAngles.x, Scapula_R.localEulerAngles.y, Scapula_R.localEulerAngles.z - (angle - Scapula_RCurrent_Angel));

            int elbow = (int)(90 * (angle / 60));

            Elbow_R.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_LCurrent_Angel), Elbow_R.localEulerAngles.z);
            Elbow_L.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_RCurrent_Angel), Elbow_R.localEulerAngles.z);


            variance = angle;
            Elbow_RCurrent_Angel = elbow;
            Elbow_LCurrent_Angel = elbow;
            Scapula_LCurrent_Angel = angle;
            Scapula_RCurrent_Angel = angle;
        }
        else if ((int)angle <= 5)
        {
            Scapula_L.localEulerAngles = Scapula_L_Angel;
            Scapula_R.localEulerAngles = Scapula_R_Angel;
        }
    }
    public void InitAngle()
    {
        variance = 0;
        #region 组件获取
        rightLeg = GameObjFind(transform, "Knee_R");//右手
        leftLeg = GameObjFind(transform, "Knee_L");

        Scapula_L = GameObjFind(transform, "Scapula_L");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/
        Shoulder_L = GameObjFind(transform, "Shoulder_L");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/
        Elbow_L = GameObjFind(transform, "Elbow_L");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/
        

        Scapula_R = GameObjFind(transform, "Scapula_R");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/
        Shoulder_R = GameObjFind(transform, "Shoulder_R");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/
        Elbow_R = GameObjFind(transform, "Elbow_R");//Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/

        if (Scapula_L_Angel != Vector3.zero)
        {
            return;
        }
        else
        {
            Scapula_L_Angel = Scapula_L.localEulerAngles;
            Shoulder_L_Angel = Shoulder_L.localEulerAngles;
            Elbow_L_Angel = Elbow_L.localEulerAngles;
            Scapula_R_Angel = Scapula_R.localEulerAngles;
            Shoulder_R_Angel = Shoulder_R.localEulerAngles;
            Elbow_R_Angel = Elbow_R.localEulerAngles;
        }
        #endregion
       
    }

    float Scapula_LCurrent_Angel = 0;
    float Elbow_LCurrent_Angel = 0;
    float Scapula_RCurrent_Angel = 0;
    float Elbow_RCurrent_Angel = 0;
    IEnumerator PlayerMove()
    {
        while (PlanMangenter.isPlay)
        {
            yield return new WaitForSeconds(0.02f);
            if (PlanMangenter.isPlay)
            {

                if (UserInfoData.CurrentAngle != variance && UserInfoData.CurrentAngle < 93)
                {
                    Debug.Log(UserInfoData.CurrentAngle);
                    
                    Scapula_L.localEulerAngles = new Vector3(Scapula_L.localEulerAngles.x, Scapula_L.localEulerAngles.y, Scapula_L.localEulerAngles.z - (UserInfoData.CurrentAngle - Scapula_LCurrent_Angel));

                    Scapula_R.localEulerAngles = new Vector3(Scapula_R.localEulerAngles.x, Scapula_R.localEulerAngles.y, Scapula_R.localEulerAngles.z - (UserInfoData.CurrentAngle - Scapula_RCurrent_Angel));

                    int elbow = (int)(90 * (UserInfoData.CurrentAngle / 60));

                    Elbow_R.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_LCurrent_Angel), Elbow_R.localEulerAngles.z);
                    Elbow_L.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_RCurrent_Angel), Elbow_R.localEulerAngles.z);


                    variance = UserInfoData.CurrentAngle;
                    Elbow_RCurrent_Angel = elbow;
                    Elbow_LCurrent_Angel = elbow;
                    Scapula_LCurrent_Angel = UserInfoData.CurrentAngle;
                    Scapula_RCurrent_Angel = UserInfoData.CurrentAngle;
                }
                else if ((int)UserInfoData.CurrentAngle <= 5)
                {
                    Scapula_L.localEulerAngles = Scapula_L_Angel;
                    Scapula_R.localEulerAngles = Scapula_R_Angel;
                }
            }
        }
    }

    public Transform GameObjFind(Transform obj, string ChildName)
    {
        var t = obj.Find(ChildName);
        if (t) return t;
        for (int i = 0; i < obj.childCount; i++)
        {
            t = GameObjFind(obj.GetChild(i), ChildName);
            if (t) return t;
        }
        return null;
    }
}
