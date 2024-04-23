using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Связаться с полем счета
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Button _fireButton;

    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _livesSprite;

    void Start()
    {
        //_fireButton.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        // Обновить данные счета
        _scoreText.text = "Score: " + 0;

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        // получить доступ к спрайту
        // присвоить ему значение на основе переданного индекса жизни
        _livesImg.sprite = _livesSprite[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }
    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _fireButton.gameObject.SetActive(false);
        StartCoroutine(blinkTextRoutin());

#if UNITY_ANDROID

#else
        // Покажем текст рестарта. Только для Виндовс
        _restartText.gameObject.SetActive(true);
        // уберем кнопку стреляния
        _fireButton.gameObject.SetActive(false);
#endif

    }

    IEnumerator blinkTextRoutin()
    {
        Debug.Log("Мерцаем!");
        while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
