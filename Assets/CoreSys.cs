using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Coder;
using XMLSaver;

public class CoreSys : MonoBehaviour
{
    public RuleMainFrame frame;
    public List<BD> bD;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 15;
        bD = Saver.Load

        OpenScene("Main");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OpenScene(string str)
    {
        SceneManager.LoadScene(str, LoadSceneMode.Additive);
        switch (str)
        {
            case ("Main"):



                break;
        }
    }
}
