using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cats : MonoBehaviour
{
    public float moveSpeed = 0.5f; //이동속도
    public float Scale = 0.6f; //크기
    public float attackSpeed = 1f; //공격속도
    //사정거리는 각자 콜라이더를 조절해서 정할 예정
    public float hp = 80; //체력
    private float maxHp; //최대 체력
    public float damage = 6; //공격력
    public bool attackType; //공격 타입

    public bool isDead = false; //죽었는지


    public bool isContact = false;

    [Tooltip("자식에 있는 오브젝트를 넣어주세요.")]
    public AnimalAnimation AnimalAnimation;
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
        //if (gameObject.transform != null)
        //{
        //    if (isContact)
        //    {
        //        Vector2 stop = Vector2.zero;
        //        Move(stop);
        //        AnimalAnimation.Jump();
        //    }
        //    else
        //    {
        //        Vector2 goRight = Vector2.right;
        //        Move(goRight);
        //        AnimalAnimation.Walk();
        //    }

        //    IsDead();
        //}
        Vector2 goRight = Vector2.right;
        Move(goRight);
    }

    //움직임
    public void Move(Vector2 moveDir)
    {
        AnimalAnimation.Walk();
        transform.localScale = new Vector3(-Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
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
        Destroy(this);
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
