using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using MikroFramework.Singletons;
using MikroFramework.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public struct OnPlayerHitByEnemy {

}
public enum EnemyState
{
   Alive,
   Die
}
public class L11Enemy : MonoMikroSingleton<Level12Manager>
{
    public bool IsDie;
    [SerializeField] private float speed;
    private Vector2 startLocation;
    private Vector2 nextSpot;
    private GameObject target;
    [SerializeField] private float startWaitTime;
    private bool canMove = true;

    private SpriteRenderer spriteRenderer;

    private Animator animator;
    [SerializeField] private float patrollRangeX;
    
    private Vector3 spriteScale;
    [SerializeField] private float attackInterval = 2f;

    [SerializeField] private Trigger2DCheck fightPlayerCheck;

    private bool canAttack = false;
    private bool isDie = false;
    private bool isDodging = false;
    private bool isChasing = false;
   
    private bool facingRight = true;

    [SerializeField]
    private Trigger2DCheck attackTrigger2DCheck;
    [SerializeField]
    private Trigger2DCheck chaseTrigger2Dcheck;

    [SerializeField] private Vector2 yRange;

    private Rigidbody2D rigidbody;

    private float min_x;
    private float max_x;
   
    private float waitTime;
    private float attackTimer;
    private float hitPlayerTimer = 1f;
 
    float startLocation_y;
    Vector2 nextAttackSpotCenter;

    private void Awake()
    {
        startLocation = transform.position;
        min_x = startLocation.x - patrollRangeX;
        max_x = startLocation.x + patrollRangeX;
       
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        waitTime = startWaitTime;
        spriteScale = transform.localScale;
        nextAttackSpotCenter = transform.position;
        startLocation_y = transform.position.y;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        attackTrigger2DCheck = GetComponentInChildren<Trigger2DCheck>();
       
    }




    protected void Start()
    {
        nextSpot = DecideDistance(transform.position);
        TypeEventSystem.RegisterGlobalEvent<OnPlayerFightHit>(OnPlayerFightHit)
            .UnRegisterWhenGameObjectDestroyed(gameObject);

    }

    [SerializeField] private AudioClip hitClip;
    private void OnPlayerFightHit(OnPlayerFightHit e) {
        if (e.Target == gameObject) {
            Debug.Log("Hit");
            spriteRenderer.color = Color.red;
            hurtTimer = 0.3f;
            AudioManager.Singleton.PlayAudioShot(hitClip, 0.7f);
            isDie = true;
        }
    }

    private void Patrolling()
    {
        //Debug.Log(canMove + "  "+ nextSpot);
        isChasing = false;
        isDodging = false;
        if (!IsDie)
        {
            rigidbody.gravityScale = 0;
        }

        if (canMove) {
            transform.position = Vector2.MoveTowards
            (transform.position, new Vector2(nextSpot.x, nextSpot.y), speed * Time.deltaTime);
        }

        if ((nextSpot.x - transform.position.x) < 0)
        {
            spriteScale.x = -1;
        }
        else if ((nextSpot.x - transform.position.x) > 0)
        {
            spriteScale.x = 1;
        }

       

        if (Vector2.Distance(transform.position, nextSpot) <= 1f)
        {
            if (waitTime <= 0)
            {
                nextSpot = DecideDistance(transform.position);
                waitTime = startWaitTime;
                canMove = true;
            }
            else
            {
                //animator.SetInteger("EnemyState", 1);
                waitTime -= Time.deltaTime;
                canMove = false;

            }
        }
        else {
           
            if (!canMove && waitTime <= 0) {
                canMove = true;
            }
            else {
                Timer.Singleton.AddDelayTask(1f, () => { canMove = true;});
            }
            
        }


    }

    private void Fire()
    {
        if (attackTimer <= 0)
        {
            animator.SetInteger("EnemyState", 2);
            attackTimer = attackInterval;
            canAttack = false;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private bool CheckReached(Vector2 nextSpot)
    {
        if (Vector2.Distance(transform.position, nextSpot) <= 0.5)
            return true;
        return false;
    }
   

    private void Dodging() {
        if ((nextAttackSpotCenter.x - transform.position.x) <= 0) spriteScale.x = -1;
        else if ((nextAttackSpotCenter.x - transform.position.x) > 0) spriteScale.x = 1;

       if (!CheckReached(nextAttackSpotCenter)) {
           bool collidePlayer = Physics2D.Raycast(nextAttackSpotCenter, Vector2.down, 0.1f, playerMask).collider;
           while (collidePlayer) {
               Debug.Log("Dodging while");
               nextAttackSpotCenter = GetDodgeCenter();
               collidePlayer = Physics2D.Raycast(nextAttackSpotCenter, Vector2.down, 0.1f, playerMask).collider;
            }

            transform.position = Vector2.MoveTowards(transform.position,
               nextAttackSpotCenter, speed * Time.deltaTime);
        }else if (CheckReached(nextAttackSpotCenter)) {
            isDodging = false;
            canAttack = true;
        }

        
    }

    [SerializeField] private LayerMask playerMask;
    private Vector2 GetDodgeCenter() {
        Vector2 randomSpot = new Vector2(transform.position.x + Random.Range(-15f, 15f), transform.position.y + Random.Range(-8f, 8f));
        bool collidePlayer = Physics2D.Raycast(randomSpot, Vector2.down, 0.1f, playerMask).collider;
        while (randomSpot.y >= yRange.y || randomSpot.y <= yRange.x || collidePlayer ) {
            Debug.Log("get dodge player while");
            randomSpot = new Vector2(transform.position.x + Random.Range(-15f, 15f), transform.position.y + Random.Range(-8f, 8f));
            collidePlayer = Physics2D.Raycast(randomSpot, Vector2.down, 0.5f, playerMask).collider;
        }

        return randomSpot;
    }

    private Vector2 DecideDistance(Vector2 currentSpot)
    {
        Vector2 randomSpot = new Vector2(Random.Range(min_x, max_x), Random.Range(yRange.x, yRange.y));
        while (randomSpot.y >= yRange.y || randomSpot.y <= yRange.x)
        {
            Debug.Log("decide distance");
            randomSpot = new Vector2(Random.Range(min_x, max_x), Random.Range(yRange.x, yRange.y));
        }

        return randomSpot;
    }

    protected virtual void Chase()
    {
        float distanceToTargetX = 0;
        float distanceToTargetY = 0;

        Vector2 targetPos = new Vector2(L11Player.Singleton.transform.position.x,
            L11Player.Singleton.transform.position.y);
        distanceToTargetX = Mathf.Abs(targetPos.x - transform.position.x);
        distanceToTargetY = Mathf.Abs(targetPos.y - transform.position.y);

        if (distanceToTargetX >= 5 || distanceToTargetY>=1f) {
            //Debug.Log(distanceToTargetX);
            if (distanceToTargetX <= 5 && distanceToTargetY >= 0.5f && distanceToTargetY<=5) {
                hitPlayerTimer = 2;
                isChasing = false;
                isDodging = true;
                nextAttackSpotCenter = GetDodgeCenter();
                Dodging();
                return;
                
            }
            transform.position = Vector2.MoveTowards
                (transform.position, targetPos, speed * Time.deltaTime);
        }
        else {
            
            //hit player
            hitPlayerTimer -= Time.deltaTime;
            if (hitPlayerTimer <= 0) {
                hitPlayerTimer = 1.5f;
                
                animator.SetTrigger("Fight");
                Timer.Singleton.AddDelayTask(1f, () => {
                    isChasing = false;
                });

                Timer.Singleton.AddDelayTask(0.15f, () => {
                    if (fightPlayerCheck.Triggered)
                    {
                        if (!isDie)
                        {
                            TypeEventSystem.SendGlobalEvent<OnPlayerFail>();
                            TypeEventSystem.SendGlobalEvent<OnPlayerHitByEnemy>();
                            Debug.Log("OnPlayerHit");
                        }
                    }
                   
                });


                
                
            }
        }
    }

    private void Update()
    {
       
        CheckMouseHover();
        ChangeSortingOrder();
        hurtTimer -= Time.deltaTime;

        if (hurtTimer <= 0 && isDie) {
           // spriteRenderer.color = Color.white;
            DOTween.To(() => spriteRenderer.color, value => spriteRenderer.color = value,
                    new Color(1,1 , 1, 0), 1f)
                .OnComplete(() => Destroy(this.gameObject));
        }

        if (!isDie) {
            if (chaseTrigger2Dcheck.Triggered && !attackTrigger2DCheck.Triggered)
            {
                //chase player
                if (!isDodging)
                {
                    Chase();
                }
                else
                {
                    Dodging();
                }
            }
            else if (attackTrigger2DCheck.Triggered && chaseTrigger2Dcheck.Triggered)
            {
                //move away (50%), chase (50)
                if (!isDodging && !isChasing)
                {
                    int ran = Random.Range(0, 2);
                    target = L11Player.Singleton.gameObject;
                    if (ran == 0)
                    {

                        isDodging = true;
                        nextAttackSpotCenter = GetDodgeCenter();
                    }
                    else
                    {
                        isChasing = true;

                    }
                }
                else
                {
                    if (isDodging)
                    {

                        Dodging();
                    }

                    if (isChasing)
                    {

                        Chase();
                    }

                }
            }
            else if (!attackTrigger2DCheck.Triggered && !chaseTrigger2Dcheck.Triggered)
            {
                //patrol
                Patrolling();
            }
        }

        
        transform.localScale = spriteScale;
    }

    private void ChangeSortingOrder() {
        if (transform.position.y > L11Player.Singleton.transform.position.y) {
            spriteRenderer.sortingOrder = 0;
        }
        if (transform.position.y < L11Player.Singleton.transform.position.y)
        {
            spriteRenderer.sortingOrder =2;
        }
        
    }

    private void CheckMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider)
        {
            if (hit.collider is CircleCollider2D && hit.collider.gameObject == this.gameObject)
            {
                return;
            }
        }

     
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

   

   
    private float hurtTimer = 0.3f;
    public void OnAttacked(float damage)
    {
        
       
        
    }

}
 


