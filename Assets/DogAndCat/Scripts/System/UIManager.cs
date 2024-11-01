using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}
