using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerHands))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactionMask = ~0;
    private PlayerInput inputHandler;
    private PlayerHands hands;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInput>();
        hands = GetComponent<PlayerHands>();
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("PlayerInteraction needs a player camera.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (inputHandler.LeftClickPressed) HandleHand(HandType.Left);
        if (inputHandler.RightClickPressed) HandleHand(HandType.Right);
    }

    private void HandleHand(HandType hand)
    {
        HoldableItems item = hands.GetItem(hand);
        if (item != null)
        {
            item.Use();
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactionMask, QueryTriggerInteraction.Ignore))
        {
            IInteractable target = hit.collider.GetComponentInParent<IInteractable>();
            target?.Interact(hand);
        }
    }
}
