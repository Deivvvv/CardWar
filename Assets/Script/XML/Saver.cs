using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace Saver
{
    static class XMLSaver
    {
        //GameData
        public static void ISaveGameData(GameData gameData, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("AllCard", gameData.AllCard));

            int a = gameData.BlackList.Count;
            root.Add(new XElement("BlackList", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("BlackList" + i, gameData.BlackList[i]));
            }

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}.xml", saveDoc.ToString());

        }
        public static void ILoadGameData(string path, CardConstructor cardConstructor)
        {

            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                GameData gameData = new GameData();

                gameData.AllCard = int.Parse(root.Element("AllCard").Value);


                int a = int.Parse(root.Element("BlackList").Value);
                gameData.BlackList = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    gameData.BlackList.Add(int.Parse(root.Element($"BlackList{i}").Value));
                }

                cardConstructor.TransfData(gameData, null);

            }

        }

        //CardSet
        public static void ISaveCardSet(CardSet cardSet, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("Name", cardSet.Name));

            int a = cardSet.OrigCard.Count;
            root.Add(new XElement("OrigCard", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("OrigCard" + i, cardSet.OrigCard[i]));
            }

            root.Add(new XElement("OrigCount", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("OrigCount" + i, cardSet.OrigCount[i]));
            }

            root.Add(new XElement("AllCard", 40));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}.xml", saveDoc.ToString());

        }
        public static void ILoadCardSet( string path, ColodConstructor colodConstructor)
        {

            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardSet cardSet = new CardSet();
                cardSet.Name = root.Element("Name").Value;

                cardSet.AllCard = int.Parse(root.Element("AllCard").Value);

                int a = int.Parse(root.Element("OrigCard").Value); 
                cardSet.OrigCard = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardSet.OrigCard.Add(int.Parse(root.Element($"OrigCard{i}").Value));
                }

                cardSet.OrigCount = new List<int>();
                for (int i = 0; i < a; i++)
                {
                    cardSet.OrigCount.Add(int.Parse(root.Element($"OrigCount{i}").Value));
                }


                colodConstructor.TransfData(cardSet);

            }
        }

        //Card
        public static void ISave(CardBase cardBase, string path)
        {

            XElement root = new XElement("root");

            root.Add(new XElement("Name", cardBase.Name));

            int a = cardBase.Stat.Length;
            root.Add(new XElement("Stat", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("Stat" + i, cardBase.Stat[i]));
            }

            a = cardBase.Trait.Length;
            root.Add(new XElement("Trait", a));
            for (int i = 0; i < a; i++)
            {
                root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
            }

            string path1 = "";
            a = cardBase.Image.Length;
            root.Add(new XElement("Image", a));
            for (int i = 0; i < a; i++)
            {
                path1 += $"{cardBase.Image[i]}.";
                // root.Add(new XElement("Trait" + i, cardBase.Trait[i]));
            }
            root.Add(new XElement("ImageSt", path1));

            //string[] subs = path1.Split('.');
            //byte[] = string[] subs

            /*
             byte[] bytes = Encoding.ASCII.GetBytes(someString);
Вам нужно будет превратить его обратно в строку, подобную этой:

string someString = Encoding.ASCII.GetString(bytes);
             */

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

                int a = int.Parse(root.Element("Stat").Value);
                cardBase.Stat = new int[a];
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat[i] = int.Parse(root.Element($"Stat{i}").Value);
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new string[a];
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait[i] = root.Element($"Trait{i}").Value;
                }

                //Load Image
                string path1 = root.Element($"ImageSt").Value;

                string[] subs = path1.Split('.');
                a = int.Parse(root.Element($"Image").Value);
                byte[] bat = new byte[a];
                for (int i = 0; i < a; i++)
                {
                    bat[i] = byte.Parse(subs[i]);
                }
                cardBase.Image = bat;


                cardConstructor.LocalCard.Add(cardBase);

            }
        }
        public static void ILoad(string path, ColodConstructor colodConstructor)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;

                int a = int.Parse(root.Element("Stat").Value);
                cardBase.Stat = new int[a];
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat[i] = int.Parse(root.Element($"Stat{i}").Value);
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new string[a];
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait[i] = root.Element($"Trait{i}").Value;
                }


                //Load Image
                string path1 = root.Element($"ImageSt").Value;

                string[] subs = path1.Split('.');
                a = int.Parse(root.Element($"Image").Value);
                byte[] bat = new byte[a];
                for (int i = 0; i < a; i++)
                {
                    bat[i] = byte.Parse(subs[i]);
                }
                cardBase.Image = bat;

                colodConstructor.LocalCard.Add(cardBase);

            }
        }
        public static void ILoad(string path, Stol stol)
        {
            if (path != "")
            {
                XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

                CardBase cardBase = new CardBase();

                cardBase.Name = root.Element("Name").Value;

                int a = int.Parse(root.Element("Stat").Value);
                cardBase.Stat = new int[a];
                for (int i = 0; i < a; i++)
                {
                    cardBase.Stat[i] = int.Parse(root.Element($"Stat{i}").Value);
                }



                a = int.Parse(root.Element("Trait").Value);
                cardBase.Trait = new string[a];
                for (int i = 0; i < a; i++)
                {
                    cardBase.Trait[i] = root.Element($"Trait{i}").Value;
                }


                //Load Image
                string path1 = root.Element($"ImageSt").Value;

                string[] subs = path1.Split('.');
                a = int.Parse(root.Element($"Image").Value);
                byte[] bat = new byte[a];
                for (int i = 0; i < a; i++)
                {
                    bat[i] = byte.Parse(subs[i]);
                }
                cardBase.Image = bat;

                stol.BufferColod.Add(cardBase);

            }
        }
    }
}
