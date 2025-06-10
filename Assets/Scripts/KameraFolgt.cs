using UnityEngine;

public class KameraFolgt : MonoBehaviour
{
    public Transform ziel; // Die Kiwi
    public Vector3 offset = new Vector3(0f, 6f, -4f); //von oben leicht schräg
    public float folgegeschwindigkeit = 5f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (ziel != null)
        {
            Vector3 zielposition = ziel.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, zielposition, ref velocity, 0.15f);
        }

        // Kamera immer in die selbe Richtung blicken lassen
        transform.rotation = Quaternion.Euler(60f, 0f, 0f); // leicht schräge Draufsicht
    }
}