using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Đừng quên import thư viện này

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // Prefab quái
    public Transform[] waypoints;        // Danh sách các waypoint
    public float spawnInterval = 10f;    // Thời gian nghỉ giữa các wave
    public int baseEnemiesPerWave = 3;   // Số lượng quái của wave đầu tiên
    public int enemyIncrementPerWave = 2;// Số quái tăng thêm mỗi wave
    public int totalWaves = 10;          // Tổng số wave

    private int currentWave = 0;
    private List<GameObject> currentWaveEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            int enemiesThisWave = baseEnemiesPerWave + currentWave * enemyIncrementPerWave;
            Debug.Log("Wave " + (currentWave + 1) + " spawn " + enemiesThisWave + " quái.");
            
            // Spawn enemy cho wave hiện tại
            for (int i = 0; i < enemiesThisWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f); // Giữ khoảng cách giữa lần spawn
            }
            
            // Chờ cho đến khi danh sách enemy của wave hiện tại rỗng (tức là tất cả quái đã bị tiêu diệt)
            yield return new WaitUntil(() => currentWaveEnemies.Count == 0);
            
            currentWave++;
            yield return new WaitForSeconds(spawnInterval);
        }
        
        Debug.Log("Tất cả wave đã hoàn thành!");
        // Thêm một chút delay nếu cần, sau đó chuyển scene
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("NextLevel"); 
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("⚠️ Enemy Prefab chưa được gán trong Inspector!");
            return;
        }
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("⚠️ Waypoints chưa được gán trong Inspector!");
            return;
        }
        
        // Spawn quái tại waypoint đầu tiên với một chút offset
        float offsetX = Random.Range(-0.2f, 0.2f);
        float offsetY = Random.Range(-0.2f, 0.2f);
        Vector3 spawnPos = waypoints[0].position + new Vector3(offsetX, offsetY, 0);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentWaveEnemies.Add(enemy);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetWaypoints(waypoints);
        }
        else
        {
            Debug.LogError("⚠️ EnemyPrefab không có script EnemyMovement!");
        }

        // Thêm component để thông báo khi enemy bị destroy
        EnemyDestructionNotifier notifier = enemy.AddComponent<EnemyDestructionNotifier>();
        notifier.spawner = this;
    }

    // Hàm gọi lại khi enemy bị destroy, loại bỏ enemy khỏi danh sách wave hiện tại
    public void OnEnemyDestroyed(GameObject enemy)
    {
        if (currentWaveEnemies.Contains(enemy))
        {
            currentWaveEnemies.Remove(enemy);
        }
    }
}
