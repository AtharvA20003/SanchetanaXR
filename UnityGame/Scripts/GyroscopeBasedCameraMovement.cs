using UnityEngine;

public class GyroscopeBasedCameraMovement : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion initialRotation;

    void Start()
    {
        // Check if the device supports gyroscope
        gyroEnabled = SystemInfo.supportsGyroscope;

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            // Set initial rotation to correct the orientation
            initialRotation = Quaternion.Euler(90f, 0f, 0f); // Adjust as per your game setup
        }
        else
        {
            Debug.LogError("Gyroscope not supported on this device.");
        }
    }

    void Update()
    {
        if (gyroEnabled)
        {
            // Gyroscope attitude gives the device's orientation
            Quaternion deviceRotation = gyro.attitude;

            // Convert from right-handed to left-handed coordinate system
            Quaternion correctedRotation = new Quaternion(
                deviceRotation.x,
                deviceRotation.y,
                -deviceRotation.z,
                -deviceRotation.w
            );

            // Apply rotation to the player GameObject
            transform.localRotation = initialRotation * correctedRotation;
        }
    }
}
