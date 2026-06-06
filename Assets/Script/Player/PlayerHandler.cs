using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject interactTextGameObject;

    private Vector2 moveInput;
    private IInteractable currentInteractable;

    private PlayerInstanceData PlayerData => GameManager.Instance?.PlayerInstanceData;

    public void Initialize()
    {
        GameManager.Instance.DialogueManager.AddDialogueCameraTarget(ConstString.Entity.Player, transform);
    }

    void Update()
    {
        ReadInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void ReadInput()
    {
        if (!PlayerData.IsPlayerCanControl)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        FlipSprite();
        UpdateAnimation();

        if (currentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentInteractable.Interact();
                interactTextGameObject.SetActive(false);
            }
                
        }
    }

    private void Move()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void FlipSprite()
    {
        if (spriteRenderer == null || moveInput.x == 0f) return;
        spriteRenderer.flipX = moveInput.x < 0f;
    }

    private void UpdateAnimation()
    {
        animator.SetBool("IsMoving", moveInput != Vector2.zero);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleInteractEnter(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleInteractExit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleInteractEnter(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HandleInteractExit(other.gameObject);
    }

    private void HandleInteractEnter(GameObject go)
    {
        IInteractable interactable = go.GetComponent<IInteractable>();

        if (interactable != null && currentInteractable == null)
        {
            if (interactable.CanInteract())
            {
                interactTextGameObject.SetActive(true);
                currentInteractable = interactable;
            }
        }
    }

    private void HandleInteractExit(GameObject go)
    {
        IInteractable interactable = go.GetComponent<IInteractable>();

        if (interactable != null && currentInteractable == interactable)
        {
            interactTextGameObject.SetActive(false);
            currentInteractable = null;
        }
    }
}
