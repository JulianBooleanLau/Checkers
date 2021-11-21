using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameMenu : MonoBehaviour
{

    public InputField name1;
    public InputField name2;
    public static string name1Text;
    public static string name2Text;
    public SaveUser loadedObject;
    public int name1Found = 0;
    public int name2Found = 0;

    public void Create() {
        //read in the save file into a string array
        string[] readArray = File.ReadAllLines(Application.dataPath + "/save.txt");

        //if the file is not empty
        if(new FileInfo(Application.dataPath + "/save.txt").Length > 0)
        {
            for (int i = 0; i < readArray.Length; i++)
            {
                //convert string to json
                loadedObject = JsonUtility.FromJson<SaveUser>(readArray[i]);

                //check if name user types is already in the save
                if(loadedObject.Name == name1.text)
                {
                    name1Found = 1;
                    name1Text = loadedObject.Name;
                }

                if(loadedObject.Name == name2.text)
                {
                    name2Found = 1;
                    name2Text = loadedObject.Name;
                }
            }
        }

        //creates user if it isnt found
        if(name1Found != 1)
        {
            CreateNewUser(name1);
            name1Text = name1.text;
        }

        if(name2Found != 1)
        {
            CreateNewUser(name2);
            name2Text = name2.text;
        }
    }

    void CreateNewUser(InputField userName)
    {
        SaveUser user = new SaveUser{
                Name = userName.text,
                Wins = 0,
                Losses = 0,
        };
            
        string json = JsonUtility.ToJson(user);
        File.AppendAllText(Application.dataPath + "/save.txt", json + "\n");
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
