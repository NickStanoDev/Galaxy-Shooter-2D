using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 7.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
   [SerializeField]
    private AudioSource _audioSource;




    private void Start()
    {
        //get access to the SpawnManager script
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    
    }




    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            FireLaser();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // new Vector3(1, 0, 0) * 1 * 3.5f * real time
        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime)
        transform.Translate(direction * _speed * Time.deltaTime);
    


        //if player position on the y is greater than 0
        //y position = 0
        //else if position on the y is less than -3.8f
        //y pos = -3.8f
        //can use Mathf.Clamp to clamp it all into one line of code
        //this is to stop you from going past a certain point on the screen vertically

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {

            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //if player on the x is > 11.3f
        //x pos = -11.3f
        //else if player on the x is less than -11.3f
        //x pos = 11.3f
        //this is to loop you to the other side of the x Axis when you hit a certain point on the screen

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        //if i hit the space key
        //spawn gameObject
        //Instantiate creates an object from a prefab
        //we want the laser to spawn at the players position with transform.position, while being 0.8f above the player so the Laser looks like it is coming out of our player more realistically
        //by adding the position to a Vector3 (offset of 0.8 for every Laser)
        //no desired rotation = Quaternion.identity
        //time.time is how long the game has been running
        //&& chains multiple conditions together
        //if space key press fire 1 laser
        //if triple shot active true
        //fire 3 lasers
        //else fire 1 laser

        {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }


            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            _audioSource.Play();

             }


       

    }
    //Public methods can be called from outside this script
    public void Damage()
    {
        //if sheilds is active
        //do nothing
        //deactivate shields
        //return; stops a method
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

     


        _lives--;


        //if lives is 2
        //enable left engine
        //else if lives is 1
        //enable right engine
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }    
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }    


        _uiManager.UpdateLives(_lives);

       

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();


            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        //tripleshotactive becomes true
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    //IEnumerator TripleShotPowerDownRoutine
    //wait 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutive());
    }

    IEnumerator SpeedBoostPowerDownRoutive()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
   
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }    

}



