using UnityEngine;
using UnityEngine.SceneManagement;

public class ConeBehaviour : MonoBehaviour
{
   public GameObject player;
   HumanBehaviour behav;
   SoundManager SoundManager;

   private void Start()
   {
        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
       player = GameObject.Find("Player");
       behav = GetComponent<HumanBehaviour>();
   }

   private void OnTriggerEnter(Collider other)
   {
       if (other.gameObject == player && behav.isGameOver == true)
       {
            SoundManager.audioSource.Stop();
           SceneManager.LoadScene("GameOver");  
       }
   }
}
