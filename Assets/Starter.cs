using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    //[SerializeField] private BaseUi ui;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("Core");
        GameObject go1 = GameObject.Find("Canvas");
        go.GetComponent<CoreSys>().OpenScene(gameObject.GetComponent<BaseUi>(),go1.transform);
    }

}
