using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    // CÁC BIẾN QUAN TRỌNG:
    public float scrollSpeed = 8f; // Tốc độ cuộn đoạn đường (Tốc độ chạy của game)
    public GameObject roadSegmentPrefab; // Prefab đoạn đường (Road_Segment_01)
    public Transform playerTransform; // Để tính toán vị trí sinh đường

    // Biến quản lý đường đang có mặt trong Scene
    private List<GameObject> activeSegments = new List<GameObject>();
    private float spawnZ = 0.0f; // Vị trí Z để sinh đoạn đường tiếp theo
    private float segmentLength = 50.0f; // Chiều dài 1 đoạn đường (Cần đo chính xác)
    private int segmentsOnScreen = 7; // Số lượng đoạn đường hiển thị cùng lúc

    // Khởi tạo ban đầu
    void Start()
    {
        // Kiểm tra chiều dài đoạn đường thực tế của bạn:
        // Đoạn đường của bạn (Road_Segment_01) có Scale Z là 30 (image_fc88ae.jpg), 
        // nhưng nếu nó là 1 khối lập phương, chiều dài có thể là 30 * 1 = 30.
        // Tôi sẽ tạm dùng 30, bạn có thể phải điều chỉnh lại giá trị này.
        segmentLength = 30f;

        // Sinh các đoạn đường ban đầu
        for (int i = 0; i < segmentsOnScreen; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // --- 1. CUỘN TẤT CẢ ĐOẠN ĐƯỜNG VỀ PHÍA SAU ---

        // Dịch chuyển tất cả các đoạn đường đang hoạt động ngược chiều Z
        foreach (GameObject segment in activeSegments)
        {
            // Vector3.back tương đương với Vector3(0, 0, -1)
            segment.transform.position += Vector3.back * scrollSpeed * Time.deltaTime;
        }

        // --- 2. TÁI SỬ DỤNG HOẶC XÓA ĐOẠN ĐƯỜNG ĐÃ QUA ---

        // Kiểm tra đoạn đường đầu tiên
        if (activeSegments.Count > 0)
        {
            // Nếu đoạn đường đầu tiên đi qua vị trí Z của nhân vật (giả sử Z=0) một khoảng nhất định (ví dụ: segmentLength / 2)
            if (activeSegments[0].transform.position.z < playerTransform.position.z - segmentLength)
            {
                DeleteSegment();
                SpawnSegment(); // Sinh đoạn đường mới ngay sau đó
            }
        }
    }

    // Hàm sinh đoạn đường mới
    private void SpawnSegment()
    {
        GameObject newSegment = Instantiate(roadSegmentPrefab, transform.forward * spawnZ, transform.rotation);
        activeSegments.Add(newSegment);

        // Cập nhật vị trí sinh tiếp theo
        spawnZ += segmentLength;
    }

    // Hàm xóa đoạn đường cũ
    private void DeleteSegment()
    {
        Destroy(activeSegments[0]);
        activeSegments.RemoveAt(0);
    }
}