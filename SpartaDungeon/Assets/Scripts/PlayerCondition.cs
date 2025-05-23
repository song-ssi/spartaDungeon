using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health;}}
    Condition hunger { get {return uiCondition.hunger;}}
    Condition stamina { get {return uiCondition.stamina;}}
    Condition jumpgauge { get {return uiCondition.jumpgauge;}}

    public float noHungerHealthDecay;
    public event Action onTakeDamage;

    void Start()
    {
    }

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        jumpgauge.Add(jumpgauge.passiveValue * Time.deltaTime);

        if(hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if(health.curValue <= 0f)
        {
            Die();
        }
    }
    

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // 점프게이지 감소
    public void MinusJumpGauge(float amount)
    {
        jumpgauge.Subtract(amount);
    }

    // 점프가 가능한지 (점프게이지 0일시 점프 못함)
    public bool possiblejump(float amount)
    {
        if(jumpgauge.PossibleJump(amount))
            return true;
        else
            return false;
    }


    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }



}
