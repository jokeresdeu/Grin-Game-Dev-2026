using UnityEngine;
using TMPro;

public class TargetCube : MonoBehaviour
{
    [SerializeField] TextMeshPro hpLabel;

    public int   hp         = 1;
    public int   scoreValue = 1;  
    public float fallStep   = 0.7f;

    void Start() => RefreshLabel();

    public void Hit()
    {
        hp--;
        RefreshLabel();

        if (hp <= 0)
        {
            GameManager.Instance?.AddScore(scoreValue);
            KunaiOrbitController.Instance?.AddNewKunai();
            CubeSpawner.Instance?.OnCubeDestroyed(this);
            Destroy(gameObject);
        }
        else
        {
            KunaiOrbitController.Instance?.AddNewKunai();
        }
    }

    public void StepDown()
    {
        transform.position += Vector3.down * fallStep;

        float viewY = Camera.main.WorldToViewportPoint(transform.position).y;
        if (viewY < -0.1f)
        {
            CubeSpawner.Instance?.OnCubeDestroyed(this);
            Destroy(gameObject);
        }
    }

    public void SetHp(int value)
    {
        hp = value;
        RefreshLabel();
    }

    void RefreshLabel()
    {
        if (hpLabel != null) hpLabel.text = hp.ToString();
    }
}
