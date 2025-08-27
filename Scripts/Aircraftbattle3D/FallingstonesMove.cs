using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingstonesMove : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOLocalMoveX(-250,5F).SetEase(Ease.Linear).OnComplete(FallingstonesMoveEnd);
        
    }

    void FallingstonesMoveEnd()
    {
        if (gameObject.activeInHierarchy)
        {
            PlayerAircraft.grade += 0.5f;
            PlayerAircraft.accomplishnumber.text = "躲过陨石波数：" + PlayerAircraft.grade;
            AudioPlayer.instantiate.PlayerTherapeutistAudio("完成动作鼓励");//完成后的语音
        }
        
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up*0.5f,Space.Self);
    }
   

}
