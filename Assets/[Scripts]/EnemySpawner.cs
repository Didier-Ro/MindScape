using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public Vector2[] VectorsToSpawn;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < 4; i++)
        {
            GameObject enemy = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.EnemyChase, VectorsToSpawn[i], new Vector3(0, 0, 0));
            enemy.GetComponent<Enemy>().AssignTarget(target);
        }
        
    }
}