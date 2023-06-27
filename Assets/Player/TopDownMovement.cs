using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private Rigidbody2D rb2d;
    private Vector2 moveInput;
    private bool isFacingLeft = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;

        Vector2 mousePos = Input.mousePosition;
        float widthScale = mousePos.x / (float)Screen.width;
        if(widthScale > 0.5) { // is on right side
            if(isFacingLeft) {
                isFacingLeft = false;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        if(widthScale <= 0.5) { // is on left side
            if(!isFacingLeft) {
                isFacingLeft = true;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
