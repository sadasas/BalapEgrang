using UnityEngine;
using Player;
using TMPro;
using System.Collections.Generic;


public class ChooseCharacterHandlerUI : MonoBehaviour
{

    int m_currentSelection = 0;
    GameObject[] m_objSelection;
    GameObject m_currentObj;
    ///Rotate character setting
    float m_startPosition;
    bool isSelected;

    [SerializeField] LayerMask m_characterLayer;
    [SerializeField] PlayerType[] m_characterSelection;
    [SerializeField] Transform m_posObj;
    [SerializeField] Vector3 m_nextPosOffset;
    [SerializeField] TextMeshProUGUI m_nameCharacterText;
    [SerializeField] TextMeshProUGUI m_speedText;
    [SerializeField] TextMeshProUGUI m_accelerationText;
    [SerializeField] GameObject m_lock;
    [SerializeField] GameObject m_selectBtn;
    [SerializeField] float m_turnSpeed;

    void Start()
    {
        InitObj();
        LoadDataPlayer();
        ShowCurrentSelection();
    }

    void Update()
    {

        RotateCharacterSelection();

    }

    void LoadDataPlayer()
    {

        var data = PlayerManager.s_Instance.DataPlayer.CurrentCharacterSelection;
        if (data == null || data == "") return;
        foreach (var item in m_characterSelection)
        {
            if (item.Name.Equals(data)) break;
            m_currentSelection++;
        }

    }

    void CheckLock(string name)
    {
        foreach (var item in PlayerManager.s_Instance.DataPlayer.CharacterCollections)
        {
            if (item.Name.Equals(name))
            {
                m_lock.SetActive(false);
                m_selectBtn.SetActive(true);
                return;
            }
        }
        m_selectBtn.SetActive(false);
        m_lock.SetActive(true);
    }

    void RotateCharacterSelection()
    {
        if (Input.touchCount == 0) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (Physics.Raycast(ray, m_characterLayer))
                {
                    m_startPosition = touch.position.x;
                    isSelected = true;
                }
                break;
            case TouchPhase.Moved:
                if (!isSelected) return;
                if (m_startPosition > touch.position.x)
                {
                    m_currentObj.transform.Rotate(Vector3.up, m_turnSpeed * Time.deltaTime);
                }
                else if (m_startPosition < touch.position.x)
                {

                    m_currentObj.transform.Rotate(Vector3.up, -m_turnSpeed * Time.deltaTime);
                }
                break;
            case TouchPhase.Ended:
                isSelected = false;
                break;
            default:
                break;
        }

    }

    void InitObj()
    {
        var listObj = new List<GameObject>();
        var nextPos = m_posObj.position;
        foreach (var data in m_characterSelection)
        {
            var newObj = Instantiate(data.Prefab, nextPos, Quaternion.identity);
            listObj.Add(newObj);
            nextPos += m_nextPosOffset;
        }
        m_objSelection = listObj.ToArray();
    }

    void ShowCurrentSelection()
    {
        var data = m_characterSelection[m_currentSelection];

        CheckLock(data.Name);

        m_nameCharacterText.text = data.Name;
        m_speedText.text = data.Speed.ToString();
        m_accelerationText.text = data.Acceleration.ToString();

        if (m_currentObj) m_currentObj.transform.position += m_nextPosOffset;
        m_currentObj = m_objSelection[m_currentSelection];
        m_currentObj.transform.position = m_posObj.position;
    }

    public void SelectCharacter()
    {
        PlayerManager.s_Instance.UpdateCurrentSelection(m_characterSelection[m_currentSelection]);
        var stage = StageManager.s_Instance.StageSelected.StageIndex;
        GameManager.s_Instance.LoadScene(stage);

    }

    public void NextCharacter()
    {
        if (m_currentSelection + 1 < m_characterSelection.Length)
        {
            m_currentSelection++;
        }
        else
        {
            m_currentSelection = 0;
        }
        ShowCurrentSelection();
    }

    public void BackCharacter()
    {
        if (m_currentSelection - 1 >= 0)
        {
            m_currentSelection--;
        }
        else m_currentSelection = m_characterSelection.Length - 1;
        ShowCurrentSelection();

    }


}
