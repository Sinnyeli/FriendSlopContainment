using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>Expands downward and keeps decorative shell segments below the last playable floor.</summary>
public sealed class FloorExpansion : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject playableFloorPrefab;
    [Tooltip("Only outer walls/shaft visuals. No hub, floor slab, gameplay scripts or colliders.")]
    [SerializeField] GameObject continuationShellPrefab;
    [Tooltip("Optional existing Floor_00; prevents duplicating your manually built first floor.")]
    [SerializeField] GameObject existingFirstFloor;
    [SerializeField] Transform floorParent;
    [Header("Layout")]
    [SerializeField] Vector3 firstFloorLocalPosition = Vector3.zero;
    [SerializeField, Min(.1f)] float floorSpacing = 15f;
    [SerializeField, Min(1)] int initialUnlockedFloors = 1;
    [SerializeField, Min(0)] int continuationSegments = 3;
    [Header("Events")]
    [SerializeField] UnityEvent onFloorUnlocked;

    readonly List<GameObject> playableFloors = new List<GameObject>();
    readonly List<GameObject> shells = new List<GameObject>();
    bool initialized;
    bool expanding;
    public int UnlockedFloorCount => playableFloors.Count;
    Transform Parent => floorParent != null ? floorParent : transform;

    void Start() { Initialize(); }
    bool Initialize()
    {
        if (initialized) return true;
        if (playableFloorPrefab == null || (continuationSegments > 0 && continuationShellPrefab == null))
        {
            Debug.LogError("Assign playable floor and continuation shell prefabs.", this);
            return false;
        }
        initialized = true;
        if (existingFirstFloor != null)
        {
            existingFirstFloor.transform.SetParent(Parent, false);
            Place(existingFirstFloor, 0);
            existingFirstFloor.SetActive(true);
            playableFloors.Add(existingFirstFloor);
        }
        while (playableFloors.Count < initialUnlockedFloors) CreatePlayableFloor();
        RefreshContinuation();
        return true;
    }

    /// <summary>Call after your purchasing system has validated/paid for a floor unlock.</summary>
    public void UnlockNextFloor() { TryUnlockNextFloor(); }
    public bool TryUnlockNextFloor()
    {
        if (!Application.isPlaying || expanding || !Initialize()) return false;
        expanding = true;
        try
        {
            CreatePlayableFloor();
            RefreshContinuation();
            onFloorUnlocked?.Invoke();
            return true;
        }
        finally { expanding = false; }
    }

    /// <summary>Loads a monotonically increasing unlocked-floor count; never removes live floors.</summary>
    public void EnsureUnlockedFloorCount(int count)
    {
        if (!Application.isPlaying || expanding || count < 1 || !Initialize()) return;
        expanding = true;
        try
        {
            while (playableFloors.Count < count) CreatePlayableFloor();
            RefreshContinuation();
        }
        finally { expanding = false; }
    }

    void CreatePlayableFloor()
    {
        int index = playableFloors.Count;
        var floor = Instantiate(playableFloorPrefab, Parent, false);
        floor.name = "Floor_" + index.ToString("00");
        Place(floor, index);
        floor.SetActive(true);
        playableFloors.Add(floor);
    }
    void Place(GameObject item, int index)
    {
        item.transform.localPosition = firstFloorLocalPosition + Vector3.down * (index * floorSpacing);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
    }
    void RefreshContinuation()
    {
        // Reuse decorative instances rather than rebuilding every time.
        while (shells.Count < continuationSegments)
        {
            var shell = Instantiate(continuationShellPrefab, Parent, false);
            shells.Add(shell);
        }
        // i = index for current shell
        for (int i = 0; i < shells.Count; i++)
        {
            var shell = shells[i];
            if (shell == null) continue;
            bool needed = i < continuationSegments;
            shell.SetActive(needed);
            if (!needed) continue;
            int index = playableFloors.Count + i;
            shell.name = "ContinuationShell_" + index.ToString("00");
            Place(shell, index);
        }
    }
    [ContextMenu("Unlock Next Floor (Play Mode)")]
    void DebugUnlock() { UnlockNextFloor(); }
    void OnValidate()
    {
        floorSpacing = Mathf.Max(.1f, floorSpacing);
        initialUnlockedFloors = Mathf.Max(1, initialUnlockedFloors);
        continuationSegments = Mathf.Max(0, continuationSegments);
    }
}
