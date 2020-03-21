using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulser : MonoBehaviour
{
    float timer = 2.61f;

    bool sizeIncreasing = false;

    void Update()
    {
        if (timer <= 0)
        {
            timer = 2.61f;
            sizeIncreasing = !sizeIncreasing;
        }

        if (sizeIncreasing)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 0.05f;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime * 0.05f;
        }
        timer -= Time.deltaTime;
    }
}
