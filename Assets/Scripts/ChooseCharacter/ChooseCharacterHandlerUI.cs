using UnityEngine;
using Player;
using TMPro;
using System.Collections.Generic;


/// <summary>
/// TODO:
/// MAKE current obj selected rotate depend on touch drag position
/// </summary>
public class ChooseCharacterHandlerUI : MonoBehaviour
{

    int m_currentSelection = 0;
    GameObject[] m_objSelection;
    GameObject m_currentObj;

    [SerializeField] PlayerType[] m_characterSelection;
    [SerializeField] Transform m_posObj;
    [SerializeField] Vector3 m_nextPosOffset;
    [SerializeField] TextMeshProUGUI m_nameCharacterText;
    [SerializeField] TextMeshProUGUI m_speedText;
    [SerializeField] TextMeshProUGUI m_accelerationText;

    void Start()
    {

        InitObj();
        ShowCurrentSelection();
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

        m_nameCharacterText.text = data.Name;
        m_speedText.text = data.Speed.ToString();
        m_accelerationText.text = data.Acceleration.ToString();

        if (m_currentObj) m_currentObj.transform.position += m_nextPosOffset;
        m_currentObj = m_objSelection[m_currentSelection];
        m_currentObj.transform.position = m_posObj.position;
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
