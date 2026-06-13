using System.Collections;
using UnityEngine;
public class Cup : MonoBehaviour
{
    public CupGameManager GameManager;
    public Vector3 TargetPosition;
    private float _liftHeight = 2.0f;
    private float _animSpeed = 5.0f;
    public void OnMouseDown()
    {
        if (GameManager != null)
            GameManager.OnCupClicked(this);
    }
    public IEnumerator LiftUp()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * _liftHeight;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * _animSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
    public IEnumerator MoveDown()
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(start.x, 0, start.z); 
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * _animSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
    public IEnumerator MoveTo(Vector3 destination, float duration)
    {
        Vector3 start = transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, destination, t);
            yield return null;
        }
    }
}
