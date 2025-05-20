using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static BackpackLocalData;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour,IInteractable
{
    [SerializeField] string _prompt;
    [SerializeField] Animator dooranimator;

    [SerializeField] int itemsID;
    [SerializeField] bool haveItem = true;
    public string InteractionPrompt => _prompt;

    [SerializeField] GameObject _interactPanel;
    [SerializeField] GameObject _allowUI;

    [SerializeField] GameObject _notAllowUI;
    Playercontrol1 playercontrol;
    [SerializeField]
    bool _isSelect;
    [SerializeField]float interactTime;

    public Interactor _interactor;


    public Interactor interactor
    {
        get { return _interactor; }
        set { _interactor = value; }
    }

    public GameObject interactPanel
    {
        get { return _interactPanel; }
        set { _interactPanel = value; }
    }

    public GameObject allowUI => _allowUI;

    public GameObject notAllowUI => _notAllowUI;


    public bool isSelect
    {
        get
        {
            return _isSelect;
        }
        set
        {
            _isSelect = value;


        }
    }

    private void Awake()
    {
        
    }

    void Start()
    {
        interactPanel = GameManager.Instance.InteractPanel;
        dooranimator.Play("CabinetDoorClose");
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

            if (haveItem)
            {
                if (!allowUI.activeSelf) allowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) isSelect = false;

            }
            else if (GameManager.Instance.IsControlling)
            {
                if (allowUI.activeSelf) allowUI.SetActive(false);
                if (!notAllowUI.activeSelf) notAllowUI.SetActive(true);
                    if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) isSelect = false;

            }

        }
        if (!isSelect) { if (allowUI.activeSelf) allowUI.SetActive(false); if (notAllowUI.activeSelf) notAllowUI.SetActive(false); }
    }
    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }


    public bool Interact(Interactor interactor)
    {
        if (GameManager.Instance.IsControlling&&haveItem)
        {
            playercontrol = GameManager.Instance.playercontrol;
            GameManager.Instance.IsControlling = false;
            playercontrol.GetComponent<Animator>().Play("search");
            dooranimator.Play("CabinetDoorOpen");
            interactPanel.SetActive(true);
            interactPanel.GetComponent<InteractSlider>().StartInteract(interactTime);



            NoticeManager.Instance.SendDialogue("沁梨", "讓我看看裡面有什麼", 2, 0);
           
            StartCoroutine(WaitForSeconds(interactTime, () =>
            {
                NoticeManager.Instance.SendNotice(GameManager.Instance.GetBackpackItemById(itemsID).name, 2f,0);
                dooranimator.Play("CabinetDoorClose");

                BackpackLocalItem backpackLocalItem = new()
                {
                    uid = Guid.NewGuid().ToString(),
                    id = itemsID,
                    isNew = true
                };

                BackpackLocalData.Instance.items.Add(backpackLocalItem);
                BackpackLocalData.Instance.SaveBackpack();
                MissionManager.Instance.CheackMission();
                haveItem = false;
            }));
           
         //   NoticeManager.Instance.SendNotice(GameManager.Instance.GetBackpackItemById(itemsID).name, 2f, interactTime);


            return true;
        } else { return false; }
    }
}
