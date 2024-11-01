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

    //���⿡ �ִ� fillAmount�� ���� �Ŵ�.
    public List<Image> spawnDelayAmount = new();

    //���� �������� �ƴ��� 
    private List<bool> canSpawn = new();

    //��Ÿ�� 2��
    public List<float> spawnDelayScale = new();
    //���� ��Ÿ��
    private List<float> currentSpawnDelay = new();

    public TextMeshProUGUI goldText;

    private int playerLevel = 0;
    //�ڿ� ���Ѽ�
    public int[] maxGold = { 100, 150, 200, 250, 300, 350, 400 };
    //������ ���
    public int[] levelUpCost = { 40, 80, 120, 160, 200, 240 };
    //�ʴ� ������ ���
    public int[] goldPerSecond = { 6, 10, 14, 18, 22, 26, 30 };
    //���� �������� ���
    private int currentGold = 0;



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
    //��尡 ������ ���͹��ε� 1�ʷ� ����
    public float goldIncreaseInterval = 1f;

    private void Update()
    {
        UpdateSpawnDelay();
        
        if (Time.time > goldIncreasedTime + goldIncreaseInterval)
        {
            StartCoroutine(SetGoldText(currentGold));
            currentGold += goldPerSecond[playerLevel];
            goldIncreasedTime = Time.time;
        }
    }

    private void UpdateSpawnDelay()
    {
        for (int i = 0; i < spawnDelayScale.Count; i++)
        {
            currentSpawnDelay[i] += Time.deltaTime;
            //���� ��Ÿ���� ���س��� ��Ÿ�� ���� ũ�� True�� �ȴ�.
            canSpawn[i] = spawnDelayScale[i] <= currentSpawnDelay[i];

            //�׸��ڴ� ������ ���ؾ� ������.
            //�׷��� canSpawn, �� ��ȯ �����Ѱ��� ���� false������ ������.
            SpawnButtonShadow[i].SetActive(canSpawn[i] == false);
            spawnDelayAmount[i].fillAmount = (spawnDelayScale[i] - currentSpawnDelay[i]) / spawnDelayScale[i];
        }
    }

    public void Spawn(int id)
    {
        //������ �ȵǸ� �ٷ� �������� ����������.
        if (canSpawn[id - 1] == false)
        {
            return;
        }
        GameManager.Instance.player.SpawnButton(id);
        currentSpawnDelay[id - 1] = 0f;
    }

    public float goldDisPlayDuration;

    IEnumerator SetGoldText(int startGold)
    {
        float startTime = Time.time;
        float endTime = Time.time + goldDisPlayDuration;

        while (Time.time < endTime)
        {
            goldText.text = Mathf.Lerp(currentGold, startGold, (endTime - Time.time) / goldDisPlayDuration).ToString("n0");
            yield return null;
        }
    }

    public void LevelUp()
    {
        if (currentGold >= maxGold[playerLevel])
        {
            playerLevel++;
        }
    }
}
