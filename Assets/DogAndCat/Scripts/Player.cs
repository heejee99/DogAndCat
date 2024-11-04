using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float gold = 0f;
    public int level = 1;
    public int exp = 0;
    public float hp;

    public int value = 6;

    public bool isDead = false;

    public GameObject coolTimeErrorTextPrefab;
    public Transform coolTimeErrorTextPosition;

    private void Awake()
    {
        GameManager.Instance.player = this;
    }


    private void Start()
    {
    }

    private void Update()
    {
        //Gold();
        //if (Input.GetKeyDown("space"))
        //{
        //    StartCoroutine(SpecialMoveAttack());
        //}
        isSpecialMoveCoolTime();
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

    }

    //private float currentSpeicalMoveAttackTime;
    //private float currentSpeicalMoveDuration = 1f;
    //Vector2 startPos;
    //Vector2 endPos;

    //파티클프리팹
    public GameObject specialMoveEffectPrefabs;

    public float specialMoveCoolTimeduration = 2f;
    float currentSpecialMoveCoolTime = 0f;
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

    public void SpecialMoveAttack()
    {
        //float endTime = Time.time + currentSpeicalMoveDuration;
        //startPos = new Vector2(transform.position.x, 0);
        //Vector2 endPos = Vector2.zero; 
        //int count = 0;
        //while (Time.time < endTime)
        //{
        //    count++;
        //    print($"반복횟수 : {count}");
        //    float t = (endTime - Time.time) / currentSpeicalMoveDuration;
        //    Vector2 diraction = Vector2.Lerp(startPos, endPos, 1 - t);
        //    RaycastHit2D hit = Physics2D.Raycast(startPos, diraction.normalized);
        //    Debug.DrawRay(startPos, diraction.normalized * 10, Color.red);

        //    if(hit.collider != null)
        //    {
        //        Debug.Log("Hit : " + hit.collider.name);
        //    }
        //    if (count > 100)
        //    {
        //        break;
        //    }
        //    yield return null;
        //}
        //지난 시간이 쿨타임보다 짧으면 못 씀
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
    }

    //IEnumerator OnSpecialMoveText()
    //{
    //    UIManager.Instance.SetCoolTimeErrorText();
    //    yield return new WaitForSeconds(specialMoveCooltimeduration);
    //}

    IEnumerator SetCoolTimeErrorText()
    {

        GameObject coolTimeErrorText = Instantiate(coolTimeErrorTextPrefab, UIManager.Instance.goldErrorTextPosition);
        coolTimeErrorText.transform.localPosition = Vector3.zero; // Vector3.Lerp(goldErrorText.transform.localPosition, tartgetPos, Time.time);
        coolTimeErrorText.transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(2f);
        Destroy(coolTimeErrorText);

    }
}
