using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

using BattleTable;


public class CardConstructor : MonoBehaviour
{
   // private List<Legion> actualLegion = new List<Legion>();

   // private Transform selector;
   // //private Transform ruleSelector;

   // private Guild curentGuild;
   // private string cardChar;

   // //constant
   // //private List<Constant> constants;
   // //private int curentConstant;

   // //ResetSystem
   // //private int oldAllCard;
   // //private List<CardBase> oldCard;
   // //private List<int> newCard;

   // //private int cardMod;
   // //private int cardModSize= 0;
   // //private int selectId;
   // private CardBase cardBase;
   // //public List<CardBase> LocalCard;
   // // [SerializeField]

   // [SerializeField]
   // private RuleMainFrame frame;
   // [SerializeField]
   // private CardConstructorUi Ui;
   // //  [SerializeField]
   //// private GameData gameData;

   // [SerializeField]
   // private GameSetting gameSetting;
   // private int selectId;
   // //private List<int> newData;
   // //private int curentFiltr;
   // //private bool filterRevers;

   // //private string origPath;
   //// private string origPathAlt;

   // public RenderTexture rTex;
   // public Camera captureCamera;

   // [SerializeField]
   // private TextMeshProUGUI TT;
   // [SerializeField]
   // private TextMeshProUGUI TT1;


   // void PointerClick()
   // {
   //     int linkIndex = TMP_TextUtilities.FindIntersectingLink(TT, Input.mousePosition, Camera.main);
   //     TMP_LinkInfo linkInfo = new TMP_LinkInfo();

   //     if (linkIndex == -1)
   //     {
   //         linkIndex = TMP_TextUtilities.FindIntersectingLink(TT1, Input.mousePosition, Camera.main);
   //         if (linkIndex == -1)
   //         {
   //             Debug.Log("Open link -1");
   //             return;
   //         }
   //         else
   //             linkInfo = TT1.textInfo.linkInfo[linkIndex];
   //     }
   //     else
   //         linkInfo = TT.textInfo.linkInfo[linkIndex];


   //     string selectedLink = linkInfo.GetLinkID();
   //     Debug.Log("Open link " + selectedLink);
   //     DeCoder(selectedLink);
   // }

   // void DeCoder(string str)
   // {
   //     string[] com = str.Split('_');
   //     int a = 0 , b=0;
   //     switch (com[0])
   //     {
   //         case ("Selector"):
   //             //GenerateTextExtend(com[1]);
   //             LoadSelector(com[1]);
   //             break;
   //         case ("Select"):
   //             a = int.Parse( com[2]);
   //             //gameSetting.Rule.FindIndex(x => x.Name == str)
   //             b = int.Parse(com[3]);
   //             //switch (com[1])
   //             //{
   //             //    //case ("Walk"):
   //             //    //    //b = int.Parse(com[3]);
   //             //    //    cardBase.WalkMood = gameSetting.Library.Rule[a].Rule[b];
   //             //    //    break;
   //             //    //case ("Action"):
   //             //    //   // b = int.Parse(com[3]);
   //             //    //    cardBase.ActionMood = gameSetting.Library.Rule[a].Rule[b];
   //             //    //    break;
   //             //    //case ("Def"):
   //             //    //    //b = int.Parse(com[3]);
   //             //    //    cardBase.DefMood = gameSetting.Library.Rule[a].Rule[b];
   //             //    //    break;
   //             //    //case ("Races"):
   //             //    //    cardBase.Races = cardBase.Guild.Races[a];
   //             //    //    break;
   //             //    //case ("Stat"):
   //             //    //    SwitchStat(a,b);
   //             //    //    break;
   //             //    //case ("Rule"):
   //             //    //    SwitchRule(a, b);
   //             //    //    break;
   //             //}
   //             break;
   //         case ("Rule"):
   //             a = int.Parse(com[2]);
   //             switch (com[1])
   //             {
   //                 case ("Down"):
   //                     DeliteRule(a);
   //                     break;
   //             }
   //             break;

   //         case ("Stat"):
   //             Debug.Log(com[1]);
   //             a = int.Parse(com[2]);
   //             switch (com[1])
   //             {
   //                 case ("Up"):
   //                     StatUp(a);
   //                     break;
   //                 case ("Down"):
   //                     StatDown(a);
   //                     break;
   //             }
   //             break;
   //         default:

   //             break;
   //     }
   // }

    
   // void CardCouner()
   // {
   //     if(selectId != -1)
   //         Ui.CounterCard.text = $"{selectId+1}/{gameSetting.AllCardPath.Count}";
   //     else
   //         Ui.CounterCard.text = $"0/{gameSetting.AllCardPath.Count}";
   // }

   // private string LinkSupport(string colorText, string linkText, string mainText)
   // {
   //     return $"<link={linkText}><color={colorText}>{mainText}</color></link>";
   // }
   // void GenerateText()
   // {
   //     string color = "#F4FF04";
   //     string str1;
   //     string str = $"?????? {cardBase.Name}";
   //     str += "\n??????????: " + LinkSupport(color, "Selector_Races", $"{cardBase.Races.Name}");
   //     //str += "\n????????????: " + LinkSupport(color, "Selector_Legions", $"{cardBase.Legions.Name}");
   //     //str += "\n???????????????????? ????????????: " + LinkSupport(color, "Selector_CivilianGroups", $"{cardBase.CivilianGroups.Name}");

   //     str += "\n??????: ";
   //     str1 = cardBase.Tayp;
   //     str += LinkSupport(color, "Selector_Tayp", $"{str1}");

   //     str += "\n??????: ";
   //     str1 = cardChar;
   //     str += LinkSupport(color, "Selector_CardChar", $"{str1}");

   //     str += "\n?????????? ???? ????????: ";
   //     str1 = (cardBase.WalkMood != null) ? cardBase.WalkMood.Name : "Null";
   //     str += LinkSupport(color, "Selector_Rule*Walk", $"{str1}");

   //     str += "\n?????????????? ?????????? ????????: ";
   //     str1 = (cardBase.ActionMood != null) ?  cardBase.ActionMood.Name : "Null";
   //     str += LinkSupport(color, "Selector_Rule*Acton", $"{str1}");

   //     str += "\n?????????????? ?????????? ????????????: ";
   //     str1 = (cardBase.DefMood != null) ?  cardBase.DefMood.Name : "Null";
   //     str += LinkSupport(color, "Selector_Rule*Def", $"{str1}");

   //     str += "\n????????????????????????????\n";
   //     for (int i =0; i < cardBase.Stat.Count; i++)
   //     {
   //         str += LinkSupport(color, $"Selector_Stat*{i}", $"<sprite name={cardBase.Stat[i].IconName}> {cardBase.Stat[i].Name}");
   //         str += $"   { cardBase.StatSize[i]} *{cardBase.Stat[i].Cost}/4 "; 
   //         str += LinkSupport(color, $"Stat_Down_{i}", $"-");
   //         str += " ";
   //         str += LinkSupport(color, $"Stat_Up_{i}", $"+");
   //         str += "\n";
   //     }
   //     str += LinkSupport(color, $"Selector_Stat*{cardBase.Stat.Count}", $"???????????????????????????? ????????????????");

   //     HeadSimpleTrigger head = null;
   //     str += "\n????????????\n";
   //     for (int i = 0; i < cardBase.Trait.Count; i++)
   //     {
   //         head = gameSetting.Rule[cardBase.TraitSize[i]];
   //         //Debug.Log(cardBase.Trait[i]);
   //         str += LinkSupport(color, $"Selector_Rule*{i}", $"{head.Name} ");
   //         str += $"  {head.Cost}/4 ";
   //         str += LinkSupport(color, $"Rule_Down_{i}", $"-");
   //        // str += " ";
   //        // str += LinkSupport(color, $"Rule_Plus_{i}", $"+");
   //         str += "\n";
   //     }
   //     str += LinkSupport(color, $"Selector_Rule*{cardBase.Trait.Count}", $"???????????????????????????? ??????????");
   //     str += "\n";
   //     str += $"\n???????? ?????????? ({cardBase.Mana})";

   //     TT.text = str;
   // }

   // void GenerateTextExtendClear()
   // {
   //     TT1.text = "";
   // }
   // void GenerateTextExtend(string text)
   // {
   //     string color = "#F4FF04";
   //     string[] com = text.Split('*');
   //     string str = "";
   //     switch (com[0]) 
   //     {
   //         case ("Races"):
   //             str += "?????????? ?? ?????????????? ??????????????";
   //             for (int i = 0; i < cardBase.Guilds.Races.Count; i++)
   //                 str += LinkSupport(color, $"Select_Races_{i}", $"\n{cardBase.Guilds.Races[i].Name}");

   //             break;
   //         case ("Rule"):
   //             str += "?????????????????? ????????????????";
   //             switch (com[1]) 
   //             {
   //                 case ("Walk"):
   //                     break;
   //                 case ("Action"):
   //                     break;
   //                 case ("Def"):
   //                     break;
   //                 case ("All"):
   //                     break;
   //             }

   //             break;
   //     }


   //     TT1.text = str;
   // }

   // #region IO card System
   // void Update()
   // {
   //     if (Input.GetMouseButtonDown(0))
   //         PointerClick();
   //     if (Input.GetMouseButtonDown(1))
   //     {
   //         selector.gameObject.active = false;
   //         //Ui.TextWindow.active = false;
   //         //ComandClear();
   //         // Ui.TextWindow.active = false;
   //     }
   // }

   // void Enject()
   // {

   //     {
   //         int width = 100;
   //         int height = 150;
   //         Texture2D texture = new Texture2D(width, height);

   //         RenderTexture targetTexture = RenderTexture.GetTemporary(width, height);
   //         targetTexture.depth = 2;

   //         captureCamera.targetTexture = targetTexture;
   //         captureCamera.Render();
   //         RenderTexture.active = targetTexture;

   //         Rect rect = new Rect(0, 0, width, height);
   //         texture.ReadPixels(rect, 0, 0);
   //         texture.Apply();
   //         captureCamera.targetTexture = rTex;

   //         cardBase.Image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
   //         // cardBase.Image = texture.EncodeToPNG();
   //     }

   //     cardBase.Name = Ui.NameFlied.text;

   //     // CardBase card = Core.CardClone(cardBase);

   //     string path = curentGuild.Name
   //          + "/" + cardBase.Tayp
   //          + "/" + cardChar
   //          + "/";
   //     if (selectId != -1)
   //     {
   //         //int a = gameSetting.AllCard.FindIndex(x => x.Id == cardBase.Id);
   //         //if (a != -1)
   //         //{
   //         //    cardBase.Body = gameSetting.AllCard[a].Body;
   //         //    gameSetting.AllCard[a] = Core.CardClone(cardBase);
   //         //}
   //         //CardView.ViewCard(cardBase);

   //         //if (gameSetting.GameDataFile.Data[gameSetting.SystemKey].MasterKey != path)//Delite old card
   //         //{
   //         //    gameSetting.SystemKey = gameSetting.AllCard.FindIndex(x => x.Id == cardBase.Id);
   //         //    //int a = gameSetting.GameDataFile.Data.FindIndex(x => x.MasterKey == path);
   //         //    //string cod = gameSetting.AllCard[selectId];

   //         //}
   //         //int a = gameSetting.AllCard.FindIndex(x => x.Id == cardBase.Id);
   //         //if (a != -1)
   //         //    gameSetting.AllCard[a] = Core.CardClone(cardBase);
   //         //CardView.ViewCard(cardBase);
   //         //path = gameSetting.AllCardPath[cardBase.Id];
   //         //string[] com = path.Split('_');
   //         //a = int.Parse(com[0]);
   //         //path = gameSetting.GameDataFile.Data[a].MasterKey + "/" + com[1];
   //     }
   //     else
   //     {
   //         if (gameSetting.AllCard.Count < 20)
   //         {
   //             cardBase.Body = gameSetting.CardBody[gameSetting.AllCard.Count];
   //             CardView.ViewCard(cardBase);
   //             gameSetting.AllCard.Add(Core.CardClone(cardBase));
   //         }
   //         path = SetID(path);
   //     }

   //     CardCouner();
   //     XMLSaver.Save(cardBase, path );
   //     CardView.CardList();
   // }
   // string SetID(string path)
   // {
   //     int a = gameSetting.GameDataFile.Data.FindIndex(x => x.MasterKey == path);
   //     SubGameData sub = gameSetting.GameDataFile.Data[a];
   //     if (sub.Key != " ")
   //     {
   //         sub.Key += $"/{sub.Size}";
   //         sub.KeyComplite += $"/{a}_{sub.Size}";
   //     }
   //     else
   //     {
   //         sub.Key = $"{sub.Size}";
   //         sub.KeyComplite = $"{a}_{sub.Size}";
   //     }
   //     Debug.Log(sub.MasterKey);

   //     cardBase.Id = sub.Size;

   //     selectId = gameSetting.AllCardPath.Count;
   //     gameSetting.SystemKey = a;

   //     gameSetting.AllCardPath.Add($"{a}_{sub.Size}");
   //    // path += ""+sub.Size;
   //     sub.Size++;
   //     return path;
   // }

   // //       // CardCouner();
   // //        //string str = gameSetting.AllCardPath[cardBase.Id];
   // //        //string[] com = str.Split('_');
   // //        //int a = com[com.Length - 1];
   // //        //com[com.Length - 1] = "" + (a + 1);
   // //        //for()

   // //        //gameSetting.AllCardPath[cardBase.Id] = ""


   // //        //  string path = "";

   // //        if (gameData.BlackList.Count > 0)
   // //        {
   // //            int a = gameData.BlackList[0];

   // //            //path = $"/Resources/Hiro{a}";
   // //            //gameData.AllCard[a] = path;

   // //            //gameSetting.AllCard[a].Body.gameObject.active = true;
   // //            //card.Body = gameSetting.AllCard[a].Body;
   // //            //gameSetting.AllCard[a] = card;

   // //            //CardView.ViewCard(card);


   // //            gameData.BlackList.RemoveAt(0);
   // //        }
   // //        else
   // //        {
   // //            // path = origPath + $"{gameSetting.AllCard.Count}";

   // //            AddEdit(LocalCard.Count, card);
   // //            //    AddEdit(LocalCard.Count);

   // //            LocalCard.Add(card);
   // //            CardCouner();
   // //            CardViews();
   // //            //  NewCard(LocalCard.Count - 1);
   // //        }

   // //    }
   // //    else
   // //    {

   // //        card.Body = LocalCard[selectId].Body;

   // //        AddEdit(selectId, LocalCard[selectId]);

   // //        LocalCard[selectId] = card;

   // //        CardView.ViewCard(card);

   // //    }
   // //    CardCouner();

   // //    //Sort();
   // //}
   // void Inject()
   // {
   //     //ReLoadCard();

   //     if (selectId > -1)
   //     {
   //         cardBase = Core.CardClone(gameSetting.AllCard[selectId]);


   //         //???????????????? ???????????? ?? ???????????????? ????????????
   //         Ui.NameFlied.text = cardBase.Name;
   //         ReCalculate();
   //         CardCouner();
   //     }
   // }
   // void Delite()
   // {
   //     ReLoadCard();
   //     if (selectId != -1)
   //     {
   //         SwitchCard(-1);
   //     }
   //     Ui.NameFlied.text = cardBase.Name;
   // }
   // #endregion

   // #region Save/Reset/Load System(Data Control)

   // void PreLoad()
   // {
   //     //origPath = $"/Resources/Data/Hiro";
   //     //origPathAlt = $"/Resources/Data";

   //     Core.LoadGameSetting(gameSetting);
   //     Core.LoadRules();


   //     selectId = -1;

   //     //LocalCard = new List<CardBase>();
   //     //newCard = new List<int>();
   //     //oldCard = new List<CardBase>();



   //     //GameObject GO = Ui.BaseCard.GetChild(0).gameObject;
   //     //GO.GetComponent<Image>().color = gameSetting.SelectColor[0];
   //     //GO.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); ;
   //     Ui.NewCardButton.GetComponent<Image>().color = gameSetting.SelectColor[0];
   //     Ui.NewCardButton.GetComponent<Button>().onClick.AddListener(() => SwitchCard(-1)); 

   //     Ui.ModButton[0].onClick.AddListener(() => CardView.Mod(false)); 
   //     Ui.ModButton[1].onClick.AddListener(() => CardView.Mod(true)); 

   //     Ui.EjectButton.onClick.AddListener(() => Enject());
   //     Ui.InjectButton.onClick.AddListener(() => Inject());
   //     Ui.DeliteButton.onClick.AddListener(() => Delite());

   //     Ui.SaveButton.onClick.AddListener(() => Save());
   //     Ui.ResetButton.onClick.AddListener(() => Reset());



   //     gameSetting.CardBody = Ui.CardBody;
   //     DataManager.GenerateKey(frame, gameSetting.Library);
   //     //gameSetting.Game
   //     //   gameData = XMLSaver.LoadGameData(origPathAlt);

   //     // oldAllCard = gameData.AllCard;

   //     LoadBase();

   //     CardCouner();
   //     //CardView.CardList();
   //     //CardViews();
   // }

   // //public void TransfData(GameData gameData1, GameData gameData2)
   // //{
   // //    gameData2 = new GameData();

   // //    gameData2.AllCard = gameData1.AllCard;
   // //    int a = gameData1.BlackList.Count;
   // //    gameData2.BlackList = new List<int>();
   // //    for (int i = 0; i < a; i++)
   // //    {
   // //        gameData2.BlackList.Add(gameData1.BlackList[i]);
   // //    }

   // //    gameData = gameData1;
   // //    gameDataReserv = gameData2;
   // //}


   // void LoadBase()
   // {

   //     //string path = "";
   //     //int a = gameData.AllCard;
   //     //for (int i = 0; i < a; i++)
   //     //{
   //     //    path = origPath + $"{i}";
   //     //    LocalCard.Add(XMLSaver.Load(path));
   //     //   // NewCard(i);
   //     //}

   //     //a = gameData.BlackList.Count;
   //     //for (int i = 0; i < a; i++)
   //     //{
   //     //    LocalCard[gameData.BlackList[i]].Body.gameObject.active = false;
   //     //}

   //     for(int i =0;i< Ui.CardBody.Count; i++)
   //     {
   //         SwitchCardButton(i, Ui.CardBody[i].gameObject.GetComponent<Button>());

   //         Ui.CardBody[i].gameObject.GetComponent<Image>().color = gameSetting.SelectColor[1];
   //         //SwitchCardButton
   //     }
   // }

   // void Reset()
   // {
   //     //int a = newCard.Count;
   //     //int b = 0;
   //     //for (int i = 0; i < a; i++)
   //     //{
   //     //    b = newCard[i];
   //     //    LocalCard[b] = oldCard[i];
   //     //    CardView.ViewCard(oldCard[i]);
   //     //}

   //     ////Remove add LocalCard
   //     //a = LocalCard.Count;
   //     //if (oldAllCard < a)
   //     //{
   //     //    a--;
   //     //    for (int i = oldAllCard - 1; i < a; i++)
   //     //    {
   //     //        Destroy(LocalCard[oldAllCard].Body.gameObject);
   //     //        LocalCard.RemoveAt(oldAllCard);
   //     //    }
   //     //}

   //     ////BlackList

   //     //TransfData(gameDataReserv, gameData);

   //     //// a = gameData.BlackList.Count;
   //     //// for (int i = 0; i < a; i++)
   //     //// {
   //     ////     b = gameData.BlackList[i];
   //     ////     if (LocalCard.Count > b)
   //     ////     {
   //     ////         LocalCard[b].Body.gameObject.active = true;
   //     ////     }
   //     //// }

   //     ////// TransfData(gameSetting.GlobalMyData, gameData);


   //     //// a = gameData.BlackList.Count;
   //     //// for (int i = 0; i < a; i++)
   //     //// {
   //     ////     b = gameData.BlackList[i];
   //     ////     LocalCard[b].Body.gameObject.active = false;
   //     //// }

   //     ////ResetData
   //     //newCard = new List<int>();
   //     //oldCard = new List<CardBase>();
   // }

   // //void AddEdit(int a, CardBase card)
   // //{
   // //    for (int i = 0; i < newCard.Count; i++)
   // //    {
   // //        if (a == newCard[i])
   // //            return;
   // //    }
   // //    newCard.Add(a);
   // //    oldCard.Add(card);
   // //}
   // void Save()
   // {

   //     XMLSaver.SaveGameData(gameSetting.GameDataFile);
   //     //// gameData.AllCard = LocalCard.Count;
   //     //// TransfData(gameData, gameSetting.GlobalMyData);
   //     //int b = 0;
   //     //string path = "";
   //     //for (int i = 0; i < newCard.Count; i++)
   //     //{
   //     //    b = newCard[i];
   //     //    path = origPath + $"{b}";
   //     //    XMLSaver.Save(LocalCard[b], path);
   //     //}

   //     //XMLSaver.SaveGameData(gameData, origPathAlt);
   //     //oldAllCard = gameData.AllCard;
   //     //newCard = new List<int>();
   //     //oldCard = new List<CardBase>();
   // }
   // #endregion

   // #region Create UI


   // void AddStatButton(bool plus, Button button, int a)
   // {
   //     button.onClick.RemoveAllListeners();
   //     if (plus)
   //         button.onClick.AddListener(() => StatUp(a));
   //     else
   //         button.onClick.AddListener(() => StatDown(a));
   // }
   // void SwitchCardButton(int a, Button button)
   // {
   //     button.onClick.AddListener(() => SwitchCard(a));
   // }

   // //void NewCard(int i)
   // //{
   // //    int a = Ui.BaseCard.childCount - 1;
   // //    GameObject GO = Instantiate(Ui.OrigCard);
   // //    GO.transform.SetParent(Ui.BaseCard);

   // //    Button button = GO.GetComponent<Button>();
   // //    SwitchCardButton(a, button);

   // //    GO.GetComponent<Image>().color = gameSetting.SelectColor[1];

   // //    LocalCard[i].Body = GO.transform;

   // //    CardView.ViewCard(LocalCard[i]);
   // //}




   // #endregion

   // #region Ui Use

   // void StatUp(int a)
   // {
   //     cardBase.StatSize[a]++;
   //     ReCalculate();
   //     if (cardBase.Mana > 10)
   //     {
   //         cardBase.StatSize[a]--;
   //         ReCalculate();
   //     }
   //    // Ui.StatUi[a].AllCount.text = "" + cardBase.StatSize[a];
   // }
   // void StatDown(int a)
   // {
   //     if (cardBase.StatSize[a] > 1)
   //     {
   //         cardBase.StatSize[a]--;
   //        // Ui.StatUi[a].AllCount.text = "" + cardBase.StatSize[a];
   //     }
   //     else if (a != 0)
   //         DeliteStat(a);


   //     ReCalculate();
   // }
   // void ReCalculate()
   // {
   //     int a = 0;
   //     for (int i = 0; i < cardBase.Stat.Count; i++)
   //     {
   //         if (cardBase.Stat[i] != null)
   //         {
   //             a += cardBase.StatSize[i] * cardBase.Stat[i].Cost;
   //         }
   //     }
   //     cardBase.Mana = Mathf.CeilToInt(a / 4f);

   //     GenerateText();
   //     //Ui.ManaCount.text = $"????????:{a}/4={cardBase.Mana}";
   // }


   // void SwitchCard(int a)
   // {
   //     if (a == -1)
   //     {
   //         if(selectId != -1)
   //             CardView.CardColor(selectId, 1);
   //         selectId = -1;
   //         Ui.NewCardButton.GetComponent<Image>().color = gameSetting.SelectColor[0];

   //     }
   //     else if(a >=gameSetting.AllCard.Count)
   //         return;
   //     else
   //     {
   //         string[] com = gameSetting.AllCardPath[a].Split('_');
   //         gameSetting.SystemKey = int.Parse(com[0]);
   //     }

   //     if (selectId == -1)
   //         Ui.NewCardButton.GetComponent<Image>().color = gameSetting.SelectColor[0];

   //     CardView.CardColor(selectId, 1);
   //     selectId = a;
   //     CardView.CardColor(a,0);

   //     CardCouner();

   // }
   // #endregion
   // void DeliteStat(int a)
   // {
   //     cardBase.Stat.RemoveAt(a);
   //     cardBase.StatSize.RemoveAt(a);
   //     cardBase.StatSizeLocal.RemoveAt(a);
   // }
   // void DeliteRule(int a)
   // {
   //     cardBase.Trait.RemoveAt(a);
   // }

   // void SwitchRule(int a, int b, int c)
   // {
   //    // Debug.Log($"{a} {b} {c}");
   //     string str = gameSetting.Library.Rule[b].Name + "_" + gameSetting.Library.Rule[b].Rule[c];
   //     int d = gameSetting.Rule.FindIndex(x => x.Name == gameSetting.Library.Rule[b].Rule[c]);

   //    // Debug.Log(cardBase.Trait.Count);
   //     if (a >= cardBase.Trait.Count)
   //     {
   //         cardBase.Trait.Add(str);
   //         cardBase.TraitSize.Add(0);
   //         //AddRuleUi();
   //     }
   //     else
   //         cardBase.Trait[a] = str;

   //     //StatCaseUi caseUi = Ui.RuleUi[a];
   //     //int b = gameSetting.Rule.Find(x => x.Name == str);
   //     cardBase.TraitSize[a] = d;
   //     // caseUi.Name.text = str;
   //     ReCalculate();
   //     selector.gameObject.active = false;
   // }
   // void SwitchTayp(int a)
   // {
   //     if(cardBase.Tayp != frame.ClassCard[a])
   //     {
   //         cardBase.Trait = new List<string>();
   //         cardBase.TraitSize = new List<int>();
   //         SwitchCard(-1);
            
   //     }
   //     cardBase.Tayp = frame.ClassCard[a];
   //     selector.gameObject.active = false;
   //     GenerateText();
   // }
   // void SwitchCardChar(int a )
   // {
   //     cardChar = frame.CardTayp[a];
   //     selector.gameObject.active = false;
   //     GenerateText();
   // }

   // void SwitchStat(int a, int b)
   // {
   //     if (a >= cardBase.Stat.Count)
   //     {
   //         cardBase.Stat.Add(gameSetting.Library.Constants[b]);
   //         cardBase.StatSize.Add(0);
   //         cardBase.StatSizeLocal.Add(0);
   //         // AddStatUi();
   //         // Ui.StatUi[0].ButtonSwitch.enabled = false;
   //     }

   //     // StatCaseUi caseUi = Ui.StatUi[a];
   //     cardBase.StatSize[a] = 0;
   //     cardBase.Stat[a] = gameSetting.Library.Constants[b];
   //     StatUp(a);
   //     selector.gameObject.active = false;

   //     // caseUi.Name.text = cardBase.Stat[a].Name;
   //     // caseUi.Icon.sprite = cardBase.Stat[a].Icon;

   //     //  caseUi.SellCount.text = $"{ cardBase.Stat[a].Cost}/4";
   //     //caseUi.AllCount.text = "1";
   //     //if (cardBase.StatSize[a] == 0)
   //     //{
   //     //    DeliteStat(a);
   //     //    return;
   //     //}
   // }

   // #region SwitchGuild

   // void OpenGuildSelector()
   // {
   //     Ui.GuildSelector.active = true;
   // }

   // void LoadGuildSelector()
   // {
   //     GameObject GO = null;


   //     for (int i = 0; i < gameSetting.Library.Guilds.Count; i++)
   //     {
   //         GO = Instantiate(Ui.OrigButtonBanner);
   //         GO.transform.SetParent(Ui.GuildSelector.transform);

   //         GO.GetComponent<Image>().sprite = gameSetting.Library.Guilds[i].Icon;
   //         SetSwitchGuildButton(i, GO.GetComponent<Button>());
   //     }
   // }

   // void SetSwitchGuildButton( int a, Button button)
   // {
   //     button.onClick.AddListener(() => SwitchGuild(a));
   // }

   // void SwitchGuild(int i)
   // {
   //     Ui.GuildSelector.active = false;
   //     curentGuild = gameSetting.Library.Guilds[i];
   //     Ui.GuildBanner.sprite = curentGuild.Icon;

   //     ReLoadCard();


   // }
   // #endregion
   // void ReLoadCard()
   // {

   //     cardBase = new CardBase();
   //     cardBase.Stat = new List<Constant>();
   //     cardBase.StatSize = new List<int>();
   //     //for (int i =0; i < gameSetting.StatSize; i++)
   //     //{
   //     //    cardBase.Stat.Add(null);
   //     //    cardBase.StatSize.Add(0);
   //     //    SwitchStat(i, -1);
   //     //    Ui.StatUi[0].ButtonSwitch.enabled = false;
   //     //}

   //     cardBase.Tayp = frame.ClassCard[0];
   //     cardBase.Guilds = curentGuild;
   //     actualLegion = new List<Legion>();

   //     cardBase.Name = "New Hiro";
   //     SwitchRace(curentGuild.Races[0]);




   //     cardBase.TraitSize = new List<int>();
   //     cardBase.Trait = new List<string>();
   //     //for (int i = 0; i < gameSetting.RuleSize; i++)
   //     //{
   //     //    cardBase.Trait.Add(null);
   //     //    cardBase.TraitSize.Add(0);
   //     //    //SwitchStat(i, -1);
   //     //}
   // }
   // void SwitchRace(Race race)
   // {
   //     cardBase.Races = race;


   //     int a = 0;
   //     string statName = "";

   //     if (race.MainRace != null)
   //         statName = race.MainRace.MainStat.Name;
   //     else
   //         statName = race.MainStat.Name;

   //     a = gameSetting.Library.Constants.FindIndex(x => x.Name == statName);
   //     if (a > -1)
   //         SwitchStat(0, a);





   //     foreach (Legion legion in curentGuild.Legions)
   //     {
   //         a = cardBase.Races.Legions.FindIndex(x => x.Name == legion.Name);
   //         if (a >= 0)
   //             actualLegion.Add(legion);
   //     }


   //     //Ui.RaceText.text = race.Name;

   //     if (actualLegion.Count > 0)
   //         SwitchLegion(actualLegion[0]);
   // }
   // void SwitchLegion(Legion legion)
   // {
   //     cardBase.Legions = legion;
   //    // Ui.LegionText.text = legion.Name;

   //     if (legion.CivilianGroups.Count > 0)
   //         SwitchCivilian(legion.CivilianGroups[0]);
   // }

   // void SwitchCivilian(CivilianGroup civilian)
   // {
   //     cardBase.CivilianGroups = civilian;
   //     //Ui.CivilianText.text = civilian.Name;

   //     Ui.NameFlied.text = cardBase.Name;
   //     ReCalculate();
   // }

   // void LoadSelector(string data)
   // {

   //     //Debug.Log($"{data}");
   //     string[] com = data.Split('*');
   //     int a = 0; 
   //     int b = 0;
   //     int sizeData =0;
   //     //Debug.Log($"{data} {sizeData} {a} {b}");
   //     switch (com[0])
   //     {
   //         case("Race"):
   //             sizeData = curentGuild.Races.Count;
   //             break;

   //         case ("Legion"):
   //             sizeData = actualLegion.Count;
   //             break;

   //         case ("Civilian"):
   //             sizeData = cardBase.Legions.CivilianGroups.Count;
   //             break;

   //         case ("Stat"):
   //             a = int.Parse(com[1]);
   //             sizeData = gameSetting.Library.Constants.Count;
   //             break;

   //         case ("Rule"):
   //             a = int.Parse(com[1]);
   //             sizeData = gameSetting.Library.Rule.Count;
   //             break;

   //         case ("Tayp"):
   //             sizeData = frame.ClassCard.Count;
   //             break;
   //         case ("CardChar"):
   //             sizeData = frame.CardTayp.Count;
   //             break;
   //         default:
   //             a=int.Parse(com[1]);
   //             b = int.Parse(com[2]);
   //             sizeData = gameSetting.Library.Rule[b].Rule.Count;

   //             break;
   //     }
   //     selector.gameObject.active = true;
   //     //Debug.Log($"{data} {sizeData} {a} {b}");

   //     //if(selector.ChildCount > sizeData)
   //     //for(int i = sizeData; i< selector.ChildCount;)

   //     GameObject GO = null;
   //     if (selector.childCount > sizeData) 
   //     {
   //         for (int i = selector.childCount; i > sizeData; i--)
   //         {
   //             //Debug.Log($"{sizeData} {selector.childCount} {i}");
   //             Destroy(selector.GetChild(i-1).gameObject);
   //         } 
   //     }
   //     else if  (selector.childCount < sizeData)
   //     {
   //         for (int i = selector.childCount; i < sizeData; i++)
   //         {
   //             GO = Instantiate(Ui.OrigButton);
   //             GO.transform.SetParent(selector);
   //         }
   //     }

   //     //gameSetting.Library.Legions.Count;
   //     for (int i =0; i< sizeData;  i++)
   //     {
   //         GO = selector.GetChild(i).gameObject;
   //         Text text = GO.transform.GetChild(0).gameObject.GetComponent<Text>();
   //         switch (com[0])
   //         {
   //             case ("Race"):
   //                 text.text = curentGuild.Races[i].Name;
   //                 break;
   //             case ("Legion"):
   //                 text.text = actualLegion[i].Name;
   //                 break;
   //             case ("Civilian"):
   //                 text.text = cardBase.Legions.CivilianGroups[i].Name;
   //                 break;
   //             case ("Stat"):
   //                 text.text = gameSetting.Library.Constants[i].Name;
   //                 break;
   //             case ("Rule"):
   //                 text.text = gameSetting.Library.Rule[i].Name + $"({gameSetting.Library.Rule[i].Rule.Count})";
   //                 break;

   //             case ("Tayp"):
   //                 text.text = frame.ClassCard[i];
   //                 break;
   //             case ("CardChar"):
   //                 text.text = frame.CardTayp[i];
   //                 break;
   //             default:
   //                 text.text = gameSetting.Library.Rule[b].Rule[i];
   //                 break;
   //         }
   //         Debug.Log(text.text);
   //         //Debug.Log($"{data} {i} {a} {b}");
   //         SetSwitchButton(i, GO.GetComponent<Button>(), com[0], a,b);
   //     }
   // }

   // void SetSwitchButton(int a, Button button, string data, int b, int c)
   // {
   //     button.onClick.RemoveAllListeners();
   //     switch (data)
   //     {
   //         case ("Race"):
   //             button.onClick.AddListener(() => SwitchRace(curentGuild.Races[a]));
   //             break;
   //         case ("Legion"):
   //             button.onClick.AddListener(() => SwitchLegion(actualLegion[a]));
   //             break;
   //         case ("Civilian"):
   //             button.onClick.AddListener(() => SwitchCivilian(cardBase.Legions.CivilianGroups[a]));
   //             break;
   //         case ("Stat"):
   //             button.onClick.AddListener(() => SwitchStat(b,a));
   //             //text.text = gameSetting.Lybrary.Constants[i].Name;
   //             break;
   //         case ("Rule"):
   //             button.onClick.AddListener(() => LoadSelector(gameSetting.Library.Rule[a].Name +"*"+b + "*" + a));
   //             break;
   //         case ("Tayp"):
   //             button.onClick.AddListener(() => SwitchTayp(a));
   //             break;
   //         case ("CardChar"):
   //             button.onClick.AddListener(() => SwitchCardChar(a));
   //             break;
   //         default:
   //             button.onClick.AddListener(() => SwitchRule(b, c,a));
   //             //text.text = gameSetting.Lybrary.Rule[b].Rule[i].Name;
   //             break;
   //     }
   // }

   // // Start is called before the first frame update
   // void Start()
   // {
   //     cardChar = frame.CardTayp[0];

   //     CreateCore();

   //     PreLoad();

   //     //LoadRuleSelector(0);


   //     // CreateStatButton();
   //    // Inject();

   //     //CreateRuleList();
   //     //LoadRuleSelector(0);

   //     //GenerateFiltr();

   // }

   // void CreateCore()
   // {
   //     XMLSaver.SetGameSetting(gameSetting);
   //     XMLSaver.LoadMainRule(gameSetting.Library);

   //     GameObject GO = Instantiate(Ui.SelectorOrigin);
   //     selector = GO.transform;
   //     selector.SetParent(Ui.SelectorOrigin.transform);
   //     GO.active = false;

   //     // CreateSystemButton();


   //     // gameSetting.library.Ru
   //     SwitchGuild(0);

   //     //LoadGuildSelector();

   //     //Ui.GuildButton.onClick.AddListener(() => OpenGuildSelector());
   // }
}
