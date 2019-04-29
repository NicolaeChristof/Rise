using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingObjectBehavior : MonoBehaviour {

    // Public References
    public AudioClip fallingSound;

    public AudioClip thunkSound;
    public Rigidbody rb;
    public GameObject falling;
    // Private References
    private AudioSource _source;

    // Private Fields
    private Vector3 _newScale;
    private Vector3 v;


    // Start is called before the first frame update
    void Start() {

        _source = GetComponent<AudioSource>();

        _newScale = Vector3.zero;

    }

    // Update is called once per frame
    void Update() {
        if(GameModel.paused)
        {
            if (rb.velocity != new Vector3(0,0,0))
            {
                v = rb.velocity;
            }
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            if(rb.velocity == new Vector3(0, 0, 0))
            {
                rb.velocity = v;
            }
        }
           

        if (transform.position.y < 3) {

            Destroy(gameObject);

        }
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(fallingSound, _volume);

        }

    }

    void OnTriggerStay (Collider collider) {



    }

    void OnTriggerExit (Collider collider) {



    }

    void OnCollisionEnter (Collision collision) {

        if (collision.gameObject.tag.Equals("Player")) {

            collision.gameObject.GetComponent<PlayerController>().stunPlayer(0.5f);

            GameModel.squirrelHealth--;
            GameObject Health = GameObject.Find("Health Bar");
            Health.GetComponent<HealthUI>().UpdateHealth();

        }

        float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
        _source.PlayOneShot(thunkSound, _volume);

        transform.DOScale(_newScale, 0.75f)
                .OnComplete(()=>Destroy(gameObject));

    }

    void OnCollisionStay (Collision collision) {



    }

    void OnCollisionExit (Collision collision) {



    }
}
