using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHands : MonoBehaviour
{
    public HoldableItems leftHandItem;
    public HoldableItems rightHandItem;

    public HoldableItems GetItem(HandType hand)
    {
        if (hand == HandType.Left)
        {
            return leftHandItem;
        }

        return rightHandItem;
    }
}