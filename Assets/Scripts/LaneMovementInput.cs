using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LaneMovementInput : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float laneDistance = 4f;
    public float jumpForce = 8f;
    public float gravity = -20f;
    public float laneChangeSmooth = 10f;

    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 25f;

    private int currentLane = 1;
    private int lastLane = 1;
    private float verticalVelocity;
    private Vector3 targetPosition;

    private CharacterController controller;
    private Animator anim;

    private float sideHitTimer = 0f;
    private int sideHitCount = 0;
    public float sideHitTimeWindow = 1.5f;

    private float lastSideHitTime = -999f;
    public float sideHitCooldown = 0.3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        ApplyGravity();
        Move();
        Animate();

        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += speedIncreaseRate * Time.deltaTime;
        }

        if (sideHitTimer > 0)
        {
            sideHitTimer -= Time.deltaTime;
            if (sideHitTimer <= 0)
                sideHitCount = 0;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    void Move()
    {
        Vector3 move = Vector3.forward * forwardSpeed;
        move.y = verticalVelocity;

        float targetX = (currentLane - 1) * laneDistance;
        float deltaX = targetX - transform.position.x;
        move.x = deltaX * laneChangeSmooth;

        controller.Move(move * Time.deltaTime);
    }

    public void MoveLeft()
    {
        lastLane = currentLane;
        currentLane = Mathf.Max(0, currentLane - 1);
    }

    public void MoveRight()
    {
        lastLane = currentLane;
        currentLane = Mathf.Min(2, currentLane + 1);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    void Animate()
    {
        float blendValue = controller.velocity.magnitude / forwardSpeed;
        anim.SetFloat("Blend", blendValue);

        bool isJumping = !controller.isGrounded;
        anim.SetBool("IsJumping", isJumping);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            Vector3 hitDir = hit.point - transform.position;
            hitDir.y = 0;
            hitDir.Normalize();

            float dot = Vector3.Dot(hitDir, transform.forward);

            if (dot > 0.7f)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("TallObstacle"))
                {
                    GameOverManager.instance.GameOver();
                }

                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("LowObstacle"))
                {
                    if (controller.isGrounded)
                    {
                        GameOverManager.instance.GameOver();
                        Debug.Log("落地");
                    }
                    else
                    {
                        Debug.Log("跳跃穿过矮障碍，合法");
                        return;
                    }
                }
            }
            else 
            {
                if (Time.time - lastSideHitTime > sideHitCooldown)
                {
                    lastSideHitTime = Time.time;

                    if (sideHitTimer > 0)
                    {
                        sideHitCount++;
                    }
                    else
                    {
                        sideHitCount = 1;
                    }

                    sideHitTimer = sideHitTimeWindow;

                    Debug.Log("Hit obstacle from side, count: " + sideHitCount);

                    currentLane = lastLane;
                    Debug.Log("Side collision! Bounce back to lane: " + currentLane);

                    if (sideHitCount >= 2)
                    {
                        GameOverManager.instance.GameOver();
                    }
                }
                else
                {
                    Debug.Log("Side hit ignored due to cooldown.");
                }
            }
        }
    }
}