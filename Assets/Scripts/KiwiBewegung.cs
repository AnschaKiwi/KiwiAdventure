using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KiwiBewegung : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    [Tooltip("Bewegungsgeschwindigkeit der Kiwi")]
    public float bewegungsgeschwindigkeit = 3f;

    private Rigidbody rb;
    [SerializeField] private float sprungKraft = 3f;
    private bool willSpringen = false;
    private Vector2 bewegungseingabe;
    private PlayerControls steuerung;

    private void Awake()
    {
        steuerung = new PlayerControls();
        steuerung.Player.Move.performed += ctx => bewegungseingabe = ctx.ReadValue<Vector2>();
        steuerung.Player.Move.canceled += ctx => bewegungseingabe = Vector2.zero;
        steuerung.Player.Jump.performed += ctx => willSpringen = true; // Korrekt registrieren!
    }

    private void OnEnable()
    {
        steuerung.Enable();
    }
    private void OnDisable()
    {
        steuerung.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody-Komponente nicht gefunden!");
        }
        // Optional: Rotation einfrieren, damit die Kiwi nicht umkippt
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        Vector3 richtung = new Vector3(bewegungseingabe.x, 0f, bewegungseingabe.y);
        Vector3 zielPosition = rb.position + richtung * bewegungsgeschwindigkeit * Time.fixedDeltaTime;
        rb.MovePosition(zielPosition);

        if (willSpringen)
        {
            rb.AddForce(Vector3.up * sprungKraft, ForceMode.Impulse);
            willSpringen = false;
        }
    }

    private void Update()
    {
        bool istInBewegung = bewegungseingabe.sqrMagnitude > 0.01f;
        GetComponent<Animator>().SetBool("bewegtSich", istInBewegung);

        if (istInBewegung)
        {
            Vector3 richtung = new Vector3(bewegungseingabe.x, 0f, bewegungseingabe.y);
            // Quaternion zielRotation = Quaternion.LookRotation(richtung, Vector3.up);
            Quaternion zielRotation = Quaternion.LookRotation(richtung, Vector3.up) * Quaternion.Euler(270f, 180f, 0f);
            Quaternion neueRotation = Quaternion.Slerp(rb.rotation, zielRotation, 10f * Time.deltaTime);
            rb.MoveRotation(neueRotation);
        }
        // else: Keine Rotation zurücksetzen!
    }
}
