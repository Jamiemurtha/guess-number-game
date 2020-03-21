using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksController : MonoBehaviour
{
    ParticleSystem firework;

    ParticleSystem.MainModule fireworkMain;
    void Awake()
    {
        firework = GetComponent<ParticleSystem>();
        fireworkMain = firework.main;
        fireworkMain.startColor = new Color(Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f));
    }
    void Update()
    {
        if (!firework.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
