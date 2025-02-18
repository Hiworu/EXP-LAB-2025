using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

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
    private float previousTotalMosquitoBlood;
    SoundManager SoundManager;
    public TextMeshProUGUI timerText; // Aggiungi questa variabile

    void Start()
    {
        // Trova il componente PlayerMovement
        SoundManager = FindAnyObjectByType<SoundManager>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script non trovato!");
        }

        // Salva il valore iniziale di TimerLivello
        initialTimerLivello = TimerLivello;
        // Inizializza previousTotalMosquitoBlood
        previousTotalMosquitoBlood = playerMovement.totalMosquitoBlood;
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
            SoundManager.audioSource.Stop();
            SceneManager.LoadScene("WinScreen");
        }

        // Aggiorna il testo del timer
        UpdateTimerText();

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
            float Scale = playerMovement.totalMosquitoBlood / 2;
            // Aggiorna la scala di mosquitoButt
            playerMovement.mosquitoButt.transform.localScale = new Vector3(Scale, Scale, Scale);
            // Aggiorna previousTotalMosquitoBlood
            previousTotalMosquitoBlood = playerMovement.totalMosquitoBlood;

            if (playerMovement.totalMosquitoBlood <= 20)
            {   
                SoundManager.audioSource.Stop();
                SceneManager.LoadScene("GameOver");
            }
            if (playerMovement.totalMosquitoBlood >= PlayerMovement.maxMosquitoBlood)
            {   
                SoundManager.audioSource.Stop();
                SceneManager.LoadScene("GameOver");
            }
        }
        
        void UpdateTimerText()
        {
            if (timerText != null)
            {
                timerText.text = "Time: " + Mathf.Ceil(TimerLivello).ToString();
            }
        }
    }
}
