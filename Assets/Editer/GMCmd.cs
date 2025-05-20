using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BackpackLocalData;

public class GMCmd 
{
    [MenuItem("GmCmd/Ū�����")]

    public static void ReadTable()
    {
        BackpackTable backpackTable = Resources.Load<BackpackTable>("Backpack/BackpackTable");
        foreach(BackpackTableItem backpackItem in backpackTable.DataList)
        {
            Debug.Log(string.Format("�iid�j:{0},�iname�j:{1}",backpackItem.id,backpackItem.name));
        }
    }

    [MenuItem("GmCmd/�ЫحI�]���ռƾ�")]
    public static void CreateLocalBackpackData()
    {
        
        BackpackLocalData.Instance.items = new List<BackpackLocalItem>();
        for(int  i = 1; i < 2; i++)
        {
            BackpackLocalItem backpackLocalItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = i,
                
                isNew = true
            };
            BackpackLocalData.Instance.items.Add(backpackLocalItem);
        }
        BackpackLocalData.Instance.SaveBackpack();
    }
    [MenuItem("GmCmd/Ū���I�]���ռƾ�")]
    public static void ReadLocalBackpackData()
    {
        List<BackpackLocalItem> readItems = BackpackLocalData.Instance.LoadBackpack();
        GameManager.Instance.backpackLocalItem = BackpackLocalData.Instance.items;
        foreach (BackpackLocalItem item in readItems)
        {
            Debug.Log(item);
        }
    }

    [MenuItem("GmCmd/���}�I�]")]
    public static void OpenBackpack()
    {
        GameManager.Instance.OpenBackpackPanel();
    }
    [MenuItem("GmCmd/��oID�d")]
    public static void GetIDCard()
    {
        BackpackLocalItem backpackLocalItem = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = 5,
            isNew = true
        };
        BackpackLocalData.Instance.items.Add (backpackLocalItem);
        BackpackLocalData.Instance.SaveBackpack ();

    }


}
