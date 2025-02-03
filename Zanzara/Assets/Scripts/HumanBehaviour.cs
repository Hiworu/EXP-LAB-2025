using UnityEngine;

public class HumanBehaviour : MonoBehaviour
{

    public GameObject head;
    public GameObject viewCone;

    public GameObject player;
    private GameObject cones;
    public bool isColliding = false;

    private float nextRotationTime = 0f;
    private float rotationInterval = 2f; // Adjust the interval as needed
    private bool isRotating = false;
    private float rotationSpeed = 2f; // Adjust the speed as needed
    private Quaternion targetRotation;

    private void Start()
    {
        player = GameObject.Find("Player");
        cones = GameObject.Find("ViewCones");
        cones.SetActive(false);
    }

    private void Update()
    {
        if(isRotating == true)
        {
            if (Time.time >= nextRotationTime)
            {
                Debug.Log("Rotating head");
                RotateHeadRandomly();
                nextRotationTime = Time.time + rotationInterval;
            }

            // Smoothly rotate towards the target rotation
            head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
    }

    private void RotateHeadRandomly()
    {
        float randomYRotation = Random.Range(-90f, 90f); // 180 degrees range
        targetRotation = Quaternion.Euler(0, randomYRotation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with: " + other.gameObject.name);
        if (other.gameObject == player)
        {
            isColliding = true;

            switch (gameObject.name)
            {
                case "Awareness0":
                    Debug.Log(gameObject.name);
                    cones.SetActive(false);
                    isRotating = true;
                    break;
                case "Awareness1":
                    Debug.Log(gameObject.name);
                    cones.SetActive(true);
                    isRotating = true;
                    //head movement + periferal cone view che si attiva e disattiva
                    break;
                case "Awareness2":
                    Debug.Log(gameObject.name);
                    cones.SetActive(true);
                    cones.transform.localScale = new Vector3(2, 2, 2);
                    isRotating = true;
                    //head movement + bigger view cone always active + player death if inside
                    break;
            }
        }
        
    }
}
