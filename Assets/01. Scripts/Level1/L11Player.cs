using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using MikroFramework.Singletons;
using MikroFramework.Utilities;
using UnityEngine;

public enum L11PlayerState {
    Alive,
    Dead
}

public struct OnPlayerFightHit {
    public GameObject Target;
}
public class L11Player : MonoMikroSingleton<L11Player> {
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 targetVelocity;

    private Rigidbody2D rigidbody;
    [SerializeField]
    private float speedLerpSpeed = 0.1f;

    [SerializeField] private float doodleMoveFrametime = 0.1f;
    [SerializeField] private float doodleIdleFrrametime = 0.25f;

    [SerializeField] private Trigger2DCheck fightTrigger2D;

    private bool isFighting = false;

    private Animator animator;

    private Material mat;

    public L11PlayerState PlayerState = L11PlayerState.Alive;
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
        animator = GetComponent<Animator>();
    }

    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnPlayerHitByEnemy>(OnPlayerHitByEnemy)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnPlayerHitByEnemy(OnPlayerHitByEnemy e) {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        
        Timer.Singleton.AddDelayTask(0.3f, () => {
            spriteRenderer.color = Color.white;
        });
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !isFighting)
        {
            isFighting = true;
            animator.SetTrigger("Fight");

            if (fightTrigger2D.Triggered) {
                if (fightTrigger2D.Colliders.Count > 0) {
                    Timer.Singleton.AddDelayTask(0.2f, () => {
                        if (fightTrigger2D.Colliders.Count > 0 && fightTrigger2D.Colliders[0]) {
                            TypeEventSystem.SendGlobalEvent<OnPlayerFightHit>(new OnPlayerFightHit()
                            {
                                Target = fightTrigger2D.Colliders[0].gameObject
                            });
                        }
                       

                    });
                    
                }
            }
            

            Timer.Singleton.AddDelayTask(0.67f, () => {
                isFighting = false;
            });
        }
    }

    private void FixedUpdate() {
        float horizontal = 0;
        float vertical = 0;
        
        if (Input.GetKey(KeyCode.A)) {
            horizontal = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
        }

        if (Input.GetKey(KeyCode.S)) {
            vertical = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1;
        }

       

        if (horizontal != 0 || vertical != 0) {
            mat.SetFloat("_DoodleFrameTime", doodleMoveFrametime);
        }else if (horizontal == 0 || vertical == 0) {
            mat.SetFloat("_DoodleFrameTime", doodleIdleFrrametime);
        }

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        targetVelocity = direction * moveSpeed;

        rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, targetVelocity, speedLerpSpeed);

        if (rigidbody.velocity.x > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
