using Unity.Cinemachine;
using UnityEngine;

public class AniCam : MonoBehaviour
{
    CinemachineCamera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        GameManager.Instance.PlayingAni();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CamAniStart()
    {
        GameManager.Instance.PlayingAni();
    }
    public void CamAniEnd()
    {

        Debug.Log("AniEnd");
        GameManager.Instance.StopAni();

    }

    public void DestroyCam()
    {
        Destroy(gameObject);
    }

}
