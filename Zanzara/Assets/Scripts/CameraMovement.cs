using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // L'oggetto target intorno a cui la camera ruoterà
    public float distance = 10.0f; // Distanza dal target
    public float autoRotateSpeed = 20.0f; // Velocità di rotazione automatica sull'asse X
    public float ySpeed = 2.0f; // Velocità di spostamento sull'asse Y
    public float yMinLimit = -20f; // Limite minimo di rotazione sull'asse Y
    public float yMaxLimit = 80f; // Limite massimo di rotazione sull'asse Y

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        if (target)
        {
            x += autoRotateSpeed * Time.deltaTime; // Rotazione automatica sull'asse X

            // Controlla se il player collide con la parte superiore o inferiore dello schermo
            if (PlayerCollidesWithTopScreen())
            {
                y += ySpeed * Time.deltaTime; // Alza la camera
            }
            else if (PlayerCollidesWithBottomScreen())
            {
                y -= ySpeed * Time.deltaTime; // Abbassa la camera
            }

            // Clampa la rotazione sull'asse Y
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    bool PlayerCollidesWithTopScreen()
    {
        // Implementa la logica per verificare se il player collide con la parte superiore dello schermo
        // Puoi usare un raycast o altre tecniche per determinare la collisione
        return false; // Placeholder
    }

    bool PlayerCollidesWithBottomScreen()
    {
        // Implementa la logica per verificare se il player collide con la parte inferiore dello schermo
        // Puoi usare un raycast o altre tecniche per determinare la collisione
        return false; // Placeholder
    }
}
