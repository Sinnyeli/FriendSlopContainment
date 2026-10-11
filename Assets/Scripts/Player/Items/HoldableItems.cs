using UnityEngine;

public class HoldableItems: MonoBehaviour, IUsable
{
    public virtual void Use()
    {
        Debug.Log(gameObject.name + " used.");
    }
}