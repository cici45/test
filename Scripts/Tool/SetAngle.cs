using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetAngle : MonoBehaviour
{
    //public Animator anim;      //����
    //public Transform head;     //ͷ��
    //public Transform lefthand;  //����
    //public Transform righthand; //����
    //public Transform leftfoot;  //���
    //public Transform rightfoot; //�ҽ�
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
        Debug.Log("��ʼֵ��"+transform.localEulerAngles.z);
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


    #region �ⲿ��ת
    /// <summary>
    /// �����ⲿ�ؽڵ��޸�
    /// </summary>
    /// <param name="transforms">�����ⲿ�ؽڵ�</param>
    /// <param name="angle">ʵʱ�Ƕ�ֵ</param>
    /// <param name="max">�趨������˶��Ƕ�</param>
    public void ElbowRotationAni(float angle)
    {

        elbowTransform.localEulerAngles = new Vector3(elbowTransform.localEulerAngles.x, elbowTransform.localEulerAngles.y, -angle);
        Debug.Log("�Ƕȣ�" + elbowTransform.localEulerAngles.z);
        elbowcurrentangle = angle;
    }
    #endregion

    #region ƽ��
    static float currenterLshoulder;
    static float currenterLelbow;
    static float currenterRshoulder;
    static float currenterRelbow;
    /// <summary>
    /// ˮƽ�˶�  
    /// ��ʼ��
    /// Scapula_L 60  -25  50
    /// Shoulder_L130  35 -10  ��Ҫ�޸ĵ�
    /// Elbow_L  -25  0  -35   ��Ҫ�޸ĵ�
    /// 
    /// Scapula_R 0  -10  -40
    /// Shoulder_R -5  -5  -10 ��Ҫ�޸ĵ�
    /// Elbow_R  0  0  -35     ��Ҫ�޸ĵ�
    /// </summary>
    /// <param name="Lshoulder">��-�粿�ؽڵ�</param>
    /// <param name="Lelbow">���ⲿ�ؽڵ�</param>
    /// <param name="Rshoulder">��-�粿�ؽڵ�</param>
    /// <param name="Relbow">��-�ⲿ�ؽڵ�</param>
    /// <param name="shoulderangle">�粿�仯ֵ</param>
    /// <param name="elbowangle">�ⲿ�仯ֵ</param>
    public static void ShouldersAndElbow(Transform Lshoulder,Transform Lelbow,Transform Rshoulder,Transform Relbow,float shoulderangle,float elbowangle)
    {
        // ��ؽ�����Ƕ� 45��  ��ؽ�����Ƕ� 75     �������ĽǶ�ֵ=����ģ������˶��Ƕ�ֵ*��ʵʱ�Ƕ�ֵ/��Ϸ������õ�����˶��Ƕ�ֵ��
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

    #region �Ͼ�
    static Vector3 Lshoulder01current;
    static Vector3 Lshouldercurrent;
    static Vector3 Lelbowcurrent;
    static Vector3 Rshouldercurrent;
    static Vector3 Relbowcurrent;
    /// <summary>
    /// ˮƽ�˶�  
    /// ��ʼ��
    /// Scapula_L 50  -20  40    20  -20  100  ��ֵ��30  0  60
    /// Shoulder_L 100  0 -25  ��Ҫ�޸ĵ� 100  0  0 ��ֵ�� 0  0  25
    /// Elbow_L  50  -25  -120   ��Ҫ�޸ĵ�50  0  -35  ��ֵ��0  25  85
    /// 
    /// Scapula_R 25  -115 -30
    /// Shoulder_R 20  -25  25 ��Ҫ�޸ĵ�     20  -25  -50  ��ֵ��0  25  75
    /// Elbow_R  -20  -50  -120    ��Ҫ�޸ĵ� 20  0  -35  ��ֵ��40  50  85
    /// </summary>
    /// /// <param name="Lshoulder01">��-���ιǲ��ؽڵ�</param>
    /// <param name="Lshoulder">��-�粿�ؽڵ�</param>
    /// <param name="Lelbow">���ⲿ�ؽڵ�</param>
    /// <param name="Rshoulder">��-�粿�ؽڵ�</param>
    /// <param name="Relbow">��-�ⲿ�ؽڵ�</param>
    /// <param name="shoulderangle">�粿�仯ֵ</param>
    /// <param name="elbowangle">�ⲿ�仯ֵ</param>
    public static void UpthrowAni(Transform Lshoulder01,Transform Lshoulder, Transform Lelbow,  Transform Rshoulder, Transform Relbow,  float angle)
    {
        // ��ؽ�����Ƕ� 45��  ��ؽ�����Ƕ� 75     �������ĽǶ�ֵ=����ģ������˶��Ƕ�ֵ*��ʵʱ�Ƕ�ֵ/��Ϸ������õ�����˶��Ƕ�ֵ��
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

    #region ��������
    static float currentabdominal = 0;

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="transform">�����ؽڵ�RootPart1_M��ģ������˶���65</param>
    /// <param name="angle"></param>
    public static void Abdominalflexible(Transform transform, float angle)
    {

        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - (angle - currentabdominal));
        currentabdominal = angle;
    }

    #endregion

    #region ����������չ
    static float thighcurrentangel=0;
    /// <summary>
    /// ����������չ��������
    /// ģ��ʵ������˶���Χ40��
    /// ����״̬�³�ʼֵ  Hip_L��-180��0��-90����Knee_L��0��0��90����Hip_R��0��-180��-90����Knee_R��0��0��90��
    /// ��չ״̬�³�ʼֵ  Hip_L��-140��0��-90����Knee_L��0��0��90����Hip_R��40��-180��-90����Knee_R��0��0��90��
    /// </summary>
    /// <param name="hip_L">�����Źؽڵ�</param>
    /// <param name="hip_R">�����Źؽڵ�</param>
    /// <param name="angle">ת����ʵ�ʵ��˶��Ƕ�</param>
    public static void ThighAdductionOrAbduce(Transform hip_L,Transform hip_R,float angle)
    {
        hip_L.localEulerAngles = new Vector3(hip_L.localEulerAngles.x+(angle- thighcurrentangel), hip_L.localEulerAngles.y, hip_L.localEulerAngles.z);
        hip_R.localEulerAngles = new Vector3(hip_R.localEulerAngles.x - (angle - thighcurrentangel), hip_R.localEulerAngles.y, hip_R.localEulerAngles.z);
        thighcurrentangel = angle;
    }
    #endregion

    #region ����ת
    static float wristcurrentangle=0;
    /// <summary>
    /// ����ת
    /// �ֲ���ʼֵ  Scapula_R��20��-130��-70����Shoulder_R��-15��-25��0����Elbow_R��40��0��0����Wrist_R��0��0��0��
    /// ����Χ0-60
    /// �޸�Elbow_R��xֵ
    /// </summary>
    /// <param name="wrist">�ⲿ�ؽڵ�Elbow_R</param>
    /// <param name="angle">ʵ��ת�����˶��ĽǶ�ֵ</param>
    public static void WristSpin(Transform wrist,float angle)
    {
        wrist.localEulerAngles = new Vector3(wrist.localEulerAngles.x - (angle- wristcurrentangle), wrist.localEulerAngles.y, wrist.localEulerAngles.z);
        wristcurrentangle = angle;
    }
    #endregion

    #region ��֫Э��
    static float currentshoulder_R_angle=0;
    /// <summary>
    /// ��֫Э�� 
    /// Scapula_R��ʼ��0��-110��-50��  ���Χ 30
    /// Shoulder_R��-15��-25��0��
    /// Elbow_R��40��0��-20��
    /// Shoulder_L��ʼ��130��25��-10��  ���Χ 30
    /// Scapula_L��60��-10��20��
    /// Elbow_L��0��0��-25��
    /// </summary>
    /// <param name="shoulder_R">�޸�Scapula_R�ؽڵ�</param>
    /// <param name="shoulder_L">�޸�Shoulder_L�ؽڵ�</param>
    /// <param name="shoulder_angle"></param>
    public static void ShoulderCoordinate(Transform shoulder_R,Transform shoulder_L,float shoulder_angle)
    {
        shoulder_R.localEulerAngles = new Vector3(shoulder_R.localEulerAngles.x - (shoulder_angle - currentshoulder_R_angle), shoulder_R.localEulerAngles.y, shoulder_R.localEulerAngles.z);
        shoulder_L.localEulerAngles = new Vector3(shoulder_L.localEulerAngles.x, shoulder_L.localEulerAngles.y, shoulder_L.localEulerAngles.z - (shoulder_angle - currentshoulder_R_angle));
        currentshoulder_R_angle = shoulder_angle;
    }
    #endregion

    #region �������г���֫
    static float current_Bike_Upperlimb_angle = 0;
    /// <summary>
    /// shoulder_L��λShoulder_L(120,25,0);Elbow_L(0,0,-90)
    /// ��λShoulder_L(120,25,-30);Elbow_L(0,0,-60)
    /// �޸�z��   ���Χ30
    /// 
    /// Scapula_R��-15��-110��-50����Shoulder_R��-15��-25��0����ShoulderPart1_R��0��0��0����Elbow_R��60��0��-90��
    /// ShoulderPart1_R��0��30��0����Elbow_R��60��0��-60��
    /// </summary>
    /// <param name="shoulder_L">Shoulder_L�ڵ�z��</param>
    /// <param name="elbow_L">Elbow_L�ڵ�z��</param>
    /// <param name="shoulder_R">ShoulderPart1_R�ڵ�Y��</param>
    /// <param name="elbow_R">Elbow_R�ڵ�Z��</param
    /// <param name="angle">�仯��ʵ�ʽǶ�ֵ</param>
    public static void Bike_Upperlimb(Transform shoulder_L, Transform elbow_L, Transform shoulder_R, Transform elbow_R, float angle)
    {
        shoulder_L.localEulerAngles = new Vector3(shoulder_L.localEulerAngles.x, shoulder_L.localEulerAngles.y, shoulder_L.localEulerAngles.z + (angle - current_Bike_Upperlimb_angle));
        elbow_L.localEulerAngles = new Vector3(elbow_L.localEulerAngles.x, elbow_L.localEulerAngles.y, elbow_L.localEulerAngles.z - (angle - current_Bike_Upperlimb_angle));

        shoulder_R.localEulerAngles = new Vector3(shoulder_R.localEulerAngles.x, shoulder_R.localEulerAngles.y + (angle - current_Bike_Upperlimb_angle), shoulder_R.localEulerAngles.z);
        elbow_R.localEulerAngles = new Vector3(elbow_R.localEulerAngles.x, elbow_R.localEulerAngles.y, elbow_R.localEulerAngles.z + (angle - current_Bike_Upperlimb_angle));
        current_Bike_Upperlimb_angle = angle;
    }
    #endregion

    #region �������г���֫

    static float current_Bike_Lowerlimbs_angle=0;
    /// <summary>
    /// ����²ȳ�ʼHip_L��-170��0��-40����Knee_L��-5��0��60����Ankle_L��-15��0��-20��
    /// Hip_L��-170��0��-110����Knee_L��-5��0��130����Ankle_L��-15��0��-20��
    /// Hip_L.z���Χ70   Knee_L.z���Χ70
    /// �ҽŸ�λ��ʼֵHip_R��20��-180��-110����Knee_R��-5��0��130����Ankle_R��-15��0��-20��
    /// Hip_R��20��-180��-30�� Knee_R��-5��0��60��
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

    #region ǰ����ת

    #endregion

    //private void OnAnimatorIK()
    //{
    //    anim.SetLookAtWeight(1);
    //    anim.SetLookAtPosition(head.position); //ͷ������
    //    anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    //    anim.SetIKPosition(AvatarIKGoal.LeftHand, lefthand.position); //����λ��
    //    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //    anim.SetIKPosition(AvatarIKGoal.RightHand, righthand.position); //����λ��
    //    anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
    //    anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftfoot.position); //���λ��
    //    anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
    //    anim.SetIKPosition(AvatarIKGoal.RightFoot, rightfoot.position); //�ҽ�λ��


    //}

}
