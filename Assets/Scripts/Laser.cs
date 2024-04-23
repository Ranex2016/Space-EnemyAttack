using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    void Update()
    {
        MoveUp();
    }
    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        // проверим что лазер улетел за пределы видимости
        if (transform.position.y > 8)
        {

            //Если у лазера есть родительский компонент то уничтож его с содержимым
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                //Уничтожение игрового объекта к которому привязан скрипт
                Destroy(this.gameObject);
            }
        }
    }
}
