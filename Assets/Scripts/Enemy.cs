using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private bool _isAlive = true;


    void Start()
    {

        _isAlive = true;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");
        }
        _anim.ResetTrigger("OnEnemyDeath");

    }

    void Update()
    {
        CalculateMovment();
        WeaveMovement();
        if (Time.time > _canFire && _isAlive)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }


        }

    }

    void CalculateMovment()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);



        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }




    }

    void WeaveMovement()
    {
        if (transform.position.y < 4 && transform.position.x > 0.1)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else if (transform.position.y < 4 && transform.position.x <= 0)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();


            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _isAlive = false;
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }




        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _isAlive = false;
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

        }

        if (other.tag == "LaserBeam")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _isAlive = false;
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }





    }

}

