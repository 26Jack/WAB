using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;
    [SerializeField] private PlayerMovement _player;

    public float scrollMult = 0.01f;

    void Start()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerMovement>();
        }
    }

    void Update()
    {
        if (_player != null)
        {
            float speed = _player.currentSpeed * scrollMult;
            Vector2 direction = new Vector2(_x, _y).normalized;
            Vector2 scrollAmount = direction * speed * Time.deltaTime;
            _img.uvRect = new Rect(_img.uvRect.position + scrollAmount, _img.uvRect.size);
        }
    }
}