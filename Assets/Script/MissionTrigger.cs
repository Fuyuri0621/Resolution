using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] string prompt;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (MissionManager.Instance.CheackMission(prompt))
            {
                Destroy(gameObject);
            }
        }
    }
}
