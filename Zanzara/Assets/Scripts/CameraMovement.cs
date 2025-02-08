using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // L'oggetto target intorno a cui la camera ruoterà
    public Transform player; // Il player
    public GameObject BiteCam = null;
    public float distance = 10.0f; // Distanza dal target
    public float autoRotateSpeed = 20.0f; // Velocità di rotazione automatica sull'asse X
    public float ySpeed = 2.0f; // Velocità di spostamento sull'asse Y
    public float yMinLimit = -20f; // Limite minimo di spostamento sull'asse Y
    public float yMaxLimit = 80f; // Limite massimo di spostamento sull'asse Y
    [HideInInspector] public bool pause = false; // Variabile per controllare lo stato di pausa

    private float x = 0.0f;
    private float y = 0.0f;
    private Coroutine fovCoroutine;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        if (pause)
        {
            return;
        }
        else
        {
            if (target)
            {   
                Camera biteCamera = BiteCam.GetComponent<Camera>();
                biteCamera.fieldOfView = 40;
                BiteCam.SetActive(false);

                x += autoRotateSpeed * Time.deltaTime; // Rotazione automatica sull'asse X

                if (PlayerCollidesWithTopScreen() && Input.GetKey(KeyCode.W))
                {
                    y += ySpeed * Time.deltaTime; // Alza la camera
                }
                else if (PlayerCollidesWithBottomScreen() && Input.GetKey(KeyCode.S))
                {
                    y -= ySpeed * Time.deltaTime; // Abbassa la camera
                }

                // Clampa la posizione sull'asse Y
                y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(0, x, 0); // Mantieni la rotazione sull'asse X
                Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + new Vector3(target.position.x, y, target.position.z);

                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    bool PlayerCollidesWithTopScreen()
    {
        // Ottieni la posizione del player in coordinate dello schermo
        Vector3 playerScreenPosition = Camera.main.WorldToViewportPoint(player.position);

        // Verifica se il player è vicino alla parte superiore dello schermo
        return playerScreenPosition.y >= 0.8f; // % dell'altezza dello schermo
    }

    bool PlayerCollidesWithBottomScreen()
    {
        // Ottieni la posizione del player in coordinate dello schermo
        Vector3 playerScreenPosition = Camera.main.WorldToViewportPoint(player.position);

        // Verifica se il player è vicino alla parte inferiore dello schermo
        return playerScreenPosition.y <= 0.2f; // % dell'altezza dello schermo
    }

    public void GraduallyChangeFOV(float targetFOV)
    {
        if (fovCoroutine != null)
        {
            StopCoroutine(fovCoroutine);
        }
        fovCoroutine = StartCoroutine(ChangeFOVCoroutine(targetFOV));
    }
    private IEnumerator ChangeFOVCoroutine(float targetFOV)
    {   
         Camera biteCamera = BiteCam.GetComponent<Camera>();
        float startFOV = biteCamera.fieldOfView;

        while (Mathf.Abs(biteCamera.fieldOfView - targetFOV) > 0.01f)
        {
            biteCamera.fieldOfView = Mathf.Lerp(biteCamera.fieldOfView, targetFOV, Time.deltaTime);
            yield return null;
        }

        biteCamera.fieldOfView = targetFOV;
    }
}