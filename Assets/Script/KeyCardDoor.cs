using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static BackpackLocalData;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class KeyCardDoor : MonoBehaviour,IInteractable
{
    [SerializeField] string _prompt;



    [SerializeField] int needItemsID;
    public string InteractionPrompt => _prompt;



    [SerializeField] GameObject _interactPanel;
    [SerializeField] GameObject _allowUI;
    [SerializeField] GameObject _notAllowUI;
    public Interactor _interactor;

    Animator _animator;
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


    Playercontrol1 playercontrol;
    [SerializeField]
    bool _isSelect;
     bool allowInteract = false;
    [SerializeField]float interactTime;

    


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
        _animator=GetComponent<Animator>();
    }
    void Start()
    {
        interactPanel = GameManager.Instance.InteractPanel;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect)
        {
            if (BackpackLocalData.Instance.LoadBackpack().Find(x => x.id.Equals(needItemsID)) != null)
            {
                allowInteract = true;
            }
            else if (allowInteract) { allowInteract = false; }
            ;

            if (allowInteract)
            {
                if (!allowUI.activeSelf) _allowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) isSelect = false;

            }
            else
            {
                if (!notAllowUI.activeSelf) _notAllowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) _isSelect = false;
            }

        };
        if (!isSelect) { if (allowUI.activeSelf) allowUI.SetActive(false); if (notAllowUI.activeSelf) notAllowUI.SetActive(false); }
    }

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

    public bool Interact(Interactor interactor)
    {

        if (allowInteract)
        {


            
            interactPanel.SetActive(true);
            interactPanel.GetComponent<InteractSlider>().StartInteract(interactTime);

            if (!_animator.GetBool("isOPEN"))
            {
                StartCoroutine(WaitForSeconds(interactTime, () =>
                {
                    MissionManager.Instance.CheackMission(_prompt);
                    allowUI.GetComponentInChildren<Text>().text = "關 門";
                    _animator.SetBool("isOPEN", true);
                }));

            }
            else
            {
                StartCoroutine(WaitForSeconds(interactTime, () =>
                {
                    allowUI.GetComponentInChildren<Text>().text = "開 門";
                    _animator.SetBool("isOPEN", false);
                }));

            }


            return true;
        }



        
        else
        {
            return false;
        }
    }
}
