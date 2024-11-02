using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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

    public Animator canLevelUpAnimation;


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
                StopCoroutine(SetGoldText(currentTotalGold));
            }
            goldIncreasedTime = Time.time;
        }

        SetLevelValueText();
        SetLevelUpCostText();

        if (currentTotalGold >= currentLevelUpCost && playerLevel < 6)
        {
            canLevelUpAnimation.SetTrigger("canLevelUp");
        }
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
        if (canSpawn[id - 1] == false)
        {
            return;
        }
        GameManager.Instance.player.SpawnButton(id);
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
}
