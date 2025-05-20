using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HackPC : MonoBehaviour,IInteractable
{
    [SerializeField] GameObject[] emenys;
    [SerializeField] int needItemsID;
    [SerializeField]bool allowInteract = true;
    [SerializeField] string _prompt;
    public string InteractionPrompt => _prompt;
    GameObject stage;
    [SerializeField] GameObject _interactPanel;
    [SerializeField] GameObject _allowUI;
    [SerializeField] GameObject _notAllowUI;
    [SerializeField] GameObject HackStage;
    public GameObject interactPanel { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public GameObject allowUI => _allowUI;

    public GameObject notAllowUI => _notAllowUI;

    bool _isSelect;
    public bool isSelect
    {
        get{return _isSelect;}
        set { _isSelect = value; }
    }
  
    public Interactor _interactor;
    public Interactor interactor
    {
        get { return _interactor; }
        set { _interactor = value; }
    }
    public bool Interact(Interactor interactor)
    {
        if (allowInteract)
        {
            NoticeManager.Instance.SendMessage("使用WASD移動來吃到黑點", 3f);
            allowInteract = false;
            allowUI.SetActive(false);
            GameManager.Instance.SetIsControlling(false);
            Debug.Log("hacking");
            stage = Instantiate(HackStage);
            stage.transform.position = transform.position - new Vector3(0,200,0);
            stage.transform.rotation = UnityEngine.Quaternion.identity;
           // stage.transform.parent = transform;
            stage.GetComponent<StageEventManager>().PC = this;
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect)
        {
            if (needItemsID != 0)
            {
                if (BackpackLocalData.Instance.LoadBackpack().Find(x => x.id.Equals(needItemsID)) != null)
                {
                    allowInteract = true;
                }
                else if (allowInteract) { allowInteract = false; }
            }

            if (allowInteract)
            {
                if (!allowUI.activeSelf) allowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) isSelect = false;

            }
            else
            {
                if (!notAllowUI.activeSelf) _notAllowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) _isSelect = false;
            }
        }
        if (!isSelect) { if (allowUI.activeSelf) allowUI.SetActive(false); if (notAllowUI.activeSelf) notAllowUI.SetActive(false); }
    }

    public void HackComplete()
    {
        Debug.Log("hackcomplete");
        Destroy(stage);
        MissionManager.Instance.CheackMission(_prompt);


        foreach (GameObject g in emenys)
        {
            g.SetActive(true);
        }
    }
}
