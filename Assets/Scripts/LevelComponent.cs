using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    [SerializeField] BoxCollider2D _boxCollider;
    List<GameObject> _coinList = new List<GameObject>();
    List<GameObject> _enemieList = new List<GameObject>();
    [SerializeField] GameObject _coins;
    [SerializeField] GameObject _enemies;
    private void Awake()
    {
        foreach (GameObject enemie in _enemieList)
        {
            _enemieList.Add(enemie);
        }
        foreach (GameObject coin in _coinList)
        {
            _coinList.Add(coin);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LevelBag.Instance.SpawnLevel();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _boxCollider.enabled = false;
            LevelBag.Instance.DeSpawnLevel();
        }
    }
    private void OnEnable()
    {
        EnableLevel();
    }
    private void OnDisable()
    {
        DisableLevel();
    }
    private void EnableLevel()
    {
        if (_enemieList.Count > 0)
        {
            foreach (GameObject enemieComponent in _enemieList)
            {
                enemieComponent.SetActive(true);
            }
        }
        if (_coinList.Count > 0)
        {
            foreach (GameObject coinComponent in _coinList)
            {
                coinComponent.SetActive(true);
            }
        }
        _boxCollider.enabled = true;
    }
    private void DisableLevel()
    {
        if (_enemieList.Count > 0)
        {
            foreach (GameObject enemieComponent in _enemieList)
            {
                enemieComponent.SetActive(false);
            }
        }
        if (_coinList.Count > 0)
        {
            foreach (GameObject coinComponent in _coinList)
            {
                coinComponent.SetActive(false);
            }
        }
    }
}
