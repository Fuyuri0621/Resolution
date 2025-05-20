using UnityEngine;

public class AniTrigger : MonoBehaviour
{
    [SerializeField] GameObject aniCammara;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            aniCammara.SetActive(true);




            Destroy(gameObject);
        }
    }
}
