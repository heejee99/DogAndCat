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
        print("타워가 데미지를 입음");
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

    //파티클프리팹
    public GameObject specialMoveEffectPrefabs;

    public float specialMoveCoolTimeduration = 2f;
    public bool isCoolTime = true;

    //필살기가 쿨타임인지 불타입으로 리턴해주는 함수
    public void isSpecialMoveCoolTime()
    {
        //쿨타임 시간이면 false를
        if (Time.time < currentSpecialMoveCoolTime + specialMoveCoolTimeduration)
        {
            //Debug.LogError("아직은 쿨타임입니다.");
            isCoolTime = true;
        }
        //쿨타임이 다 차면 true를 리턴한다
        else
        {

            //Debug.LogError("쿨타임 다 참.");
            isCoolTime = false;
        }
        //currentSpecialMoveCoolTime = Time.time;
    }

    //필살기 범위
    public float specialMoveAttackRange_X = 7.8f;
    public float specialMoveAttackRange_Y = 4f;

    public void SpecialMoveAttack()
    {
        if (isCoolTime)
        {
            StartCoroutine(SetCoolTimeErrorText());
            return;
        }

        //파티클 생성하고
        GameObject specialMoveEffect = Instantiate(specialMoveEffectPrefabs, transform);
        specialMoveEffect.transform.localPosition = new Vector3(-3.8f, -1, 0);
        currentSpecialMoveCoolTime = Time.time;

        //실제로 데미지 주기
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

        //끝나고나면 배열 지우기
        enemyColliders = null;
    }

    //기즈모 그릴것인가?
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
