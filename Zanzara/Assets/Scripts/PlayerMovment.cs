using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public float speed = 5.0f; // Velocità di movimento del player
    Rigidbody myBody;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ottieni l'input dell'utente
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Ottieni la direzione della MainCamera
        Transform cameraTransform = Camera.main.transform;

        // Calcola il movimento relativo alla camera
        Vector3 right = cameraTransform.right;
        Vector3 up = cameraTransform.up;

        // Normalizza i vettori per evitare che il movimento sia più veloce in diagonale
        right.Normalize();
        up.Normalize();

        // Calcola il movimento del player
        Vector3 movement = right * moveHorizontal + up * moveVertical;
        myBody.linearVelocity = movement * speed *Time.deltaTime;
    }
}
