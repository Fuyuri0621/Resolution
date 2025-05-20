using UnityEngine;

public class FixtpTrigger : MonoBehaviour
{
    [SerializeField]Transform tanTongpos;
   [SerializeField] Transform chinLipos;

  [SerializeField]  GameObject[] guards;
   /* private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (GameManager.Instance.controlingCharacter)
            {
                case AllPlayerCharacter.ChinLi:
                    {
                        MissionManager.Instance.SwitchMission(17);
                        FixMachine(other.gameObject);
                        break;
                    }
                case AllPlayerCharacter.TanTong:
                    {
                        MissionManager.Instance.SwitchMission(18);
                        BlocktheGuard(other.gameObject);
                        break;
                    }
            }
        }

        Destroy(gameObject);
    }
   */
   public void FixMachine()
    {
        MissionManager.Instance.SwitchMission(18);
        GameManager.Instance.allowSwitch = false;
        GameObject player = GameManager.Instance.GetPlayerGameObject(AllPlayerCharacter.ChinLi);
        player.transform.position = chinLipos.position;

       // GameManager.Instance.GetPlayerGameObject(AllPlayerCharacter.TanTong).transform.position = tanTongpos.position;
        GameManager.Instance.DisableCharacter(AllPlayerCharacter.TanTong);

    }
    public void BlocktheGuard()
    {
        MissionManager.Instance.SwitchMission(19);
        GameObject player = GameManager.Instance.GetPlayerGameObject(AllPlayerCharacter.TanTong);
        GameManager.Instance.allowSwitch = false;
        player.transform.position = tanTongpos.position;
        player.transform.rotation = tanTongpos.rotation;

       // GameManager.Instance.GetPlayerGameObject(AllPlayerCharacter.ChinLi).transform.position = chinLipos.position;
        GameManager.Instance.DisableCharacter(AllPlayerCharacter.ChinLi);

        for (int i = 0; i < guards.Length; i++)
        {
            guards[i].SetActive(true);
        }
    }
}
