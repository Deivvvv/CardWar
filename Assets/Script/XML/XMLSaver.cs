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
        //CardBase cardBase = new CardBase();
        //cardBase.Name = "New Hiro";
        //cardBase.Stat = new int[13];
        //cardBase.Trait = new string[5];
        //string path = Application.dataPath + "/Resources";
        //string name = "no";
        //Save(cardBase, path, name);
        //Load(path+"/"+name);
    }
    void Save(CardBase cardBase, string path, string name)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        XElement root = new XElement("root");

        root.Add(new XElement("Name", cardBase.Name));

        //root.Add(new XElement("Slash", cardBase.Slash));
        //root.Add(new XElement("DistSlash", cardBase.DistSlash));
        //root.Add(new XElement("NoArmorSlash", cardBase.NoArmorSlash));
        //root.Add(new XElement("ArmorBreaker", cardBase.ArmorBreaker));

        //root.Add(new XElement("Helmet", cardBase.Helmet));
        //root.Add(new XElement("Hp", cardBase.Hp));

        //root.Add(new XElement("ShildTayp", cardBase.ShildTayp));
        //root.Add(new XElement("Shild", cardBase.Shild));

        //root.Add(new XElement("Moving", cardBase.Moving));
        //root.Add(new XElement("Quirkiness", cardBase.Quirkiness));
        //root.Add(new XElement("Evasion", cardBase.Evasion));
        //root.Add(new XElement("Somersault", cardBase.Somersault));

        root.Add(new XElement("Stat", cardBase.Stat.Length));
        for (int i = 0; i < cardBase.Stat.Length; i++)
        {
            root.Add(new XElement("Stat" + i, cardBase.Stat[i]));
        }

        root.Add(new XElement("Trait", cardBase.Trait.Length));
        for (int i =0; i < cardBase.Trait.Length; i++)
        {
            root.Add(new XElement("Trait"+i, cardBase.Trait[i]));
        }

     //   root.Add(new XElement("Mana", cardBase.Mana));


        XDocument saveDoc = new XDocument(root);
        File.WriteAllText($"{path}/{name}.xml", saveDoc.ToString());

    }

    void Load(string path)
    {
        if (path !="")
        {
            XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

            CardBase cardBase = new CardBase();

            cardBase.Name = root.Element("Name").Value;


            cardBase.Stat = new int[int.Parse(root.Element("Stat").Value)];
            for (int i = 0; i < cardBase.Stat.Length; i++)
            {
                cardBase.Stat[i] = int.Parse(root.Element($"Stat{i}").Value);
                Debug.Log(cardBase.Stat[i]);
            }




            cardBase.Trait = new string[int.Parse(root.Element("Trait").Value)];
            for (int i = 0; i < cardBase.Trait.Length; i++)
            {
                cardBase.Trait[i] = root.Element($"Trait{i}").Value;
                Debug.Log(cardBase.Trait[i]);
            }

        }
    }
}
