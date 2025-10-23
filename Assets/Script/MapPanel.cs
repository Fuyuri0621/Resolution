using UnityEngine;

public class MapPanel : MonoBehaviour
{
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
