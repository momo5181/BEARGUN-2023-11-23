using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
public int waypointIndex = 0; // 为 waypointIndex 添加初始值

 public override void Enter()
 {

 }
 public override void Exit()
 {

 }
 public override void Perform()
 {
 PatrolCycle();
 if(enemy.CanSeePlayer())
 {
stateMachine.ChangeState(new AttackState());
 }
 }
 public void PatrolCycle()
 { 
    if(enemy.Agent.remainingDistance<0.00001f)
    {
       if(waypointIndex<enemy.patrolWayPoints.Count-1)
         waypointIndex++;
        else
        waypointIndex=0;
       enemy.Agent.SetDestination(enemy.patrolWayPoints[waypointIndex].position);
       
    }
 }


}
