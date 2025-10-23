using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface IInteractable
{
    string InteractionPrompt { get; }

  
     GameObject allowUI { get; }
     GameObject notAllowUI { get; }

     bool isSelect { get; set; }

    Interactor interactor { get; set; }
    public bool Interact(Interactor interactor );
}
