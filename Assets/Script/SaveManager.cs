using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public Datacontainer[] saveSlots;
    public GameObject soltA;
    public GameObject soltB;
    public GameObject soltC;

    private void Awake()
    {
        Debug.Log(MissionManager.Instance);

    }

    private void Start()
    {
        Refreshslot(0);
        Refreshslot(1);
        Refreshslot(2);
    }
    void Refreshslot(int solt)
    {
        Debug.Log("refresh"+solt);
        if (solt == 0)
        {
            soltA.transform.Find("location").GetComponent<TextMeshProUGUI>().text = saveSlots[solt].currentScene.ToString();
            soltA.transform.Find("mission").GetComponent<TextMeshProUGUI>().text = MissionManager.Instance.GetMissionName(saveSlots[solt].lastmissionid);
            soltA.transform.Find("time").GetComponent<TextMeshProUGUI>().text = saveSlots[solt].savedate;
        }
        if (solt == 1)
        {
            soltB.transform.Find("location").GetComponent<TextMeshProUGUI>().text = saveSlots[solt].currentScene.ToString();
            soltB.transform.Find("mission").GetComponent<TextMeshProUGUI>().text = MissionManager.Instance.GetMissionName(saveSlots[solt].lastmissionid);
            soltB.transform.Find("time").GetComponent<TextMeshProUGUI>().text = saveSlots[solt].savedate;
        }
        else if (solt == 2)
        {
            soltC.transform.Find("location").GetComponent<TextMeshProUGUI>().text = saveSlots[solt].currentScene.ToString();
            soltC.transform.Find("mission").GetComponent<TextMeshProUGUI>().text = MissionManager.Instance.GetMissionName(saveSlots[solt].lastmissionid);
            soltC.transform.Find("time").GetComponent<TextMeshProUGUI>().text = saveSlots[solt].savedate;
        }
    }
    public void LoadSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlots.Length)
        {
            Debug.LogWarning("無效存檔欄位索引");
            return;
        }

        // 暫存資料傳遞，這裡也可以用靜態欄位或 ScriptableObject 方式
        TempSave.selectedData = saveSlots[slotIndex];

        // 載入遊戲場景
        SceneManager.LoadScene(TempSave.selectedData.currentScene.ToString()); // 替換為你的場景名稱
    }

    public void SaveSlot(int slotIndex)
    {
        Debug.Log("save" + slotIndex);
        if (slotIndex < 0 || slotIndex >= saveSlots.Length)
        {
            Debug.LogWarning("無效存檔欄位索引");
            return;
        }

        // 暫存資料傳遞，這裡也可以用靜態欄位或 ScriptableObject 方式
        TempSave.selectedData = saveSlots[slotIndex];

        GameManager.Instance.Savesavedata();
        saveSlots[slotIndex] = TempSave.selectedData;
        Refreshslot(slotIndex);
    }
}

public static class TempSave
{
    public static Datacontainer selectedData;
}
