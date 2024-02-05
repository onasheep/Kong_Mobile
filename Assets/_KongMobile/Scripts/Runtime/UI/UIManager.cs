using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanel { get; private set; }
    public GameObject GameWinPanel { get; private set; }

    private Slider playerHpSlider = default;
    private Slider bossHpSlider = default;
    private Slider playerStaminaSlider = default;

    private PlayerController player = default;
    private Rhino rhino = default;

    // Start is called before the first frame update
    private void Awake()
    {
        GameOverPanel = this.gameObject.GetChildObj("GameOverPanel");
        GameWinPanel = this.gameObject.GetChildObj("GameWinPanel");
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        playerHpSlider = this.gameObject.GetChildObj("PlayerHp").GetComponent<Slider>();
        bossHpSlider = this.gameObject.GetChildObj("BossHp").GetComponent<Slider>();
        playerStaminaSlider = this.gameObject.GetChildObj("PlayerStamina").GetComponent<Slider>();

        player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();
        rhino = GFunc.GetRootObj("Enemy_Rhino").GetComponent<Rhino>();

    }

    // Update is called once per frame
    void Update()
    {
        ChangeHpbar();
        ChangeBossHp();
        ChangePlayerStamina();
    }

    private void ChangePlayerStamina()
    {
        playerStaminaSlider.value = player.Stamina / player.MaxStamina;
    }
    private void ChangeBossHp()
    {
        bossHpSlider.value = rhino.Hp / rhino.MaxHP;
    }
    private void ChangeHpbar()
    {
            playerHpSlider.value = player.Hp / player.MaxHp;
    }
}
