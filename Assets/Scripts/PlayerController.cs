using System;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;
	[SerializeField]
	private float thrusterForce = 1000f;

	[SerializeField]
	private float thrusterFeulBurnSpeed = 1f;
	[SerializeField]
	private float thrustFeulRegenSpeed = 0.3f;
	private float thrusterFeulAmount = 1f;

	public float GetThrusterFeulAmount()
    {
		return thrusterFeulAmount;
    }

	[Header("Spring settings")]
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

	private PlayerMotor motor;
	private ConfigurableJoint joint;


	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();

		SetJointSettings(jointSpring);
	}

	void Update()
	{
		RaycastHit _hit;
		if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f))
        {
			joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
			joint.targetPosition = new Vector3(0f, 0f, 0f);
        }
		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		//Apply movement
		motor.Move(_velocity);

		//Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		motor.RotateCamera(_cameraRotationX);

		Vector3 _thrusterForce = Vector3.zero;
        //Calcualte thruster force
        if (Input.GetButton("Jump") && thrusterFeulAmount >0f)
        {
			thrusterFeulAmount -= thrusterFeulBurnSpeed * Time.deltaTime;
			if(thrusterFeulAmount>= 0.01f)
            {
				_thrusterForce = Vector3.up * thrusterForce;
				SetJointSettings(0f);
			}
        }
        else
        {
			thrusterFeulAmount += thrustFeulRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }
		thrusterFeulAmount = Mathf.Clamp(thrusterFeulAmount, 0f, 1f);

        if (Input.GetButtonDown("Jump"))
        {
			motor.Jump();

        }
		// Apply the thruster force 
		motor.ApplyThruster(_thrusterForce);
	}
	private void SetJointSettings(float _jointSpring)
    {
		joint.yDrive = new JointDrive {
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce };
    }

}