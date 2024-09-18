using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaComponent : MonoBehaviour
{
    [SerializeField] private float _startMoveSpeed = 1;
    [SerializeField] private float _timeDivision = 100;
    [SerializeField] private float _currentMoveSpeed;
    [SerializeField] private float _currentTime = 0;
    private void Start()
    {
        _currentMoveSpeed = _startMoveSpeed;
    }
    void Update()
    {
        _currentTime += Time.deltaTime;
        _currentMoveSpeed = _startMoveSpeed + (_currentTime / _timeDivision);
        Vector3 targetPosition = transform.position + Vector3.up;
        float step = _currentMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
