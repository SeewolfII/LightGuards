using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int m_playerNumber;

    // movement variables
    private float m_startZ;

    // jump variables
    private bool m_isJumping, m_doubleJump;
    private float m_jumpTimer, m_jumpPower;
    private float m_gravityMultip = 1;
    private Vector3 m_jumpMove;

    // rotation variables
    private float m_currentAngle;
    private CharacterController m_controller;
    private InputManager m_input;

    // chaos variables
    public Transform m_chaosPlane;
    public PlayerController m_otherPlayer;
    
    private float Perc
    {
        get { return m_perc; }
        set
        {
            m_perc = Mathf.Clamp01(value);

            m_radius = Perc * (m_maxRadius - m_minRadius) + m_minRadius;

            m_size = Perc * (m_maxSize - m_minSize) + m_minSize;
            transform.localScale = Vector3.one * m_size;

            m_speed = m_maxSpeed + m_minSpeed - (Perc * (m_maxSpeed - m_minSpeed) + m_minSpeed);

            m_jumpPower = m_maxJump + m_minJump - (Perc * (m_maxJump - m_minJump) + m_minJump);

            m_gravity = m_maxGravity + m_minGravity - (Perc * (m_maxGravity - m_minGravity) + m_minGravity);
        }
    }

    private float m_perc, m_size, m_radius, m_gravity, m_speed;
    private Material m_chaos;

    // static variables
    private static bool m_canDoubleJump;
    private static float m_jumpTime, m_airControlFactor,
        m_gravityIncrease, m_swapSpeed, m_maxSize, m_minSize,
        m_maxRadius, m_minRadius, m_maxJump, m_minJump,
        m_maxGravity, m_minGravity, m_rotationSpeed,
        m_maxSpeed, m_minSpeed;
    private static AnimationCurve m_jumpCurve;


    private void Awake()
    {
        if (m_playerNumber == 1)
        {
            m_canDoubleJump = PlayerManager.Get.doubleJump;
            m_gravityIncrease = PlayerManager.Get.gravityIncrease;
            m_jumpTime = PlayerManager.Get.jumpTime;
            m_airControlFactor = PlayerManager.Get.airControlFactor;
            m_jumpCurve = PlayerManager.Get.jumpCurve;
            m_rotationSpeed = PlayerManager.Get.rotationSpeed;
            m_swapSpeed = PlayerManager.Get.swapSpeed;

            m_maxSpeed = PlayerManager.Get.maxSpeed;
            m_minSpeed = PlayerManager.Get.minSpeed;
            m_maxSize = PlayerManager.Get.maxPlayerSize;
            m_minSize = PlayerManager.Get.minPlayerSize;
            m_maxRadius = PlayerManager.Get.maxPlayerRadius;
            m_minRadius = PlayerManager.Get.minPlayerRadius;
            m_maxJump = PlayerManager.Get.maxPlayerJump;
            m_minJump = PlayerManager.Get.minPlayerJump;
            m_maxGravity = PlayerManager.Get.maxGravity;
            m_minGravity = PlayerManager.Get.minGravity;
        }

        m_controller = GetComponent<CharacterController>();
        m_jumpTimer = m_jumpTime;
        m_startZ = transform.position.z;

        Perc = 0.5f;
        m_size = Perc * (m_maxSize + m_minSize);
        m_radius = Perc * (m_maxRadius + m_minRadius);
        m_speed = Perc * (m_maxSpeed + m_minSpeed);
        m_jumpPower = Perc * (m_maxJump + m_minJump);
        m_gravity = Perc * (m_maxGravity + m_minGravity);
    }

    private void Start()
    {
        m_input = InputManager.Get;
        m_chaos = m_chaosPlane.GetComponent<Renderer>().material;

        Swap();
    }

    private void Update()
    {
        // update chaos material
        m_chaos.SetVector("_Player" + m_playerNumber + "_Pos", transform.position);
        m_chaos.SetFloat("_Player" + m_playerNumber + "_Rad", m_radius);


        // DEBUG
        if (InputManager.PlayerCount < 2)
            return;


        Move();

        if (Input.GetKey(m_input.PlayerSet["Swap" + m_playerNumber]))
            Swap();
    }

    private void Move()
    {
        float value = 0;
        value += Input.GetKey(m_input.PlayerSet["Right" + m_playerNumber]) ? 1 : 0;
        value -= Input.GetKey(m_input.PlayerSet["Left" + m_playerNumber]) ? 1 : 0;

        // Movement
        Vector3 move = new Vector3(value, 0, 0);
        move = move * Time.deltaTime * m_speed;

        // Air Control
        if (m_isJumping)
        {
            move = (move + (m_airControlFactor * m_jumpMove)) / (m_airControlFactor + 1);
            move.y = 0;
        }

        // add jumping
        if (Input.GetKeyDown(m_input.PlayerSet["Jump" + m_playerNumber]))
        {
            if (m_controller.isGrounded)
            {
                m_jumpMove = move;
                m_isJumping = true;
            }
            else if (m_canDoubleJump && !m_doubleJump)
            {
                m_isJumping = true;
                m_jumpTimer = m_jumpTime;
                m_jumpMove = move;
                m_doubleJump = true;
            }
        }
        else
        {
            // apply gravity
            move.y += m_gravity * Time.deltaTime * m_gravityMultip;
            m_jumpMove = move;
        }

        // add jumping
        if (m_isJumping)
        {
            if (m_jumpTimer > 0)
            {
                move.y += m_jumpCurve.Evaluate(1 - (m_jumpTimer / m_jumpTime)) * m_jumpPower;
                m_jumpTimer -= Time.deltaTime;

                // hit ceiling check
                Ray ray = new Ray(transform.position, Vector3.up);
                if (Physics.SphereCast(ray, m_controller.radius * 0.8f * m_size, m_controller.height / 2  * m_size))
                    m_jumpTimer = 0;
            }

            if (m_jumpTimer <= 0 && m_controller.isGrounded)
            {
                m_isJumping = false;
                m_jumpMove = Vector3.zero;
                m_jumpTimer = m_jumpTime;
                m_doubleJump = false;
            }
        }

        // calculate gravitation multiplier
        if (m_controller.isGrounded || move.y > 0) m_gravityMultip = 1;
        else if (move.y < 0) m_gravityMultip += m_gravityIncrease;

        // Apply final movement
        m_controller.Move(move);

        // lock z axis
        transform.position = new Vector3(transform.position.x, transform.position.y, m_startZ);
        
        // calculate rotation
        if (value > 0)
            m_currentAngle = Mathf.Clamp(m_currentAngle - m_rotationSpeed
            * Time.deltaTime, -90, 90);
        else if (value < 0)
            m_currentAngle = Mathf.Clamp(m_currentAngle + m_rotationSpeed
            * Time.deltaTime, -90, 90);
        
        // set rotation
        transform.eulerAngles = Vector3.up * m_currentAngle;
    }

    private void Swap()
    {
        Ray ray = new Ray(m_otherPlayer.transform.position, Vector3.up);
        if (Physics.SphereCast(ray, m_controller.radius * m_otherPlayer.m_size, (m_controller.height / 2) * m_otherPlayer.m_size))
            return;

        float amount = Time.deltaTime * m_swapSpeed;

        float target = Perc - amount;
        if (target < 0)
            amount -= target;

        Perc -= amount;
        m_otherPlayer.Perc = 1 - Perc;
    }
}