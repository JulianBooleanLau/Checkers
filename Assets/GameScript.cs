using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    //UI variables
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public Timer timer;

    //Captured pieces appear on side
    public CapturedPieceScript capturedPieceScript; //Can be used to call methods like: capturedPieceScript.addRedPiece();

    //Variables initalized using the drag and drop on inspector
    public GameObject r; //Red piece prefab
    public GameObject b; //Black piece prefab
    public GameObject rKing; //Red king piece prefab
    public GameObject bKing; //Black king piece prefab
    private Transform selectedPiece; //Current selected piece
 

    //Variables used for piece highlighting
    public Material highlightMaterial; //Material used when piece is highlighted
    private Material selectedPieceStartingMaterial;
    private string selectedPieceStartingTag;
    public int whosTurnIsIt; //1 = red , -1 = black
    public bool didCaptureOccur; //True if a capture was made, false if it was a simple move

    //variable for cycling through the json file
    public SaveUser loadedObject;
    public bool isOver = false;   
    

    GameObject[,] pieceArray = new GameObject[8, 8];

    // Start is called before the first frame update
    void Start()
    {
        RedScoreScript.redCurrScore = 0;
        BlackScoreScript.blackCurrScore = 0;
        selectedPiece = null;
        whosTurnIsIt = 1;
        didCaptureOccur = false;

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
        checkWinner();

        movePiece();
    }

    //Allows the user to select a game piece
    void movePiece()
    {
        //Do things if you click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objectFound = new RaycastHit();
            bool click = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out objectFound);
            if (click)
            {
                Vector3 pos = objectFound.transform.position;

                if ( ( (objectFound.transform.tag == "redPiece"  && whosTurnIsIt == 1) || (objectFound.transform.tag == "blackPiece" && whosTurnIsIt == -1)) && selectedPiece == null && didCaptureOccur == false)
                {
                    Transform pieceFound = objectFound.transform;
                    selectPiece(pieceFound);
                }
                else if (objectFound.transform.tag == "selectedPiece" && selectedPiece != null && didCaptureOccur == false)
                {
                    deselectCurrent();
                }
                //The reason why the if statement is weird is due to floating point number comparions.
                else if (objectFound.transform.tag == "square" && selectedPiece != null)
                {
                    selectSquare(objectFound.transform);
                }
            }
            //If the player clicks a free square(ie no piece is there) while a piece is highlighted
            //then move the highlighted piece to the new square and change it's colour to the original colour
        }
    }


    //Handling selecting a piece
    void selectPiece(Transform pieceFound)
    {   
        //Find out what was clicked on
        selectedPiece = pieceFound;

        //Highlighting
        //Material highlightMaterial = Resources.Load("Assets/Materials/Highlight", typeof(Material)) as Material;
        selectedPieceStartingMaterial = selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material;
        selectedPieceStartingTag = selectedPiece.tag;
        
        selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material = highlightMaterial;
        selectedPiece.tag = "selectedPiece";
    }

    void selectSquare(Transform square)
    {
         //Change position of red piece to a different square if it's valid
        if (selectedPieceStartingTag == "redPiece" && selectedPiece.transform.childCount == 1)
        {
            //checking for legal moves for red pieces
            redCheck(square);
        }

        //Change position of black piece to a different square if it's valid
        else if (selectedPieceStartingTag == "blackPiece" && selectedPiece.transform.childCount == 1)
        {
            //checking for legal moves for black pieces
            blackCheck(square);
        }        

        //Change position of king piece to a different square if it's valid
        //Applies to both black and red king
        else if (selectedPiece.transform.childCount == 2)
        {
            //Checks for moves that kings can make
            kingCheck(square);
        }

        //Check if the selected piece has been moved to a king square.
        checkForKing();

        if(didCaptureOccur == false)
        {
            //Change piece colour back to its original colour and set selected piece to null
            deselectCurrent();
        }
        
    }

    void redSimpleMove(Transform square)
    {
        //To move red piece 1 up 1 right
        if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850)) && didCaptureOccur == false)
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z + 1);
            whosTurnIsIt *= -1;
            timer.currentTime = timer.startTime;
        }
        //To move red piece 1 up 1 left
        else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850)) && didCaptureOccur == false)
        {
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z + 1);
            whosTurnIsIt *= -1;
            timer.currentTime = timer.startTime;
        }
    }

    void redCaptureMove(Transform square)
    {
        //For red piece to capture 2 up 2 right
        if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850)))
        && checkForCapturablePiece((float)(selectedPiece.position.x + 1), (float)(selectedPiece.position.z + 1)))
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 2, selectedPiece.position.y, selectedPiece.position.z + 2);
            didCaptureOccur = true;
        } //For red piece to capture 2 up 2 left
        else if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850)))
        && checkForCapturablePiece((float)(selectedPiece.position.x - 1), (float)(selectedPiece.position.z + 1)))
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x + -2, selectedPiece.position.y, selectedPiece.position.z + 2);
            didCaptureOccur = true;
        }
    }

    void blackSimpleMove(Transform square)
    {
        //To move black piece 1 down 1 right
        if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)) && didCaptureOccur == false)
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z - 1);
            whosTurnIsIt *= -1;
            timer.currentTime = timer.startTime;
        }
        //To move black piece 1 down 1 left
        else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)) && didCaptureOccur == false)
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z - 1);
            whosTurnIsIt *= -1;
            timer.currentTime = timer.startTime;
        }
    }

    void blackCaptureMove(Transform square)
    {
        //For black piece to capture 2 down 2 right
        if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 2.150)))
        && checkForCapturablePiece((float)(selectedPiece.position.x + 1), (float)(selectedPiece.position.z - 1)))
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x + +2, selectedPiece.position.y, selectedPiece.position.z - 2);
            didCaptureOccur = true;
        }
        //For black piece to capture 2 down 2 left
        else if ((Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 2.150)))
        && checkForCapturablePiece((float)(selectedPiece.position.x - 1), (float)(selectedPiece.position.z - 1)))
        {
            //Move the piece to this square
            selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 2, selectedPiece.position.y, selectedPiece.position.z - 2);
            didCaptureOccur = true;
        }
    }


    void redCheck(Transform square)
    {
        //Checking for legal single tile move

        redSimpleMove(square);

        //Checking for legal capture move

        redCaptureMove(square);
    }

    void blackCheck(Transform square)
    {
        //Checking for legal single tile move
        blackSimpleMove(square);
        //Checking for legal capture move
        blackCaptureMove(square);
    }

    void kingCheck(Transform square)
    {
        redSimpleMove(square);
        redCaptureMove(square);
        blackSimpleMove(square);
        blackCaptureMove(square);
    }

    void checkForKing()
    {
        if(selectedPieceStartingTag == "redPiece" && selectedPiece.transform.position.z >= 7f)
        {
            Transform pieceToDestroy = selectedPiece;
            GameObject temp = Instantiate(rKing, new Vector3(selectedPiece.transform.position.x, 0.22f, selectedPiece.transform.position.z), Quaternion.identity);
            Destroy(selectedPiece.gameObject);
            selectPiece(temp.transform);
            
        }

        if (selectedPieceStartingTag == "blackPiece" && selectedPiece.transform.position.z <= 0f)
        {
            Transform pieceToDestroy = selectedPiece;
            GameObject temp = Instantiate(bKing, new Vector3(selectedPiece.transform.position.x, 0.22f, selectedPiece.transform.position.z), Quaternion.identity);
            Destroy(selectedPiece.gameObject);
            selectPiece(temp.transform);
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
                RedScoreScript.redCurrScore++; //Increase red score

                //Add the captured piece to sideboard.
                if (hitColliders[0].gameObject.transform.childCount == 1) //Normal piece
                {
                    capturedPieceScript.addBlackPiece();
                }
                else if (hitColliders[0].gameObject.transform.childCount == 2) //King piece
                {
                    capturedPieceScript.addBlackKingPiece();
                }

                Destroy(hitColliders[0].gameObject);

                return true;
            }
            //Black capturing red piece
            else if (hitColliders[0].gameObject.tag == "redPiece" && selectedPieceStartingTag == "blackPiece")
            {
                BlackScoreScript.blackCurrScore++; //Increase black score

                //Add the captured piece to sideboard.
                if (hitColliders[0].gameObject.transform.childCount == 1) //Normal piece
                {
                    capturedPieceScript.addRedPiece();
                }
                else if (hitColliders[0].gameObject.transform.childCount == 2) //King piece
                {
                    capturedPieceScript.addRedKingPiece();
                }

                Destroy(hitColliders[0].gameObject);

                return true;
            }
        }
        return false;
    }

    public void deselectCurrent()
    {
        if(selectedPiece != null)
        {
            selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material = selectedPieceStartingMaterial;
            selectedPiece.tag = selectedPieceStartingTag;
            selectedPieceStartingTag = null;
            selectedPieceStartingMaterial = null;
            selectedPiece = null;
        }
    }

    void checkWinner()
    {
        //Check if player wins (12 points = win)
        if (RedScoreScript.redCurrScore == 12) 
        {
            WinnerTextScript.currWinner = "Red Wins!";
            updateStats(NameMenu.name1Text, NameMenu.name2Text);
        }

        else if (BlackScoreScript.blackCurrScore == 12) 
        {
            WinnerTextScript.currWinner = "Black Wins!";
            updateStats(NameMenu.name2Text, NameMenu.name1Text);

        }
    }

    void updateStats(string name1, string name2)
    {
        string[] readArray = File.ReadAllLines(Application.dataPath + "/save.txt");

        if(isOver == false)
        {
            isOver = true;
            for (int i = 0; i < readArray.Length; i++)
            {
                //convert string to json
                loadedObject = JsonUtility.FromJson<SaveUser>(readArray[i]);

                //check if name user types is already in the save
                if(loadedObject.Name == name1)
                {
                    loadedObject.Wins = loadedObject.Wins + 1;
                    readArray[i] = JsonUtility.ToJson(loadedObject);
                }

                if(loadedObject.Name == name2)
                {
                    loadedObject.Losses = loadedObject.Losses + 1;
                    readArray[i] = JsonUtility.ToJson(loadedObject);
                }
            }

            File.WriteAllLines(Application.dataPath + "/save.txt", readArray);
            File.AppendAllText(Application.dataPath + "/history.txt",  name1+ " (W) Vs. " + name2 + " (L)\n");

        }
        
        WinScreen.SetActive(true);
    }

    public class SaveUser 
    {
        public string Name;
        public int Wins;
        public int Losses;
    }
}

