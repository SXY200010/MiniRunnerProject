using UnityEngine;

public class FogFollower : MonoBehaviour
{
    public Transform player;
    public float distanceAhead = 20f;
    public Vector3 offset = new Vector3(0, 0, 0);

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("FogFollower: 找不到带有 Player 标签的对象！");
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + Vector3.forward * distanceAhead + offset;
        transform.position = targetPos;
    }
}
