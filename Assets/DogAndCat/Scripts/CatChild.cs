using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CatChild : MonoBehaviour
{
    Cat cat;
    private void Awake()
    {
        cat = GetComponentInParent<Cat>();
    }
    public void TakeAttack()
    {
        //감지된 적이 한명이라도 있으면 공격 시작
        if (cat.detectedEnemies.Length > 0)
        {
            //범위 공격일때
            if (cat.isRangeAttackType)
            {
                foreach(Collider2D detectedEnemy in cat.detectedEnemies)
                {
                    if (detectedEnemy.CompareTag("Player"))
                    {
                        if(detectedEnemy.TryGetComponent<Dog>(out Dog dog))
                        {
                            print($"고양이가 강아지{dog.name}를 범위공격함");
                            dog.TakeDamage(cat.damage);
                        }
                        else if (detectedEnemy.TryGetComponent<Player>(out Player player))
                        {
                            print("고양이가 타워를 범위공격함");
                            player.TakeDamage(cat.damage);
                        }
                    }
                }
            }

            //단일 공격일때
            else
            {
                //가장 적은 체력
                float leastHp = float.MaxValue;
                //타겟
                IHealth target = null;
                foreach(Collider2D detectedEnemy in cat.detectedEnemies)
                {
                    if (detectedEnemy.CompareTag("Player"))
                    {
                        if (detectedEnemy.TryGetComponent<IHealth>(out IHealth dog))
                        {
                            if (dog.hpBarAmount < leastHp)
                            {
                                leastHp = dog.hpBarAmount;
                                target = dog;
                            }
                        }
                        //else if(detectedEnemy.TryGetComponent<Player>(out Player player))
                        //{
                        //    if (player.hpBarAmount < leastHp)
                        //    {
                        //        leastHp = player.hpBarAmount;
                        //        targetDog = GetComponent<Dog>;
                        //    }
                        //}
                    }
                }

                if (target != null)
                {
                    target.TakeDamage(cat.damage);
                    print($"체력이 가장 낮은 {target}에게 데미지를 주었다.");
                    print($"체력 비율 : {target.hpBarAmount}");
                }
            }

        }
    }
}
