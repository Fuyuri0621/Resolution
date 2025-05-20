using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static BackpackLocalData;


public enum AllPlayerCharacter
{
    ChinLi,
    TanTong,


}
public class GameManager : MonoBehaviour
{
    public Datacontainer datacontainer;
    public Light lights;
    public AllPlayerCharacter controlingCharacter;
    bool _IsGameRunning;
    bool _IsControlling;
    public bool IsControlling;

    public bool IsGameRunning;

    public bool isBattling =false;

    public bool allowSwitch = true;
    public bool Ispause = false;

    public float attackTimer = 0;

    [SerializeField] Playercontrol1 ChinLicontrol;
    [SerializeField] Playercontrol1 TanTongcontrol;
    [SerializeField] public Playercontrol1 playercontrol
    {
        get 
        {
            if (controlingCharacter == AllPlayerCharacter.ChinLi)
            {
                return ChinLicontrol;
            }
            else
            {
                return TanTongcontrol;
            }
        } 
    }

    [SerializeField] public List<BackpackLocalItem> backpackLocalItem;
    [SerializeField]GameObject backpackpanel;
    [SerializeField] public GameObject InteractPanel;
    [SerializeField] GameObject canvas;
    GameObject failPanel;
    [SerializeField] BackpackTable backpackTable;
    
    private static GameManager _instance;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        _instance = this;
        failPanel = canvas.transform.Find("FailPanel").gameObject;
     //   datacontainer = Resources.Load<Datacontainer>("Datacontainer");

     //   MissionManager.Instance.SwitchMission(datacontainer.lastmissionid);
    }
    
    public static GameManager Instance
    {
        get { return _instance; }
    }
    void Start()
    {
        IsControlling = true;
        IsGameRunning = true;

      
     //   TanTongcontrol.gameObject.transform.position = datacontainer.lastCheckpoint;
     //   ChinLicontrol.gameObject.transform.position = datacontainer.lastCheckpoint +Vector3.back*2;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTimer != 0) { attackTimer = Math.Max(0, attackTimer-Time.deltaTime); }
    }

    public void OpenBackpackPanel()
    {

            Instantiate(backpackpanel, canvas.transform);
        
    }

    public BackpackTable GetBackpackTable()
    {
        if (backpackTable == null)
        {
            backpackTable = Resources.Load<BackpackTable>("Backpack/BackpackTable");
        }

        return backpackTable;
    }

    public List<BackpackLocalItem> GetBackpackLocalData()
    {
        return BackpackLocalData.Instance.LoadBackpack();
    }

    public List<BackpackLocalItem> GetSortBackPackLocalData()
    {
        List<BackpackLocalItem> localItems = BackpackLocalData.Instance.LoadBackpack();
        localItems.Sort(new BackpackItemComprarer());
        return localItems;
    }
    public BackpackLocalItem GetBackpackLocalItemById(int id)
    {

        List<BackpackLocalItem> backpackLocalItemList = BackpackLocalData.Instance.items;
        foreach (BackpackLocalItem item in backpackLocalItemList)
        {
            if (item.id == id)
            {
                return item;
            }
        }

        return null;
    }
    public BackpackTableItem GetBackpackItemById(int id)
    {
        
        List<BackpackTableItem> backpackDataList = GetBackpackTable().DataList;
        foreach (BackpackTableItem item in backpackDataList)
        {
            if (item.id == id)
            {
                return item;
            }
        }

        return null;
    }

    public BackpackLocalItem GetBackpackItemByUid(string uid)
    {
        List<BackpackLocalItem> backpackDataList = GetBackpackLocalData();
        foreach (BackpackLocalItem item in backpackDataList)
        {
            if (item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }

    public class BackpackItemComprarer : IComparer<BackpackLocalItem>
    {
        public int Compare(BackpackLocalItem a, BackpackLocalItem b)
        {
            BackpackTableItem x = GameManager.Instance.GetBackpackItemById(a.id);
            BackpackTableItem y = GameManager.Instance.GetBackpackItemById(b.id);

            int idcomparison = x.id.CompareTo(y.id);
            return idcomparison;
        }
    }

    public void OnSwitch(InputValue value)
    {
        if (allowSwitch)
        {
            switch (controlingCharacter)
            {
                case AllPlayerCharacter.ChinLi:
                    controlingCharacter = AllPlayerCharacter.TanTong;
                    TanTongcontrol.Switch();
                    ChinLicontrol.Switch();
                    break;
                case AllPlayerCharacter.TanTong:
                    controlingCharacter = AllPlayerCharacter.ChinLi;
                    TanTongcontrol.Switch();
                    ChinLicontrol.Switch();
                    break;
            }
        }
    }

    public void OnOpenBackpack(InputValue value)
    {
        if(IsGameRunning&&IsControlling) 
        {
            OpenBackpackPanel();
            PauseGame();
        }
        
    }
    public void PauseGame()
    {

        Debug.Log("PauseGame");
        if (!Ispause)
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Ispause = true;
            IsGameRunning = false;
            IsControlling = false;

            Time.timeScale = 0f;
        }
        else
        {
           Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Ispause = false;
            IsGameRunning = true;
            IsControlling = true;

        }
    }

    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }

   public List <GameObject> battlingEmemies = new List <GameObject>();


    public void AddtBattleEmemies(GameObject ememy)
    {
        battlingEmemies.Add(ememy);
        CheckBattle();
    }

    public void RemovetBattleEmemies(GameObject ememy)
    {
        battlingEmemies.Remove(ememy);
        CheckBattle();
    }
    public void CheckBattle()
    {
        if(battlingEmemies.Count > 0&&!isBattling)
        {
            StartBattle();
        }
        else if (battlingEmemies.Count == 0&&isBattling)
        {
            EndBattle();
        }
    }
    public void StartBattle()
    {
        isBattling = true;
        if (ChinLicontrol.gameObject.activeInHierarchy)
        {
            ChinLicontrol.StartBattle();
        }
        if (TanTongcontrol.gameObject.activeInHierarchy)
        {
            TanTongcontrol.StartBattle();
        }
    }
    public void EndBattle()
    {
        isBattling = false;
        if (ChinLicontrol.gameObject.activeInHierarchy)
            ChinLicontrol.EndBattle();
        if (TanTongcontrol.gameObject.activeInHierarchy)
            TanTongcontrol.EndBattle();

        MissionManager.Instance.CheackMission();
    }
    public void SetIsControlling(bool isControlling)
    {
        IsControlling = isControlling;
    }
    public bool RequestAttack()
    {
        if (attackTimer == 0)
        {
            attackTimer = 2f;
            return true;
        }
        else
        { return false; }
    }

    public GameObject GetCtrolingCharacter()
    {
        if (controlingCharacter == AllPlayerCharacter.ChinLi)
        {
            return ChinLicontrol.gameObject;
        }
        else
        {
            return TanTongcontrol.gameObject;
        }
    }
    public bool IsthisCtrolingCharacter(AllPlayerCharacter c)
    {
        if (c == controlingCharacter)
            return true;
        else
            return false;
    }

    public GameObject GetPlayerGameObject(AllPlayerCharacter c)
    {
        if(c == AllPlayerCharacter.ChinLi)
        {
            return ChinLicontrol.gameObject;
        }
        else
        {
            return TanTongcontrol.gameObject;
        }
    }

    public void GameOver()
    {
        failPanel.SetActive(true);
        PauseGame();
    }
    public void PlayingAni()
    {
        IsControlling=false;
        IsGameRunning=false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StopAni()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsControlling =true;
        IsGameRunning=true;
    }

    public void DisableCharacter(AllPlayerCharacter c)
    {
        if (c == AllPlayerCharacter.ChinLi)
        {
            ChinLicontrol.gameObject.SetActive(false);
        }
        else
        {
            TanTongcontrol.gameObject.SetActive(false);
        }
    }
    public void ActiveChinLi(Transform t) { ActiveCharacter(AllPlayerCharacter.ChinLi, t); }
    public void ActiveCharacter(AllPlayerCharacter c,Transform t)
    {
        if (c == AllPlayerCharacter.ChinLi)
        {
            ChinLicontrol.gameObject.SetActive(true);
            ChinLicontrol.gameObject.transform.position=t.position;
        }
        else
        {
            TanTongcontrol.gameObject.SetActive(true);
            TanTongcontrol.gameObject.transform.position = t.position;
        }
    }
     
    public void UpdateCheckPoint()
    {
        datacontainer.lastCheckpoint = GetPlayerGameObject(controlingCharacter).transform.position;
        datacontainer.lastmissionid = MissionManager.Instance.Getnowmissionid();
    }
    
    public void TurnoffLight()
    {
        lights.color = Color.red;
        lights.intensity = 0.6f;
    }
    public void TurnonLight()
    {
        lights.color = Color.white;
        lights.intensity = 1f;
    }
}
