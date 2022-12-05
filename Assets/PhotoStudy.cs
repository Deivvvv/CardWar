using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoStudy : MonoBehaviour
{
    public Camera RayCamera;
    public Transform LeftLowerArm;
    public Transform LeftArm;
    public Transform LeftHand;
    public Transform LeftHandItem;
    public Transform LeftLeg;

    public Transform RightLowerArm;
    public Transform RightArm;
    public Transform RightHand;
    public Transform RightHandItem;
    public Transform RightLeg;

    public Transform Hips;
    public Transform Torse;
    public Transform Head;
    public Transform Back;
    public Transform Hair;

    int[] index;
    string model;
    void SetModel(string modelName)
    {
        model = modelName;//HumanM
        switch (modelName) 
        {

            case ("Human"):
                index = new int[15];
                break;
        }
        
    }
    void Selector(int a)//������������ SwitchPart � ��������� ��������� ��� ����� �� ����� ���������
    {

    }
    void SwitchPart(int a)
    {
        int size = 0;
        switch (model)
        {
            case ("Human"):
                switch (a)
                {
                    case (0):
                        size = RightLowerArm.childCount;
                        break;
                    case (1):
                        size = RightArm.childCount;
                        break;
                    case (2):
                        size = RightHand.childCount;
                        break;
                    case (3):
                        size = RightHandItem.childCount;
                        break;
                    case (4):
                        size = RightLowerArm.childCount;
                        break;

                    case (5):
                        size = LeftLowerArm.childCount;
                        break;
                    case (6):
                        size = LeftArm.childCount;
                        break;
                    case (7):
                        size = LeftHand.childCount;
                        break;
                    case (8):
                        size = LeftHandItem.childCount;
                        break;
                    case (9):
                        size = LeftLowerArm.childCount;
                        break;

                    case (10):
                        size = Hips.childCount;
                        break;
                    case (11):
                        size = Torse.childCount;
                        break;
                    case (12):
                        size = Head.childCount;
                        break;
                    case (13):
                        size = Back.childCount;
                        break;
                    case (14):
                        size = Hair.childCount;
                        break;
                }
                break;
        }
        index[a]++;
        if (index[a] >= size)
            index[a] = 0;

        SetPart(a);
        ReadModel();
    }
    void SetPart(int a)
    {
        Transform trans =null;
        switch (model)
        {
            case ("Human"):
                switch (a)
                {
                    case (0):
                        trans = RightLowerArm;
                        break;
                    case (1):
                        trans = RightArm;
                        break;
                    case (2):
                        trans = RightHand;
                        break;
                    case (3):
                        trans = RightHandItem;
                        break;
                    case (4):
                        trans = RightLowerArm;
                        break;

                    case (5):
                        trans = LeftLowerArm;
                        break;
                    case (6):
                        trans = LeftArm;
                        break;
                    case (7):
                        trans = LeftHand;
                        break;
                    case (8):
                        trans = LeftHandItem;
                        break;
                    case (9):
                        trans = LeftLowerArm;
                        break;

                    case (10):
                        trans = Hips;
                        break;
                    case (11):
                        trans = Torse;
                        break;
                    case (12):
                        trans = Head;
                        break;
                    case (13):
                        trans = Back;
                        break;
                    case (14):
                        trans = Hair;
                        break;
                }
                break;
        }
        for (int i = 0; i < trans.childCount; i++)
            trans.GetChild(i).gameObject.active = false;

        trans.GetChild(index[a]).gameObject.active = true;
    }
    void ReadModel()
    {

    }

    void Play()
    {
        //0 -acsesyar
    }

    // Start is called before the first frame update
    void Start()
    {
        SetModel("Human");
    }

    // Update is called once per frame


    public List<Material> selectMaterial;//3 ���������� 0- �� ������� 1- �������� -2 �������
    GameObject openTrigger;
    GameObject selectTrigger;
    string openPart ="2";
    string selectPart ="1";
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OpenPart();
        }
    }
    void FixedUpdate()
    {
        UseRayCast();
    }
    void OpenPart()
    {
        if(openPart != selectPart)
        {
            openPart = selectPart;
            if (openTrigger != null)
                openTrigger.GetComponent<Renderer>().material = selectMaterial[0];

            openTrigger = selectTrigger;

            openTrigger.GetComponent<Renderer>().material = selectMaterial[2];
        }
    }
    void UseRayCast()
    {
        Ray ray = RayCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
            if(hit.collider.gameObject.GetComponent<CallBodyPart>())
            {
                GameObject go = hit.collider.gameObject;
                //Debug.Log(go);
                string str = go.GetComponent<CallBodyPart>().NameComponent;
                if (str != selectPart)
                {
                    if (str == openPart)
                        return;
                    if (selectTrigger != null)
                       // if (selectPart != openPart)
                            selectTrigger.GetComponent<Renderer>().material = selectMaterial[0];

                    selectTrigger = go;

                    selectTrigger.GetComponent<Renderer>().material = selectMaterial[1];
                    selectPart = str;
                    if (openTrigger != null)
                        openTrigger.GetComponent<Renderer>().material = selectMaterial[2];
                }

            }
        //Debug.Log(ray);
        //var points = new Vector3[2];
        //points[0] = ray.origin;
        //points[1] = hit.point;
        //lineRenderer.SetPosition(0,points[0]);
        //lineRenderer.SetPosition(1, points[1]);
        // Debug.Log(hit.collider.gameObject);

    }
}
