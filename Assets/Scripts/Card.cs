using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] Image image;
    public Attack AttackValue;
    public CardPlayer player;
    public Transform atkPosref;
    public Vector2 OriginalPosition;
    Vector2 OriginalScale;
    Color OriginalColor;
    bool isClickable = true;

    private void Start()
    {
        OriginalPosition = this.transform.position;
        OriginalScale = this.transform.localScale;
        image = GetComponent<Image>();
        OriginalColor = image.color;
    }
    public void OnClick()
    {
        if (isClickable)
        {
            OriginalPosition = this.transform.position;
            player.SetChosenCard(this);
        }
    }

    public void Reset()
    {
        transform.position = OriginalPosition;
        transform.localScale = OriginalScale;
        GetComponent<Image>().color = OriginalColor;
    }

    public void SetClickable(bool value)
    {
        isClickable = value;
    }

    private void OnDestroy()
    {
        var button = GetComponentInChildren<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void Show()
    {
        image.color = Color.white;
    }

    public void Hide()
    {
        image.color = Color.black;
    }
}
