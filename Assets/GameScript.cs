using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject r; //Red Piece
    public GameObject b; //Black Piece
    private Transform selectedPiece;
    private Material selectedPieceStartingMaterial;

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

    void movePiece()
    {


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objectFound = new RaycastHit();
            bool click = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out objectFound);
            Debug.Log(objectFound.transform.position);
            if (click && objectFound.transform.tag == "piece" && selectedPiece == null)
            {
                selectPiece(objectFound, click);
            }
            else if (click && objectFound.transform.tag == "square" && selectedPiece != null && (objectFound.transform.position.x == selectedPiece.position.x + 0.9 && objectFound.transform.position.z == selectedPiece.position.z + 0.9)) 
            {
                selectSquare(objectFound.transform);
            }
               
                //If the player clicks a free square(ie no piece is there) while a piece is highlighted
                //then move the highlighted piece to the new square and change it's colour to the original colour
        }
    }

    void selectPiece(RaycastHit pieceFound, bool click)
    {
        //click somewhere
        
        Transform movingPiece;
        
        //find out what was clicked on
        movingPiece = pieceFound.transform;
        selectedPiece = movingPiece;


        //highlighting
        Material yourMaterial = Resources.Load("Materials/Highlight", typeof(Material)) as Material;
        selectedPieceStartingMaterial = movingPiece.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material;
        movingPiece.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = yourMaterial;
    }

    void selectSquare(Transform square)
    {
        //Change position of piece to the empty square
        selectedPiece.transform.position = new Vector3(selectedPiece.position.x + 1, selectedPiece.position.y, selectedPiece.position.z + 1);

        //Change piece colour back to its original colour and set selected piece to null
        deselectCurrent();
        
    }

    void deselectCurrent()
    {
        selectedPiece.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = selectedPieceStartingMaterial;
        selectedPiece = null;
    }

    // Update is called once per frame
    void Update()
    {

        movePiece();

    }
}

