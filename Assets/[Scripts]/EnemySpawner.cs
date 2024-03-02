using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private void Start()
    {
        GameObject enemy = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.EnemyChase, new Vector2(1, 1), new Vector3(0, 0, 0));
        enemy.GetComponent<Enemy>().AssignTarget(target);
        GameObject enemy2 =  PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.EnemyChase, new Vector2(5, 1), new Vector3(0, 0, 0));
        enemy2.GetComponent<Enemy>().AssignTarget(target);
    }
}