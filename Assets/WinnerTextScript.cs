using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Edit UI text

public class WinnerTextScript : MonoBehaviour
{
    public Text WinnerScoreText;
    public static string currWinner = "";

    // Start is called before the first frame update
    void Start()
    {
        WinnerScoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        WinnerScoreText.text = currWinner;
    }
}