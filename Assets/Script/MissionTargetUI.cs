using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionTargetUI : MonoBehaviour
{
    [SerializeField] public int waypointID;

    [SerializeField] Vector3 offset = new Vector3(0.5f, 1.5f, 0.5f);
    [SerializeField] Vector3 _Radius = new Vector3(0.5f, 1.5f, 0.5f);
    [SerializeField] LayerMask _playerMask;

    public readonly Collider[] _colliders = new Collider[2];
    [SerializeField] private int _numFound;

    Image icon;
    // Start is called before the first frame update

    private void Awake()
    {
        icon = GetComponentInChildren<Image>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _numFound = Physics.OverlapBoxNonAlloc(transform.position + offset, _Radius, _colliders, transform.rotation, _playerMask);
        if (_numFound > 0)
        {
            icon.gameObject.SetActive(false);
        }else if (!icon.gameObject.activeInHierarchy)
        {
            icon.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(transform.position + offset, _Radius*2);
    }
}
