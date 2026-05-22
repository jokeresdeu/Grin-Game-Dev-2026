using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

namespace RPG
{
    public class HeroAnimatorEventProxy : MonoBehaviour
    {
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>() ?? GetComponentInParent<Character>();
        }

        public void SetExpression(string expression)
        {
            if (character == null) return;
            character.SetExpression(expression);
        }

        public void ResetAnimation()
        {
            if (character == null) return;
            character.UpdateAnimation();
        }
    }
}
