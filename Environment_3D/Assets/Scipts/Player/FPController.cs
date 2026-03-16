using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    #region General variables
    [Header("Movement & Look")]
    [SerializeField] Transform CamHolder;//Ref en el inspector del objeto a rotar
    [SerializeField] float speed = 5f;
    [SerializeField] float sensitivity = 0.1f;//sensibilidad del raton

    [Header("Jump & GroundCheck")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool IsGrounded;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    #endregion

    //Variables de autorefencia
    Rigidbody rb;

    //Variables de input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Lock del cursor del raton
        Cursor.lockState = CursorLockMode.Locked;//Lockea el cursor en el centro de la pantalla
        Cursor.visible = false;//Apaga la visualizacion del cursor
    }

    // Update is called once per frame
    void Update()
    {
        //GroundCheck
        IsGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void CameraLook()
    {
        //Rotacion horizontal del personaje
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Rotacion vertical de la camara
        lookRotation += (-lookInput.y * sensitivity);
        //limita la rotacion vertical para evitar que la camara gire completamente
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        CamHolder.transform.localEulerAngles = new Vector3(lookRotation, 0, 0);
    }

    private void Movement()
    {
        //Definir los dos vectores que permiten la aceleración
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);

        //convertir la dirección al eje mundial (Local -> World)
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcular el cambio de velocidad (aceleración)
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        //Aplicación del movimiento (DIRECCIÓN + ACELERACIÓN)
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput= context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    #endregion 

}
