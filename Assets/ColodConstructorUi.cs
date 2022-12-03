using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SubSys;

public class ColodConstructorUi : MonoBehaviour
{
    public Transform GuildList;
    public Transform ColodList;
    public TMP_InputField NameTT;
    public Text cardCount;
    public Text colodCount;
    public Text allCardCount;
    public TextMeshProUGUI InfoPanel;
    public GameObject SaveWindow;
    public Transform ColodWindow;
    public CardBody MainBody;
    // public Transform GuildList;
    public TextMeshProUGUI TT;

    void Awake()
    {
        Sorter.SetUi(TT);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PointerClick();
        if (Input.GetMouseButtonDown(1))
        {
            // Ui.TextWindow.active = false;
            // ComandClear();
            // Ui.TextWindow.active = false;
        }
    }
    void PointerClick()
    {
        TMP_LinkInfo linkInfo = new TMP_LinkInfo();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TT, Input.mousePosition, Camera.main);
        if (linkIndex == -1)
            return;
        linkInfo = TT.textInfo.linkInfo[linkIndex];

        string selectedLink = linkInfo.GetLinkID();
        Debug.Log("Open link " + selectedLink);
        Sorter.ReadCode(selectedLink);
    }

}
