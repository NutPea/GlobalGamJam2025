using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{
    public class MiniReactionHandler : MonoBehaviour
    {
        [SerializeField] private int goodMaxAmount = 99;
        [SerializeField] private int goodMinAmount = 30;
        [SerializeField] private int flaseMaxAmount = 20;
        [SerializeField] private int flaseinAmount = 2;

        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image emoji;
        [SerializeField] private List<Sprite> goodEmoji;
        [SerializeField] private List<Sprite> badEmoji;

        public void Setup(bool isGood)
        {
            gameObject.SetActive(true);
            int amount = isGood ? Random.Range(goodMinAmount,goodMaxAmount) : Random.Range(flaseinAmount,flaseMaxAmount);
            amountText.text = amount.ToString();
            Sprite foundSprite = null;
            if (isGood)
            {
                foundSprite = goodEmoji[Random.Range(0, goodEmoji.Count)];
            }
            else
            {
                foundSprite = badEmoji[Random.Range(0, badEmoji.Count)];
            }
            emoji.sprite = foundSprite;
        }


    }
}
