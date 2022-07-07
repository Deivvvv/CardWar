using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using TMPro;

using UnityEditor;
using UnityEngine.TextCore;
using UnityEngine.U2D;
using System.Linq;

using Coder;




namespace XMLSaver
{
    static class Saver
    {
        static List<string> nullString = new List<string>();
        static List<int> nullInt = new List<int>();

        static XElement root = null;
        static string mainPath = Application.dataPath + $"/Resources/";
        static CoreSys core;
        public static void SetCore(CoreSys coreSys) { core = coreSys; }
        static string v = "V1", actualV = "V1";

        #region Lang
        static string lang;
        public static void LoadDataLang(string str) { lang = str; }
        static string FindLang(string path, int a)
        {
            XElement root = null;
            //string str = $"{path}/{lang}/{a}.L" 

            Debug.Log($"{path}L/{lang}/{a}.L");
            if (File.Exists($"{path}L/{lang}/{a}.L"))
                root = XDocument.Parse(File.ReadAllText($"{path}/L/{lang}/{a}.L")).Element("root");
            else if(File.Exists($"{path}L/Eng/{a}.L"))
                root = XDocument.Parse(File.ReadAllText($"{path}/L/Eng/{a}.L")).Element("root");

            string str = "";
            if(root != null)
                str = root.Element("I").Value;

            Debug.Log(str);
            return str;

        }
        static void AddLangFile(string path,string str,int a)
        {
            Debug.Log(path);
            path = $"{path}L/{lang}/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            XElement root = new XElement("root");

            root.Add(new XElement("I", str));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{path}{a}.L", saveDoc.ToString());
        }
        static void RemoveWord(string path,int a)
        {
            XElement root = null;
            string str = "";
            string[] com1;
            List<string> com;

            str = FindLang(path, a);
            int f = str.Split('/').Length;
            com1 = Directory.GetFiles($"{path}L/", "*", SearchOption.TopDirectoryOnly);
            for(int i=0;i< com1.Length; i++)
            {
                Debug.Log(com1[i]);
                path = $"{com1[i]}/{a}.L";
                if (File.Exists(path))
                {
                    root = XDocument.Parse(File.ReadAllText(path)).Element("root");
                    str = root.Element("I").Value;
                    com = new List<string>(str.Split('/'));
                    if (com.Count >= f)
                    {
                        com.RemoveAt(a);
                        if (com.Count == 0)
                            File.Delete(path);
                        else
                        {
                            str = com[0];
                            for (int i1 = 1; i1 < com.Count; i1++)
                                str += "/" + com[i1];

                            root = new XElement("root");

                            root.Add(new XElement("I", str));

                            XDocument saveDoc = new XDocument(root);
                            File.WriteAllText(path, saveDoc.ToString());
                        }
                       
                    }
                }
               
            }
        }
        static void RemoveLocal(string path, int a)
        {
            string[] com = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < com.Length; i++)
            {
                path = $"{com[i]}/{a}.L";
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        static string GetWord(string path, int a)
        {
            string str1 ="",str2="";
            //TranfWord()
            return str1 + "*" + str2;
        }

        static void TranfWord(string path, string text, int a)
        {
            string[] com = text.Split('*');
            string[] lang = com[0].Split('/');
            com = com[1].Split('/');

            string str = "";
            for (int i=0; i< lang.Length; i++)
            {
                str = $"{path}L/{lang[i]}/{a}.L";
                if (File.Exists(path))
                {
                    root = XDocument.Parse(File.ReadAllText(path)).Element("root");
                    str = root.Element("I").Value;
                    str += "/" + com[i];
                }
                else
                    str = com[i];

                root = new XElement("root");

                root.Add(new XElement("I", str));

                XDocument saveDoc = new XDocument(root);
                File.WriteAllText(path, saveDoc.ToString());

            }
        }
        #endregion

        #region BD
        //static void NewKeyConfig()
        //{
        //    if (v != actualV)
        //    {
        //        root = XDocument.Parse(File.ReadAllText($"{path}{i}/-1.H")).Element("root");
        //        str = root.Element("Key").Value;
        //        bd.Key = new List<string>(str.Split('/'));
        //    }
        //    else
        //    {
        //        root = XDocument.Parse(File.ReadAllText($"{path}{i}/-1.H")).Element("root");
        //        str = root.Element("Key").Value;
        //        List<string> list1 = new List<string>(str.Split('/'));
        //        List<string> list2 = core.frame.SetKey(i); 
        //        if (list1.Count != list2.Count)
        //        {
        //            NewKeyConfig(i);
        //        }
        //        else
        //        {

        //        }
        //    }
        //    root = XDocument.Parse(File.ReadAllText($"{path}{i}/-1.H")).Element("root");
        //    str = root.Element("Key").Value;
        //    List<string> list1 = new List<string>(str.Split('/'));
        //    List<string> list2 = core.frame.SetKey(i);
        //    if(list1.Count == list2.Count)
        //    {

        //    }
        //    bool indent = true;
        //    for (int i1 =0;i1<)

        //}

        static void BDReload(int a)
        { 
            core.bD = new List<BD>(new BD[core.frame.Tayp.Count]);
            string path = mainPath + "BD/";
            for (; a < core.frame.Tayp.Count; a++)
                if (!Directory.Exists($"{path}{a}/"))
                {
                    Directory.CreateDirectory($"{path}{a}/");
                    core.bD[a] = new BD();
                    core.bD[a].Name = core.frame.Tayp[a];
                    core.bD[a].Key =core.frame.SetKey(a);
                    SaveBDMain(a);
                }
        }

        public static void LoadBDAll()
        {
            mainPath = Application.dataPath + $"/Resources/{v}";
            int l = core.frame.Tayp.Count;
            List<BD> bD = new List<BD>(new BD[l]);
            string path = mainPath + "BD/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            if (fCount + 1 < l)
                BDReload(fCount);

            string[] com;
            string str = "";
            BD bd = null;
            for (int i=0;i< l; i++)
            {
                bd = new BD();
                str = FindLang($"{path}{i}/", -1);
                com = str.Split('/');
                bd.Name = com[0];
                bd.Info = com[1];
                bD[i] = bd;
                root = XDocument.Parse(File.ReadAllText($"{path}{i}/-1.H")).Element("root");
                str = root.Element("Key").Value;
                bd.Key = new List<string>(str.Split('/'));
                bd.KeyId = new int[bd.Key.Count];
                bd.Hide = new bool[bd.Key.Count];
                for (int i1=0; i1 < bd.Key.Count;i1++)
                    bd.KeyId[i1] = DeCoder.ReturnIndex(bd.Key[i1]);

                // com = Directory.GetFiles($"{path}{i}/", "*.H");
                fCount = Directory.GetFiles($"{path}{i}/", "*.H").Length;
                bd.Base = new List<MainBase>(new MainBase[fCount]);
                for (int i1 = 0; i1 < fCount; i1++)
                  LoadBD(i, i1);


            }


            core.bD = bD;
        }
        public static void SaveBDMain(int a)
        {
            string path = mainPath + $"BD/{a}/";
            string str = $"{core.bD[a].Name}/{core.bD[a].Info}";
            //Debug.Log(str);
            AddLangFile(path, str, -1);
            root = new XElement("root");

            root.Add(new XElement("Key", ReturnListData(core.bD[a].Key)));


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"-1.H", saveDoc.ToString());
        }

        public static void LoadBD( int a, int b)
        {
            string path = mainPath + $"BD/{a}/";
            string[] com, com1;
            string str = "";
            MainBase mainBase = new MainBase();
            root = XDocument.Parse(File.ReadAllText($"{path}{b}.H")).Element("root");

            mainBase.Color = root.Element("Color").Value;
            mainBase.Cost = int.Parse( root.Element("Cost").Value);
            mainBase.Look = bool.Parse(root.Element("Look").Value);

            if(core.frame.Tayp[a] == "Stat")
            {
                mainBase.Sub = new SubInt();
                mainBase.Sub.Regen = bool.Parse(root.Element("Regen").Value);
                mainBase.Sub.Antipod = int.Parse(root.Element("Antipod").Value);
                str = root.Element("AntiStat").Value;
                if (str != " ")
                    mainBase.Sub.AntiStat = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                else
                    mainBase.Sub.AntiStat = nullInt;

               str = root.Element("DefStat").Value;
                if (str != " ")
                    mainBase.Sub.DefStat = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                else
                    mainBase.Sub.DefStat = nullInt;
            }

            mainBase.Text = new List<SubText>(new SubText[core.bD[a].Key.Count]);


            for (int i = 0; i < mainBase.Text.Count; i++)
            {
                str = root.Element("Text"+i).Value;

                if (str != " ")
                    mainBase.Text[i].Text = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                else
                    mainBase.Text[i].Text = nullInt;
            }

            str = FindLang(path, b);
            if (str != "")
            {
                com = str.Split('/');
                mainBase.Name = com[0];
                mainBase.Info = com[1];
            }

            core.bD[a].Base[b] = mainBase;
        }
        static string ReturnListData(List<int> list)
        {
            string str = " ";
            if (list.Count > 0)
            {
                str = ""+list[0];
                for (int i = 1; i < list.Count; i++)
                    str += $"/{list[i]}";
            }
            return str;
        }
        static string ReturnListData(List<string> list)
        {
            string str = " ";
            if (list.Count > 0)
            {
                str = list[0];
                for (int i = 1; i < list.Count; i++)
                    str += $"/{list[i]}";
            }
            return str;
        }

        public static void SaveBD( int a, int b)
        {
            string[] com = null;
            string str ="";
            string path = mainPath + $"BD/{a}/";
            MainBase mainBase = core.bD[a].Base[b];

            str = $"{mainBase.Name}/{mainBase.Info}";
            AddLangFile(path, str, a);

            root = new XElement("root");

            root.Add(new XElement("Color", mainBase.Color));
            root.Add(new XElement("Cost", mainBase.Cost));
            root.Add(new XElement("Look", mainBase.Look));

            for (int i = 0; i < core.bD[a].Key.Count; i++)
                root.Add(new XElement("Text" + i, ReturnListData(core.bD[a].Base[b].Text[i].Text)));

            if (core.frame.Tayp[a] == "Stat")
            {
                root.Add(new XElement("Regen", mainBase.Sub.Regen));

                root.Add(new XElement("Antipod", mainBase.Sub.Antipod));

                root.Add(new XElement("AntiStat", ReturnListData(mainBase.Sub.AntiStat)));
                root.Add(new XElement("DefStat", ReturnListData(mainBase.Sub.DefStat)));
            }


            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{a}.H", saveDoc.ToString());
        }



        #endregion

        #region Rule
        static void RuleReload( int a, int f)
        {
            string path = mainPath + "Rule/";
            for (; a < f; a++)
            {
                if (!Directory.Exists($"{path}{a}/"))
                {
                    Directory.CreateDirectory($"{path}{a}/");
                    SaveRuleMain(new SubRuleHead(),a);
                }
            }
        }

        public static void LoadAllRule()
        {
            string[] com = null, com1 =null;
            string str = "";
            string path = mainPath + "Rule/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            int f = DeCoder.ReturnIndex("Tag");

            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            if (fCount+1 < core.bD[f].Base.Count)
                RuleReload( fCount, core.bD[f].Base.Count);

            List<SubRuleHead> head = new List<SubRuleHead>(new SubRuleHead[core.bD[f].Base.Count]);
            for(int i =0; i < head.Count;i++)
            {
                head[i] = new SubRuleHead();
                head[i].Name = core.bD[f].Base[i].Name;
                root = XDocument.Parse(File.ReadAllText($"{path}{i}.HR")).Element("root");
                str = root.Element("Id").Value;
                //com = str.Split('/');
                head[i].Index = new List<int>(str.Split('/').Select(int.Parse).ToArray());
                f = head[i].Index.Count;
                head[i].Rule = new List<string>(new string[f]);
                if (f > 0)
                    head[i].LastIndex = head[i].Index[f - 1] +1;

                str = FindLang($"{path}{i}", -1);
                com = str.Split('/');
                if (com.Length < f)
                {
                    int a = com.Length;
                    string oldLang = lang;
                    lang = "Eng";
                    str = FindLang($"{path}{i}", -1);
                    com = str.Split('/');

                    for (int i1 = a; i1 < f; i1++)
                        head[i].Rule[i1] = com[i1];

                    lang = oldLang;
                    str = FindLang($"{path}{i}", -1);
                    com = str.Split('/');
                }

                for (int i1 = 0; i1 < com.Length; i1++)
                    head[i].Rule[i1] = com[i1];

                if (com.Length < f)
                    SaveRuleMain(head[i], i);

            }

            core.head = head;
        }
        static void SaveRuleMain(SubRuleHead head, int a)
        {
            string path = mainPath + "Rule/";
            root = new XElement("root");

            if (head.Index.Count > 0)
            {
                string str1 = "" +head.Index[0];
                string str2 = head.Rule[0];
                for (int i = 1; i < head.Index.Count; i++)
                {
                    str1 += "/" + head.Index[i];
                    str2 += "/" + head.Rule[i];
                }
                root.Add(new XElement("Id", str1));
                AddLangFile(path, str2, a);

            }
            else
                root.Add(new XElement("Id", " "));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path +$"{a}.HR", saveDoc.ToString());

        }

        public static HeadRule LoadRule(int a, int b)
        {
            HeadRule head = new HeadRule();
            string path = mainPath + $"Rule/{a}/{b}.R";
            XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

            return head;
        }
        public static void SaveRule(int a, int b)
        {
            string path = mainPath + $"Rule/{a}/{b}.R";
            root = new XElement("root");
            
            root.Add(new XElement("Id", " "));

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path + $"{a}.R", saveDoc.ToString());

        }

        public static void TransfRule(int a, int b, bool full)
        {
            //Over - мега верси€, востанваливающа€ маршруты в механиках и картах
            //full - ут€желенна€ верси€ с востановливающа€ маршруты в механиках
            //lite - стандарнаы€ веср€и где мигрирует только локализаци€
            //easy - онли перенос без миграции локализации
        }

        public static void DeliteRule(int a, int b)
        {
            string path = mainPath + $"Rule/";
            RemoveWord(path, a);
            path += $"{a}/";
            RemoveLocal(path, b);
            path += $"{b}.R";
            File.Delete(path);

            core.head[a].Rule.RemoveAt(b);
            core.head[a].Index.RemoveAt(b);
            if (core.head[a].Index.Count == 0)
                core.head[a].LastIndex = 0;
            SaveBDMain(a);
        }
        #endregion

        //#region GameData

        //public static void SaveGameData(GameData gameData)
        //{
        //    if (gameData == null)
        //        return;

        //    SubGameData sub = null;
        //    for (int i = 0; i < gameData.Data.Count; i++)
        //    {
        //        sub = gameData.Data[i];

        //        if (sub.Size > 0)
        //        {
        //            XElement root = new XElement("root");

        //            root.Add(new XElement("Size", sub.Size));
        //            root.Add(new XElement("Key", sub.Key));

        //            XDocument saveDoc = new XDocument(root);
        //            File.WriteAllText(Application.dataPath + "/Resources/Data/" + sub.MasterKey + "Data.xml", saveDoc.ToString());
        //        }
        //    }
        //}

      
        //public static void LoadGameData(SubGameData sub)
        //{
        //    string path = Application.dataPath + "/Resources/Data/"+ sub.MasterKey + "Data.xml";
        //    Debug.Log(path);
        //    if (!File.Exists(path))
        //        return;
        //    XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");
        //    sub.Key = root.Element("Key").Value;
        //    sub.Size = int.Parse(root.Element("Size").Value);
        //   // Debug.Log(sub.Key);
        //    //Debug.Log(path);
        //    //GameData gameData = new GameData();
        //    //string str;
        //    //string[] com;

        //    ////if (File.Exists($"{path}/Data.xml"))
        //    ////{
        //    //XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
        //    //gameData.Size = int.Parse(root.Element("Size").Value);

        //    //str = root.Element("Data").Value;
        //    //Debug.Log(str);
        //    //com = str.Split('_');
        //    //gameData.Guild = new List<GameDataData>();
        //    //for (int i = 0; i < com.Length; i++)
        //    //    gameData.Guild.Add(LoadGameDataRoot(path, com[i]));

        //    //return gameData;
        //}
        //#endregion
        //#region LoadData

        //public static BD LoadBD(string path)
        //{
        //    BD bd = new BD();
        //    if (File.Exists(path))
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");
        //        bd.Name = root.Element("Name").Value;
        //        bd.Info = root.Element("Info").Value;
        //    }

        //    return bd;
        //}
        //public static void SaveAllBD(List<BD> bD)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/";
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    for (int i = 0; i < bD.Count; i++)
        //    {
        //        if (!Directory.Exists(path + $"/{i}/"))
        //            Directory.CreateDirectory(path + $"/{i}/");
        //        SaveBD(bD[i], path + $"{i}.{lang}");
        //        //SaveBase(i, 0, new MainBase());
        //    }

        //}
        //public static void SaveBD(BD bD, string path)
        //{
        //    XElement root = new XElement("root");
        //    root.Add(new XElement("Name", bD.Name));
        //    root.Add(new XElement("Info", bD.Info));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());

        //}


        //public static List<BD> LoadAllData()
        //{
        //    List<BD> bD = new List<BD>();
        //    string path = Application.dataPath + $"/Resources/Data/";
        //    string[] tayp = Directory.GetFiles(path, $"*.{lang}");
        //    {
        //        string[] tayp1 = Directory.GetFiles(path, $"*.Eng");
        //        if (tayp.Length != tayp1.Length)
        //            tayp = tayp1;
        //    }

        //    for (int i = 0; i < tayp.Length; i++)
        //    {
        //        bD.Add(LoadBD(tayp[i]));

        //       string[] com = Directory.GetFiles(path + $"/{i}/", "*.H");


        //        for (int i1 = 0; i1 < com.Length; i1++) 
        //        {
        //            bD[i].Base.Add(LoadBase(com[i1], tayp[i])); 
        //        }

        //    }
        //    return bD;

        //}
        //public static MainBase LoadBase(string path, string tayp)
        //{
        //    MainBase mainBase = new MainBase();
        //    XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

        //   // mainBase.SysName = root.Element("Sys").Value;
        //   // mainBase.Tayp = tayp;
        //    mainBase.ColorName = root.Element("ColorName").Value;

        //    //switch(tayp){}


        //    if (File.Exists(path + "_" + lang))
        //    {
        //        root = XDocument.Parse(File.ReadAllText(path + "_" + lang)).Element("root");
        //    }
        //    else if (File.Exists(path + "_Eng"))
        //    {
        //        root = XDocument.Parse(File.ReadAllText(path + "_Eng")).Element("root");
        //    }
        //    else
        //        return mainBase;

        //    mainBase.Name = root.Element("Name").Value;
        //    mainBase.Info = root.Element("Info").Value;

        //    return mainBase;
        //}

        //public static void SaveBase(int a, int b, MainBase mainBase)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/{a}/{b}.H_{lang}";
        //    XElement root = new XElement("root");
        //    root.Add(new XElement("Name", mainBase.Name));
        //    root.Add(new XElement("Info", mainBase.Info));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());


        //    path = Application.dataPath + $"/Resources/Data/{a}/{b}.H";
        //    root = new XElement("root");
        //    root.Add(new XElement("ColorName", mainBase.ColorName));
        //    //root.Add(new XElement("Info", mainBase.Info));

        //    saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());
        //}
        //#endregion
        //public static void LoadAtlas(SpriteRenderer sr, TMP_SpriteAsset  tmp)
        //{
        //    //string path = Application.dataPath + $"/Resources/Icon/";
        //    //string[] com = Directory.GetFiles(path, "*.png");

        //    //Texture2D[] textures = new Texture2D[1024];
        //    //Texture2D tx;
        //    //for (int i = 0; i < com.Length; i++)
        //    //{
        //    //    tx = new Texture2D(2, 2);
        //    //    byte[] FileData = File.ReadAllBytes(com[i]);
        //    //    tx.LoadImage(FileData);
        //    //    textures[i] = tx;
        //    //}




        //    //tx = new Texture2D(1024, 1024);
        //    //tx.PackTextures(textures, 0, 1024);

        //    ////sr.sprite = Sprite.Create(tx, new Rect(0.0f, 0.0f, tx.width, tx.height), new Vector2(0.5f, 0.5f), 100.0f);

        //    ////tmp.spriteSheet = tx;
        //    ////tmp.UpdateLookupTables();
        //    //path = Application.dataPath + $"/Resources/D.png";

        //    //byte[] bytes = tx.EncodeToPNG();
        //    //File.WriteAllBytes(path, bytes);//.png

        //}
        //#region CardSet
        //public static void SaveCardSets(List<string> list, string guild)
        //{
        //    string path = Application.dataPath + $"/Resources/CardSet/{guild}.xml";


        //    XElement root = new XElement("root");
        //    string str;
        //    if (list.Count == 0)
        //        str = " ";
        //    else
        //    {
        //        str = list[0];
        //        for (int i = 1; i < list.Count; i++)
        //            str += "/" + list[i];
        //    }
        //    root.Add(new XElement("Path", str));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //}
        //public static List<string> LoadCardSets(string guild)
        //{
        //    List<string> colod;

        //    string path = Application.dataPath + $"/Resources/CardSet/{guild}";
        //    if (Directory.Exists(path))
        //        Directory.CreateDirectory(path);

        //    path += ".xml";
        //    if (File.Exists(path))
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

        //        string str = root.Element("Path").Value;
        //        if (str != " ")
        //            colod = new List<string>(str.Split('/'));
        //        else
        //            colod = new List<string>();
        //    }
        //    else
        //        colod = new List<string>();

        //    return colod;
        //}
        //public static void SaveCardSet(CardSet cardSet, string path)
        //{
        //    path = Application.dataPath + $"/Resources/CardSet/" + path + ".xml";
        //    XElement root = new XElement("root");

        //    string str = " ";
        //    if (cardSet.Path.Count > 0)
        //    {
        //        str = cardSet.Path[0];
        //        for (int i = 1; i < cardSet.Path.Count; i++)
        //            str += "/" + cardSet.Path;
        //    }
        //    root.Add(new XElement("Path", str));


        //    str = " ";
        //    if (cardSet.Size.Count > 0)
        //    {
        //        str = ""+cardSet.Size[0];
        //        for (int i = 1; i < cardSet.Size.Count; i++)
        //            str += "/" + cardSet.Size;
        //    }
        //    root.Add(new XElement("Size", str));

        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path, saveDoc.ToString());

        //}
        //public static CardSet LoadCardSet( string name)
        //{
        //    string path = Application.dataPath + $"/Resources/CardSet/" + name+".xml";
        //    CardSet cardSet = new CardSet();
        //    string[] com = name.Split('/');
        //    cardSet.Name = com[1];
        //    cardSet.Path = new List<string>();
        //    cardSet.Size = new List<int>();
        //    if (File.Exists(path))
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText(path)).Element("root");

        //        string str = root.Element("Path").Value;
        //        cardSet.Path = new List<string>(str.Split('/'));

        //        str = root.Element("Size").Value;
        //        com = str.Split('/');
        //        foreach (string str1 in com)
        //            cardSet.Size.Add(int.Parse(str1));

        //    }
        //    return cardSet;
        //}
        //#endregion
        //#region Card

        ////public static void SetData(string path)
        ////{
        ////    root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
        ////    //card = 
        ////}

        //public static Sprite LoadTexture(string FilePath)
        //{
        //    // Load a PNG or JPG file from disk to a Texture2D
        //    // Returns null if load fails
        //    byte[] FileData = File.ReadAllBytes(FilePath+".X");
        //    Texture2D texture = new Texture2D(2, 2);
        //    texture.LoadImage(FileData);

        //    return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            
        //}
            

        //public static void Save(CardBase cardBase, string path)
        //{
        //    path = Application.dataPath + $"/Resources/Data/" + path;
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    path += ""+cardBase.Id;
        //    XElement root = new XElement("root");

        //    root.Add(new XElement("Name", cardBase.Name));

        //    root.Add(new XElement("Guild", cardBase.Guilds.Name));

        //    root.Add(new XElement("Races", cardBase.Races.Name));
        //    root.Add(new XElement("Legions", cardBase.Legions.Name));
        //    root.Add(new XElement("CivilianGroups", cardBase.CivilianGroups.Name));

        //    root.Add(new XElement("Tayp", cardBase.Tayp));
        //    root.Add(new XElement("Mana", cardBase.Mana));


        //    string str1 ="", str2 = "";
        //    for (int i = 0; i < cardBase.Stat.Count; i++)
        //    {
        //        str1 += cardBase.Stat[i].Name;
        //        str2 += cardBase.StatSize[i];

        //        if(i < cardBase.Stat.Count - 1)
        //        {
        //            str1 += "_";
        //            str2 += "_";
        //        }
        //    }
        //    root.Add(new XElement("Stat", str1));
        //    root.Add(new XElement("StatSize", str2));
        //    str1 = "";

        //    for (int i = 0; i < cardBase.Trait.Count; i++)
        //    {
        //        str1 += cardBase.Trait[i];

        //        if (i < cardBase.Trait.Count - 1)
        //            str1 += "_";
        //    }
        //    if (cardBase.Trait.Count == 0)
        //        str1 = " ";

        //    root.Add(new XElement("Trait", str1));


        //    Texture2D itemBGTex = cardBase.Image.texture;
        //    byte[] bytes = itemBGTex.EncodeToPNG();
        //    File.WriteAllBytes(path+".X", bytes);//.png


        //    XDocument saveDoc = new XDocument(root);
        //    File.WriteAllText(path +".xml", saveDoc.ToString());

        //}



        //public static CardBase Load(string path)
        //{
        //    path = Application.dataPath + $"/Resources/Data/" + path;
        //  //  Debug.Log(path);
        //    CardBase cardBase = new CardBase();
        //    XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
            

        //    #region Main
        //    cardBase.Name = root.Element("Name").Value;
        //        //Debug.Log(path);

        //        string data = root.Element("Guild").Value;
        //        int a = gameSetting.Library.Guilds.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.Guilds = gameSetting.Library.Guilds[a];

        //        data = root.Element("Races").Value;
        //        a = cardBase.Guilds.Races.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.Races = cardBase.Guilds.Races[a];

        //        data = root.Element("Legions").Value;
        //        a = cardBase.Guilds.Legions.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.Legions = cardBase.Guilds.Legions[a];

        //        data = root.Element("CivilianGroups").Value;
        //        a = cardBase.Legions.CivilianGroups.FindIndex(x => x.Name == data);
        //        if (a < 0)
        //            Debug.Log(data);
        //        cardBase.CivilianGroups = cardBase.Legions.CivilianGroups[a];
        //    #endregion
        //    cardBase.Tayp = root.Element("Tayp").Value;

        //    cardBase.Mana = int.Parse(root.Element("Mana").Value);

        //    data = root.Element("Stat").Value;
        //    string[] com = data.Split('_');
        //    cardBase.Stat = new List<Constant>();
        //    for (int i = 0; i < com.Length; i++)
        //    {
        //        cardBase.Stat.Add( gameSetting.Library.Constants.Find(x => x.Name == com[i]));
        //    }

        //    data = root.Element("StatSize").Value;
        //    com = data.Split('_');
        //    cardBase.StatSize = new List<int>();
        //    cardBase.StatSizeLocal = new List<int>();
        //    for (int i = 0; i < com.Length; i++)
        //    {
        //        cardBase.StatSize.Add(int.Parse(com[i]));
        //        cardBase.StatSizeLocal.Add(0);
        //    }
        //    data = root.Element("Trait").Value;
        //    com = data.Split('_');
        //    if (com[0] != " ")
        //        cardBase.Trait = new List<string>(com);
        //    else
        //        cardBase.Trait = new List<string>();

        //    for(int i =0;i<cardBase.Trait.Count; i++)
        //    {
        //        cardBase.TraitSize.Add(0);
        //    }


        //    cardBase.Image = LoadTexture(path);





        //    return cardBase;
        //}
        //#endregion
        //#region Rule
        //public static void LoadMainRule( ActionLibrary library)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
        //    if (path != "")
        //    {
        //        XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
                
        //        string str = root.Element("RuleTag").Value;
        //        string[] com = str.Split('_');
        //        List<SubRuleHead> text = new List<SubRuleHead>();
        //        SubRuleHead subRule = null;
        //        for (int i =0; i < com.Length; i++)
        //        {
        //            subRule = new SubRuleHead();

        //            str = root.Element($"RuleName{i}").Value;
        //            string[] com1 = str.Split('_');

        //            subRule.Name = com[i];
        //            subRule.Rule = new List<string>();

        //            foreach( string str1 in com1)
        //                subRule.Rule.Add(str1);


        //            text.Add(subRule);
        //        }

        //        library.Rule = text;
        //    }
        //}
        //public static void SaveMainRule( ActionLibrary library)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/Rule/MainRule"; ;
        //    if (path != "")
        //    {
        //        XElement root = new XElement("root");
        //        string str;
        //        string strMain = "";

        //        //root.Add(new XElement($"RuleSize", library.Rule.Count));

        //        for (int i =0;i< library.Rule.Count;i++)
        //        {
        //            strMain += library.Rule[i].Name;
        //            str = "";

        //            for (int i1 = 0; i1 < library.Rule[i].Rule.Count; i1++)
        //            {
        //                str += library.Rule[i].Rule[i1];
        //                if(i1+1 < library.Rule[i].Rule.Count)
        //                    str += "_";

        //            }
        //            root.Add(new XElement($"RuleName{i}", str));

        //            if (i + 1 < library.Rule.Count)
        //                str += "_";
        //        }

        //        root.Add(new XElement($"RuleTag", strMain));


        //        XDocument saveDoc = new XDocument(root);
        //        File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //    }
        //}

        //private static string SaveRuleCore(RuleForm ruleForm)
        //{
        //    string text = "" + ruleForm.Card
        //        + "_" + ruleForm.StatTayp
        //        + "_" + ruleForm.Stat
        //        + "_" + ruleForm.Mod
        //        + "_" + ruleForm.Num;

        //    return text;
        //}
        //private static string SaveRuleCoreSimple(RuleForm ruleForm)
        //{
        //    string text = "" + ruleForm.Card 
        //        + ":" + ruleForm.StatTayp 
        //        + "_" + ruleForm.Stat 
        //        + "_" + ruleForm.Mod 
        //        + "_" + ruleForm.Num;

        //    return text;
        //}
        //private static RuleForm LoadRuleCore(string str)
        //{
        //    RuleForm ruleForm = new RuleForm();

        //    string[] subs = str.Split('_');
        //    ruleForm.Card = subs[0];
        //    ruleForm.StatTayp = subs[1];
        //    ruleForm.Stat = subs[2];
        //    ruleForm.Mod = int.Parse(subs[3]);
        //    ruleForm.Num = int.Parse(subs[4]);

        //    return ruleForm;
        //}

        //private static XElement SaveRuleIfAction(IfAction ifAction, XElement root, string text)
        //{
        //    RuleForm ifCore = null;
            
        //    root.Add(new XElement($"{text}Result", ifAction.Result));
        //    root.Add(new XElement($"{text}Prioritet", ifAction.Prioritet));
        //    root.Add(new XElement($"{text}Point", ifAction.Point));

        //    root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
        //    string str = "";// + b2 = ifAction.Core.; 
        //    for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
        //    {
        //        ifCore = ifAction.Core[i2];
        //        str = SaveRuleCore(ifCore);
        //        root.Add(new XElement($"{text}Core{i2}", str));
        //    }
        //    return root;
        //}
        //private static XElement SaveRuleAction(RuleAction ifAction, XElement root, string text)
        //{
        //    RuleForm ifCore = null;
        //    root.Add(new XElement($"{text}Name", $"{ifAction.Name}"));
        //    root.Add(new XElement($"{text}Point", $"{ifAction.MinPoint}|{ifAction.MaxPoint}"));
        //    root.Add(new XElement($"{text}ActionMood", ifAction.ActionMood));
        //    root.Add(new XElement($"{text}Action", ifAction.Action));
        //   // root.Add(new XElement($"{text}Num", ifAction.Num));
        //    root.Add(new XElement($"{text}ForseMood", ifAction.ForseMood));

        //    root.Add(new XElement($"{text}CoreCount", ifAction.Core.Count));
        //    string str = "";// + b2 = ifAction.Core.; 
        //    for (int i2 = 0; i2 < ifAction.Core.Count; i2++)
        //    {
        //        ifCore = ifAction.Core[i2];
        //        str = SaveRuleCore(ifCore);
        //        root.Add(new XElement($"{text}Core{i2}", str));
        //    }
        //    return root;
        //}

        //private static IfAction LoadRuleIfAction(XElement root, string text)
        //{
        //    IfAction ifAction = new IfAction();

        //    ifAction.Result = int.Parse(root.Element($"{ text}Result").Value);
        //    ifAction.Prioritet = int.Parse(root.Element($"{ text}Prioritet").Value);
        //    ifAction.Point = int.Parse(root.Element($"{ text}Point").Value);

        //    int a = int.Parse(root.Element($"{ text}CoreCount").Value);

        //    for (int i = 0; i < a; i++)
        //    {
        //        ifAction.Core.Add(LoadRuleCore(root.Element($"{text}Core{i}").Value));
        //    }

        //    return ifAction;
        //}
        //private static RuleAction LoadRuleAction(XElement root, string text)
        //{
        //    RuleAction ifAction = new RuleAction();


        //    ifAction.Name = root.Element($"{ text}Name").Value;

        //    string str = root.Element($"{ text}Point").Value;
        //    string[] subs = str.Split('|');

        //    ifAction.MinPoint = int.Parse(subs[0]);
        //    ifAction.MaxPoint = int.Parse(subs[1]);

        //    ifAction.ActionMood = int.Parse(root.Element($"{ text}ActionMood").Value);
        //    ifAction.Action = root.Element($"{ text}Action").Value;
        //   // ifAction.Num = int.Parse(root.Element($"{ text}Num").Value);
        //    ifAction.ForseMood = int.Parse(root.Element($"{ text}ForseMood").Value);
            
        //    int a = int.Parse(root.Element($"{ text}CoreCount").Value); 
            
        //    for (int i = 0; i < a; i++)
        //    {
        //        ifAction.Core.Add(LoadRuleCore(root.Element($"{text}Core{i}").Value));
        //    }

        //    return ifAction;

        //}

        //private static string SaveRuleIfActionSimple(IfAction ifAction)
        //{
        //    string text =
        //        "" + frame.EqualString[ifAction.Result]
        //        + "_" + ifAction.Prioritet
        //        + "_" + ifAction.Point + "*";
        //    int b = ifAction.Core.Count;
        //    for (int i = 0; i < b; i++)
        //    {
        //        text += SaveRuleCoreSimple(ifAction.Core[i]);
        //        if (i + 1 != b)
        //            text += "|";
        //    }
        //    return text;
        //}
        //private static string SaveRuleActionSimple(RuleAction ifAction)
        //{
        //    string text =
        //        "" + ifAction.MinPoint
        //        + "_" + ifAction.MaxPoint
        //        + "_" + frame.PlayerString[ifAction.ActionMood]
        //        + "_" + ifAction.Action
        //      //  + "_" + ifAction.Num
        //        + "_" + frame.ForseTayp[ifAction.ForseMood]
        //        + "*";

        //    int b = ifAction.Core.Count;
        //    for (int i = 0; i < b; i++)
        //    {
        //        text += SaveRuleCoreSimple(ifAction.Core[i]);
        //        if (i + 1 != b)
        //            text += "|";
        //    }

        //    return text;
        //}


        //public static void SaveSimpleRule(HeadRule head)
        //{
        //    SimpleTrigger simpleTrigger = new SimpleTrigger();
        //    string path = Application.dataPath + $"/Resources/Data/SimpleRule/{head.Tag}_{head.Name}";
        //    if (path != "")
        //    {
        //        int a = 0;
        //        XElement root = new XElement("root");
        //        string str = "" + head.Name
        //           + "_" + head.Cost
        //           + "_" + head.CostExtend
        //           + "_" + head.LevelCap
        //           + "_" + head.Player;
        //        root.Add(new XElement("Head", str));

        //        str = "";
        //        int b = head.NeedRule.Count;
        //        for (int i = 0; i < b; i++)
        //        {
        //            str += head.NeedRule[i];
        //            if (i + 1 != b)
        //                str += "_";
        //        }
        //        root.Add(new XElement("HeadNeedRule", str));

        //        str = "";
        //        b = head.EnemyRule.Count;
        //        for (int i = 0; i < b; i++)
        //        {
        //            str += head.EnemyRule[i];
        //            if (i + 1 != b)
        //                str += "_";
        //        }
        //        root.Add(new XElement("HeadEnemyRule", str));

        //        str = "";
        //        TriggerAction triggerAction = null;
        //        IfAction ifAction = null;
        //        RuleAction ruleAction = null;
        //        string text = "";
        //        int b1 = 0;
        //        int b2 = 0;
        //        b = head.TriggerActions.Count;

        //        for (int i = 0; i < b; i++)
        //        {
        //            triggerAction = head.TriggerActions[i];
        //            str += frame.PlayerString[triggerAction.TargetPalyer]
        //                +"_" + frame.Trigger[triggerAction.Trigger]
        //                + "_" + triggerAction.CountMod
        //                + "_" + triggerAction.CountModExtend;

        //            text = "";
        //            b1 = triggerAction.PlusAction.Count;
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text += SaveRuleIfActionSimple(triggerAction.PlusAction[i1]);
        //                if (i1 + 1 != b1)
        //                    text += "/";
        //            }
        //            root.Add(new XElement($"Trigger{i}PlusAction", text));

        //            text = "";
        //            b1 = triggerAction.MinusAction.Count;
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text += SaveRuleIfActionSimple(triggerAction.MinusAction[i1]);
        //                if (i1 + 1 != b1)
        //                    text += "/";
        //            }
        //            root.Add(new XElement($"Trigger{i}MinusAction", text));


        //            text = "";
        //            b1 = triggerAction.Action.Count;
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {

        //                if(triggerAction.Action[i1].Action == "ActionFlash")
        //                {   
        //                    a = triggerAction.Action[i1].Core.Count-1;
        //                    triggerAction.Action[i1].Action = "PreAction";
        //                    text += SaveRuleActionSimple(triggerAction.Action[i1]);

        //                    triggerAction.Action[i1].Core[a].Mod = -triggerAction.Action[i1].Core[a].Mod;
        //                    triggerAction.Action[i1].Action = "PostAction";
        //                    text += SaveRuleActionSimple(triggerAction.Action[i1]);

        //                    triggerAction.Action[i1].Core[a].Mod = -triggerAction.Action[i1].Core[a].Mod;
        //                    triggerAction.Action[i1].Action = "ActionFlash";
        //                }
        //                else
        //                    text += SaveRuleActionSimple(triggerAction.Action[i1]);

        //                if (i1 + 1 != b1)
        //                    text += "/";
        //            }
        //            root.Add(new XElement($"Trigger{i}Action", text));


        //            if (i + 1 != b)
        //                str += "|";
        //        }
        //        root.Add(new XElement("TriggerHead", str));
        //        XDocument saveDoc = new XDocument(root);
        //        File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //    }

        //}


        //private static XElement root;
        //public static void SetSimpleRoot(string tag)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/SimpleRule/{tag}";
        //    if (path != "")
        //    {
        //        root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");
        //    }
        //}
        //public static string LoadSimpleRule(string mood, int b = 0 )
        //{
        //    string path = "";
        //    switch (mood)
        //    {
        //        case ("Head"):
        //            path = root.Element($"Head").Value;
        //            break;
        //        case ("HeadNeedRule"):
        //            path = root.Element($"HeadNeedRule").Value;
        //            break;
        //        case ("HeadEnemyRule"):
        //            path = root.Element($"HeadEnemyRule").Value;
        //            break;
        //        case ("Trigger"):
        //            path = root.Element($"TriggerHead").Value;
        //            break;
        //        case ("TriggerPartPlus"):
        //            path = $"{root.Element($"Trigger{b}PlusAction").Value}";
        //            break;
        //        case ("TriggerPartMinus"):
        //            path = $"{root.Element($"Trigger{b}MinusAction").Value}";
        //            break;
        //        case ("TriggerPart"):
        //            path = $"{root.Element($"Trigger{b}Action").Value}";
        //            break;
        //    }
        //    return path;
        //}

        //public static void SaveRule(HeadRule head)
        //{
        //    string path = Application.dataPath + $"/Resources/Data/Rule/{head.Tag}_{head.Name}"; ;
        //    if (path != "")
        //    {
        //        XElement root = new XElement("root");

        //        root.Add(new XElement("HeadName", head.Name));
        //        root.Add(new XElement("HeadCost", head.Cost));
        //        root.Add(new XElement("HeadNameText", head.NameText));
        //        root.Add(new XElement("Tag", head.Tag));
        //        root.Add(new XElement("HeadCostExtend", head.CostExtend));
        //        root.Add(new XElement("HeadLevelCap", head.LevelCap));
        //        root.Add(new XElement("HeadPlayer", head.Player));


        //        int b = head.NeedRule.Count;
        //        root.Add(new XElement("HeadNeedRuleCount", b));
        //        for (int i = 0; i < b; i++)
        //        {
        //            root.Add(new XElement($"HeadNeedRule{i}", head.NeedRule[i]));
        //        }

        //        b = head.EnemyRule.Count;
        //        root.Add(new XElement("HeadEnemyRuleCount", b));
        //        for (int i = 0; i < b; i++)
        //        {
        //            root.Add(new XElement($"HeadEnemyRule{i}", head.EnemyRule[i]));
        //        }


        //        TriggerAction triggerAction = null;
        //        IfAction ifAction = null;
        //        RuleAction ruleAction = null;

        //        b = head.TriggerActions.Count;

        //        root.Add(new XElement("TriggerActionCount", b));

        //        string text = "";
        //        int b1 = 0;
        //        int b2 = 0;

        //        for (int i = 0; i < b; i++)
        //        {
        //            triggerAction = head.TriggerActions[i];


        //            //public bool CountMod;
        //            //public bool CountModExtend;
        //            root.Add(new XElement($"TargetPalyer{i}", triggerAction.TargetPalyer));
        //            root.Add(new XElement($"Trigger{i}", triggerAction.Trigger));


        //            root.Add(new XElement($"CountMod{i}", triggerAction.CountMod));
        //            root.Add(new XElement($"CountModExtend{i}", triggerAction.CountModExtend));


        //            b1 = triggerAction.PlusAction.Count;
        //            root.Add(new XElement($"PlusActionCount{i}", b1));
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                ifAction = triggerAction.PlusAction[i1];
        //                text = $"Trigger{i}PlusAction{i1}";
        //                SaveRuleIfAction(ifAction, root, text);

        //            }



        //            b1 = triggerAction.MinusAction.Count;
        //            root.Add(new XElement($"MinusActionCount{i}", b1));
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                ifAction = triggerAction.MinusAction[i1];
        //                text = $"Trigger{i}MinusAction{i1}";
        //                root = SaveRuleIfAction(ifAction, root, text);
        //            }



        //            b1 = triggerAction.Action.Count;
        //            root.Add(new XElement($"ActionCount{i}", b1));
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                ruleAction = triggerAction.Action[i1];
        //                text = $"Trigger{i}Action{i1}";

        //                root = SaveRuleAction(ruleAction, root, text);
        //            }
        //        }


        //        XDocument saveDoc = new XDocument(root);
        //        File.WriteAllText($"{path}.xml", saveDoc.ToString());
        //    }
        //}

        //public static HeadRule LoadRule(string tag)
        //{
        //    HeadRule head = null;
        //    string path = Application.dataPath + $"/Resources/Data/Rule/{tag}";
        //    if (path != "")
        //    {

        //        XElement root = XDocument.Parse(File.ReadAllText($"{path}.xml")).Element("root");

        //        head = new HeadRule();
        //        head.Name = root.Element($"HeadName").Value;
        //        head.Cost = int.Parse(root.Element($"HeadCost").Value);
        //        head.NameText = root.Element($"HeadNameText").Value;
        //        head.Tag = root.Element($"Tag").Value;

        //        head.CostExtend = int.Parse(root.Element($"HeadCostExtend").Value);
        //        head.LevelCap = int.Parse(root.Element($"HeadLevelCap").Value);
        //        //head.CostMovePoint = int.Parse(root.Element($"CostMovePoint").Value);
        //        head.Player = bool.Parse(root.Element($"HeadPlayer").Value);


        //        int b = int.Parse(root.Element("HeadNeedRuleCount").Value);
        //        for (int i = 0; i < b; i++)
        //        {
        //            head.NeedRule.Add(root.Element($"HeadNeedRule{i}").Value);
        //        }

        //        b = int.Parse(root.Element("HeadEnemyRuleCount").Value);
        //        for (int i = 0; i < b; i++)
        //        {
        //            head.EnemyRule.Add(root.Element($"HeadEnemyRule{i}").Value);
        //        }


        //        TriggerAction triggerAction = null;
        //        IfAction ifAction = null;
        //        RuleAction ruleAction = null;


        //        string text = "";
        //        int b1 = 0;
        //        int b2 = 0;

        //        b = int.Parse(root.Element("TriggerActionCount").Value);
        //        for (int i = 0; i < b; i++)
        //        {
        //            triggerAction = new TriggerAction();

        //            triggerAction.PlusAction = new List<IfAction>();
        //            triggerAction.MinusAction = new List<IfAction>();
        //            triggerAction.Action = new List<RuleAction>();


        //            triggerAction.CountMod = bool.Parse(root.Element($"CountMod{i}").Value);
        //            triggerAction.CountModExtend = bool.Parse(root.Element($"CountModExtend{i}").Value);

        //            triggerAction.TargetPalyer = int.Parse(root.Element($"TargetPalyer{i}").Value);
        //            triggerAction.Trigger = int.Parse(root.Element($"Trigger{i}").Value);

        //            b1 = int.Parse(root.Element($"PlusActionCount{i}").Value);
        //            //Debug.Log(b1);
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text = $"Trigger{i}PlusAction{i1}";
        //                triggerAction.PlusAction.Add(LoadRuleIfAction(root, text));

        //            }

        //            b1 = int.Parse(root.Element($"MinusActionCount{i}").Value);
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text = $"Trigger{i}MinusAction{i1}";
        //                triggerAction.MinusAction.Add(LoadRuleIfAction(root, text));
        //            }

        //            b1 = int.Parse(root.Element($"ActionCount{i}").Value);
        //            for (int i1 = 0; i1 < b1; i1++)
        //            {
        //                text = $"Trigger{i}Action{i1}";
                        
        //                triggerAction.Action.Add(LoadRuleAction(root, text));
        //            }

        //            head.TriggerActions.Add(triggerAction);
        //        }



        //        // ruleConstructor.SetRule(head);

        //    }
        //    return head;
        //}
        //#endregion
    }
}
