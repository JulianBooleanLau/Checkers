using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    //Variables initalized using the drag and drop on inspector
    public GameObject r; //Red piece prefab
    public GameObject b; //Black piece prefab
    private Transform selectedPiece; //Current selected piece
 

    //Variables used for piece highlighting
    public Material highlightMaterial; //Material used when piece is highlighted
    private Material selectedPieceStartingMaterial;
    private string selectedPieceStartingTag;   
    

    GameObject[,] pieceArray = new GameObject[8, 8];

    // Start is called before the first frame update
    void Start()
    {
        selectedPiece = null;
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
                //Debug.Log("Pos X" + pos.x.ToString("f5"));
                //Debug.Log("pos Y" + pos.y.ToString("f5"));
                //Debug.Log("pos Z" + pos.z.ToString("f5"));
                //Debug.Log(objectFound.transform.position);

                if ((objectFound.transform.tag == "redPiece" || objectFound.transform.tag == "blackPiece") && selectedPiece == null)
                {
                    selectPiece(objectFound, click);
                }
                else if (objectFound.transform.tag == "selectedPiece" && selectedPiece != null)
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
    void selectPiece(RaycastHit pieceFound, bool click)
    {   
        //Find out what was clicked on
        selectedPiece = pieceFound.transform;


        //Highlighting
        //Material highlightMaterial = Resources.Load("Assets/Materials/Highlight", typeof(Material)) as Material;
        selectedPieceStartingMaterial = selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material;
        selectedPieceStartingTag = selectedPiece.tag;
        
        selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material = highlightMaterial;
        selectedPiece.tag = "selectedPiece";
    }

    void selectSquare(Transform square)
    {
        
        //Change position of piece to the empty square
        if (selectedPieceStartingTag == "redPiece")
        {
            //Checking for legal single tile move

            //To move red piece 1 up 1 right
            if ( Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900) ) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850) ) )
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z + 1);
            }
            //To move red piece 1 up 1 left
            else if( Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x -1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 0.850) ) )
            {
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z + 1);
            }

            //Checking for legal capture move

            //For red piece to capture 2 up 2 right
            if ( (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 1.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850)) ) && checkForCapturablePiece( (float)(selectedPiece.position.x + 0.900), (float)(selectedPiece.position.z + 0.850) ) )
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 2, selectedPiece.position.y, selectedPiece.position.z + 2);
            } //For red piece to capture 2 up 2 left
            else if ( (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 2.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z + 1.850)) ) && checkForCapturablePiece((float)(selectedPiece.position.x - 1.100), (float)(selectedPiece.position.z + 0.850)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + - 2, selectedPiece.position.y, selectedPiece.position.z + 2);
            }



        }

        //Change position of piece to the empty square
        if (selectedPieceStartingTag == "blackPiece")
        {
            //Checking for legal single tile move
            if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x - 1.100)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)))
            {
                //Move the piece to this square
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x - 1, selectedPiece.position.y, selectedPiece.position.z - 1);
            }
            else if (Mathf.Approximately(square.transform.position.x, (float)(selectedPiece.position.x + 0.900)) && Mathf.Approximately(square.transform.position.z, (float)(selectedPiece.position.z - 1.150)))
            {
                selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z - 1);
            }

        }

        //Change piece colour back to its original colour and set selected piece to null
        deselectCurrent();
        
    }

    bool checkForCapturablePiece(float x, float z) {
        
        Vector3 spawnPos = new Vector3(x, 0.500f, z);
        
        Collider[] hitColliders;
        
        float radius = (0.01f);
        
        //Checks if a object is inbetween the selected piece's original spot and the capture spot
        if (Physics.CheckSphere(spawnPos, radius)) 
        {

            hitColliders = Physics.OverlapSphere(spawnPos, radius);
            
            //Case for Red capturing a black piece
            if( (hitColliders[0].gameObject.tag == "blackPiece" && selectedPieceStartingTag == "redPiece" ) || (hitColliders[0].gameObject.tag == "redPiece" && selectedPieceStartingTag == "blackPiece"))
            {
                Destroy(hitColliders[0].gameObject);
                return true;
            } 

        }
        return false;
    }

void deselectCurrent()
    {
        selectedPiece.GetChild(0).gameObject.GetComponent<Renderer>().material = selectedPieceStartingMaterial;
        selectedPiece.tag = selectedPieceStartingTag;
        selectedPieceStartingTag = null;
        selectedPieceStartingMaterial = null;
        selectedPiece = null;
    }

}

