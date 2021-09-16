using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Not gonna lie, I may have borrowed this script from another project I'm working on, just because I needed to get this
 *  working. I will probably clean this up later.
 */

public class BossPillar : MonoBehaviour
{
    [SerializeField] private List<Vector3> _points;
    private int _currentTarget = 0;
    [SerializeField] public bool _isCurentlyActive;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _delayTime = 0;
    private float _delayStartTime;
    private float _tolerance; // the distance moved in one fixed update

    private void Start()
    {
        _tolerance = _moveSpeed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if (_isCurentlyActive)
        {
            if (transform.position != _points[_currentTarget])
            {
                MovePlatform();
            }
            else
            {
                UpdateTarget();
            }
        }
    }

    private void MovePlatform()
    {
        Vector3 _heading = _points[_currentTarget] - transform.position;
        if (_heading.magnitude < _tolerance)
        {
            transform.position = _points[_currentTarget];
        }
        else
        {
            transform.position += (_heading / _heading.magnitude) * _moveSpeed * Time.fixedDeltaTime;
            _delayStartTime = Time.time;
        }
    }

    private void UpdateTarget()
    {
        if (Time.time - _delayStartTime >= _delayTime)
        {
            _currentTarget++;
            if(_currentTarget >= _points.Count)
            {
                _isCurentlyActive = false;
            }
        }
    }

    public void ActivatePillar()
    {
        _isCurentlyActive = true;
    }
}