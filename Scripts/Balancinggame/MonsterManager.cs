using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOLocalMoveY(-1220,5f).SetEase(Ease.Linear).OnComplete(MoveEnd);
    }

    void MoveEnd()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float showtime = 5;
    bool ist = true;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.DOPause();

        transform.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(End);

    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
