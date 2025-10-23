using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackLocalData
{
    private static BackpackLocalData _instance;

    public static BackpackLocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackpackLocalData();
            }
            return _instance;
        }
    }

    public List<BackpackLocalItem> items;

    
    public void SaveBackpack()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("BackpackLocalData", inventoryJson);
        PlayerPrefs.Save();
    }

    public List<BackpackLocalItem> LoadBackpack()
    {
        if (items != null)
        {
            return items;
        }
        if (PlayerPrefs.HasKey("BackpackLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("BackpackLocalData");
            BackpackLocalData backpackLocalData = JsonUtility.FromJson<BackpackLocalData>(inventoryJson);
            items = backpackLocalData.items;
            return items;
        }
        else
        {
            items = new List<BackpackLocalItem>();
            return items;
        }
    }

    [Serializable]
    public class BackpackLocalItem
    {
        public string uid;

        public int id;

        public string name;

        public bool isNew;


        public override string ToString()
        {
            return string.Format("id:{0}", id);
        }
    }
}
