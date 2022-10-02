using System.Linq;
using UnityEngine;

public class NPCFollowPath : MonoBehaviour
{
    private const float NearByDistance = 0.1f;
    [SerializeField] float speedPerUnit = 2f;
    [SerializeField] NPCPath npcPath;
    [SerializeField] bool keepNPCPosY = true;

    private Vector3[] pathToFollow;
    private int targetCheckpointIndex;
    private Rigidbody npcBody;
    private Vector3 currentForce;

    void Start()
    {
        npcBody = GetComponent<Rigidbody>();
        UpdatePathToFollow();
        npcPath.OnPathChanged += NpcPath_OnPathChanged;
    }

    private void NpcPath_OnPathChanged()
    {
        UpdatePathToFollow();
    }

    private void UpdatePathToFollow()
    {
        print("Update npc path");
        pathToFollow = npcPath.GeneratePath().Select(p =>
        {
            if (keepNPCPosY)
            {
                p.y = transform.position.y;
            }
            return p;
        }).ToArray();

        targetCheckpointIndex = FindClosedCheckpointIndex();
    }

    private int FindClosedCheckpointIndex()
    {
        var npcPosition = transform.position;
        var closestDistance = float.MaxValue;
        var closestIndex = 0;
        // Even points (indexes: 1, 3, 5, ...) are for bezier curve
        // Odd points (indexes: 0, 2, 4, ...) are actual checkpoints
        for (int i = 0; i < pathToFollow.Length; i += 2)
        {
            var distance = (npcPosition - pathToFollow[i]).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    private void FixedUpdate()
    {
        var sectionStartPoint = transform.position;
        var sectionTargetPoint = pathToFollow[targetCheckpointIndex];
        var direction = sectionTargetPoint - sectionStartPoint;
        var targetForce = direction.normalized * speedPerUnit * Time.fixedDeltaTime;

        var appliedForce = Vector3.Lerp(currentForce, targetForce, 5 * Time.fixedDeltaTime);

        npcBody.velocity = Vector3.zero;
        npcBody.AddForce(appliedForce);
        CheckNextPathSection(direction);

        currentForce = appliedForce;
    }

    private void Update()
    {
        var sectionTargetPoint = pathToFollow[targetCheckpointIndex];
        var direction = sectionTargetPoint - transform.position;
        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
    }

    private void CheckNextPathSection(Vector3 direction)
    {
        if (direction.magnitude < NearByDistance)
        {
            targetCheckpointIndex++;
            if (targetCheckpointIndex >= pathToFollow.Length)
            {
                targetCheckpointIndex = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pathToFollow != null && targetCheckpointIndex < pathToFollow.Length)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathToFollow[targetCheckpointIndex], NearByDistance);
        }
    }
}
