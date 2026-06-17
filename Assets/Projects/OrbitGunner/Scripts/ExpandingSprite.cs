using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class ExpandingSprite : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private float _age;
        private float _duration;
        private float _startScale;
        private float _endScale;
        private Color _startColor;

        public static void Spawn(Sprite sprite, Color color, Vector3 position,
            float startScale, float endScale, float duration, int sortingOrder)
        {
            var go = new GameObject("OG_Effect");
            go.transform.position = position;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = sortingOrder;

            var effect = go.AddComponent<ExpandingSprite>();
            effect._renderer = sr;
            effect._duration = Mathf.Max(0.01f, duration);
            effect._startScale = startScale;
            effect._endScale = endScale;
            effect._startColor = color;

            go.transform.localScale = Vector3.one * startScale;
        }

        private void Update()
        {
            _age += Time.deltaTime;
            float t = Mathf.Clamp01(_age / _duration);

            float scale = Mathf.Lerp(_startScale, _endScale, t);
            transform.localScale = Vector3.one * scale;

            Color c = _startColor;
            c.a = Mathf.Lerp(_startColor.a, 0f, t);
            _renderer.color = c;

            if (t >= 1f)
                Destroy(gameObject);
        }
    }
}
