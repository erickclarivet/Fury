using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private int _life = 4;
    [SerializeField] private GameObject _heartFullPrefab;
    [SerializeField] private List<Transform> _hearts;
    [SerializeField] private Sprite _emptyHeart;

    // Start is called before the first frame update
    void Start()
    {

    }

    public int InitLife(int life)
    {
        Vector3 position = new Vector3(0, 0, 0);
        _hearts = new List<Transform>();
        for (int i = 0; i < _life; i++)
        {
            GameObject newhHeart = Instantiate(_heartFullPrefab);
            newhHeart.transform.SetParent(this.transform);
            newhHeart.transform.localScale = new Vector3(100, 100, 0);
            newhHeart.transform.localPosition = new Vector3(position.x + i * 60, position.y, position.z);
            _hearts.Add(newhHeart.transform);
        }
        return _life;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int LooseHeart()
    {
       _hearts[_life - 1].GetComponent<SpriteRenderer>().sprite = _emptyHeart;
       _life--;
        return _life;
    }
}
