using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMeasure : MonoBehaviour
{
    private int numberOfFrames = 0;
    private float elapsedTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        numberOfFrames++;
    
        if (Time.time > elapsedTime)
        {
            Debug.Log("FPS: " + numberOfFrames);
            numberOfFrames = 0;
            
            elapsedTime = Time.time + 1;
        }
    }
}
