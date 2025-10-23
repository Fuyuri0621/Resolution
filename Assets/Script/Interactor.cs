using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform _interactionPoint;
    [SerializeField] Vector3 _interactionPointRadius = new Vector3 (0.5f,1.5f,0.5f);
    [SerializeField] LayerMask _interactableMask;

                     public readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _numFound = Physics.OverlapBoxNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, transform.rotation, _interactableMask);

        if (_numFound > 0)
        {
            _colliders[0].GetComponent<IInteractable>().interactor = this;
            _colliders[0].GetComponent<IInteractable>().isSelect = true;

            var interactable = _colliders[0].GetComponent<IInteractable>();

            if (interactable != null & Keyboard.current.fKey.wasPressedThisFrame)
            {
                interactable.Interact(this);
            }
        }
        else if(_colliders[0]!=null) _colliders[0] = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_interactionPoint.position, _interactionPointRadius);
    }
}
