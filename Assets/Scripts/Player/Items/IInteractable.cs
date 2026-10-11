using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // To use with empty hand.
    void Interact(HandType hand);
}
