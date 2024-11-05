using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonManager<UIManager> 
{
    public List<GameObject> SpawnButtonShadow = new();

    //여기에 있는 fillAmount를 받을 거다.
    public List<Image> spawnDelayAmount = new();

    //스폰 가능한지 아닌지 
    private List<bool> canSpawn = new();

    //쿨타임 2초
    public List<float> spawnDelayScale = new();
    //현재 쿨타임
    private List<float> currentSpawnDelay = new();

    public TextMeshProUGUI goldText;

    private int playerLevel = 0;

    //자원 상한선
    public int[] maxGold = { 100, 150, 200, 250, 300, 350, 400 };
    private int currentMaxGold;
    //레벨업 비용
    public int[] levelUpCost = { 40, 80, 120, 160, 200, 240, 100 };
    private int currentLevelUpCost;
    //초당 오르는 골드
    public int[] goldPerSecond = { 6, 10, 14, 18, 22, 26, 30 };
    private int currentGoldPerSecond;
    //현재 보유중인 골드
    private int currentTotalGold = 0;

    public TextMeshProUGUI levelValueText;
    public TextMeshProUGUI levelUpCostText;
    public TextMeshProUGUI enemyHpText;
    public TextMeshProUGUI playerHpText;

    //돈이 없을때 마다 떠야하니까 프리팹으로 만들어줬다.
    public GameObject goldErrorTextPrefab;
    public GameObject coolTimeErrorTextPrefab;

    public Transform goldErrorTextPosition;

    public Animator canLevelUpAnimation;

    //스폰하기에 충분한 돈을 가지고 있는지
    private bool haveSpawnValue;

    public int[] spawnValue = { 50, 100, 200, 400, 400};


    private float hpBarAmount;

    Vector3 originPos;

    public Animator specialMoveAnimation;


    protected override void Awake()
    {
        base.Awake();
        
    }
    private void Start()
    {
        for (int i = 0; i < spawnDelayScale.Count; i++)
        {
            currentSpawnDelay.Add(spawnDelayScale[i]);
            canSpawn.Add(false);
        }

    }

    private float goldIncreasedTime;
    //골드가 오르는 인터벌인데 1초로 줬음
    public float goldIncreaseInterval = 1f;

    private void Update()
    {
        UpdateSpawnDelay();

        //현재 정보에 대한 초기화 -> 레벨에 따라 달라짐
        currentMaxGold = maxGold[playerLevel];
        currentLevelUpCost = levelUpCost[playerLevel];
        currentGoldPerSecond = goldPerSecond[playerLevel];

        if (Time.time > goldIncreasedTime + goldIncreaseInterval)
        {
            StartCoroutine(SetGoldText(currentTotalGold));
            currentTotalGold += currentGoldPerSecond;
            if (currentTotalGold >= currentMaxGold)
            {
                currentTotalGold = currentMaxGold;
                //StopCoroutine(SetGoldText(currentTotalGold));
            }
            goldIncreasedTime = Time.time;
        }

        SetLevelValueText();
        SetLevelUpCostText();
        SetEnemyHpText();
        SetPlayerHpText();

        if (currentTotalGold >= currentLevelUpCost && playerLevel < 6)
        {
            canLevelUpAnimation.SetTrigger("canLevelUp");
        }
        
        if (!GameManager.Instance.player.isCoolTime)
        {
            specialMoveAnimation.SetTrigger("onCoolTime");
        }

        //지면 이미지 켜짐
        OnDefeatImage();
    }

    private void UpdateSpawnDelay()
    {
        for (int i = 0; i < spawnDelayScale.Count; i++)
        {
            currentSpawnDelay[i] += Time.deltaTime;
            //현재 쿨타임이 정해놓은 쿨타임 보다 크면 True가 된다.
            canSpawn[i] = spawnDelayScale[i] <= currentSpawnDelay[i];

            //그림자는 스폰을 못해야 켜진다.
            //그래서 canSpawn, 즉 소환 가능한가에 대해 false여야지 켜진다.
            SpawnButtonShadow[i].SetActive(canSpawn[i] == false);
            spawnDelayAmount[i].fillAmount = (spawnDelayScale[i] - currentSpawnDelay[i]) / spawnDelayScale[i];
        }
    }

    public void Spawn(int id)
    {
        //스폰이 안되면 바로 리턴으로 빠져나간다.
        if (canSpawn[id - 1] == false || currentTotalGold < spawnValue[id - 1])
        {
            haveSpawnValue = false;
            Debug.LogError("돈이 부족합니다");
            StartCoroutine(SetGoldErrorText());
            return;
        }

        haveSpawnValue = true;
        GameManager.Instance.player.SpawnButton(id);
        currentTotalGold -= spawnValue[id- 1];
        currentSpawnDelay[id - 1] = 0f;
    }

    //몇초동안 보이게 할 것인지
    public float goldDisPlayDuration = 0.5f;

    IEnumerator SetGoldText(int startGold)
    {
        //float startTime = Time.time;
        float endTime = Time.time + goldDisPlayDuration;

        while (Time.time < endTime)
        {
            //뒤에 goldDisPlayDuration 때문에 startGold에서부터 골드가 오르는 거다.
            goldText.text = $"{Mathf.Lerp(currentTotalGold, startGold, (endTime - Time.time) / goldDisPlayDuration).ToString("n0")} / {currentMaxGold}원";
            yield return null;
        }
    
    }

    public void LevelUp()
    {
        if (playerLevel > 5)
        {
            Debug.LogError("최고 레벨입니다.");
            playerLevel = 6;
            return;
        }
        else if (currentTotalGold < currentLevelUpCost)
        {
            Debug.LogError("돈이 부족합니다");
            StartCoroutine(SetGoldErrorText());
            return;
        }
        currentTotalGold -= currentLevelUpCost;
        playerLevel++;
    }

    public void SetLevelValueText()
    {
        levelValueText.text = $"Lv : {playerLevel + 1}";
    }

    public void SetLevelUpCostText()
    {
        levelUpCostText.text = $"{currentLevelUpCost} 원";
    }

    public void SetEnemyHpText()
    {
        enemyHpText.text = $"{GameManager.Instance.enemy.hp} / {GameManager.Instance.enemy.maxHp}";
    }
    public void SetPlayerHpText()
    {
        playerHpText.text = $"{GameManager.Instance.player.hp} / {GameManager.Instance.player.maxHp}";
    }

    public float showDelayGoldErrorText = 1f;
    public float currentTimeGoldErrorText;

    //돈이 부족한 에러 메시지를 띄우고 싶은데 잘 안된다. -> 프리팹을 만들면 될듯 !
    IEnumerator SetGoldErrorText()
    {
        GameObject goldErrorText = Instantiate(goldErrorTextPrefab, goldErrorTextPosition);
        //Vector3 tartgetPos = goldErrorTextPosition.transform.position + Vector3.up * 10;
        goldErrorText.transform.localPosition = Vector3.zero; // Vector3.Lerp(goldErrorText.transform.localPosition, tartgetPos, Time.time);
        goldErrorText.transform.localRotation = Quaternion.identity;

        //TextMeshProUGUI goldErrorTextMesh = goldErrorText.GetComponent<TextMeshProUGUI>();
        //if (goldErrorTextMesh != null)
        //{
        //    goldErrorTextMesh.color = Mathf.Lerp(goldErrorTextMesh.color.a, )
        //}
        yield return new WaitForSeconds(2f);

        //다시 게임을 시작할때 참조가 안될 수 있다.
        if (goldErrorText != null)
        {
            Destroy(goldErrorText);
        }
    }

    public Button reStartButton;
    //플레이어 체력이 0이되면 패배창 활성화
    public GameObject defeatImage;
    public void OnDefeatImage()
    {
        if (GameManager.Instance.player.isDead)
        {
            //다시 게임을 시작할때 참조가 안될 수 있다.
            if (defeatImage != null)
            {
                defeatImage.SetActive(true);
            }
        }
    }

    public void ClickReStartButton()
    {
        if (reStartButton != null)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    //IEnumerator SetCoolTimeErrorText()
    //{
    //    if (GameManager.Instance.player.isSpecialMoveCoolTime())
    //    {
    //        GameObject coolTimeErrorText = Instantiate(coolTimeErrorTextPrefab, goldErrorTextPosition);
    //        coolTimeErrorText.transform.localPosition = Vector3.zero; // Vector3.Lerp(goldErrorText.transform.localPosition, tartgetPos, Time.time);
    //        coolTimeErrorText.transform.localRotation = Quaternion.identity;
    //        yield return new WaitForSeconds(2f);
    //        Destroy(coolTimeErrorText);
    //    }

    //}
}
