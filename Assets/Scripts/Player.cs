using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefabs;

    [SerializeField]
    private GameObject _tripleShotPrefabs;
    [SerializeField]
    private GameObject _shieldVizualizer;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSoundClip;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject explotionsPrefabs;

    [SerializeField]
    private GameObject _laserContainer;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private Joystick joystick;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, -3.0f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        //Проверим что Интерфейс существует
        if (_uiManager == null)
        {
            Debug.LogError("Значение UIManager не существует, сылка нулевая");
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
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput;
        float verticalInput;

//Если запуск на Андройде то назначим джойстик иначе обычные кнопки управления
#if UNITY_ANDROID
        horizontalInput = joystick.Horizontal;
        verticalInput = joystick.Vertical;
#else
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
#endif

        // проверить активин ли увеличитель скорости
        // если активен то увеличить скорость
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_isSpeedBoostActive == true)
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
            Debug.Log("Скорость увеличена!");
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }


        // Ограничение экрана по Y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.0f, 5.5f), 0);
        // Ограничение экрана по X
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser_Enemy")
        {
            Debug.Log("Лазер попал по игроку!");
            Destroy(other.gameObject);
            Damage();
        }
    }

    public void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        // Проверь активен ли трайной лазер
        if (_isTripleShotActive == true)
        {
            GameObject tripleShot = Instantiate(_tripleShotPrefabs, transform.position + new Vector3(2, 0f, 0), Quaternion.identity);
        }
        else
        {
            GameObject newLaser = Instantiate(_laserPrefabs, transform.position + new Vector3(-0.04f, 0.7f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
    {

        // Проверь активность щита

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            // Отключить визуализацию щита
            _shieldVizualizer.SetActive(false);
            return;
        }

        _lives--;
        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        // Обновим спрайт жизней
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            //Связаться с менеджером спавна врагов
            //Сказать что хватит спавнить
            _spawnManager.OnPlayerDeath();
            Instantiate(explotionsPrefabs, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        // Включить тройной выстрел
        // и запустить корутину
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    // Подождать 5 секонды
    // Выключить трайной выстрел
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        //Включить визуализацию щита
        _shieldVizualizer.SetActive(true);
    }

    //Добавить метод который добавить игроку 10 очков к общему результату
    public void AddScore(int points)
    {
        _score += points;
        //Связаться и UI и обновить общий счет
        _uiManager.UpdateScore(_score);
    }
}
