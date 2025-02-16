using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanBehaviour : MonoBehaviour
{
    public GameObject head;
    public GameObject viewCone;

    public GameObject player;
    private GameObject cones;
    
    public bool isColliding = false;
    public bool isGameOver = false; 

    private float nextRotationTime = 0f;
    private float rotationInterval = 2f; // Adjust the interval as needed
    private bool isRotating = false;
    private bool headUpDown = false;
    private float rotationSpeed = 2f; // Adjust the speed as needed
    private Quaternion targetRotation;
    


     private void Start()
    {
        player = GameObject.Find("Player");
        cones = GameObject.Find("ViewCones");
        head.transform.localRotation = Quaternion.Euler(0, -90, -90);
    }

    private void Update()
    {
        if (isRotating == true)
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
        float randomYRotation = Random.Range(-140f, -20f); // 180 degrees range
        float randomZRotation = Random.Range(-120f, -60f); 
        targetRotation = Quaternion.Euler(0, randomYRotation, -90);
        if(headUpDown == true)
        {
            targetRotation = Quaternion.Euler(0, randomYRotation, randomZRotation);
        }
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
                    headUpDown = true;
                    //head movement + periferal cone view 
                    break;
                case "Awareness2":
                    Debug.Log(gameObject.name);
                    cones.SetActive(true);
                    headUpDown = true;
                    isRotating = true;
                    isGameOver = true;
                    //head movement + bigger view cone moves up and down + player death if inside
                    break;
            }
        }
    }
}
