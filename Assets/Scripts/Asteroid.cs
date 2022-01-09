using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
       //getting spawnmanager script component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }




    // Update is called once per frame
    void Update()
    {
        //rotate object on the zed axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    //check for laser collision (trigger)
    //instantiate explosion at the position of the asteroid (us)
    //destroy the explosion after 3 seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }
}
