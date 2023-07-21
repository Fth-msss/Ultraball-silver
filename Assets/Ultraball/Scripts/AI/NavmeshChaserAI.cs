using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshChaserAI : NavmeshEnemy
{
    //variables
    [SerializeField]
    public List<GameObject> patrolpoints;
    public GameObject playerobject;
    //states
    public NavEnemyBaseState currentState;
    public NavEnemyChaseState m_ChaseState = new();
    public NavEnemyIdleState m_IdleState = new();
    

    private void Start()
    {
        Agent= GetComponent<NavMeshAgent>();
        currentState = m_IdleState;
        currentState.EnterState(this);


    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(NavEnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    private void OnCollisionEnter(Collision collision)
    {


       
        currentState.OnCollisionEnter(this,collision);

       
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player")) {  playerobject = collider.gameObject; }
        currentState.OnTriggerEnter(this,collider);
    }
}


public abstract class NavEnemyBaseState 
{
   public abstract void EnterState(NavmeshChaserAI main);

   public abstract void UpdateState(NavmeshChaserAI main);

   public abstract void OnCollisionEnter(NavmeshChaserAI main,Collision collision);

   public abstract void OnTriggerEnter(NavmeshChaserAI main,Collider collider); 
   
}

public class NavEnemyIdleState :NavEnemyBaseState
{
    NavmeshChaserAI m_Chase;
    List<GameObject> patrollist;


    //variables
    float patrolTimer = 1;
    int patrolcounter = 0;
    float timertime = 1;
    void Patrol() 
    {
      
        if (m_Chase.Agent.velocity.sqrMagnitude <= 0) 
        {
            
            //linear patrol
            if (patrolcounter >= patrollist.Count) { patrolcounter = 0;}
            m_Chase.Agent.SetDestination(patrollist[patrolcounter].transform.position);
            patrolcounter++;
        }
       
    }

    void timer() 
    {
            
            timertime -= Time.deltaTime;
            if (timertime <= -1)
            {
            timertime = patrolTimer;
            Patrol();
            }
    }

    public override void EnterState(NavmeshChaserAI main)
    {
        //mager.SwitchState(mager.m_IdleState);
        m_Chase = main;
        patrollist = main.patrolpoints;
    }

    public override void UpdateState(NavmeshChaserAI main)
    {
        timer();
    }

    public override void OnCollisionEnter(NavmeshChaserAI main, Collision collision)
    {
        //same as chase
    }

    public override void OnTriggerEnter(NavmeshChaserAI main, Collider collider)
    {
        if (collider.CompareTag("Player")) { Debug.Log("player dedected"); main.SwitchState(main.m_ChaseState); }

        


    }
}

public class NavEnemyChaseState :NavEnemyBaseState
{
    GameObject target;

    void Chase(NavmeshChaserAI main) 
    {
        main.Agent.SetDestination(target.transform.position);
    }

    public override void EnterState(NavmeshChaserAI main) 
    {
    //no brakes on this pain train
    main.Agent.autoBraking = false;
    target = main.playerobject;

       

    }

   

    public override void UpdateState(NavmeshChaserAI main) 
    {
    Chase(main);
    }

    public override void OnCollisionEnter(NavmeshChaserAI main, Collision collision)
    {

        if (collision.gameObject.CompareTag("Player")) 
        {
            Vector3 force = new Vector3(0,100,0) + main.Agent.transform.forward*100000;
            collision.gameObject.GetComponent<PlayerObject>().AddExternalForce(force);
            Debug.Log("player is on the way to brazil");

        }


      
        
        
    }

    public override void OnTriggerEnter(NavmeshChaserAI main, Collider collider)
    {
        //do nothing
    }
}