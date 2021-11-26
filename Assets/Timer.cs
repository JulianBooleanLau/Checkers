using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startTime = 31;
    public float currentTime = 31;
    public Text timeText;

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;   
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
