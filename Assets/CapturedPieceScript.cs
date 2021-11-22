using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedPieceScript : MonoBehaviour
{
    public GameObject r; //Red piece prefab
    public GameObject b; //Black piece prefab
    public GameObject rKing; //Red king piece prefab
    public GameObject bKing; //Black king piece prefab

    GameObject[,] redCapturedArray = new GameObject[4, 3]; //All pieces Red player Captured
    GameObject[,] blackCapturedArray = new GameObject[4, 3]; //all pieces Black player captured

    // Start is called before the first frame update
    void Start()
    {
        rKing.tag = "Untagged";
        bKing.tag = "Untagged";
        r.tag = "Untagged";
        b.tag = "Untagged";

        redCapturedArray = new GameObject[4, 3]   { { null, null, null},
                                                    { null, null, null},
                                                    { null, null, null},
                                                    { null, null, null} };

        blackCapturedArray = new GameObject[4, 3]   { { null, null, null},
                                                      { null, null, null},
                                                      { null, null, null},
                                                      { null, null, null} };
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void updateCapturedBoard()
    {


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (redCapturedArray[i, j] != null)
                {
                    Instantiate(redCapturedArray[i, j], new Vector3(i - 6.5f, 0.02f, j), Quaternion.identity);
                }
            }
        }


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (blackCapturedArray[i, j] != null)
                {
                    Instantiate(blackCapturedArray[i, j], new Vector3(i + 10.5f, 0.02f, j + 5f), Quaternion.identity);
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
                if (blackCapturedArray[i, j] == null)
                {
                    blackCapturedArray[i, j] = r;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
        updateCapturedBoard();
    }

    public void addRedKingPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (blackCapturedArray[i, j] == null)
                {
                    blackCapturedArray[i, j] = rKing;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
        updateCapturedBoard();
    }

    public void addBlackPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (redCapturedArray[i, j] == null)
                {
                    redCapturedArray[i, j] = b;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
        updateCapturedBoard();
    }

    public void addBlackKingPiece()
    {
        int loop = 1; //Used to break out of the for

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (redCapturedArray[i, j] == null)
                {
                    redCapturedArray[i, j] = bKing;
                    loop = 0;
                    break;
                }
            }

            if (loop == 0) break;
        }
        updateCapturedBoard();
    }
}
