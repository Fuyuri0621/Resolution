using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BackpackLocalData;

public class GMCmd 
{
    [MenuItem("GmCmd/讀取表格")]

    public static void ReadTable()
    {
        BackpackTable backpackTable = Resources.Load<BackpackTable>("Backpack/BackpackTable");
        foreach(BackpackTableItem backpackItem in backpackTable.DataList)
        {
            Debug.Log(string.Format("【id】:{0},【name】:{1}",backpackItem.id,backpackItem.name));
        }
    }

    [MenuItem("GmCmd/創建背包測試數據")]
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
    [MenuItem("GmCmd/讀取背包測試數據")]
    public static void ReadLocalBackpackData()
    {
        List<BackpackLocalItem> readItems = BackpackLocalData.Instance.LoadBackpack();
        GameManager.Instance.backpackLocalItem = BackpackLocalData.Instance.items;
        foreach (BackpackLocalItem item in readItems)
        {
            Debug.Log(item);
        }
    }

    [MenuItem("GmCmd/打開背包")]
    public static void OpenBackpack()
    {
        GameManager.Instance.OpenBackpackPanel();
    }
    [MenuItem("GmCmd/獲得ID卡")]
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

    [MenuItem("GmCmd/重設測試存檔")]
    public static void ResetSavetest()
    {
        Datacontainer saveA = Resources.Load<Datacontainer>("Datacontainer");
        Datacontainer saveB = Resources.Load<Datacontainer>("Datacontainer 1");
        Datacontainer saveC = Resources.Load<Datacontainer>("Datacontainer2");

        saveA.currentScene = ALLSecne.B4;
        saveA.lastCheckpoint = new Vector3(-81, 24, 58);
        saveA.lastmissionid = 8;
        saveA.healAmout = 5;

        saveB.currentScene = ALLSecne.inLobby;
        saveB.lastCheckpoint = new Vector3(-40, 5, 24);
        saveB.lastmissionid = 2;
        saveB.healAmout = 5;

        saveC.currentScene = ALLSecne.outLobby;
        saveC.lastCheckpoint = new Vector3(-40, 5, 24);
        saveC.lastmissionid = 26;
        saveC.healAmout = 5;

    }
    [MenuItem("GmCmd/結束對話")]
    public static void SstopTalk()
    {
        DialogueController.Instance.CloseDialogue();
    }
}
