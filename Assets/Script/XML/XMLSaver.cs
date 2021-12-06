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
        Load(path+"/"+name);
    }
    void Save(CardBase cardBase, string path, string name)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        XElement root = new XElement("root");

        root.Add(new XElement("Name", cardBase.Name));

        root.Add(new XElement("Slash", cardBase.Slash));
        root.Add(new XElement("DistSlash", cardBase.DistSlash));
        root.Add(new XElement("NoArmorSlash", cardBase.NoArmorSlash));
        root.Add(new XElement("ArmorBreaker", cardBase.ArmorBreaker));

        root.Add(new XElement("Helmet", cardBase.Helmet));
        root.Add(new XElement("Hp", cardBase.Hp));

        root.Add(new XElement("ShildTayp", cardBase.ShildTayp));
        root.Add(new XElement("Shild", cardBase.Shild));
        
        root.Add(new XElement("Moving", cardBase.Moving));
        root.Add(new XElement("Quirkiness", cardBase.Quirkiness));
        root.Add(new XElement("Evasion", cardBase.Evasion));
        root.Add(new XElement("Somersault", cardBase.Somersault));

        for (int i =0; i < 5; i++)
        {
            root.Add(new XElement("Trait"+i, "Null"));
        }

        root.Add(new XElement("Mana", cardBase.Mana));


        XDocument saveDoc = new XDocument(root);
        File.WriteAllText($"{path}/{name}.xml", saveDoc.ToString());


        Debug.Log($"{path}/{name}.xml");
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

            CardBase card = new CardBase();
            // string PaletteName = root.Value("name");
            //Debug.Log(root.Attribute("Name").Value);
            //card.Name = root.Attribute("Name").Value;

            Debug.Log(path);
        }
    }
}
