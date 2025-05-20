using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private static DialogueController _instance;
    public static DialogueController Instance
    {
        get { return _instance; }
    }

    [SerializeField] GameObject canvas;
    private Coroutine _typing;


    [SerializeField] private GameObject _continueButton;

    [SerializeField] private GameObject _dialoguePanel;



    [SerializeField] public GameObject select1;

    [SerializeField] public GameObject select2;

    Action actionA;
    Action actionB;



    public GameObject Talker;

    private Queue<Dialogue> _dialogues;

    [SerializeField] private TextMeshProUGUI _dialogueText;

    [SerializeField] private TextMeshProUGUI _nameText;

    private float _wordSpeed = 0.05f;

    private void Awake()
    {
        _instance = this;


    }
    private void Start()
    {
        _dialogueText = _dialoguePanel.transform.Find("Content").GetComponent<TextMeshProUGUI>();
        _nameText = _dialoguePanel.transform.Find("Speaker").GetComponent<TextMeshProUGUI>();
        select1 = _dialoguePanel.transform.Find("Select1").gameObject;
        select2 = _dialoguePanel.transform.Find("Select2").gameObject;
        _continueButton = _dialoguePanel.transform.Find("continue").gameObject;
    }
    private IEnumerator Type()
    {
        Dialogue dialogue = _dialogues.Dequeue();
        _nameText.text = dialogue.Name;


        _dialogueText.text = string.Empty;

        foreach (char letter in dialogue.Sentence.ToCharArray())
        {
            _dialogueText.text += letter;

            yield return new WaitForSecondsRealtime(_wordSpeed);
        }


        if (dialogue.HaveSelect == true)
        {

            select1.SetActive(true);
            select2.SetActive(true);
            select1.GetComponent<TextMeshProUGUI>().text = dialogue.Select1;
            select2.GetComponent<TextMeshProUGUI>().text = dialogue.Select2;
            select1.GetComponent<DialogueButtom>().id = dialogue.Select1Action;
            select2.GetComponent<DialogueButtom>().id = dialogue.Select2Action;
        }
        else
        {
            select1.SetActive(false);
            select2.SetActive(false);
            _continueButton.SetActive(true);
        }


    }

    public void CloseDialogue()
    {
        StopCoroutine(_typing);
        _dialoguePanel.SetActive(false);
        GameManager.Instance.PauseGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SpeakNextSentence()
    {
        _continueButton.SetActive(false);

        if (_dialogues.Count > 0)
        {
            _typing = StartCoroutine(Type());
        }
        else
        {
            CloseDialogue();



        }
    }

    public void Talk(Dialogues dialoguess)
    {

        string[] dialogueText;
        string[] nameText;
        bool[] haveSelect;
        string[] select1;
        string[] select2;
        int[] select1Action;
        int[] select2Action;
        Dialogues d = dialoguess;
        dialogueText = new string[d.DialogueList.Count];
        nameText = new string[d.DialogueList.Count];
        haveSelect = new bool[d.DialogueList.Count];
        select1 = new string[d.DialogueList.Count];
        select2 = new string[d.DialogueList.Count];
        select1Action = new int[d.DialogueList.Count];
        select2Action = new int[d.DialogueList.Count];
        for (int i = 0; i < d.DialogueList.Count; i++)
        {
            dialogueText[i] = d.DialogueList[i].Sentence;
            nameText[i] = d.DialogueList[i].Name;
            haveSelect[i] = d.DialogueList[i].HaveSelect;
            select1[i] = d.DialogueList[i].Select1;
            select2[i] = d.DialogueList[i].Select2;
            select1Action[i] = d.DialogueList[i].Select1Action;
            select2Action[i] = d.DialogueList[i].Select2Action;
        }
        Queue<Dialogue> dialogues = new Queue<Dialogue>();
        for (int i = 0; i < dialogueText.Length; i++)
        {
            dialogues.Enqueue(new Dialogue(nameText[i], dialogueText[i], haveSelect[i], select1[i], select2[i], select1Action[i], select2Action[i]));
        }

        _dialogues = dialogues;
        _dialoguePanel.SetActive(true);


        _typing = StartCoroutine(Type());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.PauseGame();
    }
    public void Talk(Queue<Dialogue> dialogues)
    {
        _dialogues = dialogues;
        _dialoguePanel.SetActive(true);


        _typing = StartCoroutine(Type());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.PauseGame();
    }

    public void ButtomClick(int id)
    {
        if (id == 1)
        {
            select1.SetActive(false);
            select2.SetActive(false);
            Talker.GetComponent<NPC>().ButtomClick(1);
            SpeakNextSentence();
        }
        else if (id == 2)
        {
            select1.SetActive(false);
            select2.SetActive(false);
            Talker.GetComponent<NPC>().ButtomClick(2);
            SpeakNextSentence();
        }
        else if (id == 3) { SceneManager.LoadScene("B3"); }
        else if (id == 4) { SceneManager.LoadScene("B4"); }
        else if (id == 5) { MissionManager.Instance.SwitchMission(20); SpeakNextSentence(); }
        else if (id == 6) { GameManager.Instance.TurnoffLight(); SpeakNextSentence(); }
        else if (id == 7) { GameManager.Instance.TurnonLight(); SpeakNextSentence(); }

    }
}
