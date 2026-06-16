using UnityEngine;
using UnityEngine.Audio;

public class Hitbox : MonoBehaviour
{
    public bool isBomb = false;
    public GameObject wholeTarget;
    public GameObject slicedTarget;

    private bool isSliced = false;

    public AudioClip sliceSound;
    public AudioMixerGroup sfxMixerGroup;

    public void Slice()
    {
        if (isSliced == false)
        {
            isSliced = true;

            if (isBomb == true)
            {
                if (GameOver.instance != null)
                {
                    GameOver.instance.TriggerGameOver();
                    HealthManager.instance.healthText.gameObject.SetActive(false);
                }

                Destroy(this.gameObject);
            }
            else
            {
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.AddPoints(10);
                }

                wholeTarget.SetActive(false);
                slicedTarget.SetActive(true);

                slicedTarget.transform.SetParent(null);

                Rigidbody2D[] parts = slicedTarget.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D part in parts)
                {
                    Vector2 randomForce = new Vector2(Random.Range(-3f, 3f), Random.Range(1f, 3f));
                    part.AddForce(randomForce, ForceMode2D.Impulse);
                }

                if (sliceSound != null)
                {
                    GameObject audioObject = new GameObject("TempSliceAudio");
                    audioObject.transform.position = transform.position;

                    AudioSource tempAudioSource = audioObject.AddComponent<AudioSource>();
                    tempAudioSource.clip = sliceSound;

                    if (sfxMixerGroup != null)
                    {
                        tempAudioSource.outputAudioMixerGroup = sfxMixerGroup;
                    }

                    tempAudioSource.Play();

                    Destroy(audioObject, sliceSound.length);
                }

                Destroy(slicedTarget, 3f);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            if (isBomb == false)
            {
                if (HealthManager.instance != null)
                {
                    HealthManager.instance.LoseHealth();
                }
            }
            Destroy(this.gameObject);
        }
    }
}