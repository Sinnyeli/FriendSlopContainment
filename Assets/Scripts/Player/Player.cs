using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Information")]
    [SerializeField] private int playerID;
    [Header("Resources")]
    [SerializeField] private int boxes;
    [Header("Current State")]
    [SerializeField] private PlayerLocation currentLocation;
    [SerializeField] private PlayerStatus currentStatus;

    public int PlayerID
    {
        get
        {
            return playerID;
        }
    }


    public int Boxes
    {
        get
        {
            return boxes;
        }
    }
    public PlayerLocation CurrentLocation
    {
        get
        {
            return currentLocation;
        }
    }

    public PlayerStatus CurrentStatus
    {
        get
        {
            return currentStatus;
        }
    }

    public void SetLocation(PlayerLocation location)
    {
        currentLocation = location;
    }

    public void SetStatus(PlayerStatus status)
    {
        currentStatus = status;
    }
     public void ReceiveBoxes(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        boxes += amount;

        Debug.Log(
            "Player " +
            playerID +
            " received " +
            amount +
            " Boxes. Total: " +
            boxes
        );
    }
}
public enum PlayerStatus
{
    Idle,
    Working,
    Dead
}


public class PlayerLocation : MonoBehaviour
{
    public string LocationName;
}

