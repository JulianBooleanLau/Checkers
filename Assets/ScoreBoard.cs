using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static int redScore;
    public static int blackScore;

    private void OnGUI()
    {
        GUI.Box(new Rect(0, Screen.height - 50, Screen.width/4, 50), "Red: " + redScore.ToString() + "\n\nBlack: " + blackScore.ToString());
    }

}
