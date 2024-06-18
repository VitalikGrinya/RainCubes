using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private Color _cubeColor = Color.white;
    
    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;

    private Renderer _renderer;
    private bool _isOnCollision;

    public event Action<Cube> LifeTimeEnded;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _renderer.material.color = _cubeColor;
        _isOnCollision = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isOnCollision == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isOnCollision = true;
            _renderer.material.color = new Color(UnityEngine.Random.value,UnityEngine.Random.value, UnityEngine.Random.value);
            StartCoroutine(LifeTime());
        }
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minLifeTime, _maxLifeTime));

        LifeTimeEnded?.Invoke(this);
    }
}