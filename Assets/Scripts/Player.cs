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
    [SerializeField]
    private float _shiftSpeed = 15f;
    //prefab for shield
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private Shield _shieldBehavior;
    [SerializeField]
    public int _maxAmmo = 15;
    [SerializeField]
    AudioClip _noAmmoSound;
    //Camera Shake Reference
    private CameraShake _camShake;
    [SerializeField]
    private GameObject _laserBeam;
    private bool _isThrusting = false;
    private float _thrusterPower = 100.0f;
    private bool _canThrust = true;
    
    




    private void Start()
    {


        _camShake = Camera.main.GetComponent<CameraShake>();
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
        if(_camShake is null)
        {
            Debug.LogError("ShakeCamera script is NULL.");
        }

        StartCoroutine(ThrustPower());

    }




    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
          
            {
            if (_maxAmmo == 0)
            {
                AudioSource.PlayClipAtPoint(_noAmmoSound, transform.position);
                return;
            }


            FireLaser();
                
        }



        }

        void CalculateMovement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


            transform.Translate(direction * _speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift)&& _canThrust == true) 
            {
                transform.Translate(direction * _shiftSpeed * Time.deltaTime);
            _isThrusting = true;
            
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isThrusting = false;
        }






            if (transform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);
            }
            else if (transform.position.y <= -3.8f)
            {

                transform.position = new Vector3(transform.position.x, -3.8f, 0);
            }


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
        

        {
            
            AmmoCount(-1);
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

        public void Damage()
        {

        StartCoroutine(_camShake.ShakeCamera());
        
        if (_isShieldActive == true)
            {
                _shieldBehavior.DamageShield();


                //_shieldVisualizer.SetActive(false); 
                return;
            }




            _lives--;



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

            _isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());

        }


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

        public void ShutDownShield()
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
        }

        public void AmmoCount(int bullets)
        {
            if(bullets >= _maxAmmo)
        {
            _maxAmmo = 15;
        }
        else
        {
            _maxAmmo += bullets;
        }
       
         _uiManager.updateAmmoCount(_maxAmmo);
        }

    public void restoreHealth()
    {
        _lives = 3;
        _uiManager.UpdateLives(_lives);
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);

    }

    public void LaserBeamActive()
    {
        _laserBeam.SetActive(true);
        StartCoroutine(CoolDownLaserBeam());
    }

    IEnumerator CoolDownLaserBeam()
    {
        yield return new WaitForSeconds(5f);
        _laserBeam.SetActive(false);
    }

    public void NegativeSpeed()
    {
        StartCoroutine(NegativeSpeedCoolDown());
        _speed /= _speedMultiplier;
        _shiftSpeed /= _speedMultiplier;
    }

    IEnumerator NegativeSpeedCoolDown()
    {
        yield return new WaitForSeconds(5);
        _speed *= _speedMultiplier;
        _shiftSpeed *= _speedMultiplier;
    }

    IEnumerator ThrustPower()
    {
        while (true)
        {
            yield return null;
            if (_isThrusting == true)
            {
                _thrusterPower -= 15 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            //Power goes down
            if (_isThrusting == false)
            {
                _thrusterPower += 5 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            //Power goes up

            if (_thrusterPower <= 0)
            {
                _thrusterPower = 0;
                _canThrust = false;
                _isThrusting = false;
            }
            if(_thrusterPower >= 25 && _canThrust == false)
            {
                _canThrust = true;
            }
            _uiManager.ThrusterValue(_thrusterPower);


        }
    }
  
    
    
    
    
    

   

    }





