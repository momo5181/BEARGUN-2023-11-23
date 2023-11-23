using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
  public override void Enter()
  {
    enemy.Agent.SetDestination(enemy.LastKnowPos);
  }

  public override void Perform()
  {
    if(enemy.CanSeePlayer())
    {
      stateMachine.ChangeState(new AttackState());   
    }
    else
    {
    searchTimer+=Time.deltaTime;
      if(searchTimer>5)
          {
            stateMachine.ChangeState(new PatrolState());
          }
    }
      

        if(enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
          searchTimer+=Time.deltaTime;
          moveTimer+=Time.deltaTime;
           
            if(moveTimer>Random.Range(3,5))// 如果移動計時器超過了3到7的隨機範圍
          {        
            enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10 )); //當進入攻擊模式，bot會開始走位
            moveTimer=0;// 增加移動計時器
            
          }  
          if(searchTimer>5)
          {
            stateMachine.ChangeState(new PatrolState());
          }
        }
    

  }

  public override void Exit()
  {
    
  }

}
