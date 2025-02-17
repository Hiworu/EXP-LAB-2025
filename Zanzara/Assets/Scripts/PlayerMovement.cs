using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // Velocità di movimento del player
    public float raycastDistance = 100.0f; // Distanza del raycast
    public float descendSpeed = 2.0f; // Velocità di discesa del player
    public float bloodCollectionTime = 10.0f; // Tempo per raccogliere il sangue
    public float bloodInExcess = 5.0f; // Sangue in eccesso
    public static float maxMosquitoBlood = 200.0f; // Sangue massimo della zanzara
    private Rigidbody myBody;
    private Vector3 startPosition;
    private bool isDescending = false;
    private bool isReturning = false;
    private bool isAtTarget = false;
    private bool isCollectingBlood = false;
    private bool hasCollectedAllBlood = false;
    private float collectionStartTime;
    private bool onSuckPoint = false;
    private float bloodToCollect = 0.0f;
    public float mosquitoBlood = 0.0f;
    public float totalMosquitoBlood = 0;
    private float pointBlood = 0.0f;
    private Vector3 targetPosition;
    private CameraMovement cameraMovement;
    private SuckPoint suckPoint;
    private GameManager gameManager;
    [SerializeField] LayerMask meatMask;
    public AnimationCurve bloodCollectionCurve; // Curva di raccolta del sangue
    public SuckingBar suckingBar;
    SoundManager SoundManager;
    public GameObject spotLight;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
        gameManager = FindAnyObjectByType<GameManager>();
        suckingBar = FindAnyObjectByType<SuckingBar>();
        SoundManager = FindAnyObjectByType<SoundManager>();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isReturning)
        {
            if (!isDescending && !isAtTarget)
            {
                startPosition = transform.position; // Aggiorna la posizione iniziale
                CastRayAndDescend();
            }
            if (isDescending)
            {
                cameraMovement.pause = true;
                DescendToTarget();
            }
        }
        else if (isAtTarget && !Input.GetKey(KeyCode.Space))
        {
            isReturning = true;
            isAtTarget = false;

            SuckingOver();
            if (onSuckPoint)
            {
                suckPoint.HasBeenBitten();
            }
        }
        else if (isDescending)
        {
            isReturning = true;
            isDescending = false;
        }

        if (isReturning)
        {
            cameraMovement.pause = true;
            ReturnToStartPosition();
        }
        else if (!isDescending && !isReturning && !isAtTarget)
        {
            cameraMovement.pause = false;
            MovePlayer();
        }

        if (isCollectingBlood)
        {
            CollectBloodOnPoint();
            SoundManager.instance.PlaySuckingSound();
        }
    }

    void MovePlayer()
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
        myBody.linearVelocity = movement * speed * Time.deltaTime;
    }

    void CastRayAndDescend()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, meatMask))
        {
            if (hit.collider.gameObject.name == "Meat")
            {
                Debug.Log("hit point");
                isDescending = true;
                isReturning = false;
                targetPosition = hit.point;
                myBody.linearVelocity = Vector3.zero; // Ferma il movimento corrente
                cameraMovement.pause = true; // Metti in pausa la camera
                suckPoint = hit.collider.GetComponent<SuckPoint>();
                if (suckPoint != null)
                {
                    pointBlood = suckPoint.pointBlood;
                }
            }
            else
            {
                cameraMovement.pause = false; // Non mettere in pausa la camera
                isDescending = false; // Resetta il flag di discesa
            }
        }
    }

    void DescendToTarget()
    {
        cameraMovement.BiteCam.SetActive(true);
        cameraMovement.GraduallyChangeFOV(20);
        spotLight.SetActive(false);


        transform.position = Vector3.MoveTowards(transform.position, targetPosition, descendSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isDescending = false;
            isAtTarget = true;
            isCollectingBlood = true;
        }
    }

    void ReturnToStartPosition()
    {   
        cameraMovement.GraduallyChangeFOV(40);
        transform.position = Vector3.MoveTowards(transform.position, startPosition, descendSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            isReturning = false;
            cameraMovement.pause = false; // Riattiva i movimenti della camera
            cameraMovement.BiteCam.SetActive(false);
            spotLight.SetActive(true);
        }
        
    }

    void CollectBloodOnPoint()
    {
        if (suckPoint != null)
        {   
            gameManager.pause = true;
            float bloodToCollect = pointBlood;
            suckingBar.SetMaxSucking(bloodToCollect);
            
            if (onSuckPoint)
            {
                bloodToCollect *= 2; // Moltiplica pointBlood per 2 se onSuckPoint è true
            }

            float elapsedTime = Time.time - collectionStartTime;
            float curveValue = bloodCollectionCurve.Evaluate(elapsedTime / bloodCollectionTime);
            float bloodIncrement = bloodToCollect * curveValue * Time.deltaTime;
            float greentime = 0.5f;

            mosquitoBlood += bloodIncrement;
            suckingBar.SetSucking(mosquitoBlood);
            totalMosquitoBlood += bloodIncrement;

            if (mosquitoBlood >= bloodToCollect)
            {
                mosquitoBlood += bloodIncrement;

                if (mosquitoBlood > bloodToCollect+ greentime)
                {
                    AddBonusBlood();
                    SuckingOver();
                }

                if (suckPoint != null && onSuckPoint)
                {
                    suckPoint.HasBeenBitten();
                }

            }
        }
    }

    void SuckingOver() //quando finisco di prendere sangue o tolgo io in anticipo input
    {
        isCollectingBlood = false;
        mosquitoBlood = 0;
        onSuckPoint = false;
        gameManager.pause = false;
        suckingBar.SetSucking(0);
        SoundManager.instance.StopSuckingSound();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Point"))
        {
            isCollectingBlood = true;
            onSuckPoint = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        SuckingOver();
    }
    void AddBonusBlood()
    {
        totalMosquitoBlood += bloodInExcess;
    }

    
}