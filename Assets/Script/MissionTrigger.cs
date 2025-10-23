using UnityEngine;
using UnityEngine.Events;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] string prompt;
    public UnityEvent onAccomplish;
    private void OnTriggerEnter(Collider other)
    {
      if(!GameManager.Instance.IsControlling)return;
        if (other.CompareTag("Player"))
        {

            if (MissionManager.Instance.CheackMission(prompt))
            {
                if (onAccomplish != null)
                {
                   onAccomplish.Invoke();
                }
                Destroy(gameObject);
            }
        }
    }
}
