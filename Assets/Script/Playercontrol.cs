using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Playercontrol : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    private Vector2 MoveVector;
    public float MoveSpeed = 5;
    float PushCd = 0;
    float FireballCd = 0;
    public int kill = 0;
    public GameObject Crosshair;
    public GameObject face;
    public GameObject fireball;
    public GameObject AimPoint;
    public GameObject menu;
   // public GameObject deadmenu;
   // public GameObject winmenu;
    //public Slider FireballUI;
   // public Slider PushUI;
 //   public Text lifeUI;
  //  public Text killUI;
    bool IsAiming = false;
  //  bool Ispause = false;
    public bool IsGameRunning = true;
    int Life = 3;
    float invincible = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameRunning)
        {
            if (invincible > 0)
                invincible -= Time.deltaTime;
            if (invincible < 0)
                invincible = 0;

          //  lifeUI.text = "Life:" + Life.ToString();
           // killUI.text = "Kill:" + kill.ToString();
            //­pºâCD
            if (FireballCd > 0)
                FireballCd -= Time.deltaTime;
            if (PushCd > 0)
                PushCd -= Time.deltaTime;
            if (FireballCd < 0)
                FireballCd = 0;
            if (PushCd < 0)
                PushCd = 0;
         //   FireballUI.value = FireballCd;
         //   PushUI.value = PushCd;

            if (MoveVector != new Vector2(0, 0))
            {
                if (!IsAiming)
                    transform.rotation = Quaternion.Euler(0, face.transform.eulerAngles.y, 0);

                transform.Translate(new Vector3(MoveVector.x, 0, MoveVector.y) * Time.deltaTime * MoveSpeed);
            }
            if (IsAiming)
            {
                Crosshair.transform.rotation = Quaternion.Euler(face.transform.eulerAngles.x, face.transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Euler(0, face.transform.eulerAngles.y, 0);
            }

            if (kill == 20)
            {

           //     winmenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                IsGameRunning = false;
            }

            if (Life == 0)
            {


             //   deadmenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                transform.localScale = Vector3.zero;
                IsGameRunning = false;
            }
            Move();
            Jump();
            Attack();
            Aim();
        }


    }
    public void Move()
    {

        MoveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        print(MoveVector);
        

    }
    public void Jump()
    {
        if (IsGameRunning)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector3.up * 50, ForceMode.Force);
        }
    }
    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsAiming)
                if (FireballCd == 0)
                {
                    Instantiate(fireball, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.Euler(face.transform.eulerAngles.x, face.transform.eulerAngles.y, 0));
                    FireballCd = 1;
                }
           
        }
    }
    public void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsAiming = true;
            Crosshair.SetActive(true);

        }
        if (Input.GetMouseButtonUp(1))
        {
            IsAiming = false;
            Crosshair.SetActive(false);

        }
    }

   /* public void Callmenu(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!Ispause)
            {
                menu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Ispause=true;
                IsGameRunning = false;
            }
            else
            {
                menu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Ispause = false;
                IsGameRunning =true;
            }

        }
    }
   */

    public void OnCollisionEnter(Collision other) 
    {
        
        if (other.gameObject.tag == "enemy"&&invincible == 0)
            {
                Life -= 1;
            invincible = 2;


            }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy" && invincible == 0)
        {
            Life -= 1;
            invincible = 2;
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackTitle()
    {
        SceneManager.LoadScene(0);
    }
}