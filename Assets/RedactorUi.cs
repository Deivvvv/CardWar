using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Coder;

public class RedactorUi : MonoBehaviour
{
    public Button ExitButton;
    public List<TextMeshProUGUI> TT;
    public TMP_InputField NameTT;

    // Start is called before the first frame update
    void Awake()
    {
        DeCoder.SetTT(gameObject.GetComponent<RedactorUi>());

        DeCoder.Read("Open|MenuHead");
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
        int linkIndex = -1;
        for (int i = 0; i < TT.Count; i++)
        {
            linkIndex = TMP_TextUtilities.FindIntersectingLink(TT[i], Input.mousePosition, Camera.main);
            if (linkIndex != -1)
            {
                linkInfo = TT[i].textInfo.linkInfo[linkIndex];
                break;
            }
        }
        if (linkIndex == -1)
            return;

        string selectedLink = linkInfo.GetLinkID();
        Debug.Log("Open link " + selectedLink);
        DeCoder.Read(selectedLink);
    }
}
