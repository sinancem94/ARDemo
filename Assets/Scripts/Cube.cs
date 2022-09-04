using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float initialForce;
    private Rigidbody _rigidbody;
    private Vector3 _initialScale;
    private bool _isActive;

    public bool IsActive => _isActive;
    
    void Start()
    {
        _initialScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (_isActive)
        {
            transform.localScale -= Vector3.one * 0.01f;

            if (transform.localScale.magnitude < 0.005)
            {
                gameObject.SetActive(false);
                _isActive = false;
            }
        }
    }

    public void Go(Vector3 pos, Vector3 dir)
    {
        transform.localScale = _initialScale;
        transform.position = pos;
        _rigidbody.AddForce(initialForce * dir);
        gameObject.SetActive(true);
        _isActive = true;
    }
}
