using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CardPlayer : MonoBehaviour
{
    public Transform atkPosRef;
    public Card chosenCard;
    [SerializeField] private TMP_Text nameText;
    public HealthBar healthBar;
    public TMP_Text healthText;
    private Tweener animationTweener;
    public float Health;
    public PlayerStats stats = new PlayerStats
    {
        MaxHealth = 100,
        RestoreValue = 5,
        DamageValue = 10
    };
    //public float MaxHealth;
    public TMP_Text NickName => nameText;
    public bool alwaysShowCard = false;
    public bool IsReady = false;

    Card[] cards;

    private void Start()
    {
        Health = stats.MaxHealth;
        cards = GetComponentsInChildren<Card>();

        if (alwaysShowCard == false)
            HideCards();
    }

    public void SetStats(PlayerStats newStats, bool restoreFullHealth = false)
    {
        this.stats = newStats;
        if (restoreFullHealth)
            Health = stats.MaxHealth;

        UpdateHealthBar();
    }

    public Attack? AttackValue
    {
        get => chosenCard == null ? null : chosenCard.AttackValue;
        //{
        //    if (chosenCard == null)
        //        return null;
        //    else
        //        return chosenCard.attack;
        //}
    }

    public void Reset()
    {
        if(chosenCard != null)
        {
            chosenCard.Reset();
        }
        chosenCard = null;
        if (alwaysShowCard == false)
            HideCards();
    }
    public void SetChosenCard(Card newCard)
    {
        if (chosenCard != null)
        {
            chosenCard.transform.DOKill();
            chosenCard.Reset();
        }

        chosenCard = newCard;
        if (alwaysShowCard)
            chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.2f, 0.2f);
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, stats.MaxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.UpdateBar(Health / stats.MaxHealth);
        healthText.text = Health + "/" + stats.MaxHealth;
    }

    public void AnimateAttack()
    {
        animationTweener = chosenCard.transform
            .DOMove(atkPosRef.position, 0.5f);
        
        if (alwaysShowCard)
            return;

        ShowCards();
    }

    public void AnimateDamage()
    {
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(0.2f);
            //.OnComplete(() => HideCards());

    }

    public void AnimateDraw()
    {
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.OriginalPosition, 1)
            .SetEase(Ease.InBack)
            .SetDelay(0.2f);
            //.OnComplete(() => HideCards());
    }

    public void AnimateShuffle()
    {

    }

    public bool IsAnimating()
    {
        return animationTweener.IsActive();
    }

    public void isClickable(bool value)
    {
        foreach(var card in cards)
        {
            card.SetClickable(value);
        }
    }

    public void ShowCards()
    {
        if (alwaysShowCard)
            return;

        foreach (var card in cards)
        {
            card.Show();
        }
    }

    public void HideCards()
    {
        if (alwaysShowCard)
            return;

        foreach (var card in cards)
        {
            card.Hide();
        }
    }
}
