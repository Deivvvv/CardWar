using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.IO;

public class XMLSaver : MonoBehaviour
{
    void Start()
    {
        CardBase cardBase = new CardBase();
        string path = Application.dataPath + "/Resources";
        string name = "no";
        Save(cardBase, path, name);
    }
    void Save(CardBase cardBase, string path, string name)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        XElement root = new XElement("root");

        root.AddFirst(new XElement("Slash", cardBase.Slash));



        XDocument saveDoc = new XDocument(root);
        File.WriteAllText($"{path}/{name}.xml", saveDoc.ToString());


        Debug.Log(path + name);
    }

    void Load(string path)
    {
        if (path !="")
        {
            XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");// XDocument.Parse(File.ReadAllText(path)).Element("root");
            //XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlAsset.text));
            //while (xmlReader.Read())
            //{
            //    ////if (xmlReader.name == "item")
            //    ////{
            //    //    if (xmlReader.GetAttribute("num") == "0")
            //    //    {
            //    //        // что-то делаем
            //    //    }
            //    ////}
            //}
        }
    }
}
