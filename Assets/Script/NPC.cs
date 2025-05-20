using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{


    private DialogueController _dialogueController;
    [SerializeField] public Dialogues DialogueContext;
    [SerializeField] string _prompt;

    string[] dialogueText;
    string[] nameText;
    bool[] haveSelect;
    string[] select1;
    string[] select2;
    int[] select1Action;
    int[] select2Action;


    public string InteractionPrompt => _prompt;

    [SerializeField]Transform TPpoint;

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

    [SerializeField] float interactTime;
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
        dialogueText = new string[DialogueContext.DialogueList.Count];
        nameText = new string[DialogueContext.DialogueList.Count];
        haveSelect = new bool[DialogueContext.DialogueList.Count];
        select1 = new string[DialogueContext.DialogueList.Count];
        select2 = new string[DialogueContext.DialogueList.Count];
        select1Action = new int[DialogueContext.DialogueList.Count];
        select2Action = new int[DialogueContext.DialogueList.Count];
    }
    // Start is called before the first frame update
    void Start()
    {
        
        _dialogueController = GameManager.Instance.gameObject
            .GetComponent<DialogueController>();

       for(int i=0 ;i< DialogueContext.DialogueList.Count; i++)
        {
            dialogueText[i]= DialogueContext.DialogueList[i].Sentence;
            nameText[i] = DialogueContext.DialogueList[i].Name;
            haveSelect[i] = DialogueContext.DialogueList[i].HaveSelect;
            select1[i] = DialogueContext.DialogueList[i].Select1; 
            select2[i] = DialogueContext.DialogueList[i].Select2;
            select1Action[i] = DialogueContext.DialogueList[i].Select1Action;
            select2Action[i] = DialogueContext.DialogueList[i].Select2Action;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect)
        {
            

                if (!allowUI.activeSelf) _allowUI.SetActive(true);
                if (interactor._colliders[0] != gameObject.GetComponent<Collider>()) isSelect = false;



        };

        if (!isSelect) { if (allowUI.activeSelf) allowUI.SetActive(false); if (notAllowUI.activeSelf) notAllowUI.SetActive(false); }
    }

    public bool Interact(Interactor interactor)
    {

        DialogueController.Instance.Talker = gameObject;

        if (GameManager.Instance.IsControlling)
        {
            GameManager.Instance.IsControlling = false;

            Queue<Dialogue> dialogues = new Queue<Dialogue>();

            for (int i = 0; i < dialogueText.Length; i++)
            {
                dialogues.Enqueue(new Dialogue(nameText[i], dialogueText[i], haveSelect[i], select1[i], select2[i], select1Action[i], select2Action[i]));
            }

            _dialogueController.Talk(dialogues);

            return true;
        }
        else return false;
    }

    public void ButtomClick(int id)
    {
        if (id == 1)
        {
            MissionManager.Instance.CheackMission(_prompt);
            interactor.gameObject.transform.position = TPpoint.position;

        }
        if (id == 3) { MissionManager.Instance.CheackMission("Elevator"); }
        else
        {


        }
    }

}
