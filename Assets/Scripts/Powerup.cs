using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shields
    private float _speed = 2.0f;

    [SerializeField]
    private float powerupID = 0;

    [SerializeField]
    private AudioClip _clip;


    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }
    // Проверить наличие столкновения
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Воспроизведение клипа в установленном месте
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            Destroy(this.gameObject);
            // Нужно вызвать метод усиления лазера
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("Активирован тройной удар!");
                        break;
                    case 1:
                        // если ID = 1 активируем скорость
                        player.SpeedBoostActive();
                        Debug.Log("Активирована скорость!");
                        break;
                    case 2:
                        // если ID = 2 активируем щиты
                        player.ShieldActive();
                        Debug.Log("Активирован щит!");
                        break;
                    default:
                        Debug.Log("Не известный ID усиления!");
                        break;
                }

            }
        }
    }
}
