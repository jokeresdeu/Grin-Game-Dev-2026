using UnityEngine;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;

namespace RPG
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rb;

        private Character character;

        private float runThreshold = 0.1f;
        private string lowerLayerName = "Lower";
        private string upperLayerName = "Upper";
        private string standState = "Stand";
        private string runState = "Run";
        private string deathState = "DeathFront";
        private string idleMeleeState = "IdleMelee";
        private string attackState = "SlashMelee1H";
        private float crossFadeTime = 0.1f;

        private int lowerLayer;
        private int upperLayer;

        private bool isDead;
        private bool isAttacking;

        private void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            character = GetComponent<Character>()
                ?? GetComponentInChildren<Character>()
                ?? GetComponentInParent<Character>()
                ?? (animator != null ? animator.GetComponent<Character>() : null)
                ?? (animator != null ? animator.GetComponentInChildren<Character>() : null)
                ?? (animator != null ? animator.GetComponentInParent<Character>() : null);

            if (animator != null && animator.GetComponent<HeroAnimatorEventProxy>() == null)
                animator.gameObject.AddComponent<HeroAnimatorEventProxy>();

            lowerLayer = animator.GetLayerIndex(lowerLayerName);
            upperLayer = animator.GetLayerIndex(upperLayerName);

            if (upperLayer >= 0)
                animator.SetLayerWeight(upperLayer, 1f);
        }

        private void Start()
        {
            PlayLower(standState);
            PlayUpper(idleMeleeState);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Die();
            }

            if (isDead) return;

            HandleMovement();

            if (Input.GetMouseButtonDown(0))
                Attack();
        }

        private void HandleMovement()
        {
            if (isAttacking || isDead) return;

            float speed = rb.linearVelocity.magnitude;

            if (speed > runThreshold)
                PlayLower(runState);
            else
                PlayLower(standState);

            PlayUpper(idleMeleeState);
        }

        public void Attack()
        {
            if (isDead || isAttacking) return;
            StartCoroutine(AttackRoutine());
        }

        private System.Collections.IEnumerator AttackRoutine()
        {
            isAttacking = true;

            PlayUpper(attackState);

            yield return new WaitForSeconds(0.6f);

            if (!isDead)
                PlayUpper(idleMeleeState);

            isAttacking = false;
        }

        public void Die()
        {
            if (isDead) return;

            isDead = true;
            isAttacking = false;

            rb.linearVelocity = Vector2.zero;

            if (character != null)
                character.SetState(CharacterState.DeathF);

            if (upperLayer >= 0)
                animator.SetLayerWeight(upperLayer, 0f);

            animator.Play(deathState, lowerLayer, 0f);
        }

        public float GetDeathDuration()
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return 0f;

            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip != null && clip.name == deathState)
                    return clip.length;
            }

            return 0f;
        }

        private void PlayLower(string stateName)
        {
            if (isDead) return;
            animator.CrossFade(stateName, crossFadeTime, lowerLayer);
        }

        private void PlayUpper(string stateName)
        {
            if (upperLayer < 0) return;
            animator.CrossFade(stateName, crossFadeTime, upperLayer);
        }
    }
}
