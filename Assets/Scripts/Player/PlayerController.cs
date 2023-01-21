using System;
using UnityEngine;
using System.Collections;

public interface IInputCallback
{
    public event Action OnMove;
    public event Action<Vector3> OnTurn;
}

public class InputBehaviour : IInputCallback
{
    float m_movePressTime = 0;
    float m_turnTreshold;
    float m_turnMinLength;

    public InputBehaviour(float turnTreshold, float turnMinLength)
    {
        m_turnTreshold = turnTreshold;
        m_movePressTime = m_turnTreshold;
        m_turnMinLength = turnMinLength;
    }

    public event Action OnMove;
    public event Action<Vector3> OnTurn;

    public void OnUpdate()
    {
        var tc = Input.touchCount;
        if (tc == 0)
            return;

        var t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Stationary)
        {
            if (m_movePressTime > 0)
                m_movePressTime -= Time.deltaTime;
            OnMove?.Invoke();
        }
        else if (t.phase == TouchPhase.Moved)
        {
            var tdir = t.deltaPosition.normalized;
            if (m_movePressTime > 0 || MathF.Abs(tdir.x) < m_turnMinLength)
                return;
            m_movePressTime = m_turnTreshold;
            OnTurn?.Invoke(tdir);
        }
    }
}

public class MovementBehaviour
{
    public enum Pos
    {
        LEFT,
        CENTER,
        RIGHT
    }

    Transform m_player;
    MonoBehaviour m_mono;
    Coroutine m_currentState;
    IInputCallback m_inputCallback;
    Pos m_currentPost = Pos.CENTER;

    float m_speed;
    float m_turnRange;
    float m_turnSpeed;

    public MovementBehaviour(
        Transform player,
        IInputCallback inputCallback,
        float speed,
        float turnSpeed,
        float turnRange)
    {
        m_player = player;
        m_mono = player.GetComponent<MonoBehaviour>();
        m_inputCallback = inputCallback;
        m_speed = speed;
        m_turnSpeed = turnSpeed;

        m_inputCallback.OnMove += MoveForward;
        m_inputCallback.OnTurn += Turn;
        m_turnRange = turnRange;
    }

    void MoveForward()
    {
        m_player.Translate((m_player.forward * m_speed) * Time.deltaTime, Space.World);
    }

    void Turn(Vector3 input)
    {
        var nextPos = (input.x < 0 ? Pos.LEFT : Pos.RIGHT);

        if (nextPos == Pos.RIGHT && m_currentPost != Pos.RIGHT)
        {
            m_currentPost = m_currentPost == Pos.CENTER ? Pos.RIGHT : Pos.CENTER;
            m_mono.StartCoroutine(Turning(m_turnRange));
        }
        else if (nextPos == Pos.LEFT && m_currentPost != Pos.LEFT)
        {
            m_currentPost = m_currentPost == Pos.CENTER ? Pos.LEFT : Pos.CENTER;
            m_mono.StartCoroutine(Turning(-m_turnRange));
        }
    }

    IEnumerator Turning(float range)
    {
        var dis = 0f;
        var lastPos = m_player.position.x;
        while (dis < MathF.Abs(range))
        {
            dis += MathF.Abs(m_player.position.x - lastPos);
            var dir = Vector3.Lerp(
                m_player.position,
               m_player.position + (m_player.right * range),
                m_turnSpeed
            );
            m_player.position = dir;
            yield return null;
        }
    }
}

public class PlayerController : MonoBehaviour
{
    InputBehaviour m_inputBehaviour;
    MovementBehaviour m_movementBehaviour;

    [Header("Input Setting")]
    [SerializeField]
    float m_turnTreshold;
    [SerializeField]
    float m_turnMinLength;

    [Header("Movement Setting")]
    [SerializeField]
    float m_speed;
    [SerializeField]
    float m_turnSpeed;
    [SerializeField]
    float m_turnRange;

    void Start()
    {
        m_inputBehaviour = new(m_turnTreshold, m_turnMinLength);
        m_movementBehaviour = new(transform, m_inputBehaviour, m_speed, m_turnSpeed, m_turnRange);
    }

    void Update()
    {
        m_inputBehaviour.OnUpdate();
    }
}
