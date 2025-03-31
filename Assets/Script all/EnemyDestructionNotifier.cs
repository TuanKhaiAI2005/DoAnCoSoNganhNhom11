using UnityEngine;

public class EnemyDestructionNotifier : MonoBehaviour
{
    public EnemySpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed(gameObject);
        }
    }
}
