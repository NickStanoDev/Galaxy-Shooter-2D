using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID forpowerups
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = shields
    private int powerupID;
   [SerializeField]
    private AudioClip _clip;
    
    
  

  
    void Update()
    {
        //move down at speed of 3 (adjust in inspector)
        //When we leave the screen, destroy this object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if  (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
        
    }

    //OnTriggerCollision
    //Only be collectable by the player (Use Tags)
    //on collected, destroy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //commmuncate with the plaer script
            //handle to the component i want
            //assign the handle to the component
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            

            //!= means not equal
            if (player != null)
            {
       
                    switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;

                    case 3:
                        player.AmmoCount(15);
                        break;
                    case 4:
                        player.restoreHealth();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }

            }
            
                   
            
            Destroy(this.gameObject);
        }
    }
}

