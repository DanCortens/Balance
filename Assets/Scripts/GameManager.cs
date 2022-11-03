using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine;


// This class handles saving and loading.
public class GameManager
{
    public void Awake()
    {

    }

    public void CreateSave(Transform _savePoint, string _scene, string _name)
    {
        //TO DO: save player's balance, upgrades, and the state of the game world
        Save save = new Save();
        save.name = _name;
        save.scene = _scene;
        save.savePoint = _savePoint;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream savefile = File.Create(Application.persistentDataPath + "/saves/" + save.name + ".sav");
        bf.Serialize(savefile, save);
        savefile.Close();
    }

    public void LoadGame(string _saveName)
    {
        //save exists?
        if (File.Exists(Application.persistentDataPath + "/saves/" + _saveName + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/" + _saveName + ".sav", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            SceneManager.LoadScene(save.scene);
            //load player position, stats, and world stuff
        }
        else
        {
            Debug.Log("Error: Save does not exist!");
        }

    }
}
