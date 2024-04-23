using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefabs;

    private SpawnManager _spawnManager;

    [SerializeField]
    private float _rotateSpeed;
    private float _speed;

    void Start()
    {
        _rotateSpeed = Random.Range(-3f, 3f);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Задать вращение астеройду
        transform.Rotate(new Vector3(0f, 0f, 1f) * _rotateSpeed);

        if (gameObject.transform.position.y < -8)
        {
            gameObject.transform.position = new Vector3(Random.Range(8f, -8f), 8f, 0f);
        }
    }

    // Проверить столкновение с лазером
    // Создать взрыв на месте астеройда
    // Удалить взрыв со сцены
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Vector3 position = this.gameObject.transform.position;
            _explosionPrefabs.transform.localScale = new Vector3(1, 1, 1);
            Instantiate(_explosionPrefabs, position, Quaternion.identity);

            Destroy(other.gameObject);
            
            //_spawnManager.StartSpawnManager();
            Destroy(gameObject.GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.25f);
        }
        if(other.tag == "Player")
        {
            Vector3 position = this.gameObject.transform.position;
            _explosionPrefabs.transform.localScale = new Vector3(1, 1, 1);
            Instantiate(_explosionPrefabs, position, Quaternion.identity);

            //Вызови повреждение у игрока
            Player player = other.transform.GetComponent<Player>();
            // проверим что игрок существует
            if (player != null)
            {
                player.Damage();
            }

            //_spawnManager.StartSpawnManager();
            Destroy(gameObject.GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.25f);
        }
    }
}
