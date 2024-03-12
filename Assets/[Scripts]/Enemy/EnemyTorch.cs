using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTorch : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private List<Torch> targets = new List<Torch>();
    [SerializeField] private int index = 0;

    private bool isInteracting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; ;
        agent.updateUpAxis = false;

        targets.AddRange(FindObjectsOfType<Torch>());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targets[index].IsLightOn())
        {
            agent.SetDestination(targets[index].transform.position);
        }
        else if (!targets[index].IsLightOn())
        {
            index++;
        }

        if (index >= targets.Count)
        {
            index = 0;
            agent.SetDestination(playerTarget.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ("Torch"))
        {
            collision.gameObject.GetComponent<EIstepable>().EActivate();
            isInteracting= true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Torch"))
        {
            collision.gameObject.GetComponent<EIstepable>().EDeactivate();
            isInteracting= false;
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == ("Torch"))
        {
            if (!collision.gameObject.GetComponent<Torch>().IsLightOn() && isInteracting)
            {
                isInteracting = false;
                index++;
            }
        }
    }*/
}
