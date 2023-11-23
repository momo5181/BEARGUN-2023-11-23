using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

   public List<Transform> patrolWayPoints;
   public GameObject debugsphere; //代替玩家最位子
   public GameObject Player{get => player;} //取得玩家信息
   private Vector3 lastKnowPos;  //取得玩家最後座標
   public Vector3 LastKnowPos{get=>lastKnowPos;set =>lastKnowPos=value;} 
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public NavMeshAgent Agent{get => agent;}
    [SerializeField]
    private string currentState;
    //public Path path; 
    private GameObject player;
    [Header("槍")]
    public Transform gunBarrel;
    [Range(0.1f,10f)]
    public float fireRate;
    [Header("視野")]
    public float sightDistance =20f;
    public float field0fView = 85f; 
    public float eyeHeight;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine=GetComponent<StateMachine>();
        agent=GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player=GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState=stateMachine.activeState.ToString();
        debugsphere.transform.position=lastKnowPos;
    }
  public bool CanSeePlayer()
  {
    if(player!=null)
    {
       if(Vector3.Distance(transform.position,player.transform.position)<sightDistance)
       {
        Vector3 targetDirection=player.transform.position-transform.position-(Vector3.up*eyeHeight);
        float angleToPlayer= Vector3.Angle(targetDirection,transform.forward);
        if(angleToPlayer>=-field0fView && angleToPlayer<=field0fView)
        {
            Ray ray=new Ray(transform.position+(Vector3.up*eyeHeight),targetDirection);
            RaycastHit hitInfo=new RaycastHit();
        if(Physics.Raycast(ray,out hitInfo,sightDistance))
         {
          if(hitInfo.transform.gameObject==player)
          {
            Debug.DrawRay(ray.origin,ray.direction*sightDistance);
            return true;
          }
          
         }    
        }
       }
    }
    return false;
 }
}
