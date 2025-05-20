

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogues/DialogueContext", fileName = "DialogueContext")]

public class Dialogues : ScriptableObject
{
    public List<Dialogue> DialogueList = new List<Dialogue>(); 
}

[Serializable]

public class Dialogue
{
     
    public string Name;
    public string Sentence;
    public bool HaveSelect;
    public string Select1;
    public string Select2;

    public int Select1Action;
    public int Select2Action;



    public Dialogue(string name, string sentence = "", bool haveSelect=false , string select1 = "", string select2 = "", int select1Action = 0, int select2Action = 0)
    {
        Name = name;



        Sentence = sentence;

        HaveSelect = haveSelect;

        Select1= select1;
        Select2= select2;

        Select1Action = select1Action;
        Select2Action = select2Action;

    }


}
