using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public void switchTurn()
    {
        GameScript.whosTurnIsIt = GameScript.whosTurnIsIt * -1;
    }
}
