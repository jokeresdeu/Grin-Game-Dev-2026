using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject attackArea;
    public GameObject attackBall;
    public GameObject expBar;
    public GameObject healthBar;
    public GameObject deathMenu;
    public GameObject player;
    public GameObject upgradeMenu;

    public Transform Lv1Point;
    public Transform Lv2Point;
    public Transform Lv3Point;
    public Transform Lv4Point;

    public Text scoreText;
    public Text bestScoreText;

    public float health = 1000f;
    public float attackSpeed = 5f;
    public float damage = 5f;

    public int expProgress = 0;
    public int attackLevel = 1;
    public int score;

    private float maxExpRange;

    void FillExpBar()
    {
        float gettingExp = (maxExpRange * expProgress) / 100;
        expBar.transform.localScale = new Vector3(
                gettingExp,
                expBar.transform.localScale.y,
                expBar.transform.localScale.z
        );
    }

    void UpgradeExpBar()
    {
        expBar.transform.localScale = new Vector3(
                expBar.transform.localScale.x - expBar.transform.localScale.x,
                expBar.transform.localScale.y,
                expBar.transform.localScale.z
        );

        
    }

    void GiveOneBallMore()
    {
        switch (attackLevel)
        {
            case 1:
                Instantiate(attackBall, Lv1Point);
                break;
            case 2:
                Instantiate(attackBall, Lv2Point);
                break;
            case 3:
                Instantiate(attackBall, Lv3Point);
                break;
            case 4:
                Instantiate(attackBall, Lv4Point);
                break;
        }
    }

    void Start()
    {
        score = 0;
        maxExpRange = expBar.transform.localScale.x;
        UpgradeExpBar();
        GiveOneBallMore();
    }

    void Update()
    {
        if (expProgress < 100) 
            FillExpBar();

        attackArea.transform.Rotate(0f, 0f, attackSpeed * Time.deltaTime);

        if (healthBar.transform.localScale.x <= 0)
        {
            if (int.Parse(bestScoreText.text) < int.Parse(scoreText.text))
            {
                bestScoreText.text = scoreText.text;
            }
            deathMenu.SetActive(true);
            //Time.timeScale = 0f;
        }

        if (expProgress >= 100 && attackLevel < 4 && attackSpeed < 360 && damage < 100)
        {
            upgradeMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PlusFireBall()
    {
        if (attackLevel < 4)
        {
            expProgress = 0;
            attackLevel += 1;
            UpgradeExpBar();
            GiveOneBallMore();
            upgradeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void PlusAttackSpeed()
    {
        if (attackSpeed < 360)
        {
            expProgress = 0;
            attackSpeed += 90;
            UpgradeExpBar();
            upgradeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void PlusAttackDamage()
    {
        if (damage < 100)
        {
            expProgress = 0;
            damage += 20;
            UpgradeExpBar();
            upgradeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
