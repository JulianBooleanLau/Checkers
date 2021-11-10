using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    //UI variables
    public GameObject PauseScreen;
    public GameObject WinScreen;
    Color endTurnColorClickable = Color.green;
    Color endTurnColorNonClickable = Color.gray;


    //Variables initalized using the drag and drop on inspector
    public GameObject r; //Red piece prefab
    public GameObject b; //Black piece prefab
    public GameObject rKing;
    public GameObject bKing;
    private Transform selectedPiece; //Current selected piece
    public static int whichPlayersTurn; //Value = 1 if it's red's turn value = -1 if it's blacks turn
    public static int moveMade;
    public static bool didEndTurn;
 

    //Variables used for piece highlighting
    public Material highlightMaterial; //Material used when piece is highlighted
    private Material selectedPieceStartingMaterial;
    private string selectedPieceStartingTag;   
    

    GameObject[,] pieceArray = new GameObject[8, 8];

    // Start is called before the first frame update
    void Start()
    {
        RedScoreScript.redCurrScore = 0;
        BlackScoreScript.blackCurrScore = 0;
        whichPlayersTurn = 1;
        selectedPiece = null;
        moveMade = 0;
        didEndTurn = false;
        pieceArray = new GameObject[8, 8] { { r, null, r, null, r, null, r, null },
                                            { null, r, null, r, null, r, null, r },
                                            { r, null, r, null, r, null, r, null },
                                            { null, null, null, null, null, null, null, null },
                                            { null, null, null, null, null, null, null, null },
                                            { null, b, null, b, null, b, null, b },
                                            { b, null, b, null, b, null, b, null },
                                            { null, b, null, b, null, b, null, b } };



        //Instatiate the board full of pieces. Note* Always loop through the pieceArray using j,i or the orientation will be sideways.
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieceArray[j, i] != null)
                {
                    Instantiate(pieceArray[j,i], new Vector3(i, 0.22f, j), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen.SetActive(true);
        }
        if(didEndTurn == true && selectedPiece != null)
        {
            deselectCurrent();
            didEndTurn = false;
        }
        mouseClick();
    }

    //Allows the user to select a game piece
    void mouseClick()
    {
        //Do things if you click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objectFound = new RaycastHit();
            bool click = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out objectFound);
            if (click)
            {
                Vector3 pos = objectFound.transform.position;

                if ( ( (objectFound.transform.tag == "redPiece" && whichPlayersTurn == 1) || (objectFound.transform.tag == "blackPiece" && whichPlayersTurn == -1) ) && selectedPiece == null && moveMade == 0)
                {
                    selectPiece(objectFound, click);
                }
                else if (objectFound.transform.tag == "selectedPiece" && selectedPiece != null && moveMade != 2)
                {
                    deselectCurrent();
                }
                //The reason why the if statement is weird is due to floating point number comparions.
                else if (objectFound.transform.tag == "square" && selectedPiece != null && moveMade != 1)
                {
                    selectSquare(objectFound.transform);
                }
            }
            //If the player clicks a free square(ie no piece is there) while a piece is highlighted
            //then move the highlighted piece to the new square and change it's colour to the original colour
        }
    }


    //Handling selecting a piece
    void selectPiece(RaycastHit pieceFound, bool click)
    {   
        //Find out what was clicked on
        selectedPiece = pieceFound.transform;

        //Highlighting
        selectedPieceStartingMaterial = selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material;
        selectedPieceStartingTag = selectedPiece.tag;
        
        selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material = highlightMaterial;
        selectedPiece.tag = "selectedPiece";
    }

    void selectSquare(Transform square)
    {
        //functions return 0 if nothing happened; they return 1 if a simple move was made (no capture); they return 2 if a piece was captured
        if (selectedPieceStartingTag == "redPiece" && selectedPiece.transform.childCount == 1 && moveMade != 1)
        {
            redPieceMove(square);
        }
        if(selectedPieceStartingTag == "blackPiece" && selectedPiece.transform.childCount == 1 && moveMade != 1)
        {
            blackPieceMove(square);
        }
        if (selectedPiece.transform.childCount == 2 && moveMade != 1)
        {
            kingPieceMove(square);
        }

        //Check what move the player made
        if(moveMade == 1)
        {
            EndTurn.img.color = endTurnColorClickable;
            //Change piece colour back to its original colour and set selected piece to null
            deselectCurrent();
        }
        else if(moveMade == 2)
        {
            EndTurn.img.color = endTurnColorClickable;
        }

        Debug.Log(moveMade);
        //Check if the selected piece has been moved to a king square.
        checkForKing();
    }

    void redPieceMove(Transform square)
    {
        //Change position of red piece to a different square if it's valid
        if (selectedPieceStartingTag == "redPiece" && selectedPiece.transform.childCount == 1)
        {
            //Checking for legal single tile move

            //To move red piece 1 up 1 right
            if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850)) && moveMade != 2)
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z + 1);
                moveMade = 1;
            }
            //To move red piece 1 up 1 left
            else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850)) && moveMade != 2)
            {
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z + 1);
                moveMade = 1;
            }

            //Checking for legal capture move

            //For red piece to capture 2 up 2 right
            if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850))) && checkForCapturablePiece((float)(selectedPiece.position.x + 1), (float)(selectedPiece.position.z + 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 2, selectedPiece.position.y, selectedPiece.position.z + 2);
                moveMade = 2;
            } //For red piece to capture 2 up 2 left
            else if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850))) && checkForCapturablePiece((float)(selectedPiece.position.x - 1), (float)(selectedPiece.position.z + 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + -2, selectedPiece.position.y, selectedPiece.position.z + 2);
                moveMade = 2;
            }
        }
    }
    void blackPieceMove(Transform square)
    {
        //Change position of black piece to a different square if it's valid
        if (selectedPieceStartingTag == "blackPiece" && selectedPiece.transform.childCount == 1)
        {
            //Checking for legal single tile move

            //To move black piece 1 down 1 right
            if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)) && moveMade != 2)
            {
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z - 1);
                moveMade = 1;
            }
            //To move black piece 1 down 1 left
            else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)) && moveMade != 2)
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z - 1);
                moveMade = 1;
            }


            //Checking for legal capture move

            //For black piece to capture 2 down 2 right
            if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 2.150))) && checkForCapturablePiece((float)(selectedPiece.position.x + 1), (float)(selectedPiece.position.z - 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + +2, selectedPiece.position.y, selectedPiece.position.z - 2);
                moveMade = 2;
            }
            //For black piece to capture 2 down 2 left
            else if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 2.150))) && checkForCapturablePiece((float)(selectedPiece.position.x - 1), (float)(selectedPiece.position.z - 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 2, selectedPiece.position.y, selectedPiece.position.z - 2);
                moveMade = 2;
            }

        }
    }
    void kingPieceMove(Transform square)
    {
        //Change position of king piece to a different square if it's valid
        //Applies to both black and red king
        if (selectedPiece.transform.childCount == 2)
        {
            //Includes red piece movement

            //To move king piece 1 up 1 right
            if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850)) && moveMade != 2)
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z + 1);
                moveMade = 1;                                         
            }
            //To move king piece 1 up 1 left
            else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850)) && moveMade != 2)
            {
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z + 1);
                moveMade = 1;
            }

            //Checking for legal capture move

            //For king piece to capture 2 up 2 right
            if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850))) && checkForCapturablePiece((float)(selectedPiece.position.x + 1), (float)(selectedPiece.position.z + 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 2, selectedPiece.position.y, selectedPiece.position.z + 2);
                moveMade = 2;
            } //For king piece to capture 2 up 2 left
            else if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850))) && checkForCapturablePiece((float)(selectedPiece.position.x - 1), (float)(selectedPiece.position.z + 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + -2, selectedPiece.position.y, selectedPiece.position.z + 2);
                moveMade = 2;
            }


            //Includes Black piece movement

            //To move king piece 1 down 1 right
            if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)) && moveMade != 2)
            {
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z - 1);
                moveMade = 1;
            }
            //To move king piece 1 down 1 left
            else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)) && moveMade != 2)
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z - 1);
                moveMade = 1;
            }


            //Checking for legal capture move

            //For king piece to capture 2 down 2 right
            if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 2.150))) && checkForCapturablePiece((float)(selectedPiece.position.x + 1), (float)(selectedPiece.position.z - 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + +2, selectedPiece.position.y, selectedPiece.position.z - 2);
                moveMade = 2;
            }
            //For king piece to capture 2 down 2 left
            else if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 2.150))) && checkForCapturablePiece((float)(selectedPiece.position.x - 1), (float)(selectedPiece.position.z - 1)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 2, selectedPiece.position.y, selectedPiece.position.z - 2);
                moveMade = 2;
            }
        }
    }

    void checkForKing()
    {
        if(selectedPieceStartingTag == "redPiece" && selectedPiece.transform.position.z >= 7f)
        {
            Instantiate(rKing, new Vector3(selectedPiece.transform.position.x, 0.22f, selectedPiece.transform.position.z), Quaternion.identity);
            Destroy(selectedPiece.gameObject);
        }

        if (selectedPieceStartingTag == "blackPiece" && selectedPiece.transform.position.z <= 0f)
        {
            Instantiate(bKing, new Vector3(selectedPiece.transform.position.x, 0.22f, selectedPiece.transform.position.z), Quaternion.identity);
            Destroy(selectedPiece.gameObject);
        }
    }

    bool checkForCapturablePiece(float x, float z) {
        
        //position that we want to check
        Vector3 spawnPos = new Vector3(x, 0.13f, z);
        
        Collider[] hitColliders;
        
        float radius = (0.001f);
        
        //Checks if a object is inbetween the selected piece's original spot and the capture spot
        if (Physics.CheckSphere(spawnPos, radius)) 
        {

            hitColliders = Physics.OverlapSphere(spawnPos, radius);

            //Case for Red capturing a black piece
            if(hitColliders[0].gameObject.tag == "blackPiece" && selectedPieceStartingTag == "redPiece" )
            {
                Destroy(hitColliders[0].gameObject);


                //Increase score
                if (selectedPieceStartingTag == "redPiece")
                {
                    RedScoreScript.redCurrScore++; //Increase red score
                }
                else
                {
                    BlackScoreScript.blackCurrScore++; //Increase black core
                }

                //Check if player wins (12 points = win)
                if (RedScoreScript.redCurrScore == 12)
                {
                    WinnerTextScript.currWinner = "Red Wins!";
                    WinScreen.SetActive(true);
                }
                else if (BlackScoreScript.blackCurrScore == 12)
                {
                    WinnerTextScript.currWinner = "Black Wins!";
                    WinScreen.SetActive(true);
                }

                return true;
            } else if (hitColliders[0].gameObject.tag == "redPiece" && selectedPieceStartingTag == "blackPiece")
            {
                Destroy(hitColliders[0].gameObject);
                return true;
            }
        }
        return false;
    }

public void deselectCurrent()
    {
        selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material = selectedPieceStartingMaterial;
        selectedPiece.tag = selectedPieceStartingTag;
        selectedPieceStartingTag = null;
        selectedPieceStartingMaterial = null;
        selectedPiece = null;
    }

}

