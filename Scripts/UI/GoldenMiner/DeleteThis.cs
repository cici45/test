using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteThis : MonoBehaviour
{
    float time = 0;
    void Update()
    {
        if (!GoldenMiner.isPause)
        {
            time += Time.deltaTime;
        }
        if(time> GoldenMiner.timer)
        {
            Destroy(gameObject);
        }
    }
}
