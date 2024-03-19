using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTorch : MonoBehaviour
{
    GAME_STATE gameState;
    private bool isSuscribed = true;
    public bool canMove = true;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private List<Torch> targets = new List<Torch>();
    [SerializeField] private int index = 0;
    

    private void Start()
    {
        SubscribeToGameManagerGameState();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; ;
        agent.updateUpAxis = false;

        targets.AddRange(FindObjectsOfType<Torch>());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canMove)
        {
            StopEnemyMovement();
        }
        

        if (targets[index].IsLightOn() && canMove)
        {
            agent.SetDestination(targets[index].transform.position);
        }

        if (!targets[index].IsLightOn() && canMove)
        {
            index++;
        }

        if (index >= targets.Count && canMove)
        {
            index = 0;
            agent.SetDestination(playerTarget.transform.position);
        }
    }

    private void OnEnable()
    {
        if (!isSuscribed)
        {
            SubscribeToGameManagerGameState();
        }
    }

    private void OnDisable()
    {
        if (isSuscribed)
        {
            GameManager.GetInstance().OnGameStateChange -= OnGameStateChange;
            isSuscribed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ("Stepable"))
        {
            collision.gameObject.GetComponent<EIstepable>().EActivate();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Stepable"))
        {
            collision.gameObject.GetComponent<EIstepable>().EDeactivate();
            
        }
    }

    private void SubscribeToGameManagerGameState()
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }

    private void OnGameStateChange(GAME_STATE _newGamestate)
    {
        canMove = _newGamestate == GAME_STATE.EXPLORATION;
    }

    private void StopEnemyMovement()
    {
        agent.velocity = Vector3.zero;
        
    }

}
