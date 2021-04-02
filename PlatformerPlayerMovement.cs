using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool CanJump = true;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpSpeed;

    [SerializeField] private float RayLength;
    [SerializeField] private float RayPositionOffset;

    Vector3 RayPositionCenter;
    Vector3 RayPositionLeft;
    Vector3 RayPositionRight;

    RaycastHit2D[] GroundHitsCenter;
    RaycastHit2D[] GroundHitsLeft;
    RaycastHit2D[] GroundHitsRight;

    RaycastHit2D[][] AllRaycastHits = new RaycastHit2D[3][];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        Jump();
    }

    private void Movement()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.velocity = new Vector2(MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.Space) && CanJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
        }
    }

    private void Jump()
    {
        RayPositionCenter = transform.position + new Vector3(0, RayLength * .5f, 0);
        RayPositionLeft = transform.position + new Vector3(-RayPositionOffset, RayLength * .5f, 0);
        RayPositionRight = transform.position + new Vector3(RayPositionOffset, RayLength * .5f, 0);

        GroundHitsCenter = Physics2D.RaycastAll(RayPositionCenter, -Vector2.up, RayLength);
        GroundHitsLeft = Physics2D.RaycastAll(RayPositionLeft, -Vector2.up, RayLength);
        GroundHitsRight = Physics2D.RaycastAll(RayPositionRight, -Vector2.up, RayLength);

        AllRaycastHits[0] = GroundHitsCenter;
        AllRaycastHits[1] = GroundHitsLeft;
        AllRaycastHits[2] = GroundHitsRight;

        CanJump = GroundCheck(AllRaycastHits);

        /*
        Debug.DrawRay(RayPositionCenter, -Vector2.up * RayLength, Color.red);
        Debug.DrawRay(RayPositionLeft, -Vector2.up * RayLength, Color.red);
        Debug.DrawRay(RayPositionRight, -Vector2.up * RayLength, Color.red);
        */
    }

    private bool GroundCheck(RaycastHit2D[][] GroundHits)
    {
        foreach (RaycastHit2D[] HitList in GroundHits)
        {
            foreach (RaycastHit2D hit in HitList)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag != "PlayerCollider")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
