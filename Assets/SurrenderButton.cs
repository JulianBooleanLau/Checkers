using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderButton : MonoBehaviour
{

    public GameScript gameScript;
    public void surrender()
    {
        if (gameScript.whosTurnIsIt == 1)
        {
            BlackScoreScript.blackCurrScore = 12;
        }
        else
        {
            RedScoreScript.redCurrScore = 12;
        }
    }
}
