using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 3f;

    [Header("Work")]
    [SerializeField] private WorkRaycast workRaycast;
    [SerializeField] private WorkAction bareHandWorkAction;

    private PlayerInput inputHandler;
    private PlayerHands playerHands;


    private void Awake()
    {
        inputHandler = GetComponent<PlayerInput>();
        playerHands = GetComponent<PlayerHands>();
    }


    private void Update()
    {
        if (inputHandler.LeftClickPressed)
        {
            HandleHand(HandType.Left);

            inputHandler.ResetLeftClick();
        }


        if (inputHandler.RightClickPressed)
        {
            HandleHand(HandType.Right);

            inputHandler.ResetRightClick();
        }
    }


    private void HandleHand(HandType hand)
    {
        HoldableItems heldItem =
            playerHands.GetItem(hand);


        // ------------------------------------------------
        // HAND HAS AN ITEM
        // ------------------------------------------------

        if (heldItem != null)
        {
            // Let the held item perform its normal function.
            heldItem.Use();


            // Also attempt Work.
            TryWork(hand, heldItem);


            return;
        }


        // ------------------------------------------------
        // HAND IS EMPTY
        // ------------------------------------------------

        TryBareHandWork();

        InteractWithWorld(hand);
    }


    // ====================================================
    // WORK
    // ====================================================

    private void TryWork(
        HandType hand,
        HoldableItems heldItem)
    {
        if (workRaycast == null)
        {
            return;
        }


        AbnormalityTool tool =
            heldItem.GetComponent<AbnormalityTool>();


        if (tool == null)
        {
            return;
        }


        WorkAction workAction =
            tool.GetWorkAction();


        if (workAction == null)
        {
            return;
        }


        workRaycast.TryWork(
            workAction
        );
    }


    private void TryBareHandWork()
{
    Debug.Log(
        "[WORK TEST] TryBareHandWork() called."
    );


    if (workRaycast == null)
    {
        Debug.LogError(
            "[WORK TEST] WorkRaycast is NULL."
        );

        return;
    }


    if (bareHandWorkAction == null)
    {
        Debug.LogError(
            "[WORK TEST] BareHandWorkAction is NULL."
        );

        return;
    }


    Debug.Log(
        "[WORK TEST] Sending Bare Hand Action to WorkRaycast."
    );


    workRaycast.TryWork(
        bareHandWorkAction
    );
}

    // ====================================================
    // NORMAL WORLD INTERACTION
    // ====================================================

    private void InteractWithWorld(
        HandType hand)
    {
        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(
                    0.5f,
                    0.5f,
                    0f
                )
            );


        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactRange))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();


            if (interactable != null)
            {
                interactable.Interact(hand);

                Debug.Log(
                    "Interacted with: " +
                    hit.collider.name
                );
            }
        }
    }
}