using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MatchHistoryMenu : MonoBehaviour
{
    public Text matchHistory;
    // Start is called before the first frame update
    void Start()
    {
        string[] readArray = File.ReadAllLines(Application.dataPath + "/history.txt");


        if(readArray.Length != 0)
        {
            for (int i = 0; i < readArray.Length; i++)
            {
                matchHistory.text = matchHistory.text + "\n" + readArray[i];
            }
        }

        else
        {
            matchHistory.text = "\nMatch History is empty..... Go play some games!";
        }
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
