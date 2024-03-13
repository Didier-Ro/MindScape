using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTorch : MonoBehaviour
{
    GAME_STATE gameState;
    private bool _isSuscribed = true;
    bool _canMove = true;

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
        /*if (!_canMove)
        {
            return;
        }*/

        if (targets[index].IsLightOn())
        {
            agent.SetDestination(targets[index].transform.position);
            
        }

        if (!targets[index].IsLightOn())
        {
            index++;
        }

        if (index >= targets.Count)
        {
            index = 0;
            agent.SetDestination(playerTarget.transform.position);
        }
    }

    private void OnEnable()
    {
        if (!_isSuscribed)
        {
            SubscribeToGameManagerGameState();
        }
    }

    private void OnDisable()
    {
        if (_isSuscribed)
        {
            GameManager.GetInstance().OnGameStateChange -= OnGameStateChange;
            _isSuscribed = false;
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
        _isSuscribed = true;
    }

    private void OnGameStateChange(GAME_STATE _newGamestate)
    {
        _canMove = _newGamestate == GAME_STATE.EXPLORATION;
    }

}
