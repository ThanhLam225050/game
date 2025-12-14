using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    // Biến để gán nhân vật cần theo dõi (Target)
    public Transform target;

    // Khoảng cách camera muốn giữ so với Target
    public float distance = 4.0f;
    public float height = 2.0f; // Độ cao so với Target

    // Tốc độ di chuyển và làm mịn vị trí camera
    public float smoothSpeed = 5.0f;

    void LateUpdate()
    {
        //// Kiểm tra xem đã gán Target chưa
        //if (target == null)
        //{
        //    Debug.LogError("Camera Target chưa được gán trong Inspector.");
        //    return;
        //}
            
        //// 1. Tính toán vị trí mong muốn của camera (Sau lưng và cao hơn Target)

        //// Vì nhân vật đang chạy tại chỗ (Z=0) và không xoay, chúng ta dùng Vector3.forward của World
        //// để đặt camera lùi lại (Vector3.back) so với vị trí hiện tại.
        //// HOẶC, nếu bạn đã thiết lập CameraPivot là con của nhân vật, Target.forward chính là hướng của nhân vật.
        //Vector3 desiredPosition = target.position - target.forward * distance;
        //desiredPosition.y = target.position.y + height;

        //// 2. Làm mịn vị trí (Smooth Movement)

        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothedPosition;

        // 3. Xoay camera để luôn nhìn vào Target

        // CẢNH BÁO: Lệnh LookAt() gây ra xung đột góc xoay với PlayerController.cs, 
        // khiến nhân vật bị lộn vòng khi chuyển làn.
        // Nếu bạn đã đặt camera là con của Pivot và Pivot là con của Target, 
        // Camera sẽ tự động đi theo và giữ góc xoay tương đối.
        // KHÔNG CẦN DÙNG LỆNH LOOKAT Ở ĐÂY.

        //transform.LookAt(target.position); // Lệnh này đã bị vô hiệu hóa
    }
}