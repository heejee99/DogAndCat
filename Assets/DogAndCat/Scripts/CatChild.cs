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
        //������ ���� �Ѹ��̶� ������ ���� ����
        if (cat.detectedEnemies.Length > 0)
        {
            //���� �����϶�
            if (cat.isRangeAttackType)
            {
                foreach(Collider2D detectedEnemy in cat.detectedEnemies)
                {
                    if (detectedEnemy.CompareTag("Player"))
                    {
                        if(detectedEnemy.TryGetComponent<Dog>(out Dog dog))
                        {
                            print($"����̰� ������{dog.name}�� ����������");
                            dog.TakeDamage(cat.damage);
                        }
                        else if (detectedEnemy.TryGetComponent<Player>(out Player player))
                        {
                            print("����̰� Ÿ���� ����������");
                            player.TakeDamage(cat.damage);
                        }
                    }
                }
            }

            //���� �����϶�
            else
            {
                //���� ���� ü��
                float leastHp = float.MaxValue;
                //Ÿ��
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
                    print($"ü���� ���� ���� {target}���� �������� �־���.");
                    print($"ü�� ���� : {target.hpBarAmount}");
                }
            }

        }
    }
}
