using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static BackpackLocalData;

public class InteractTP : MonoBehaviour,IInteractable
{
    [SerializeField] private Transform tppos;
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject _allowUI;
    [SerializeField] private GameObject _notAllowUI;
    [SerializeField] private float interactTime;

    GameObject interactPanel;
    Playercontrol1 playercontrol;
    public bool isSelect { get; set; }
    public Interactor interactor { get; set; }

    public string InteractionPrompt => _prompt;
    public GameObject allowUI => _allowUI;
    public GameObject notAllowUI => _notAllowUI;

    public bool Interact(Interactor interactor)
    {
        if (GameManager.Instance.IsControlling)
        {
            playercontrol = GameManager.Instance.playercontrol;
            GameManager.Instance.IsControlling = false;
            playercontrol.GetComponent<Animator>().Play("search");
            interactPanel.SetActive(true);
            interactPanel.GetComponent<InteractSlider>().StartInteract(interactTime);

            StartCoroutine(WaitForSeconds(interactTime, () =>
            {
                interactor.gameObject.transform.position = tppos.position;
                MissionManager.Instance.CheackMission(_prompt);

            }));
            return true;

        }else return false;
    }
    void Start()
    {
        interactPanel =GameManager.Instance.InteractPanel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            StopCoroutine(WaitForSeconds(0));

        }

        if (isSelect)
        {
                if (!allowUI.activeSelf) allowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) isSelect = false;
        }
        if (!isSelect) { if (allowUI.activeSelf) allowUI.SetActive(false); if (notAllowUI.activeSelf) notAllowUI.SetActive(false); }
    }
    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }
}
