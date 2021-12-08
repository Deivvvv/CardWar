using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Saver
{
    static class XMLSaver// : MonoBehaviour
    {

        public static void ISave(CardBase cardBase, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("Name", cardBase.Name));

            root.Add(new XElement("Stat", cardBase.Stat.Length));
            for (int i = 0; i < cardBase.Stat.Length; i++)
            {
                root.Add(new XElement("Stat" + i, cardBase.Stat[i]));
            }

            root.Add(new XElement("Trait", cardBase.Trait.Length));
            for (int i = 0; i < cardBase.Trait.Length; i++)
            {
                root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
            }

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}.xml", saveDoc.ToString());

        }

        public static void ILoad(string path, CardConstructor cardConstructor)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;


                cardBase.Stat = new int[int.Parse(root.Element("Stat").Value)];
                for (int i = 0; i < cardBase.Stat.Length; i++)
                {
                    cardBase.Stat[i] = int.Parse(root.Element($"Stat{i}").Value);
                }




                cardBase.Trait = new string[int.Parse(root.Element("Trait").Value)];
                for (int i = 0; i < cardBase.Trait.Length; i++)
                {
                    cardBase.Trait[i] = root.Element($"Trait{i}").Value;
                }

                //if (a > -1)
                //{
                //    cardConstructor.LocalCard[a] = cardBase;
                //}
                //else
                //{
                    cardConstructor.LocalCard.Add(cardBase);
               // }

            }
        }

        public static void ILoad(string path, ColodConstructor colodConstructor)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;


                cardBase.Stat = new int[int.Parse(root.Element("Stat").Value)];
                for (int i = 0; i < cardBase.Stat.Length; i++)
                {
                    cardBase.Stat[i] = int.Parse(root.Element($"Stat{i}").Value);
                }




                cardBase.Trait = new string[int.Parse(root.Element("Trait").Value)];
                for (int i = 0; i < cardBase.Trait.Length; i++)
                {
                    cardBase.Trait[i] = root.Element($"Trait{i}").Value;
                }

                colodConstructor.LocalCard.Add(cardBase);

            }
        }
    }
}
