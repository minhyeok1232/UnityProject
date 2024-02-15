using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public ParticleSystem levelUpCustom;
    public GameObject leveUpText;

    public float loopTime;
    float currentTime;


    void Start()
    {
        Instantiate(leveUpText, levelUpCustom.transform.position, levelUpCustom.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;


        if (currentTime <= 0)
        {
            Reset();
        }

    }

    void Reset()
    {
        levelUpCustom.Clear();
        levelUpCustom.Play();
        Instantiate(leveUpText, levelUpCustom.transform.position, levelUpCustom.transform.rotation);
        currentTime = loopTime;
    }
}
