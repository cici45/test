using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSkeletonControl : MonoBehaviour
{
    public Transform body_Hips, body_Spine, body_Chest, body_UpperChest,
        leftArm_Shoulder, leftArm_UpperArm, leftArm_LoowerArm, leftArm_Hand,
        rightArm_Shoulder, rightArm_UpperArm, rightArm_LoowerArm, rightArm_Hand,
        leftLeg_UpperLeg, leftLeg_LowerLeg, leftLeg_Foot, leftLeg_Toes,
        rightLeg_UpperLeg, rightLeg_LowerLeg, rightLeg_Foot, rightLeg_Toes,
        head_Neck,head_Head;

    static List<Transform> skeleton = new List<Transform>();
    void Start()
    {
        #region �����ȡ
        head_Neck = GameObjFind(transform, "BD_Neck_jnt");//��
        head_Head = GameObjFind(transform, "BD_Head_jnt");//����

        body_Hips = GameObjFind(transform, "BD_Spine_01_jnt");//����
        body_Spine= GameObjFind(transform, "BD_Spine_02_jnt");//���Źؽ�
        body_Chest = GameObjFind(transform, "BD_Spine_03_jnt");//���Źؽ�
        body_UpperChest = GameObjFind(transform, "BD_Spine_04_jnt");//���Źؽ�

        leftArm_Shoulder = GameObjFind(transform, "L_BD_Collar_jnt");//���Źؽ�
        leftArm_UpperArm = GameObjFind(transform, "L_BD_NB_Shoulder_jnt");//���Źؽ�
        leftArm_LoowerArm = GameObjFind(transform, "L_BD_NB_Forearm_Twist_02_jnt");//���Źؽ�
        leftArm_Hand = GameObjFind(transform, "L_BD_Wrist_01_jnt");//���Źؽ�

        rightArm_Shoulder = GameObjFind(transform, "R_BD_Collar_jnt");//���Źؽ�
        rightArm_UpperArm = GameObjFind(transform, "R_BD_Collar_Tip_jnt");//���Źؽ�
        rightArm_LoowerArm = GameObjFind(transform, "R_BD_NB_Forearm_Twist_01_jnt");//���Źؽ�
        rightArm_Hand = GameObjFind(transform, "R_BD_NB_Forearm_Twist_02_jnt");//���Źؽ�

        leftLeg_UpperLeg = GameObjFind(transform, "L_BD_NB_Hip_jnt");//���Źؽ�
        leftLeg_LowerLeg = GameObjFind(transform, "L_BD_NB_Knee_jnt");//��ϥ�ؽ�
        leftLeg_Foot = GameObjFind(transform, "L_BD_NB_Ankle_jnt"); //���׹ؽ�
        leftLeg_Toes = GameObjFind(transform, "L_BD_NB_Ball_jnt");  //��ǰ���ƹؽ�

        rightLeg_UpperLeg = GameObjFind(transform, "R_BD_NB_Hip_jnt");  //���Źؽ�
        rightLeg_LowerLeg = GameObjFind(transform, "R_BD_NB_Knee_jnt"); //��ϥ�ؽ�
        rightLeg_Foot = GameObjFind(transform, "R_BD_NB_Ankle_jnt");    //���׹ؽ�
        rightLeg_Toes = GameObjFind(transform, "R_BD_NB_Ball_jnt");     //��ǰ���ƹؽ�
        #endregion

        #region �������
        skeleton.Add(head_Neck);
        skeleton.Add(head_Head);

        skeleton.Add(body_Hips);
        skeleton.Add(body_Spine);
        skeleton.Add(body_Chest);
        skeleton.Add(body_UpperChest);

        skeleton.Add(leftArm_Shoulder);
        skeleton.Add(leftArm_UpperArm);
        skeleton.Add(leftArm_LoowerArm);
        skeleton.Add(leftArm_Hand);

        skeleton.Add(rightArm_Shoulder);
        skeleton.Add(rightArm_UpperArm);
        skeleton.Add(rightArm_LoowerArm);
        skeleton.Add(rightArm_Hand);

        skeleton.Add(leftLeg_UpperLeg);
        skeleton.Add(leftLeg_LowerLeg);
        skeleton.Add(leftLeg_Foot);
        skeleton.Add(leftLeg_Toes);

        skeleton.Add(rightLeg_UpperLeg);
        skeleton.Add(rightLeg_LowerLeg);
        skeleton.Add(rightLeg_Foot);
        skeleton.Add(rightLeg_Toes);
        #endregion

        #region ����
        //foreach (Transform t in skeleton)
        //{
        //    Debug.Log(t.name+"\n"+ "localEulerAngles:  " + t.localEulerAngles+"\n"+ "localRotation:  " + t.localRotation);
        //}

        //Vector2 v01 = new Vector2(-10f,-10f);
        //List<Vector2> vector2s = new List<Vector2>();
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //vector2s.Add(v01);
        //RefreshModeSkeleton(vector2s);

        #endregion

        //Debug.Log(720%360);
        //float x = Mathf.Lerp(1f, 0f, 0.02f) * 100;
        //Debug.Log("��ֵ��"+x);
    }

    /// <summary>
    /// ����ģ�͹ؽڵ�Ƕ��޸�
    /// ˳�� �ǹؽڡ����ؽڡ�
    /// ���ιؽڡ���׵�ؽڡ���׵�ؽڡ��Źؽ�
    /// ����ιؽڡ����ؽڡ�����ؽڡ�����ؽ�
    /// �Ҽ��ιؽڡ��Ҽ�ؽڡ�����ؽڡ�����ؽ�
    /// ���Źؽڡ���ϥ�ؽڡ����׹ؽڡ���ǰ���ƹؽ�
    /// ���Źؽڡ���ϥ�ؽڡ����׹ؽڡ���ǰ���ƹؽ�
    /// </summary>
    /// <param name="SkeletonRot">ʵʱ�Ƕ�ֵ</param>
    public void RefreshModeSkeleton(List<Vector2> SkeletonRot)
    {
        try
        {
            for(int index=0;index< SkeletonRot.Count; index++)
            {
                skeleton[index].localEulerAngles = new Vector3(skeleton[index].localEulerAngles.x+SkeletonRot[index].x,
                    skeleton[index].localEulerAngles.y + SkeletonRot[index].y, skeleton[index].localEulerAngles.z);
                
            }
        }
        catch(Exception e)
        {
            Debug.LogError("����"+e);
        }
    }


    /// <summary>
    /// �ݹ����
    /// </summary>
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
