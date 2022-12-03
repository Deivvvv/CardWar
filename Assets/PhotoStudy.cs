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
    public LineRenderer lineRenderer;
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
    void Selector(int a)//альтернатива SwitchPart с открытием селектора при килке по части персонажа
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            UseRayCast();
    }
    void UseRayCast()
    {
        Ray ray = RayCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
            Debug.DrawLine(ray.origin, hit.point);
        Debug.Log(ray);
       // Debug.Log(hit.collider.gameObject);

    }
}
