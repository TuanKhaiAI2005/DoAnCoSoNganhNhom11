using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemySpawnData> enemies;
        public float spawnInterval;
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public int count;
    }

    public List<Wave> waves;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        if (currentWaveIndex < waves.Count)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            Wave currentWave = waves[currentWaveIndex];
            StartCoroutine(SpawnWave(currentWave));
        }
        else
        {
            CheckEndGame();
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (var enemyData in wave.enemies)
        {
            if (enemyData.enemyPrefab == null)
            {
                Debug.LogError("Enemy Prefab bị mất hoặc đã bị hủy! Kiểm tra lại dữ liệu của wave.");
                continue;
            }

            for (int i = 0; i < enemyData.count; i++)
            {
                GameObject newEnemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);
                if (newEnemy == null)
                {
                    Debug.LogError("Không thể spawn enemy, có thể prefab bị lỗi!");
                    continue;
                }

                enemiesAlive++;

                // 👉 Gán waypoints cho enemy
                EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.SetWaypoints(waypoints);
                    enemyMovement.OnReachEnd += EnemyReachedEnd;
                }
                else
                {
                    Debug.LogError("Enemy không có script EnemyMovement!");
                }

                EnemyStats enemyScript = newEnemy.GetComponent<EnemyStats>();
                if (enemyScript != null)
                {
                    enemyScript.OnDeath += EnemyDied;
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }
        
        // 🛠 Cập nhật wave index sau khi spawn xong
        currentWaveIndex++;
    }

    void EnemyDied()
    {
        enemiesAlive--;
        CheckEndGame();
    }

    void EnemyReachedEnd()
    {
        enemiesAlive--;
        CheckEndGame();
    }

    void CheckEndGame()
    {
        Debug.Log($"📌 CheckEndGame: enemiesAlive = {enemiesAlive}, currentWaveIndex = {currentWaveIndex}, totalWaves = {waves.Count}");

        if (enemiesAlive <= 0)
        {
            if (currentWaveIndex < waves.Count) 
            {
                Debug.Log($"🔄 Wave {currentWaveIndex} kết thúc! Bắt đầu wave {currentWaveIndex + 1}");
                StartCoroutine(StartNextWave());
            }
            else 
            {
                Debug.Log("✅ Tất cả quái đã chết hoặc đến đích, chuyển scene!");

                string currentScene = SceneManager.GetActiveScene().name;

                if (currentScene == "MainGame")
                {
                    SceneManager.LoadScene("NextLevel"); // Nếu đang ở Map 1
                }
                else if (currentScene == "MainGame2")
                {
                    SceneManager.LoadScene("EndGame"); // Nếu đang ở Map 2
                }
            }
        }
    }
}
    