using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MovementType
{
    Update,
    Coroutine
}
public class EnemieMovement : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private Transform _enemy;
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _deathAnimation;
    private Transform _targetPoint;
    private bool _canMove = true;
    private void Awake()
    {
        _targetPoint = _pointA;
    }
    private void Update()
    {
        if(_canMove)
        {
            float step = _moveSpeed * Time.deltaTime;
            _enemy.position = Vector3.MoveTowards(_enemy.position, _targetPoint.position, step);

            if (Vector3.Distance(_enemy.position, _targetPoint.position) < 0.01f)
            {
                _targetPoint = (_targetPoint == _pointA) ? _pointB : _pointA;
            }
        }
    }
    public IEnumerator GetHit()
    {
        _collider.enabled = false;
        _animator.Play(_deathAnimation.name);
        yield return new WaitForSeconds(_deathAnimation.length);
        transform.gameObject.SetActive(false);
        _collider.enabled = true;
    }
}
