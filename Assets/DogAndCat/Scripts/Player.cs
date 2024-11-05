using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float gold = 0f;
    public int level = 1;
    public int exp = 0;
    public float hp = 1000;
    public float maxHp;
    public float specialMoveDamage = 100f;

    public int value = 6;

    public bool isDead = false;

    public GameObject coolTimeErrorTextPrefab;
    public Transform coolTimeErrorTextPosition;
    float currentSpecialMoveCoolTime = 0f;

    private void Awake()
    {
        GameManager.Instance.player = this;
    }


    private void Start()
    {
        maxHp = hp;
        currentSpecialMoveCoolTime = -specialMoveCoolTimeduration;
        isCoolTime = false;
    }

    private void Update()
    {
        //Gold();
        //if (Input.GetKeyDown("space"))
        //{
        //    StartCoroutine(SpecialMoveAttack());
        //}
        isSpecialMoveCoolTime();

        if (isDead)
        {
            OnDead();
        }
    }

    public void SpawnButton(int id)
    {
        ResourceManager.Instance.SpawnDog(id);
    }

    //public float Gold()
    //{
    //    gold += (1/value) * Time.deltaTime * 100;
    //    return gold;
    //}

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
        print("Ÿ���� �������� ����");
    }

    public void OnDead()
    {
        currentSpecialMoveCoolTime = 0;
        isCoolTime = false;
        Destroy(gameObject);
    }


    //private float currentSpeicalMoveAttackTime;
    //private float currentSpeicalMoveDuration = 1f;
    //Vector2 startPos;
    //Vector2 endPos;

    //��ƼŬ������
    public GameObject specialMoveEffectPrefabs;

    public float specialMoveCoolTimeduration = 2f;
    public bool isCoolTime = true;

    //�ʻ�Ⱑ ��Ÿ������ ��Ÿ������ �������ִ� �Լ�
    public void isSpecialMoveCoolTime()
    {
        //��Ÿ�� �ð��̸� false��
        if (Time.time < currentSpecialMoveCoolTime + specialMoveCoolTimeduration)
        {
            //Debug.LogError("������ ��Ÿ���Դϴ�.");
            isCoolTime = true;
        }
        //��Ÿ���� �� ���� true�� �����Ѵ�
        else
        {

            //Debug.LogError("��Ÿ�� �� ��.");
            isCoolTime = false;
        }
        //currentSpecialMoveCoolTime = Time.time;
    }

    //�ʻ�� ����
    public float specialMoveAttackRange_X = 7.8f;
    public float specialMoveAttackRange_Y = 4f;

    public void SpecialMoveAttack()
    {
        if (isCoolTime)
        {
            StartCoroutine(SetCoolTimeErrorText());
            return;
        }

        //��ƼŬ �����ϰ�
        GameObject specialMoveEffect = Instantiate(specialMoveEffectPrefabs, transform);
        specialMoveEffect.transform.localPosition = new Vector3(-3.8f, -1, 0);
        currentSpecialMoveCoolTime = Time.time;

        //������ ������ �ֱ�
        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - (specialMoveAttackRange_X / 2), transform.position.y)
            , new Vector2(specialMoveAttackRange_X, specialMoveAttackRange_Y), 0);
        foreach (var enemyCollider in enemyColliders)
        {
            if (enemyCollider.TryGetComponent<Cats>(out Cats cat))
            {
                cat.TakeDamage(specialMoveDamage);
                cat.transform.position -= Vector3.right * 2;
            }
            
        }

        //�������� �迭 �����
        enemyColliders = null;
    }

    //����� �׸����ΰ�?
    public bool drawGizmosSpecialMoveAttackRange;
    private void OnDrawGizmos()
    {
        if (drawGizmosSpecialMoveAttackRange)
        {
            Gizmos.DrawWireCube(new Vector2(transform.position.x - (specialMoveAttackRange_X / 2),
            transform.position.y), new Vector2(specialMoveAttackRange_X, specialMoveAttackRange_Y));
            Gizmos.color = Color.yellow;
        }
    }

    IEnumerator SetCoolTimeErrorText()
    {

        GameObject coolTimeErrorText = Instantiate(coolTimeErrorTextPrefab, UIManager.Instance.goldErrorTextPosition);
        coolTimeErrorText.transform.localPosition = Vector3.zero; // Vector3.Lerp(goldErrorText.transform.localPosition, tartgetPos, Time.time);
        coolTimeErrorText.transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(2f);
        Destroy(coolTimeErrorText);

    }
}
