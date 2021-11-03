using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Edit UI text

public class BlackScoreScript : MonoBehaviour
{
    public Text blackScoreText;
    public static int blackCurrScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        blackScoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        blackScoreText.text = "Black Score: " + blackCurrScore;
    }
}
