using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
    }
}
