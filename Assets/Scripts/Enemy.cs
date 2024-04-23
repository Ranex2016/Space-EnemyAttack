using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    private Player _player;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _minTimeFire = 1f;
    private float _maxTimeFire = 8f;
    private float _canFire = -1; // можно стрелять
    private bool _isLive = true;

    //переменная анимации
    private Animator _anim;
    private AudioSource _audioSource;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is NULL.");
        }

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        // инициализировать анимацию
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        // Двигать врага со скоростью 4 метра в сек
        // Если враг достигнет низа экрана, переместить его в начало
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8f, 0f);
        }

        // Стрельба
        if (Time.time > _canFire && _isLive)
        {
            _canFire = Time.time + Random.Range(_minTimeFire, _maxTimeFire);
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
            // Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            // for(int i = 0; i< lasers.Length; i++)
            // {
            //     lasers[i].AssignEnemyLaser();
            // }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // проверить столкновение
        // с лазером
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            // Проиграй анимацию взрыва противника
            _anim.SetTrigger("OnEnemyDeath");
            //_speed = 0;
            //проиграй звук взрыва
            _audioSource.Play();
            //explosionPrefabs.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            // выключаем повторное столкновение

            //Уничтож себя и колайдер
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

            //Уничтожим внутрении объекты
            gameObject.transform.Find("Thruster_Enemy_left").gameObject.SetActive(false);
            gameObject.transform.Find("Thruster_Enemy_right").gameObject.SetActive(false);

            // прибавить игроку 10 очков за уничтожение
            if (_player != null)
            {
                _player.AddScore(10);
            }
        }
        // с игроком
        if (other.tag == "Player")
        {
            //Вызови повреждение у игрока
            Player player = other.transform.GetComponent<Player>();
            // проверим что игрок существует
            if (player != null)
            {
                player.Damage();
            }
            // Проиграй анимацию взрыва себя
            _anim.SetTrigger("OnEnemyDeath");
            //_speed = 0;
            //проиграй звук взрыва
            _audioSource.Play();
            //explosionPrefabs.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //Instantiate(explosionPrefabs, transform.position, Quaternion.identity);

            //Уничтож себя и колайдер
            _isLive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

            //Уничтожим внутрении объекты
            gameObject.transform.Find("Thruster_Enemy_left").gameObject.SetActive(false);
            gameObject.transform.Find("Thruster_Enemy_right").gameObject.SetActive(false);
        }
    }
}
