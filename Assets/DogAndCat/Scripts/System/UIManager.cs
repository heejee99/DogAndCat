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
    //레벨업 비용
    public int[] levelUpCost = { 40, 80, 120, 160, 200, 240 };
    //초당 오르는 골드
    public int[] goldPerSecond = { 6, 10, 14, 18, 22, 26, 30 };
    //현재 보유중인 골드
    private int currentGold = 0;
    //1초를 재줄 timer
    private float timer = 1f;



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
        //currentSpawnDelay = spawnDelayScale.ToList();
    }
    private void Update()
    {
        UpdateSpawnDelay();
        
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

    //public void GetGold()
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= 1)
    //    {
    //        currentGold += gold
    //    }
    //}

}
