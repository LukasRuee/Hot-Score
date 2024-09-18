using System.Collections;
using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] SpriteRenderer _renderer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();
            _collider.enabled = false;
            _renderer.enabled = false;
            StartCoroutine(GetCollected());
        }
    }
    IEnumerator GetCollected()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        gameObject.SetActive(false);
        _collider.enabled = true;
        _renderer.enabled = true;
    }
}
