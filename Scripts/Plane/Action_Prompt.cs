using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Prompt : MonoBehaviour
{
    Transform rightLeg, leftLeg;
    List<Transform> skeleton = new List<Transform>();
    Vector3[] values = new Vector3[2];//int i = 0;//测试
    float variance;//总变化角度

    Transform Scapula_L;
    Transform Shoulder_L;
    Transform Elbow_L;
    Transform Scapula_R;
    Transform Shoulder_R;
    Transform Elbow_R;

    Vector3 Scapula_L_Angel;
    Vector3 Shoulder_L_Angel;
    Vector3 Elbow_L_Angel;
    Vector3 Scapula_R_Angel;
    Vector3 Shoulder_R_Angel;
    Vector3 Elbow_R_Angel;

    private void Awake()
    {
        #region 组件获取
        rightLeg = GameObjFind(transform, "Knee_R");//右手
        leftLeg = GameObjFind(transform, "Knee_L");
        #endregion

        #region 添加物体
        skeleton.Add(rightLeg);
        skeleton.Add(leftLeg);
        #endregion

        #region 初始角度值获取
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = skeleton[i].localEulerAngles;
        }
        #endregion
        Scapula_LCurrent_Angel = 0;
        Elbow_RCurrent_Angel = 0;
    }

    float Scapula_LCurrent_Angel = 0;
    float Elbow_LCurrent_Angel = 0;
    float Scapula_RCurrent_Angel = 0;
    float Elbow_RCurrent_Angel = 0;
    public void SetModelAngle_Target(float angle,float maxPos,float maxValue)
    {
        int targetAngle = (int)((Mathf.Abs(angle)/ maxPos) * maxValue);
        //Debug.Log("目标角度："+ targetAngle);
        Scapula_L = GameObjFind(transform, "Scapula_L");
        Shoulder_L = GameObjFind(transform, "Shoulder_L");
        Elbow_L = GameObjFind(transform, "Elbow_L");
        Scapula_R = GameObjFind(transform, "Scapula_R");
        Shoulder_R = GameObjFind(transform, "Shoulder_R");
        Elbow_R = GameObjFind(transform, "Elbow_R");
        if (targetAngle != variance && targetAngle < 60)
        {
           
            Scapula_L.localEulerAngles = new Vector3(Scapula_L.localEulerAngles.x, Scapula_L.localEulerAngles.y, Scapula_L.localEulerAngles.z - (targetAngle - Scapula_LCurrent_Angel));

            Scapula_R.localEulerAngles = new Vector3(Scapula_R.localEulerAngles.x, Scapula_R.localEulerAngles.y, Scapula_R.localEulerAngles.z - (targetAngle - Scapula_LCurrent_Angel));

            int elbow = (int)(90 * (targetAngle / 60.0f));
            
            Elbow_R.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_RCurrent_Angel), Elbow_R.localEulerAngles.z);
            Elbow_L.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_RCurrent_Angel), Elbow_R.localEulerAngles.z);
           
            variance = targetAngle;
            Elbow_RCurrent_Angel = elbow;
            
            Scapula_LCurrent_Angel = targetAngle;
            
        }
        else if ((int)targetAngle <= 5)
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
