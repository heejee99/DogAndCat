using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using UnityEngine;

public class DogChild : MonoBehaviour
{
    Dog dog;
    private void Awake()
    {
        dog = GetComponentInParent<Dog>();
    }
    public void TakeAttack()
    {
        //감지된 적이 한명이라도 있으면 공격 시작
        if (dog.detectedEnemies.Length > 0)
        {
            //범위공격일때
            if (dog.isRangeAttackType)
            {
                foreach (Collider2D detectedEnemy in dog.detectedEnemies)
                {
                    if (detectedEnemy.CompareTag("Enemy"))
                    {
                        if (detectedEnemy.TryGetComponent<Cat>(out Cat cat))
                        {

                            print($"강아지가 고양이{cat.name}를 범위공격함");
                            cat.TakeDamage(dog.damage);

                        }
                        if (detectedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                        {
                            print("강아지가 타워를 범위공격함");
                            enemy.TakeDamage(dog.damage);

                        }
                    }
                }
            }
            //단일공격일때
            else
            {
                //가장 적은체력
                float leastHp = float.MaxValue;
                //타겟
                IHealth target = null;
                foreach (Collider2D detectedEnemy in dog.detectedEnemies)
                {
                    if (detectedEnemy.CompareTag("Enemy"))
                    {
                        if (detectedEnemy.TryGetComponent<IHealth>(out IHealth cat))
                        { 
                            if (cat.hpBarAmount < leastHp)
                            {
                                leastHp = cat.hpBarAmount;
                                target = cat;
                            }
                            //print($"강아지가 고양이{cat.name}를 범위공격함");
                            //cat.TakeDamage(dog.damage);
                        }
                        //if (detectedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                        //{
                        //    //print("강아지가 타워를 범위공격함");
                        //    //enemy.TakeDamage(dog.damage);
                        //    if (enemy.hpBarAmount < leastHp)
                        //    {
                        //        leastHp = enemy.hpBarAmount;
                        //    }
                        //}
                    }
                    //float closestEnemyDistance = float.MaxValue;
                    //Cat closestCat = null;
                    //float targetDistance = (dog.transform.position - cat.transform.position).magnitude;
                    //if (targetDistance < dog.attackRange_X)
                    //{
                    //    closestEnemyDistance = targetDistance;
                    //    closestCat = cat;
                    //}

                    //if (closestCat != null)
                    //{
                    //    closestCat.TakeDamage(dog.damage);
                    //    print($"공격받은 고양이 이름 : {cat.name}");
                    //    return;
                    //}
                }

                if(target != null)
                {
                    target.TakeDamage(dog.damage);
                    print($"체력이 가장 낮은{target}에게 데미지를 줌");
                    print($"체력 비율 : {target.hpBarAmount}");
                }
            }
        }

    }
}
