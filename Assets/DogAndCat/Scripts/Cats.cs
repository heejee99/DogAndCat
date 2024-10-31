using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Cats : MonoBehaviour
{
    public float moveSpeed = 0.5f; //이동속도
    public float Scale = 0.6f; //크기
    public float attackInterval = 1f; //공격속도
    public float hp = 80; //체력
    private float maxHp; //최대 체력
    public float damage = 6; //공격력
    public bool attackType; //공격 타입

    public bool isDead = false; //죽었는지


    public bool isContact = false;

    [Tooltip("자식에 있는 오브젝트를 넣어주세요.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //처음엔 위에처럼 작성했는데 참조가 되지 않아서 계속 오류가 나왔다. 그래서 그냥 CharacterAnimation에서 자식 컴포넌트에 있는
    //Animator을 직접 참조하게만 하고 나는 public으로 Animator가 있는 자식 오브젝트를 직접 참조하게 해주었다.
    private void Awake()
    {

    }
    private void Start()
    {
        maxHp = hp;
    }

    private void Update()
    {
        if (isContact)
        {
            OnAttack();
            //Attack();
        }
        else
        {
            Vector2 goRight = Vector2.right;
            Move(goRight);
            animalAnimation.Walk();
        }

        IsDead();
    }

    //움직임
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(-Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    public void OnAttack()
    {
        Move(Vector2.zero);
        animalAnimation.Jump();
    }

    //데미지 입음
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (IsDead())
        {
            OnDead();
        }
    }

    public bool IsDead()
    {
        if (hp <= 0)
        {
            isDead = true;
        }
        return isDead;
    }

    private void OnDead()
    {
        GameManager.Instance.cats.Remove(this);
        Destroy(gameObject);
    }

    //적 콜라이더를 감지함
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Dogs>(out Dogs dogs))
    //    {
    //        isContact = true;
    //    }
    //}
}
