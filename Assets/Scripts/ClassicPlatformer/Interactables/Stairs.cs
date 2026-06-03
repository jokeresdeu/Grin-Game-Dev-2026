using System;
using UnityEngine;

namespace ClassicPlatformer
{
    public class Stairs : BaseInteractable
    {
        private Player _player;

        public override void Interact(Player player)
        {
            Debug.Log("Stairs");
            _player = player;
            _player.EnableVerticalMovement(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Якщо гравець ще не торкався сходів (_player порожній), ігноруємо
            if (_player == null) return;

            bool isPlayer = other.TryGetComponent(out Player player);
            if (player != _player)
                return;

            _player.EnableVerticalMovement(false);
            _player = null;
        }
    }
}