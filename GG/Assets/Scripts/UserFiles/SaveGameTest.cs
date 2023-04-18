using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameTest : MonoBehaviour
{
    public InputField nameInput;
    SaveGame save = new SaveGame("TestSave");

    public void Save()
    {
        save.Name = nameInput.text;
        save.Save();
    }

    public void Load()
    {
        save.Load();
        nameInput.text = save.Name;
    }
}
