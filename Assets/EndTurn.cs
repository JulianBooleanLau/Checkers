using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Edit UI text

public class EndTurn : MonoBehaviour
{
    public static Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GameObject.Find("EndTurn").GetComponent<Image>();
    }

    public void turnEnded()
    {
        if (GameScript.moveMade != 0)
        {
            GameScript.moveMade = 0;
            GameScript.whichPlayersTurn *= -1;
            img.color = Color.gray;
            GameScript.didEndTurn = true;
        }
    }


}
