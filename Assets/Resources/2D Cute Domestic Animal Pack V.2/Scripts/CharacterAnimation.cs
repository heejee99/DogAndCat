﻿using System;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    protected bool IsDead;
    protected Animator Animator;

    public Acting Acting { get; protected set; }

    protected void Start()
    {
        Animator = GetComponent<Animator>();
        SetAnimation(Acting.Idle);
    }

    public void Eat() => SetAnimation(Acting.Eat);
    public void Eat2() => SetAnimation(Acting.Eat2);
    public void Idle() => SetAnimation(Acting.Idle);
    public void Idle2() => SetAnimation(Acting.Idle2);
    public void Jump() => SetAnimation(Acting.Jump);
    public void Jump2() => SetAnimation(Acting.Jump2);
    public void Sleep() => SetAnimation(Acting.Sleep);
    public void Sleep2() => SetAnimation(Acting.Sleep2);
    public void Walk() => SetAnimation(Acting.Walk);
    public void Walk2() => SetAnimation(Acting.Walk2);


    public void PlayAnimationComplete()
    {
        SetAnimation(Acting.Idle);
    }

    protected abstract void SetAnimation(Acting acting);
}
