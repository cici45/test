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
        #region 组件获取
        head_Neck = GameObjFind(transform, "BD_Neck_jnt");//后颈
        head_Head = GameObjFind(transform, "BD_Head_jnt");//颈部

        body_Hips = GameObjFind(transform, "BD_Spine_01_jnt");//肚脐
        body_Spine= GameObjFind(transform, "BD_Spine_02_jnt");//左髋关节
        body_Chest = GameObjFind(transform, "BD_Spine_03_jnt");//左髋关节
        body_UpperChest = GameObjFind(transform, "BD_Spine_04_jnt");//左髋关节

        leftArm_Shoulder = GameObjFind(transform, "L_BD_Collar_jnt");//左髋关节
        leftArm_UpperArm = GameObjFind(transform, "L_BD_NB_Shoulder_jnt");//左髋关节
        leftArm_LoowerArm = GameObjFind(transform, "L_BD_NB_Forearm_Twist_02_jnt");//左髋关节
        leftArm_Hand = GameObjFind(transform, "L_BD_Wrist_01_jnt");//左髋关节

        rightArm_Shoulder = GameObjFind(transform, "R_BD_Collar_jnt");//右髋关节
        rightArm_UpperArm = GameObjFind(transform, "R_BD_Collar_Tip_jnt");//右髋关节
        rightArm_LoowerArm = GameObjFind(transform, "R_BD_NB_Forearm_Twist_01_jnt");//右髋关节
        rightArm_Hand = GameObjFind(transform, "R_BD_NB_Forearm_Twist_02_jnt");//右髋关节

        leftLeg_UpperLeg = GameObjFind(transform, "L_BD_NB_Hip_jnt");//左髋关节
        leftLeg_LowerLeg = GameObjFind(transform, "L_BD_NB_Knee_jnt");//左膝关节
        leftLeg_Foot = GameObjFind(transform, "L_BD_NB_Ankle_jnt"); //左踝关节
        leftLeg_Toes = GameObjFind(transform, "L_BD_NB_Ball_jnt");  //左前脚掌关节

        rightLeg_UpperLeg = GameObjFind(transform, "R_BD_NB_Hip_jnt");  //右髋关节
        rightLeg_LowerLeg = GameObjFind(transform, "R_BD_NB_Knee_jnt"); //右膝关节
        rightLeg_Foot = GameObjFind(transform, "R_BD_NB_Ankle_jnt");    //右踝关节
        rightLeg_Toes = GameObjFind(transform, "R_BD_NB_Ball_jnt");     //右前脚掌关节
        #endregion

        #region 添加物体
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

        #region 测试
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
        //Debug.Log("插值："+x);
    }

    /// <summary>
    /// 人物模型关节点角度修改
    /// 顺序： 颌骨关节、颈关节、
    /// 肩胛关节、胸椎关节、腰椎关节、髋关节
    /// 左肩胛关节、左肩关节、左肘关节、左腕关节
    /// 右肩胛关节、右肩关节、右肘关节、右腕关节
    /// 左髋关节、左膝关节、左踝关节、左前脚掌关节
    /// 右髋关节、右膝关节、右踝关节、右前脚掌关节
    /// </summary>
    /// <param name="SkeletonRot">实时角度值</param>
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
            Debug.LogError("错误："+e);
        }
    }


    /// <summary>
    /// 递归查找
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
