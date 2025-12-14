using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Kéo Prefabs vật cản vào đây (tạo danh sách)
    public GameObject[] stumbleObstacles; // Vật cản gây vấp té
    public GameObject[] fatalObstacles;   // Vật cản gây thua (Game Over)

    // Lựa chọn 3 vị trí để spawn (trái, giữa, phải)
    private string[] spawnPoints = { "SpawnPoint_Left", "SpawnPoint_Center", "SpawnPoint_Right" };

    public void SpawnObstaclesRandomly()
    {
        // Tỷ lệ quyết định có sinh vật cản hay không
        if (Random.value > 0.7f) // 70% cơ hội đoạn đường này có vật cản
        {
            // Chọn ngẫu nhiên một vị trí trong 3 lane
            string selectedPointName = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Transform spawnPoint = transform.Find(selectedPointName);

            if (spawnPoint == null)
            {
                Debug.LogError("Spawn Point không tìm thấy: " + selectedPointName);
                return;
            }

            // --- Logic Sinh Ngẫu nhiên ---

            // 1. Chọn ngẫu nhiên loại vật cản (Stumble hay Fatal)
            GameObject obstacleToSpawn;

            if (Random.value > 0.8f) // 20% cơ hội sinh vật cản Fatal (khó hơn)
            {
                obstacleToSpawn = fatalObstacles[Random.Range(0, fatalObstacles.Length)];
            }
            else // 80% cơ hội sinh vật cản Stumble
            {
                obstacleToSpawn = stumbleObstacles[Random.Range(0, stumbleObstacles.Length)];
            }

            // 2. Sinh vật cản tại vị trí đã chọn
            Instantiate(obstacleToSpawn, spawnPoint.position, spawnPoint.rotation, transform);
        }
    }
}