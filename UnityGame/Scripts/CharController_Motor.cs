/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {

	public float speed = 10.0f;
	public float sensitivity = 30.0f;
	public float WaterHeight = 15.5f;
	CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;
	float gravity = -9.8f;


	void Start(){
		//LockCursor ();
		character = GetComponent<CharacterController> ();
		if (Application.isEditor) {
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
		}
	}


	void CheckForWaterHeight(){
		if (transform.position.y < WaterHeight) {
			gravity = 0f;			
		} else {
			gravity = -9.8f;
		}
	}



	void Update(){
		moveFB = Input.GetAxis ("Horizontal") * speed;
		moveLR = Input.GetAxis ("Vertical") * speed;

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		//rotX = Input.GetKey (KeyCode.Joystick1Button4);
		//rotY = Input.GetKey (KeyCode.Joystick1Button5);

		CheckForWaterHeight ();


		Vector3 movement = new Vector3 (moveFB, gravity, moveLR);



		if (webGLRightClickRotation) {
			if (Input.GetKey (KeyCode.Mouse0)) {
				CameraRotation (cam, rotX, rotY);
			}
		} else if (!webGLRightClickRotation) {
			CameraRotation (cam, rotX, rotY);
		}

		movement = transform.rotation * movement;
		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(GameObject cam, float rotX, float rotY){		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}




} */


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;
    CharacterController character;
    public GameObject cam;
    float moveFB, moveLR;
    float rotX, rotY;
    public bool webGLRightClickRotation = true;
    float gravity = -9.8f;

    void Start()
    {
        // Enable the gyroscope
        Input.gyro.enabled = true;

        character = GetComponent<CharacterController>();
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;
        }
        else
        {
            gravity = -9.8f;
        }
    }

    void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * speed;
        moveLR = Input.GetAxis("Vertical") * speed;

        // Get gyroscope rotation rate
        rotX = Input.gyro.rotationRateUnbiased.y * sensitivity;
        rotY = Input.gyro.rotationRateUnbiased.x * sensitivity;

        CheckForWaterHeight();

        Vector3 movement = new Vector3(moveFB, gravity, moveLR);

        // Use gyroscope for camera rotation
        CameraRotation(cam, rotX, rotY);

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        // Rotate player horizontally based on gyroscope input
        transform.Rotate(0, rotX * Time.deltaTime, 0);

        // Rotate camera vertically based on gyroscope input
        float newCamRotationX = Mathf.Clamp(cam.transform.localEulerAngles.x - rotY * Time.deltaTime, -90f, 90f);
        cam.transform.localEulerAngles = new Vector3(newCamRotationX, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
    }
}*/


using UnityEngine;
using UnityEngine.InputSystem;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;
    CharacterController character;
    public GameObject cam;
    float moveFB, moveLR;
    public bool webGLRightClickRotation = true;
    float gravity = -9.8f;


    public InputActionReference move;
    public InputActionReference fire;
    private Vector2 _moveDirection;

    private Quaternion initialGyroRotation;
    private Quaternion initialPlayerRotation;

    void Start()
    {
        // Enable the gyroscope
        Input.gyro.enabled = true;

        character = GetComponent<CharacterController>();
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }

        // Set initial gyroscope and player rotation
        initialGyroRotation = Input.gyro.attitude;
        initialPlayerRotation = transform.rotation;
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;
        }
        else
        {
            gravity = -9.8f;
        }
    }

    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();

        CheckForWaterHeight();

        Vector3 movement = new Vector3(moveFB, gravity, moveLR);

        // Reset camera orientation on gamepad key press
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) // Replace JoystickButton1 with your gamepad key
        {
            ResetCameraOrientation();
        }

        // Use gyroscope for camera rotation
        GyroscopeBasedRotation();

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);
    }

    void GyroscopeBasedRotation()
    {
        // Get the current gyroscope attitude
        Quaternion currentGyroRotation = Input.gyro.attitude;

        // Convert from right-handed to left-handed coordinate system
        Quaternion gyroRotation = ConvertGyroRotation(currentGyroRotation);

        // Calculate the rotation difference relative to the initial orientation
        Quaternion rotationDifference = Quaternion.Inverse(initialGyroRotation) * gyroRotation;

        // Apply rotation to the player and camera
        transform.rotation = initialPlayerRotation * Quaternion.Euler(0, -rotationDifference.eulerAngles.y, 0);

        float pitch = Mathf.Clamp(-rotationDifference.eulerAngles.x, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void ResetCameraOrientation()
    {
        // Reset gyroscope initial orientation
        initialGyroRotation = Input.gyro.attitude;

        // Reset player and camera rotation
        transform.rotation = initialPlayerRotation;
        cam.transform.localRotation = Quaternion.identity;
    }

    Quaternion ConvertGyroRotation(Quaternion gyro)
    {
        // Adjust for Unity's coordinate system
        return new Quaternion(-gyro.x, -gyro.z, gyro.y, gyro.w);
    }
}
