using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update() 
    {
        // Проверить нажата ли клавиша R
        // Перезапустить сцену
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // перезагрузить текущую сцену
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Выход из игры
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
