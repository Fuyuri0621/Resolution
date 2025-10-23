using TMPro;
using UnityEngine;

public class MissionPanel : MonoBehaviour
{

    TextMeshProUGUI missionName;
    TextMeshProUGUI missionTarget;
    TextMeshProUGUI missionStory;

    private void Awake()
    {
        missionName = transform.Find("Center/DetailPanel/Top/Title").GetComponent<TextMeshProUGUI>();
        missionTarget = transform.Find("Center/DetailPanel/Bottom/Target").GetComponent<TextMeshProUGUI>();
        missionStory = transform.Find("Center/Scroll View/Viewport/Content/Description").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RefreshUI()
    {
        missionName.text = MissionManager.Instance.GetNowMission().MissionName;
        missionTarget.text = MissionManager.Instance.GetNowMission().shortDescription;
        missionStory.text = MissionManager.Instance.GetNowMission().description;
    }

    public void ClosePanel()
    {
        GameManager.Instance.PauseGame();

        Destroy(gameObject);
    }


    public void SwitchPanel(int t)
    {
        switch (t)
        {
            case 1: GameManager.Instance.OpenBackpackPanel(); break;
            case 2: GameManager.Instance.OpenInfoPanel(); break;
            case 3: GameManager.Instance.OpenMissionPanel(); break;
            case 4: GameManager.Instance.OpenMapPanel(); break;
        }

        Destroy(gameObject);

    }
}
