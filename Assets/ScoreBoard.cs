using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static int redScore = 0;
    public static int blackScore = 0;

    private void OnGUI()
    {
        GUI.Box(new Rect(500, 44, 100, 75), "Red: " + redScore.ToString() + "\n\nBlack: " + blackScore.ToString());
    }

}
