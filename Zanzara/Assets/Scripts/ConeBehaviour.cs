using UnityEngine;
using UnityEngine.SceneManagement;

public class ConeBehaviour : MonoBehaviour
{
   public GameObject player;
   HumanBehaviour behav;

   private void Start()
   {
       player = GameObject.Find("Player");
       behav = player.GetComponent<HumanBehaviour>();
   }

   private void OnTriggerEnter(Collider other)
   {
       if (other.gameObject == player && behav.isGameOver == true)
       {
           SceneManager.LoadScene("GameOver");  
       }
   }
}
