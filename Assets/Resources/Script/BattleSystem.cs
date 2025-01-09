using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Unit playerUnit;
    Unit enemyUnit;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public GameObject playerBS;
    public GameObject enemyBS;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject PanelWON;
    public GameObject PanelLOST;

    private Animator playerAnimator;
    private Animator enemyAnimator;

    // Referensi tombol
    public Button attackButton;
    public Button skillButton;
    public Button healButton;
    public Button buffButton;
    public Button debuffButton;
    public Button defendButton;

    // Metode untuk mengatur interaktivitas tombol
    private void SetButtonsInteractable(bool interactable)
    {
        attackButton.interactable = interactable;
        skillButton.interactable = interactable;
        healButton.interactable = interactable;
        buffButton.interactable = interactable;
        debuffButton.interactable = interactable;
        defendButton.interactable = interactable;
    }

    // Start is called before the first frame update
    void Start()
    {
      
       // animatorPlayer.SetBool("Idle",true);
        state = BattleState.START;
       StartCoroutine (Setupbattle());
        SetButtonsInteractable(false);
    }

    IEnumerator Setupbattle()
    {
        //memunculkan player
      GameObject playerGO =   Instantiate(playerPrefab,playerBattleStation);
        // Menghapus "(Clone)" dari nama GameObject
        playerGO.name = playerPrefab.name; 
        //mengambil komponen player
        playerUnit = playerGO.GetComponent<Unit>();

        //memunculkan musuh
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        // Menghapus "(Clone)" dari nama GameObject
        enemyGO.name = enemyPrefab.name;
        //mengambil komponen musuh
        enemyUnit = enemyGO.GetComponent<Unit>();


        playerAnimator = playerGO.GetComponentInChildren<Animator>();
        enemyAnimator = enemyGO.GetComponentInChildren<Animator>();

        dialogueText.text = "Seekor " + enemyUnit.name + " menghadangmu";

      

        playerHUD.setHUD(playerUnit);
        enemyHUD.setHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        // Trigger attack animation
        playerAnimator.SetBool("Attack", true);

        yield return new WaitForSeconds(1f);

        playerAnimator.SetBool("Attack", false);
        bool isDead =  enemyUnit.TakeDamage(playerUnit.Damage);
      
        if (enemyUnit.currentHP <= 0)
        {
            enemyUnit.currentHP = enemyUnit.minHP;
        }

        enemyHUD.setHP(enemyUnit.currentHP);

       
        dialogueText.text = "Seranganmu Berhasil!!";

        yield return new WaitForSeconds(2f);
        dialogueText.text = "Mengurangi 10 HP Musuh ";
        yield return new WaitForSeconds(2f);

        

        if (isDead)
        {
           state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerSkill()
    {
        playerAnimator.SetBool("Skill", true);

        yield return new WaitForSeconds(1f);

        playerAnimator.SetBool("Skill", false);
        bool isDead = enemyUnit.TakeDamage(playerUnit.SkillDamage);

        if (enemyUnit.currentHP <= 0)
        {
            enemyUnit.currentHP = enemyUnit.minHP;
        }

        enemyHUD.setHP(enemyUnit.currentHP);
        dialogueText.text = "Tornado Slash Berhasil!!";

        yield return new WaitForSeconds(2f);

        dialogueText.text = "Mengurangi 15 HP Musuh ";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        // Pilih aksi secara acak: 1 = Attack, 2 = Defend, 3 = Buff, 4 = Debuff
        int action = Random.Range(1, 6);
        SetButtonsInteractable(false);
        switch (action)
        {
            case 1: // Normal Attack
                dialogueText.text = enemyUnit.unitName + " menyerang!";
                enemyAnimator.SetBool("isAttack", true);

                yield return new WaitForSeconds(1f);

                enemyAnimator.SetBool("isAttack", false);
                // Mainkan suara attack musuh
                FindObjectOfType<AudioSystem>().PlayAttackGoblinSound();
                yield return new WaitForSeconds(1f);
               
                dialogueText.text = "HP Player Berkurang 8 Point ";
                yield return new WaitForSeconds(2f);

                bool isDead = playerUnit.TakeDamage(enemyUnit.Damage);
                if (playerUnit.currentHP <= 0)
                {
                    playerUnit.currentHP = playerUnit.minHP;
                }

                playerHUD.setHP(playerUnit.currentHP);
                yield return new WaitForSeconds(1f);

                if (isDead)
                {
                    state = BattleState.LOST;
                    EndBattle();
                    yield break;
                }
                break;

            case 2: // Defend
                dialogueText.text = enemyUnit.unitName + " sedang bertahan!";
                // Mainkan suara defend musuh
                FindObjectOfType<AudioSystem>().PlayDefendGoblinSound();
                enemyUnit.isDefending = true;
                yield return new WaitForSeconds(2f);
                break;

            case 3: // Buff
                dialogueText.text = enemyUnit.unitName + " meningkatkan kekuatannya!";
                // Mainkan suara buff
                FindObjectOfType<AudioSystem>().PlayBuffSound();
                enemyUnit.ApplyBuff(5, 3); // Buff damage +5 selama 3 giliran
                yield return new WaitForSeconds(2f);
                break;

            case 4: // Debuff
                dialogueText.text = enemyUnit.unitName + " melemahkan seranganmu!";
                // Mainkan suara debuff
                FindObjectOfType<AudioSystem>().PlayDebuffSound();
                playerUnit.ApplyDebuff(5, 3); // Debuff damage -5 selama 3 giliran
                yield return new WaitForSeconds(2f);
                break;

            case 5: // Skill Attack
                dialogueText.text = enemyUnit.unitName + " menggunakan serangan skill!";
                enemyAnimator.SetBool("isSkill", true);

                yield return new WaitForSeconds(1f);

                enemyAnimator.SetBool("isSkill", false);
                // Mainkan suara skill attack musuh
                FindObjectOfType<AudioSystem>().PlaySkillGoblinSound();
                yield return new WaitForSeconds(1f);
                
                bool skillIsDead = playerUnit.TakeDamage(enemyUnit.SkillDamage);
                if (playerUnit.currentHP <= 0)
                {
                    playerUnit.currentHP = playerUnit.minHP;
                }

                playerHUD.setHP(playerUnit.currentHP);
                yield return new WaitForSeconds(1f);

                if (skillIsDead)
                {
                    state = BattleState.LOST;
                    EndBattle();
                    yield break;
                }
                break;
        }

        // Update efek buff/debuff setelah giliran musuh selesai
        enemyUnit.UpdateStatusEffects();
        playerUnit.UpdateStatusEffects();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void EndBattle()
    {
        SetButtonsInteractable(false);
        if (state == BattleState.WON)
        {
            dialogueText.text = "KAMU MENANG!!";
            playerBS.SetActive(false);
            enemyBS.SetActive(false);
            PanelWON.SetActive(true);

        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "KAMU KALAH!!!";
            playerBS.SetActive(false);
            enemyBS.SetActive(false);
            PanelLOST.SetActive(true);
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Pilih Tindakanmu...";
        SetButtonsInteractable(true);
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHUD.setHP(playerUnit.currentHP);
        dialogueText.text = "Kamu Menyembuhkan Diri";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnattackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        SetButtonsInteractable(false);
        StartCoroutine( PlayerAttack()); 
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        SetButtonsInteractable(false);
        StartCoroutine(PlayerHeal());
    }

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        SetButtonsInteractable(false);
        StartCoroutine(PlayerSkill());
    }

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        SetButtonsInteractable(false);
        StartCoroutine(PlayerDefend());
    }

    IEnumerator PlayerDefend()
    {
        dialogueText.text = "Kamu bersiap bertahan!";
        playerUnit.isDefending = true;

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnBuffButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        SetButtonsInteractable(false);
        StartCoroutine(PlayerBuff());
    }

    IEnumerator PlayerBuff()
    {
        dialogueText.text = "Kamu meningkatkan kekuatanmu!";
        playerUnit.ApplyBuff(5, 3); // Buff damage +5 selama 3 giliran

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    public void OnDebuffButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        SetButtonsInteractable(false);
        StartCoroutine(PlayerDebuff());
    }

    IEnumerator PlayerDebuff()
    {
        dialogueText.text = "Kamu mengurangi kekuatan musuh!";
        enemyUnit.ApplyDebuff(5, 3); // Debuff damage -5 selama 3 giliran

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
