using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    public GameScript gameScript;
    public Timer timer;
    public Image img;
    public Text text;

    private void Update()
    {
        changeColor();
        changeText();
        if (timer.currentTime < 0) {
            endturn();
        }
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
            endturn();
        }   
    }

    public void endturn()
    {
        gameScript.whosTurnIsIt *= -1;
        gameScript.didCaptureOccur = false;
        gameScript.deselectCurrent();
        timer.currentTime = timer.startTime;
    }
}
