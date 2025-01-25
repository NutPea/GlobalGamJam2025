using UnityEngine;

namespace Game.Grid.Content
{
    public class CommunityContent : AGridContent
    {
        public enum Community { Facepalm, Tiktak, Y, Whatsup}

        [SerializeField] private Community community;
    }
}