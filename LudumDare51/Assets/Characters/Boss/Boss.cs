using UnityEngine;

public interface IBoss
{
    void GiveCoffee(GameObject pickedItem);
}

public class Boss : MonoBehaviour, IBoss
{
    [SerializeField] AngryBar _angryBar;
    [SerializeField] Rigidbody bossBody;
    [SerializeField] AudioSource audioSource;
 
    private float initialYPos;

    private IAngryBar AngryBar => _angryBar;

    void Awake()
    {
        AngryBar.OnAngry += BossAngry;
    }

    void Start()
    {
        initialYPos = transform.position.y;
    }

    void Update()
    {
        var pos = bossBody.position;
        pos.y = initialYPos;
        bossBody.MovePosition(pos);

        bossBody.velocity = bossBody.velocity - (bossBody.velocity * 3f * Time.deltaTime);
        bossBody.angularVelocity = Vector3.zero;
    }

    public void GiveCoffee(GameObject pickedItem)
    {
        Destroy(pickedItem.gameObject);
        AngryBar.LessAngry();
    }

    public void BossAngry()
    {
        audioSource.Play();
    }
}
