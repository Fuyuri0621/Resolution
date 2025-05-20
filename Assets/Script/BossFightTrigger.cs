using UnityEngine;
using UnityEngine.UI;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] BossControl boss;
    [SerializeField] GameObject bossinfoPanel;



    void SetBossInfo()
    {
        bossinfoPanel.SetActive(true);
        Slider hpsld = bossinfoPanel.transform.Find("HPSlider").GetComponent<Slider>();
        hpsld.maxValue=boss.stats.hp;
        hpsld.value=boss.stats.hp;
        
    }
    
    public void StartBossFight()
    {
        SetBossInfo();
        boss.SetTarget(GameManager.Instance.GetCtrolingCharacter());
        boss.BossinfoPanel= bossinfoPanel;
    }
}
