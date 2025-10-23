using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
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

    [SerializeField] CinemachineCamera talkCam;
    [SerializeField]CutData cutData;
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
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        List<Anifollow> movingactor = new List<Anifollow>();

        Dialogue dialogue = _dialogues.Dequeue();
        _nameText.text = dialogue.Name;


        if (cutData != null)
        {
            if (dialogue.CamLookat != "")
            {
                talkCam.LookAt = (cutData.actors.Find(x => x.GameObjectname == dialogue.CamLookat).transform);
            }
            if (dialogue.Campos != "")
            {
                if (cutData.cams.Count > 1)
                {
                    talkCam.gameObject.transform.position = cutData.cams.Find(x => x.camName == dialogue.Campos).position.position;
                }
                else
                {
                    talkCam.gameObject.transform.position = cutData.cams[0].position.position;
                }

            }
            else
            {
                talkCam.gameObject.transform.position = cutData.cams[0].position.position;
            }

            
            foreach (ActorBehaviour actbehaveiour in dialogue.ActorBehaviourList) 
            {
                Animator actor = cutData.actors.Find(x => x.GameObjectname == actbehaveiour.GameObjectname).transform.GetComponent<Animator>();

                if (actbehaveiour.waypointname != "") 
                {
                    movingactor.Add( actor.GetComponent<Anifollow>()==null ? actor.AddComponent<Anifollow>(): actor.GetComponent<Anifollow>());
                    actor.GetComponent<Anifollow>().enabled = true;
                    actor.GetComponent<Anifollow>().followTarget = cutData.waypoints.Find(x => x.gameObject.name == actbehaveiour.waypointname);
                }

                actor.Play(actbehaveiour.emoteName);

            }
          
        }

        _dialogueText.text = string.Empty;
        Debug.Log(movingactor.Count);
        foreach (char letter in dialogue.Sentence.ToCharArray())
        {
            _dialogueText.text += letter;

            yield return new WaitForSecondsRealtime(_wordSpeed);
        }

        if (movingactor.Count != 0)
        {
            bool allStopped = false;

            while (!allStopped)
            {
                allStopped = true;
                foreach (var actor in movingactor)
                {
                    if (actor.moving)
                    {
                        Debug.Log(actor.name + actor.moving);
                        allStopped = false;
                        break;
                    }
                }
                yield return null;
            }
        } 
        foreach (Anifollow actor in movingactor)
        {
           actor.gameObject.GetComponent<Anifollow>().enabled = false;
        }
        movingactor.Clear();

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
        talkCam.gameObject.SetActive(false);

        foreach (ActorData actor in cutData.actors)
        {
           if(actor.transform.GetComponent<Anifollow>() != null)
            {
                actor.transform.GetComponent<Anifollow>().enabled = false;
            }
        }
        cutData.onAccomplish.Invoke();
        cutData = null;

        if (!GameManager.Instance.IsControlling) { GameManager.Instance.IsControlling = true; }
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
        bool[] havecut;
        string[] camLookat;
        string[] campos;
        List<ActorBehaviour>[] actorbehaveData;
        Dialogues d = dialoguess;

        dialogueText = new string[d.DialogueList.Count];
        nameText = new string[d.DialogueList.Count];
        haveSelect = new bool[d.DialogueList.Count];
        select1 = new string[d.DialogueList.Count];
        select2 = new string[d.DialogueList.Count];
        select1Action = new int[d.DialogueList.Count];
        select2Action = new int[d.DialogueList.Count];
        havecut = new bool[d.DialogueList.Count];
        camLookat = new string[d.DialogueList.Count];
        campos = new string[d.DialogueList.Count];
        actorbehaveData = new List<ActorBehaviour>[d.DialogueList.Count];

        for (int i = 0; i < d.DialogueList.Count; i++)
        {
            dialogueText[i] = d.DialogueList[i].Sentence;
            nameText[i] = d.DialogueList[i].Name;
            haveSelect[i] = d.DialogueList[i].HaveSelect;
            select1[i] = d.DialogueList[i].Select1;
            select2[i] = d.DialogueList[i].Select2;
            select1Action[i] = d.DialogueList[i].Select1Action;
            select2Action[i] = d.DialogueList[i].Select2Action;

            camLookat[i] = d.DialogueList[i].CamLookat;
            campos[i] = d.DialogueList[i].Campos;
            actorbehaveData[i] = d.DialogueList[i].ActorBehaviourList;
        }
        Queue<Dialogue> dialogues = new Queue<Dialogue>();
        for (int i = 0; i < dialogueText.Length; i++)
        {
            dialogues.Enqueue(new Dialogue(nameText[i], dialogueText[i], haveSelect[i], select1[i], select2[i], select1Action[i], select2Action[i], camLookat[i], campos[i], actorbehaveData[i]));
        }



        _dialogues = dialogues;
        _dialoguePanel.SetActive(true);





        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.PauseGame();

        //有過場動畫
        if (d.havecut)
        {
            talkCam.gameObject.SetActive(true);
            cutData = GameObject.Find("TalkData/" + dialoguess.dataname).GetComponent<CutData>();
            foreach (ActorData act in cutData.actors)
            {
                if(act.startTransform != null)
                {
                    if (act.transform.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        act.transform.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    }
                        act.transform.gameObject.transform.SetPositionAndRotation(act.startTransform.transform.position, act.startTransform.localRotation);
;
                }
            }

            if (!GameManager.Instance.IsControlling) { GameManager.Instance.IsControlling = false; }
            Time.timeScale = 1f;
        }


        _typing = StartCoroutine(Type());
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
        else if (id == 3) { GameManager.Instance.datacontainer.currentScene = ALLSecne.B3; GameManager.Instance.datacontainer.lastCheckpoint = new Vector3(35, 0, -29); SceneManager.LoadScene("B3"); }
        else if (id == 4) { GameManager.Instance.datacontainer.currentScene = ALLSecne.B4; GameManager.Instance.datacontainer.lastCheckpoint = new Vector3(-4, 25, -2); SceneManager.LoadScene("B4"); }
        else if (id == 5) { MissionManager.Instance.SwitchMission(20); SpeakNextSentence(); }
        else if (id == 6) { GameManager.Instance.TurnoffLight(); SpeakNextSentence(); }
        else if (id == 7) { GameManager.Instance.TurnonLight(); SpeakNextSentence(); }
        else if (id == 8) { GameManager.Instance.datacontainer.currentScene = ALLSecne.outLobby; GameManager.Instance.datacontainer.lastCheckpoint = new Vector3(-40, 25, 24); SceneManager.LoadScene("outLobby");}
        else if (id == 8) { SceneManager.LoadScene("GameTitle"); }
    }
            
}
