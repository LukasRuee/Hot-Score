using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBag : MonoBehaviour
{
    static public LevelBag Instance;
    [SerializeField] private int _levelsHeight = 7;
    [SerializeField] private int _currentLevelHeight;
    [SerializeField] private int _loadedLevels;

    [SerializeField] private List<GameObject> _levels = new List<GameObject>();

    [SerializeField] private List<GameObject> _activeItemList = new List<GameObject>();
    [SerializeField] private List<GameObject> _usedItemList = new List<GameObject>();
    [SerializeField] private List<GameObject> _itemList = new List<GameObject>();
    private int _levelCounter;
    private void Start()
    {
        Instance = this;
        foreach (GameObject level in _levels)
        {
            _usedItemList.Add(level);
        }
        _currentLevelHeight += _levelsHeight;
        FillBag();
        for(int i = 0; i < _loadedLevels; i++)
        {
            SpawnLevel();
        }
    }

    private void FillBag()
    {
        foreach (GameObject level in _usedItemList)
        {
            _itemList.Add(level);
        }
        _usedItemList.Clear();
    }
    public void SpawnLevel()
    {
        if( _itemList.Count > 0 )
        {
            int randomItemIndex = Random.Range(0, _itemList.Count);
            GameObject level = _itemList[randomItemIndex].gameObject;
            _activeItemList.Add(level);
            _itemList.Remove(level);

            level.transform.position = new Vector3(0, _currentLevelHeight, 0);
            level.SetActive(true);
            _currentLevelHeight += _levelsHeight;
        }
        else
        {
            FillBag();
            SpawnLevel();
        }
    }
    public void DeSpawnLevel()
    {
        if (_levelCounter == 0)
        {
            _levelCounter++;
        }
        else if(_levelCounter == 1)
        {
            GameObject item = _activeItemList[0];
            item.SetActive(false);
            _activeItemList.RemoveAt(0);
            _levelCounter++;
        }
        else
        {
            GameObject item = _activeItemList[0];
            _usedItemList.Add(item);
            _activeItemList.Remove(item);
            item.SetActive(false);
        }
    }
}
