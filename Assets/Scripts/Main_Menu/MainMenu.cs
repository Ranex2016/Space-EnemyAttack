using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   [SerializeField]
   private GameObject loadScreen;
   [SerializeField]
   private Slider loadBar;

   private void Start() {
      loadScreen.SetActive(false);
   }
   public void LoadGame()
   {
      //Отобразим загрузочный экран
      loadScreen.SetActive(true);
      //Загрузить игровую сцену
      //SceneManager.LoadScene(1); // Основная игровая сцена
      StartCoroutine(LoadAsync());
   }

   // Метод-корутина, выполнится когда выйдет из цикла
   IEnumerator LoadAsync()
   {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
      while(!asyncLoad.isDone)
      {
         loadBar.value = asyncLoad.progress;
         yield return null;
      }
   }
}
