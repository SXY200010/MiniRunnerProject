using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public AudioClip pickupSound; 

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (ScoreManager.instance != null)
                ScoreManager.instance.AddCoin();

            Destroy(gameObject);
        }
    }
}
