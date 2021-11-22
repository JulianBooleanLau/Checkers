using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    public GameScript gameScript;
    public Image img;
    public Text text;

    private void Update()
    {
        changeColor();
        changeText();
    }
    void changeText()
    {
        if (gameScript.whosTurnIsIt == 1)
        {
            text.text = "End Red's turn";
        }
        else
        {
            text.text = "End Black's turn";
        }
    }

    void changeColor()
    {
        //Someone made a capture, turn the colour to green
        if (gameScript.didCaptureOccur == true)
        {
            img.GetComponent<Image>().color = Color.green;
            text.color = Color.black;
        }
        else if (gameScript.didCaptureOccur == false)
        {
            img.GetComponent<Image>().color = Color.gray;
            text.color = Color.white;
        }
    }

    public void switchTurn()
    {
        if(gameScript.didCaptureOccur == true)
        {
            gameScript.whosTurnIsIt = gameScript.whosTurnIsIt * -1;
            gameScript.didCaptureOccur = false;
            gameScript.deselectCurrent();
        }   
    }
}
