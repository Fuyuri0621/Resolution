using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Healcontrol : MonoBehaviour
{
    Animator animator;
    [SerializeField] InputActionAsset actions;
    InputAction healaction;


   [SerializeField] TextMeshProUGUI amoutText;
    Playercontrol1 playercontrol1;
    bool lastmotioniswalk;
    AllPlayerCharacter thisCharacter;

    StatusBar statusBar;

    private void Awake()
    {
        statusBar = GameObject.Find("PlayerState").GetComponent<StatusBar>();
        amoutText = GameObject.Find("HealAmout").GetComponent<TextMeshProUGUI>();
        playercontrol1 = GetComponent<Playercontrol1>();
        animator = GetComponent<Animator>();
        actions = GetComponent<PlayerInput>().actions;
        healaction = actions.FindAction("Stander/Heal");
    }
    private void Start()
    {
        thisCharacter=playercontrol1.thisCharacter;
        UptateAmouttext();
    }
    public void OnHeal(InputValue value)
    {
        if (GameManager.Instance.controlingCharacter != thisCharacter) { return; }

         lastmotioniswalk = playercontrol1.locomotionState==LocomotionState.Walk;
        if (!lastmotioniswalk) { playercontrol1.OnRun(new InputValue()); }

        if (GameManager.Instance.healAmout > 0)
        {
            animator.Play("Heal");

        }
        else
        {
            animator.Play("cantHeal");
        }
        
    }
    public void BighealCheck()
    {
        
        if (healaction != null)
        {
            float holding = healaction.ReadValue<float>();

            if (holding > 0) { animator.Play("keepHeal"); }
        }
    }
    
    public void ReturnMotion()
    {
        if (!lastmotioniswalk)
        {
            playercontrol1.OnRun(new InputValue());
        }
    }

    void UptateAmouttext()
    {
        amoutText.text = GameManager.Instance.healAmout.ToString();
    }
    
    public void HealHP(int hp)
    {
        playercontrol1.stats.hp += hp;
        statusBar.UpdateSlider("PlayerHP", playercontrol1.stats.hp);
        GameManager.Instance.healAmout--;
        UptateAmouttext();
    }
    public void AddHealamout(int amout)
    {
        GameManager.Instance.healAmout+=amout;
        UptateAmouttext();
    }

}
