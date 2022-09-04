using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float initialForce;
    private Rigidbody _rigidbody;
    private Vector3 _initialScale;
    private bool _isActive;
    private float _activeTime;
    
    public bool IsActive => _isActive;
    
    void Awake()
    {
        _initialScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody>();
        _activeTime = 0f;
    }

    
    void Update()
    {
        if (_isActive)
        {
            transform.localScale -= Vector3.one * 0.001f;
            _activeTime += Time.deltaTime;
            if (_activeTime >= 2f)
            {
                Reset();
            }
        }
    }

    public void Go(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        _rigidbody.AddForce(initialForce * dir);
        gameObject.SetActive(true);
        _isActive = true;
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        transform.localScale = _initialScale;
        _isActive = false;
        _activeTime = 0f;
    }
}
