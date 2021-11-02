using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Edit UI text

public class RedScoreScript : MonoBehaviour
{
    public Text redScoreText;
    public static int redCurrScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        redScoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        redScoreText.text = "Red Score: " + redCurrScore;
    }
}
