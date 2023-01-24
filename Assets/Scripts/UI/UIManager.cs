using UnityEngine;
using System.Collections.Generic;

public enum HUDType
{
    CTE,
}
public class UIManager : MonoBehaviour
{
    public static UIManager s_this;

    Transform m_mainCanvas;
    Dictionary<HUDType, GameObject> m_HUDs;
    [SerializeField] GameObject m_CTEhud;


    void Awake()
    {
        if (s_this != null) Destroy(s_this.gameObject);
        s_this = this;

    }

    void Start()
    {
        m_mainCanvas = GameObject.FindWithTag("MainCanvas").transform;
        InitHUD();
    }

    public void ForceHUD(HUDType hud)
    {
        m_HUDs[hud].SetActive(true);

    }

    public GameObject GetHUD(HUDType type)
    {
        return m_HUDs[type];
    }

    void InitHUD()
    {
        m_HUDs = new();
        var cte = Instantiate(m_CTEhud, m_mainCanvas);
        cte.SetActive(false);
        m_HUDs.Add(HUDType.CTE, cte);
    }
}
