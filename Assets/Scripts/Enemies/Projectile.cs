using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public int type;
    public float speed;
    public float pushForce;
    public float deathAnimTime;
    public string strikeSound;
    public string shootSound;

    private bool destroying;
    private Rigidbody2D rb2d;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        destroying = false;
        player = GameObject.Find("player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        FindObjectOfType<AudioManager>().Play(shootSound);
        //Vector2 temp = (player.transform.position - transform.position).normalized;
        //Shoot(temp);
    }
    public void Shoot(Vector2 direction)
    {
        float angle = Vector2.Angle(transform.up, direction);
        if (direction.x > 0f)
            angle *= -1;
        transform.rotation = Quaternion.AngleAxis(angle,new Vector3(0f,0f,1f));
        if (rb2d is null)
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.velocity = (transform.up * speed);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!destroying)
            rb2d.AddForce(transform.up * speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "player")
            player.GetComponent<PlayerController>().TakeDamage(damage, type, transform.position, pushForce);
        FindObjectOfType<AudioManager>().Play(strikeSound);
        StartCoroutine(StartDestruction());
    }
    private IEnumerator StartDestruction()
    {
        destroying = true;
        rb2d.Sleep();
        yield return new WaitForSeconds(deathAnimTime);
        Destroy(gameObject);
    }
}
