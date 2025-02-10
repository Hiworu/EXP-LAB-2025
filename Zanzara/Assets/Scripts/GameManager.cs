using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float TimerLivello = 60.0f; // Timer di X secondi
    public bool pause = false; // Variabile per mettere in pausa il decremento
    public float decreaseRate = 1.0f; // Tasso di decremento di totalMosquitoBlood al secondo
    public float decreaseRateStage1 = 1.0f; // Tasso di decremento per il primo terzo del timer
    public float decreaseRateStage2 = 2.0f; // Tasso di decremento per il secondo terzo del timer
    public float decreaseRateStage3 = 3.0f; // Tasso di decremento per l'ultimo terzo del timer

    private PlayerMovement playerMovement;
    private float initialTimerLivello;

    void Start()
    {
        // Trova il componente PlayerMovement
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script non trovato!");
        }

        // Salva il valore iniziale di TimerLivello
        initialTimerLivello = TimerLivello;
    }

    void Update()
    {
        // Decrementa il TimerLivello
        if (TimerLivello > 0)
        {
            TimerLivello -= Time.deltaTime;
        }
        if (TimerLivello <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }

        // Cambia il decreaseRate in base al TimerLivello
        float thirdOfTimer = initialTimerLivello / 3.0f;
        if (TimerLivello > 2 * thirdOfTimer)
        {
            decreaseRate = decreaseRateStage1; // Primo terzo del timer
        }
        else if (TimerLivello > thirdOfTimer)
        {
            decreaseRate = decreaseRateStage2; // Secondo terzo del timer
        }
        else
        {
            decreaseRate = decreaseRateStage3; // Ultimo terzo del timer
        }

        // Decrementa totalMosquitoBlood se non è in pausa
        if (!pause && playerMovement != null)
        {
            playerMovement.totalMosquitoBlood -= decreaseRate * Time.deltaTime;
            if (playerMovement.totalMosquitoBlood <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            if (playerMovement.totalMosquitoBlood >= PlayerMovement.maxMosquitoBlood)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
