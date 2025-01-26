using Game.Community;
using Game.Unit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetraenkeBub
{

    public class CommunityPostHandlerUI : MonoBehaviour
    {
        [SerializeField] private Image profilBild;

        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI extraName;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI comment;

        [SerializeField] private List<MiniReactionHandler> goodReactions;
        [SerializeField] private List<MiniReactionHandler> badReactions;

        public void Setup(UnitPresenter unitPresenter,CommunityPresenter presenter,bool isGood)
        {
            goodReactions.ForEach(n => n.gameObject.SetActive(false));
            badReactions.ForEach(n => n.gameObject.SetActive(false));
            profilBild.sprite = unitPresenter.GetProfilPicture();
            name.text = unitPresenter.GetName();
            extraName.text = "@" + unitPresenter.GetName();

            //WriteComment

            if (isGood)
            {
                for(int i = 0; i< Random.Range(3,4); i++)
                {
                    goodReactions[i].Setup(isGood);
                }
                for (int i = 0; i < Random.Range(1, 2); i++)
                {
                    badReactions[i].Setup(!isGood);
                }
            }
            else
            {
                for (int i = 0; i < Random.Range(3, 4); i++)
                {
                    badReactions[i].Setup(isGood);
                }
                for (int i = 0; i < Random.Range(1, 2); i++)
                {
                    goodReactions[i].Setup(!isGood);
                }
            }
        }

    }
}
