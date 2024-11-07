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
        //������ ���� �Ѹ��̶� ������ ���� ����
        if (dog.detectedEnemies.Length > 0)
        {
            //���������϶�
            if (dog.isRangeAttackType)
            {
                foreach (Collider2D detectedEnemy in dog.detectedEnemies)
                {
                    if (detectedEnemy.CompareTag("Enemy"))
                    {
                        if (detectedEnemy.TryGetComponent<Cat>(out Cat cat))
                        {

                            print($"�������� �����{cat.name}�� ����������");
                            cat.TakeDamage(dog.damage);

                        }
                        if (detectedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                        {
                            print("�������� Ÿ���� ����������");
                            enemy.TakeDamage(dog.damage);

                        }
                    }
                }
            }
            //���ϰ����϶�
            else
            {
                //���� ����ü��
                float leastHp = float.MaxValue;
                //Ÿ��
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
                            //print($"�������� �����{cat.name}�� ����������");
                            //cat.TakeDamage(dog.damage);
                        }
                        //if (detectedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                        //{
                        //    //print("�������� Ÿ���� ����������");
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
                    //    print($"���ݹ��� ����� �̸� : {cat.name}");
                    //    return;
                    //}
                }

                if(target != null)
                {
                    target.TakeDamage(dog.damage);
                    print($"ü���� ���� ����{target}���� �������� ��");
                    print($"ü�� ���� : {target.hpBarAmount}");
                }
            }
        }

    }
}
