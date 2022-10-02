using UnityEngine;

public interface IBoss
{
    void GiveCoffee(GameObject pickedItem);
}

public class Boss : MonoBehaviour, IBoss
{
    [SerializeField] AngryBar _angryBar;
    [SerializeField] Rigidbody bossBody;
    [SerializeField] AudioSource audioSourceMad;
    [SerializeField] AudioSource audioSourceHappy;

    private Animator animator;

    private float initialYPos;

    private IAngryBar AngryBar => _angryBar;

    void Awake()
    {
        AngryBar.OnAngry += BossAngry;
        AngryBar.OnLessAngry += BossLessAngry;
    }

    void Start()
    {
        initialYPos = transform.position.y;
        animator = GetComponent<Animator>();
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
        audioSourceMad.Play();
        animator.SetTrigger("GetAngry");
    }

    public void BossLessAngry()
    {
        audioSourceHappy.Play();
    }
}
