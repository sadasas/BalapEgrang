
using System;
using UnityEngine;

public interface IInputCallback
{
    public event Action OnMove;
    public event Action<Vector3> OnTurn;

}

public class InputBehaviour : IInputCallback
{

    float m_movePressTime = 0;
    float m_turnTreshold;

    public InputBehaviour(float turnTreshold)
    {
        m_turnTreshold = turnTreshold;

        m_movePressTime = m_turnTreshold;
    }

    public event Action OnMove;
    public event Action<Vector3> OnTurn;


    public void OnUpdate()
    {

        var tc = Input.touchCount;
        if (tc == 0) return;

        var t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Stationary)
        {
            if (m_movePressTime > 0) m_movePressTime -= Time.deltaTime;
            OnMove?.Invoke();
        }
        else if (t.phase == TouchPhase.Moved)
        {
            if (m_movePressTime > 0) return;
            m_movePressTime = m_turnTreshold;
            var tdir = t.deltaPosition.normalized;
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
    IInputCallback m_inputCallback;
    Pos m_currentPost = Pos.CENTER;

    float m_speed;
    float m_turnSpeed;

    public MovementBehaviour(Transform player, IInputCallback inputCallback, float speed, float turnSpeed)
    {
        m_player = player;
        m_inputCallback = inputCallback;
        m_speed = speed;
        m_turnSpeed = turnSpeed;

        m_inputCallback.OnMove += MoveForward;
        m_inputCallback.OnTurn += Turn;
    }


    void MoveForward()
    {
        m_player.Translate((m_player.forward * m_speed) * Time.deltaTime, Space.World);

    }
    void Turn(Vector3 input)
    {
        var nextPos = input.x > 0.9 ? Pos.RIGHT : (input.x < -0.9 ? Pos.LEFT : Pos.CENTER);

        if (nextPos == Pos.RIGHT && m_currentPost != Pos.RIGHT)
        {
            m_currentPost = m_currentPost == Pos.CENTER ? Pos.RIGHT : Pos.CENTER;
            m_player.Translate((m_player.right * m_turnSpeed) * Time.deltaTime, Space.World);
        }
        else if (nextPos == Pos.LEFT && m_currentPost != Pos.LEFT)
        {
            m_currentPost = m_currentPost == Pos.CENTER ? Pos.LEFT : Pos.CENTER;
            m_player.Translate((-m_player.right * m_turnSpeed) * Time.deltaTime, Space.World);

        }

    }
}

public class PlayerController : MonoBehaviour
{
    InputBehaviour m_inputBehaviour;
    MovementBehaviour m_movementBehaviour;

    [Header("Input Setting")]
    [SerializeField] float m_turnTreshold;
    [Header("Movement Setting")]
    [SerializeField] float m_speed;
    [SerializeField] float m_turnSpeed;

    void Start()
    {
        m_inputBehaviour = new(m_turnTreshold);
        m_movementBehaviour = new(transform, m_inputBehaviour, m_speed, m_turnSpeed);
    }

    void Update()
    {
        m_inputBehaviour.OnUpdate();
    }

}
