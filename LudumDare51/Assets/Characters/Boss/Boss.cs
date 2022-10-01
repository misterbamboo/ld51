using UnityEngine;

public interface IBoss
{
    void GiveCoffee(GameObject pickedItem);
}

public class Boss : MonoBehaviour, IBoss
{
    [SerializeField] Rigidbody bossBody;
    private float initialYPos;

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
        print("Mmmh ... Coffee !");
    }
}
