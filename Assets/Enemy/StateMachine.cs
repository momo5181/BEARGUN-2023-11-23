using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState; // 目前處於的狀態
    public PatrolState patrolState;// 巡邏狀態
    
    // Start is called before the first frame update
   public void Initialise()
   {
patrolState=new PatrolState(); // 初始化巡邏狀態
ChangeState(patrolState); // 切換到巡邏狀態
   }
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activeState!=null)// 如果目前有設置活動狀態，則執行其 Perform 
      {
        activeState.Perform();
      }  

    }
    public void ChangeState(BaseState newState)// 用於切換狀態的方法，接受新的狀態作為參數
    {


      if(activeState!=null) // 如果目前有設置活動狀態，則執行其 Exit 
      {
        activeState.Exit();
      }  

      activeState=newState; // 將目前狀態切換為新的狀態

      if(activeState!=null)   // 如果新的狀態不為空，則設置狀態機和敵人，並執行 Enter 
      {

        activeState.stateMachine=this;
        activeState.enemy=GetComponent<Enemy>();
        activeState.Enter();
      }  
      


    }
}
