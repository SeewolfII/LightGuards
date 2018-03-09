using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton
    public static PlayerManager Instance;
    public static PlayerManager Get { get { return Instance; } }

    [Header("Movement")]
    public float rotationSpeed = 720;
    public float maxSpeed = 6;
    public float minSpeed = 4;

    [Header("Gravity")]
    public float gravityIncrease = 0.05f;
    public float maxGravity = -8;
    public float minGravity = -13;

    [Header("Jump")]
    public bool doubleJump;
    public float airControlFactor = 2;
    public float jumpTime = 0.33f;
    public AnimationCurve jumpCurve;
    public float maxPlayerJump = 1;
    public float minPlayerJump = 0.2f;

    [Header("Swap")]
    public float swapSpeed = 3;
    public float maxPlayerSize = 2;
    public float minPlayerSize = 0.5f;

    [Header("Chaos")]
    public float maxPlayerRadius = 12;
    public float minPlayerRadius = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}