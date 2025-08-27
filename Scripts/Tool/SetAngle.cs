using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetAngle : MonoBehaviour
{
    //public Animator anim;      //动画
    //public Transform head;     //头部
    //public Transform lefthand;  //左手
    //public Transform righthand; //右手
    //public Transform leftfoot;  //左脚
    //public Transform rightfoot; //右脚
    float elbowcurrentangle = 0;
    Transform elbowTransform;
    Transform elbowTransform_l;
    Transform abdominal;
    Transform hip_l;
    Transform hip_r;
    Transform shoulder_L;
    Transform Scapula_r;
    Transform knee_L;
    Transform knee_r;
    // Start is called before the first frame update
    void Start()
    {
        elbowTransform = transform.Find("root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R");
        //Debug.Log(elbowTransform.name);
        elbowTransform_l = transform.Find("root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L");
        Debug.Log("初始值："+transform.localEulerAngles.z);
        abdominal = transform.Find("root/Root_M/RootPart1_M");
        hip_l= transform.Find("root/Root_M/Hip_L");
        hip_r = transform.Find("root/Root_M/Hip_R");
        shoulder_L= transform.Find("root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L");
        Scapula_r= transform.Find("root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R");
        knee_L = transform.Find("root/Root_M/Hip_L/HipPart1_L/HipPart2_L/Knee_L");
        knee_r = transform.Find("root/Root_M/Hip_R/HipPart1_R/HipPart2_R/Knee_R");

        //ElbowRotationAni(transform,40,70);

    }
    float ssangle = 0;
    // Update is called once per frame
    void Update()
    {
        if (ssangle < 30f)
        {
            ssangle += 1f;
            Bike_Upperlimb(shoulder_L, elbowTransform_l, Scapula_r, elbowTransform, ssangle);
        }
    }

    private void FixedUpdate()
    {
       
    }

    public static void SetModelAngle(List<Transform> transforms,float angle,float max)
    {
        if (transforms.Count <= 0)
        {
            for (int index = 0; index < transforms.Count; index++)
            {
                transforms[index].localEulerAngles = new Vector3(transforms[index].localEulerAngles.x + angle, transforms[index].localEulerAngles.y, transforms[index].localEulerAngles.z);
            }
        }
       
    }


    #region 肘部回转
    /// <summary>
    /// 右手肘部关节点修改
    /// </summary>
    /// <param name="transforms">右手肘部关节点</param>
    /// <param name="angle">实时角度值</param>
    /// <param name="max">设定的最大运动角度</param>
    public void ElbowRotationAni(float angle)
    {

        elbowTransform.localEulerAngles = new Vector3(elbowTransform.localEulerAngles.x, elbowTransform.localEulerAngles.y, -angle);
        Debug.Log("角度：" + elbowTransform.localEulerAngles.z);
        elbowcurrentangle = angle;
    }
    #endregion

    #region 平推
    static float currenterLshoulder;
    static float currenterLelbow;
    static float currenterRshoulder;
    static float currenterRelbow;
    /// <summary>
    /// 水平运动  
    /// 初始：
    /// Scapula_L 60  -25  50
    /// Shoulder_L130  35 -10  需要修改的
    /// Elbow_L  -25  0  -35   需要修改的
    /// 
    /// Scapula_R 0  -10  -40
    /// Shoulder_R -5  -5  -10 需要修改的
    /// Elbow_R  0  0  -35     需要修改的
    /// </summary>
    /// <param name="Lshoulder">左-肩部关节点</param>
    /// <param name="Lelbow">左—肘部关节点</param>
    /// <param name="Rshoulder">右-肩部关节点</param>
    /// <param name="Relbow">右-肘部关节点</param>
    /// <param name="shoulderangle">肩部变化值</param>
    /// <param name="elbowangle">肘部变化值</param>
    public static void ShouldersAndElbow(Transform Lshoulder,Transform Lelbow,Transform Rshoulder,Transform Relbow,float shoulderangle,float elbowangle)
    {
        // 肩关节最大活动角度 45；  肘关节最大活动角度 75     传过来的角度值=人物模型最大运动角度值*（实时角度值/游戏面板设置的最大运动角度值）
        float lshoulderangle = shoulderangle - currenterLshoulder;
        float lelbowangle = elbowangle - currenterLelbow;
        float Rshoulderangle = shoulderangle - currenterRshoulder;
        float Relbowangle = elbowangle - currenterRelbow;
        Lshoulder.localEulerAngles = new Vector3(Lshoulder.localEulerAngles.x, Lshoulder.localEulerAngles.y+ lshoulderangle, Lshoulder.localEulerAngles.z+ lshoulderangle);
        Lelbow.localEulerAngles = new Vector3(Lshoulder.localEulerAngles.x, Lshoulder.localEulerAngles.y, Lshoulder.localEulerAngles.z + lelbowangle);
        Rshoulder.localEulerAngles = new Vector3(Lshoulder.localEulerAngles.x, Lshoulder.localEulerAngles.y + Rshoulderangle, Lshoulder.localEulerAngles.z + Rshoulderangle);
        Relbow.localEulerAngles = new Vector3(Lshoulder.localEulerAngles.x, Lshoulder.localEulerAngles.y, Lshoulder.localEulerAngles.z + Relbowangle);

        currenterLshoulder = shoulderangle;
        currenterRshoulder = shoulderangle;

        currenterLelbow = elbowangle;
        currenterRelbow = elbowangle;

    }

    #endregion

    #region 上举
    static Vector3 Lshoulder01current;
    static Vector3 Lshouldercurrent;
    static Vector3 Lelbowcurrent;
    static Vector3 Rshouldercurrent;
    static Vector3 Relbowcurrent;
    /// <summary>
    /// 水平运动  
    /// 初始：
    /// Scapula_L 50  -20  40    20  -20  100  差值：30  0  60
    /// Shoulder_L 100  0 -25  需要修改的 100  0  0 差值： 0  0  25
    /// Elbow_L  50  -25  -120   需要修改的50  0  -35  差值：0  25  85
    /// 
    /// Scapula_R 25  -115 -30
    /// Shoulder_R 20  -25  25 需要修改的     20  -25  -50  差值：0  25  75
    /// Elbow_R  -20  -50  -120    需要修改的 20  0  -35  差值：40  50  85
    /// </summary>
    /// /// <param name="Lshoulder01">左-肩胛骨部关节点</param>
    /// <param name="Lshoulder">左-肩部关节点</param>
    /// <param name="Lelbow">左—肘部关节点</param>
    /// <param name="Rshoulder">右-肩部关节点</param>
    /// <param name="Relbow">右-肘部关节点</param>
    /// <param name="shoulderangle">肩部变化值</param>
    /// <param name="elbowangle">肘部变化值</param>
    public static void UpthrowAni(Transform Lshoulder01,Transform Lshoulder, Transform Lelbow,  Transform Rshoulder, Transform Relbow,  float angle)
    {
        // 肩关节最大活动角度 45；  肘关节最大活动角度 75     传过来的角度值=人物模型最大运动角度值*（实时角度值/游戏面板设置的最大运动角度值）
        float ls1x = 30 * angle - Lshoulder01current.x;
        float ls1z= 60 * angle - Lshoulder01current.z;
        Lshoulder01.localEulerAngles = new Vector3(Lshoulder01.localEulerAngles.x+ ls1x, Lshoulder01.localEulerAngles.y, Lshoulder01.localEulerAngles.z + ls1z);
        Lshoulder01current = new Vector3(Lshoulder01current.x+ ls1x, Lshoulder01current.y, Lshoulder01current.z+ ls1z);

        float lsz = 25 * angle - Lshouldercurrent.z;
        Lshoulder.localEulerAngles = new Vector3(Lshoulder.localEulerAngles.x, Lshoulder.localEulerAngles.y, Lshoulder.localEulerAngles.z + lsz);
        Lshouldercurrent = new Vector3(Lshouldercurrent.x, Lshouldercurrent.y, Lshouldercurrent.z + lsz);

        float lely = 25 * angle - Lelbowcurrent.y;
        float lelz = 85 * angle - Lelbowcurrent.z;
        Lelbow.localEulerAngles = new Vector3(Lelbow.localEulerAngles.x, Lelbow.localEulerAngles.y + lely, Lelbow.localEulerAngles.z + lelz);
        Lelbowcurrent = new Vector3(Lelbowcurrent.x, Lelbowcurrent.y+ lely, Lelbowcurrent.z + lelz);

        float rsy = 25 * angle - Rshouldercurrent.y;
        float rsz = 75 * angle - Rshouldercurrent.z;
        Rshoulder.localEulerAngles = new Vector3(Rshoulder.localEulerAngles.x, Rshoulder.localEulerAngles.y + rsy, Rshoulder.localEulerAngles.z + rsz);
        Rshouldercurrent = new Vector3(Rshouldercurrent.x, Rshouldercurrent.y + rsy, Rshouldercurrent.z + lsz);

        float relx = 40 * angle - Relbowcurrent.x;
        float rely = 50 * angle - Relbowcurrent.y;
        float relz = 85 * angle - Relbowcurrent.z;

        Relbow.localEulerAngles = new Vector3(Relbow.localEulerAngles.x + relx, Relbow.localEulerAngles.y + rely, Relbow.localEulerAngles.z + relz);
        Relbowcurrent = new Vector3(Relbowcurrent.x + relx, Relbowcurrent.y + rely, Relbowcurrent.z + relz);
    }

    #endregion

    #region 腹部屈伸
    static float currentabdominal = 0;

    /// <summary>
    /// 腹部屈伸
    /// </summary>
    /// <param name="transform">腹部关节点RootPart1_M，模型最大运动度65</param>
    /// <param name="angle"></param>
    public static void Abdominalflexible(Transform transform, float angle)
    {

        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - (angle - currentabdominal));
        currentabdominal = angle;
    }

    #endregion

    #region 大腿内收外展
    static float thighcurrentangel=0;
    /// <summary>
    /// 大腿内收外展动作重现
    /// 模型实际最大运动范围40度
    /// 内收状态下初始值  Hip_L（-180，0，-90）；Knee_L（0，0，90）；Hip_R（0，-180，-90）；Knee_R（0，0，90）
    /// 外展状态下初始值  Hip_L（-140，0，-90）；Knee_L（0，0，90）；Hip_R（40，-180，-90）；Knee_R（0，0，90）
    /// </summary>
    /// <param name="hip_L">左腿髋关节点</param>
    /// <param name="hip_R">右腿髋关节点</param>
    /// <param name="angle">转化后实际的运动角度</param>
    public static void ThighAdductionOrAbduce(Transform hip_L,Transform hip_R,float angle)
    {
        hip_L.localEulerAngles = new Vector3(hip_L.localEulerAngles.x+(angle- thighcurrentangel), hip_L.localEulerAngles.y, hip_L.localEulerAngles.z);
        hip_R.localEulerAngles = new Vector3(hip_R.localEulerAngles.x - (angle - thighcurrentangel), hip_R.localEulerAngles.y, hip_R.localEulerAngles.z);
        thighcurrentangel = angle;
    }
    #endregion

    #region 腕部旋转
    static float wristcurrentangle=0;
    /// <summary>
    /// 腕部旋转
    /// 手部初始值  Scapula_R（20，-130，-70）；Shoulder_R（-15，-25，0）；Elbow_R（40，0，0），Wrist_R（0，0，0）
    /// 活动最大范围0-60
    /// 修改Elbow_R的x值
    /// </summary>
    /// <param name="wrist">肘部关节点Elbow_R</param>
    /// <param name="angle">实际转换后运动的角度值</param>
    public static void WristSpin(Transform wrist,float angle)
    {
        wrist.localEulerAngles = new Vector3(wrist.localEulerAngles.x - (angle- wristcurrentangle), wrist.localEulerAngles.y, wrist.localEulerAngles.z);
        wristcurrentangle = angle;
    }
    #endregion

    #region 上肢协调
    static float currentshoulder_R_angle=0;
    /// <summary>
    /// 上肢协调 
    /// Scapula_R初始（0，-110，-50）  活动范围 30
    /// Shoulder_R（-15，-25，0）
    /// Elbow_R（40，0，-20）
    /// Shoulder_L初始（130，25，-10）  活动范围 30
    /// Scapula_L（60，-10，20）
    /// Elbow_L（0，0，-25）
    /// </summary>
    /// <param name="shoulder_R">修改Scapula_R关节点</param>
    /// <param name="shoulder_L">修改Shoulder_L关节点</param>
    /// <param name="shoulder_angle"></param>
    public static void ShoulderCoordinate(Transform shoulder_R,Transform shoulder_L,float shoulder_angle)
    {
        shoulder_R.localEulerAngles = new Vector3(shoulder_R.localEulerAngles.x - (shoulder_angle - currentshoulder_R_angle), shoulder_R.localEulerAngles.y, shoulder_R.localEulerAngles.z);
        shoulder_L.localEulerAngles = new Vector3(shoulder_L.localEulerAngles.x, shoulder_L.localEulerAngles.y, shoulder_L.localEulerAngles.z - (shoulder_angle - currentshoulder_R_angle));
        currentshoulder_R_angle = shoulder_angle;
    }
    #endregion

    #region 功率自行车上肢
    static float current_Bike_Upperlimb_angle = 0;
    /// <summary>
    /// shoulder_L高位Shoulder_L(120,25,0);Elbow_L(0,0,-90)
    /// 低位Shoulder_L(120,25,-30);Elbow_L(0,0,-60)
    /// 修改z轴   最大范围30
    /// 
    /// Scapula_R（-15，-110，-50）；Shoulder_R（-15，-25，0）；ShoulderPart1_R（0，0，0）；Elbow_R（60，0，-90）
    /// ShoulderPart1_R（0，30，0）；Elbow_R（60，0，-60）
    /// </summary>
    /// <param name="shoulder_L">Shoulder_L节点z轴</param>
    /// <param name="elbow_L">Elbow_L节点z轴</param>
    /// <param name="shoulder_R">ShoulderPart1_R节点Y轴</param>
    /// <param name="elbow_R">Elbow_R节点Z轴</param
    /// <param name="angle">变化的实际角度值</param>
    public static void Bike_Upperlimb(Transform shoulder_L, Transform elbow_L, Transform shoulder_R, Transform elbow_R, float angle)
    {
        shoulder_L.localEulerAngles = new Vector3(shoulder_L.localEulerAngles.x, shoulder_L.localEulerAngles.y, shoulder_L.localEulerAngles.z + (angle - current_Bike_Upperlimb_angle));
        elbow_L.localEulerAngles = new Vector3(elbow_L.localEulerAngles.x, elbow_L.localEulerAngles.y, elbow_L.localEulerAngles.z - (angle - current_Bike_Upperlimb_angle));

        shoulder_R.localEulerAngles = new Vector3(shoulder_R.localEulerAngles.x, shoulder_R.localEulerAngles.y + (angle - current_Bike_Upperlimb_angle), shoulder_R.localEulerAngles.z);
        elbow_R.localEulerAngles = new Vector3(elbow_R.localEulerAngles.x, elbow_R.localEulerAngles.y, elbow_R.localEulerAngles.z + (angle - current_Bike_Upperlimb_angle));
        current_Bike_Upperlimb_angle = angle;
    }
    #endregion

    #region 功率自行车下肢

    static float current_Bike_Lowerlimbs_angle=0;
    /// <summary>
    /// 左脚下踩初始Hip_L（-170，0，-40）；Knee_L（-5，0，60）；Ankle_L（-15，0，-20）
    /// Hip_L（-170，0，-110）；Knee_L（-5，0，130）；Ankle_L（-15，0，-20）
    /// Hip_L.z活动范围70   Knee_L.z活动范围70
    /// 右脚高位初始值Hip_R（20，-180，-110）；Knee_R（-5，0，130）；Ankle_R（-15，0，-20）
    /// Hip_R（20，-180，-30） Knee_R（-5，0，60）
    /// </summary>
    public static void Bike_Lowerlimbs(Transform hip_L,Transform knee_L, Transform hip_R, Transform knee_R,float angle)
    {
        hip_L.localEulerAngles=new Vector3(hip_L.localEulerAngles.x, hip_L.localEulerAngles.y, hip_L.localEulerAngles.z - (angle- current_Bike_Lowerlimbs_angle));
        knee_L.localEulerAngles = new Vector3(knee_L.localEulerAngles.x, knee_L.localEulerAngles.y, knee_L.localEulerAngles.z + (angle - current_Bike_Lowerlimbs_angle));

        hip_R.localEulerAngles = new Vector3(hip_R.localEulerAngles.x, hip_R.localEulerAngles.y, hip_R.localEulerAngles.z + (angle - current_Bike_Lowerlimbs_angle));
        knee_R.localEulerAngles = new Vector3(knee_R.localEulerAngles.x, knee_R.localEulerAngles.y, knee_R.localEulerAngles.z - (angle - current_Bike_Lowerlimbs_angle));
        current_Bike_Lowerlimbs_angle = angle;
    }
    #endregion

    #region 前臂旋转

    #endregion

    //private void OnAnimatorIK()
    //{
    //    anim.SetLookAtWeight(1);
    //    anim.SetLookAtPosition(head.position); //头部看向
    //    anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    //    anim.SetIKPosition(AvatarIKGoal.LeftHand, lefthand.position); //左手位置
    //    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //    anim.SetIKPosition(AvatarIKGoal.RightHand, righthand.position); //右手位置
    //    anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
    //    anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftfoot.position); //左脚位置
    //    anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
    //    anim.SetIKPosition(AvatarIKGoal.RightFoot, rightfoot.position); //右脚位置


    //}

}
