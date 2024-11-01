using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogs : MonoBehaviour
{
    public float moveSpeed = 0.5f; //이동속도
    public float Scale = 0.6f; //크기
    public float attackInterval = 1f; //공격주기
    public float hp = 50; //체력
    private float maxHp; //최대 체력
    public float damage = 8; //공격력
    public float cost = 200; //생산비용

    private bool isDead = false; //죽었는가?

    public bool attackType; //공격 타입

    public bool isContact = false; //적을 만났는가?



    [Tooltip("자식에 있는 오브젝트를 넣어주세요.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //처음엔 위에처럼 작성했는데 참조가 되지 않아서 계속 오류가 나왔다. 그래서 그냥 CharacterAnimation에서 자식 컴포넌트에 있는
    //Animator을 직접 참조하게만 하고 나는 public으로 Animator가 있는 자식 오브젝트를 직접 참조하게 해주었다.

    private void Start()
    {
        //StartCoroutine(OnAttackRange());
        maxHp = hp;
        //GameManager.Instance.dogs.Add(this);
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
            Vector2 gLeft = Vector2.left;
            Move(gLeft);
            animalAnimation.Walk();
        }
        IsDead();
        //Attack();
        //Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(attackRange, 2 * attackRange), 0);
        //foreach (Collider2D collider in colliders)
        //{
        //    if (collider.CompareTag("Cat"))
        //    {
        //        Cats cat = collider.GetComponent<Cats>();
        //        detectedCatList.Add(cat);

        //        //if (cat != null && cat.GetComponent<Rigidbody2D>() != null)
        //        //{
        //        //    dog.AnimalAnimation.Jump();
        //        //    cat.TakeDamage(dog.damage);
        //        //    print("고양이 공격 받음");
        //        //}
        //        isContact = true;
        //        print("공격 개시!");
        //    }
        //}
    }


    //움직임
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    public void OnAttack()
    {
        //멈추고
        Move(Vector2.zero);
        animalAnimation.Jump();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (IsDead())
        {
            OnDead();
        }
    }

    //죽었는지 살았는지 확인하는 함수
    public bool IsDead()
    {
        if (hp <= 0)
        {
            isDead = true;
        }

        return isDead;
    }

    //죽으면 뜨는 함수
    public void OnDead()
    {
        GameManager.Instance.dogs.Remove(this);
        Destroy(gameObject);
    }


    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Cats>(out Cats cats))
    //    {
    //        GameManager.Instance.cats.Add(cats);
    //        foreach (var cat in GameManager.Instance.cats)
    //        {
    //            cat.TakeDamage(attack);
    //            print(cat.hp);
    //        }
    //    }
    //}

    ////적 콜라이더를 감지함
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //   if(collision.TryGetComponent<Cats>(out Cats cats))
    //    {
    //        isContact = true; 
    //    }
    //}

    //public void OnAttackRange()
    //{
    //    while (true)
    //    {
    //        if ()
    //        Collider2D[] contactedColls = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 1 * attackRange, transform.position.y), new Vector2(1, 1) * attackRange / 2, 0);

    //        foreach (Collider2D contactedColl in contactedColls)
    //        {
    //            if (contactedColl.CompareTag("Cat"))
    //            {
    //                print($"지금 범위에 있는 고양이 수 : {contactedColls.Length}");
    //            }
    //        }
    //    }
    //}

    //public void Attack()
    //{
    //    //Collider2D[] contactedColls = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 1 * attackRange, transform.position.y), new Vector2(1, 1) * attackRange / 2, 0);

    //    //foreach (Collider2D contactedColl in contactedColls)
    //    //{
    //    //    if (contactedColl.CompareTag("Cat"))
    //    //    {
    //    //        print($"지금 범위에 있는 고양이 수 : {contactedColls.Length}");
    //    //    }
    //    //}
    //    animalAnimation.Jump();
    //}

}
