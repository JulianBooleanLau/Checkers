using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerStatsMenu : MonoBehaviour
{
    public Text Names;
    public Text Wins;
    public Text Losses;
    public SaveUser loadedObject;

    // Start is called before the first frame update
    void Start()
    {

        string[] readArray = File.ReadAllLines(Application.dataPath + "/save.txt");


        if(readArray.Length != 0)
        {
            for (int i = 0; i < readArray.Length; i++)
            {
                loadedObject = JsonUtility.FromJson<SaveUser>(readArray[i]);
                Names.text = Names.text + "\n" + loadedObject.Name;
                Wins.text = Wins.text + "\n" + loadedObject.Wins;
                Losses.text = Losses.text + "\n" + loadedObject.Losses;
            }
        }

        else
        {
            Names.text = "\nNo players found..... Go play some games!";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class SaveUser
    {
        public string Name;
        public int Wins;
        public int Losses;
    }
}
