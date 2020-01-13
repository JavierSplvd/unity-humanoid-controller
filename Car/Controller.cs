using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {
	
	public float maxSpeed = 100.0f;
	public float balanceWeight = 10.0f;
    public float barrerRollForce = 50.0f;
    

    public float RotationSpeed = 100.0f;

    public float angularVelocityX = 0;
    public float maxAngularVelocityX = 0.5f;

    public float angularVelocityY = 0;
    public float maxAngularVelocityY = 0.5f;

    public Vector3 forwardDebug;
    public Vector3 velocityDebug;
    public Vector3 angularVelocityDebug;

    private Rigidbody _rigidBody;
    private float maxVertical;
    private float maxHorizontal;

	// Use this for initialization
	void Start () 
	{
        _rigidBody = GetComponent<Rigidbody>();
        maxVertical = Screen.height;
        maxHorizontal = Screen.width;
    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void FixedUpdate()
	{
		UpdateFunction();
	}
	
	void UpdateFunction()
    {
        BarrerRoll();
        Translation();
        forwardDebug = transform.forward;
        angularVelocityX = _rigidBody.angularVelocity.x;
        Vector3 direction = transform.forward;

        bool hasInput = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        // if(Input.GetAxis("Vertical") != 0 && Abs(angularVelocityX) < maxAngularVelocityX){
            // Debug.Log("Torque X");
            //_rigidBody.AddTorque(transform.right * Input.GetAxis("Vertical") * 5);
        // }

        angularVelocityY = _rigidBody.angularVelocity.y;
        if(Input.GetAxis("Horizontal") != 0 && Abs(angularVelocityY) < maxAngularVelocityY){
            Debug.Log("Torque Y");
            _rigidBody.AddTorque(transform.up * Input.GetAxis("Horizontal") * 5);
            Vector3 tiltForce = -1 * Input.GetAxis("Horizontal") * balanceWeight * 0.3f * transform.right;
            _rigidBody.AddForceAtPosition(tiltForce, getBalancePosition());
            // Debug.DrawRay(balancePoint, tiltForce * 10, Color.green);
        } 
        
        if(true) {
            Debug.Log("Balancing");
            _rigidBody.AddForceAtPosition(- Vector3.up * balanceWeight, getBalancePosition());
            _rigidBody.AddForceAtPosition(Vector3.up * balanceWeight, transform.position);

            //_rigidBody.angularVelocity = 0.1f * _rigidBody.angularVelocity;
        }
        
        angularVelocityDebug = _rigidBody.angularVelocity;

    }

    Vector3 getBalancePosition() {
        Vector3 balancePoint = transform.position - transform.up * 5f;
        return balancePoint;
    }

    void Translation() {
        Vector3 direction = transform.forward;

        if(Input.GetAxis("Jump") != 0 && Abs(_rigidBody.velocity.magnitude) < maxSpeed) {
            _rigidBody.AddForceAtPosition(Input.GetAxis("Jump") * transform.up * maxSpeed, transform.position);
            Disturb();
        }

        if(Input.GetAxis("Vertical") != 0 && Abs(_rigidBody.velocity.magnitude) < maxSpeed){
            _rigidBody.AddForceAtPosition(Input.GetAxis("Vertical") * direction.normalized * maxSpeed, transform.position);
        }
        velocityDebug = _rigidBody.velocity;
    }

    void BarrerRoll() {
        if(Input.GetButton("Fire2")) {

            Debug.DrawRay(transform.position, - transform.right, Color.green);
            Debug.DrawRay(getBalancePosition(), transform.right, Color.green);

            _rigidBody.AddForceAtPosition(- transform.right * barrerRollForce, transform.position);
            _rigidBody.AddForceAtPosition(transform.right * barrerRollForce, getBalancePosition());
        }
    }

    void Disturb(){
        Vector3 randomPos = new Vector3(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y + Random.Range(-1.0f, 1.0f), transform.position.z);
        _rigidBody.AddForceAtPosition(transform.up * maxSpeed * 0.2f, randomPos);
    }

    private float Abs(float value){
        return Mathf.Abs(value);
    }
}