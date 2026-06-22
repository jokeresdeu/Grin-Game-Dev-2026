using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// A short-lived tracer line for a tower shot (hitscan combat has no projectile, so
    /// this is the visual). Self-destroys after a fraction of a second, fading out.
    /// </summary>
    public class ShotEffect : MonoBehaviour
    {
        private const float MaxLife = 0.07f;

        private static Material _shared;

        private LineRenderer _line;
        private Color _color;
        private float _life;

        public static void Spawn(Vector3 from, Vector3 to, Color color)
        {
            var go = new GameObject("Shot");
            var line = go.AddComponent<LineRenderer>();
            line.material = SharedMaterial();
            line.useWorldSpace = true;
            line.positionCount = 2;
            line.SetPosition(0, from);
            line.SetPosition(1, to);
            line.startWidth = 0.07f;
            line.endWidth = 0.07f;
            line.numCapVertices = 2;
            line.textureMode = LineTextureMode.Stretch;
            line.sortingOrder = 8;

            var fx = go.AddComponent<ShotEffect>();
            fx._line = line;
            fx._color = color;
            fx._life = MaxLife;
            fx.Apply(1f);
        }

        private static Material SharedMaterial()
        {
            if (_shared == null)
            {
                Shader shader = Shader.Find("Sprites/Default");
                _shared = new Material(shader) { name = "TD_Tracer" };
            }
            return _shared;
        }

        private void Apply(float alpha)
        {
            Color c = _color;
            c.a = alpha;
            _line.startColor = c;
            _line.endColor = c;
        }

        private void Update()
        {
            _life -= Time.deltaTime;
            if (_life <= 0f)
            {
                Destroy(gameObject);
                return;
            }
            Apply(Mathf.Clamp01(_life / MaxLife));
        }
    }
}
