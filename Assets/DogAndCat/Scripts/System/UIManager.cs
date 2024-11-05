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
    private int currentMaxGold;
    //������ ���
    public int[] levelUpCost = { 40, 80, 120, 160, 200, 240, 100 };
    private int currentLevelUpCost;
    //�ʴ� ������ ���
    public int[] goldPerSecond = { 6, 10, 14, 18, 22, 26, 30 };
    private int currentGoldPerSecond;
    //���� �������� ���
    private int currentTotalGold = 0;

    public TextMeshProUGUI levelValueText;
    public TextMeshProUGUI levelUpCostText;
    public TextMeshProUGUI enemyHpText;
    public TextMeshProUGUI playerHpText;

    //���� ������ ���� �����ϴϱ� ���������� ��������.
    public GameObject goldErrorTextPrefab;
    public GameObject coolTimeErrorTextPrefab;

    public Transform goldErrorTextPosition;

    public Animator canLevelUpAnimation;

    //�����ϱ⿡ ����� ���� ������ �ִ���
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
    //��尡 ������ ���͹��ε� 1�ʷ� ����
    public float goldIncreaseInterval = 1f;

    private void Update()
    {
        UpdateSpawnDelay();

        //���� ������ ���� �ʱ�ȭ -> ������ ���� �޶���
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

        //���� �̹��� ����
        OnDefeatImage();
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
        if (canSpawn[id - 1] == false || currentTotalGold < spawnValue[id - 1])
        {
            haveSpawnValue = false;
            Debug.LogError("���� �����մϴ�");
            StartCoroutine(SetGoldErrorText());
            return;
        }

        haveSpawnValue = true;
        GameManager.Instance.player.SpawnButton(id);
        currentTotalGold -= spawnValue[id- 1];
        currentSpawnDelay[id - 1] = 0f;
    }

    //���ʵ��� ���̰� �� ������
    public float goldDisPlayDuration = 0.5f;

    IEnumerator SetGoldText(int startGold)
    {
        //float startTime = Time.time;
        float endTime = Time.time + goldDisPlayDuration;

        while (Time.time < endTime)
        {
            //�ڿ� goldDisPlayDuration ������ startGold�������� ��尡 ������ �Ŵ�.
            goldText.text = $"{Mathf.Lerp(currentTotalGold, startGold, (endTime - Time.time) / goldDisPlayDuration).ToString("n0")} / {currentMaxGold}��";
            yield return null;
        }
    
    }

    public void LevelUp()
    {
        if (playerLevel > 5)
        {
            Debug.LogError("�ְ� �����Դϴ�.");
            playerLevel = 6;
            return;
        }
        else if (currentTotalGold < currentLevelUpCost)
        {
            Debug.LogError("���� �����մϴ�");
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
        levelUpCostText.text = $"{currentLevelUpCost} ��";
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

    //���� ������ ���� �޽����� ���� ������ �� �ȵȴ�. -> �������� ����� �ɵ� !
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

        //�ٽ� ������ �����Ҷ� ������ �ȵ� �� �ִ�.
        if (goldErrorText != null)
        {
            Destroy(goldErrorText);
        }
    }

    public Button reStartButton;
    //�÷��̾� ü���� 0�̵Ǹ� �й�â Ȱ��ȭ
    public GameObject defeatImage;
    public void OnDefeatImage()
    {
        if (GameManager.Instance.player.isDead)
        {
            //�ٽ� ������ �����Ҷ� ������ �ȵ� �� �ִ�.
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
