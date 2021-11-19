using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedPieceScript : MonoBehaviour
{
    public GameObject r; //Red piece prefab
    public GameObject b; //Black piece prefab
    public GameObject rKing; //Red king piece prefab
    public GameObject bKing; //Black king piece prefab

    GameObject[,] redCaptured = new GameObject[4, 3]; //All pieces Red player Captured
    GameObject[,] blackCaptured = new GameObject[4, 3]; //all pieces Black player captured

    // Start is called before the first frame update
    void Start()
    {
        redCaptured = new GameObject[4, 3]   { { null, null, null},
                                               { null, null, null},
                                               { null, null, null},
                                               { null, null, null} };

        blackCaptured = new GameObject[4, 3]   { { null, null, null},
                                                 { null, null, null},
                                                 { null, null, null},
                                                 { null, null, null} };
    }

    // Update is called once per frame
    void Update()
    {
        //RedScoreScript.redCurrScore 

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (redCaptured[i, j] != null)
                {
                    Instantiate(redCaptured[i, j], new Vector3(i - 6.5f, 0.02f, j), Quaternion.identity);
                }
            }
        }


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (blackCaptured[i, j] != null)
                {
                    Instantiate(blackCaptured[i, j], new Vector3(i + 10.5f, 0.02f, j + 5f), Quaternion.identity);
                }
            }
        }
    }

    public void addRedPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (redCaptured[i, j] == null)
                {
                    redCaptured[i, j] = b;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
    }

    public void addRedKingPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (redCaptured[i, j] == null)
                {
                    redCaptured[i, j] = bKing;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
    }

    public void addBlackPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (blackCaptured[i, j] == null)
                {
                    blackCaptured[i, j] = r;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
    }

    public void addBlackKingPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (blackCaptured[i, j] == null)
                {
                    blackCaptured[i, j] = rKing;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
    }
}
