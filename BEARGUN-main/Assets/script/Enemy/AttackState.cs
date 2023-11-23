using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
private float moveTimer;  //用來確定玩家暴露在雷射線內的時間計時器，當進入達到一定時間，開始攻擊
private float losePlayerTimer; //當玩家離開了bot視野的時間，超過指定時間就會回到巡邏模式
public float shotTimer;

    public override void Enter()
    {


    }

    public override void Exit()
    {

        
    }
//我來測試SSSS
    public override void Perform()
    {
      if(enemy.CanSeePlayer())
      {
        losePlayerTimer=0;  // 重置丟失玩家計時器
        moveTimer+=Time.deltaTime;// 增加移動計時器
        shotTimer+=Time.deltaTime;
        enemy.transform.LookAt(enemy.Player.transform);
        if(shotTimer>enemy.fireRate)
        {
          Shoot();
        }

        if(moveTimer>Random.Range(3,7))// 如果移動計時器超過了3到7的隨機範圍
         {
           enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5 )); //當進入攻擊模式，bot會開始走位
           moveTimer=0;// 增加移動計時器
         }  
         enemy.LastKnowPos=enemy.Player.transform.position;
    
      }
        else
         {
             losePlayerTimer+=Time.deltaTime; //增加丟失玩家計時器
             if(losePlayerTimer>2) //當離開超出8秒，回到正常巡邏模式
             {
             stateMachine.ChangeState(new SearchState()); //回到正常巡邏模式初版、次版改SEARCHSTATE
             }
         }   
    }
public void Shoot()
{
  Transform gunbarrel = enemy.gunBarrel;
  GameObject bullet=GameObject.Instantiate(Resources.Load("bullet3 1")as GameObject, gunbarrel.position,enemy.transform.rotation);
  Vector3 shootDirection=(enemy.Player.transform.position-gunbarrel.transform.position).normalized;
  bullet.GetComponent<Rigidbody>().velocity=Quaternion.AngleAxis(Random.Range(-3f,3f),Vector3.up)*shootDirection*100;
  Debug.Log("shoot!");
  shotTimer=0;
  //我是來測試GITHUB更新的
}

    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}