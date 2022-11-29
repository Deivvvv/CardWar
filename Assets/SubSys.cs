using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using XMLSaver;
using Coder;

namespace SubSys 
{
    public class Creator : MonoBehaviour
    {
        public static CardBody NewCardBody(BaseUi ui, int a)
        {
            GameObject go = Instantiate(ui.CardOrig);
            go.transform.SetParent(ui.MenuExtend);
            go.GetComponent<Button>().onClick.AddListener(() => Gallery.RedactCard(a));
            return go.GetComponent<CardBody>();
        }
        public static CardBody NewCardBody(BaseUi ui, ColodConstructorUi uiExtend, int a)
        {
            GameObject go = Instantiate(ui.CardOrig);
            go.transform.SetParent(uiExtend.ColodWindow);
            go.GetComponent<Button>().onClick.AddListener(() => Gallery.RemoveCardColod(a));
            return go.GetComponent<CardBody>();
        }

        public static GameObject NewButton(BaseUi ui, Transform t)
        {
            GameObject go = Instantiate(ui.SimpleButton);
            go.transform.SetParent(t);
            return go;
        }
        public static void Des(GameObject go)
        {
            Destroy(go);
        }
        public static Button ReturnButtonID(GameObject go,int a)
        {
            return go.transform.GetChild(a).gameObject.GetComponent<Button>();
        }
        public static void SetButtonText(GameObject go,string str)
        {
            go.transform.GetChild(0).gameObject.GetComponent<Text>().text = str;
        }

    }

    static public class Gallery
    {
        static BaseUi ui;
        static ColodConstructorUi uiExtend;

        static List<CardCase> cards;
        static List<CardBody> bodys;

        static List<CardCase> cardsColod;
        static List<CardBody> bodysColod;

        public static List<int> colodGuild;
        public static List<string> colodName;

        static List<int> size;
        static int cardSum;

        static List<string> listID;

        static List<int> listIndex;
        static int num;
        static int numColod;
        static int colod =-1;
        static int guild =-1;
        static bool redactor;
       // static bool galleryMood;
        static bool editColod;

        //public static void ResetColod(ColodConstructorUi _uiExtend)
        //{
        //    uiExtend = _uiExtend;
        //}
        static void LoadGuildList()
        {
            void SetButton(Button button, int a)
            {
                button.onClick.AddListener(() => SetGuild(a));
            }

            CoreSys core = DeCoder.GetCore();
            GameObject go = null;
            for(int i = 0; i < core.bD[core.keyGuild].Base.Count; i++)
            {
                go = Creator.NewButton(ui,uiExtend.GuildList);
                SetButton( go.GetComponent<Button>(),i);
                if (!core.bD[core.keyGuild].Base[i].Look)
                    go.active = false;

#if UNITY_EDITOR
                    go.active = true;
#endif
                Creator.SetButtonText(go, core.bD[core.keyGuild].Base[i].Name);
            }
        }
        
        static void SaveWindowSet(string mood, int id =-1)
        {//ѕодтвердить сохрание.. вернутьс€.. Ќе сохран€ть измени€
            void SetButton(Button button, string mood,int id)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OpenButton(mood, id));
            }

            switch (mood)
            {
                case ("NewColod"):
                    if (guild == -1)
                        return;
                    if (colod == -1)
                    {

                        return;
                    }
                    else if (!editColod)
                    {
                       // colod == -1;
                    }

                        break;
                case ("NewCard"):
                    DeCoder.GetCore().LoadScene($"CardCreator| |Gallery"); //PageUp
                        return;
                    break;
                case ("SwitchColod"):
                    if (!editColod)
                    {
                        SetColod(id);
                        return;
                    }
                    break;
                case ("Exit"):
                    if (!editColod) 
                    {
                        DeCoder.GetCore().LoadScene($"Main");
                        return;
                    }

                    break;
            }

            uiExtend.SaveWindow.active = true;

            SetButton(Creator.ReturnButtonID(uiExtend.SaveWindow,0), mood, 0);
            SetButton(Creator.ReturnButtonID(uiExtend.SaveWindow, 1), mood, 1);
            SetButton(Creator.ReturnButtonID(uiExtend.SaveWindow, 2), mood, 2);
        }
        static void OpenButton(string mood, int id)
        {
            uiExtend.SaveWindow.active = false;
            switch (mood)
            {
                case ("NewColod"):
                    //if(guild)
                    break;
                case ("NewCard"):
                    break;
                case ("SwitchColod"):
                    break;
                case ("Exit"):
                    break;
            }
            //ui.Buttons[6].onClick.AddListener(() => SaveWindowSet("NewColod")); //PageDown
            //ui.Buttons[7].onClick.AddListener(() => SaveColod()); //PageDown
            //ui.Buttons[8].onClick.AddListener(() => DeliteColod()); //PageDown

            ////DeCoder.GetCore().//
            //ui.ExitButton.onClick.AddListener(() => SaveWindowSet("Exit"));
            //ui.Buttons[2].onClick.AddListener(() => SaveWindowSet("CardCreator"));

        }

        static void SetGuild(int a)
        {
            guild = a;
            num = 0;
            colodGuild = new List<int>();
            colodName = new List<string>();
            Saver.LoadColodBD(a, colodGuild, colodName);


            Sorter.SetGuild(a);
            if(a !=0)
                Sorter.SplitGuild(0);
            listID = new List<string>();
            List<SubInt> cardsPath = Sorter.GetCard();
            for (int i = 0; i < cardsPath.Count; i++)
                for (int j = 0; j < cardsPath[i].Num.Count; j++)
                    for (int k = 0; k < cardsPath[i].Num[j].Num.Count; k++)
                        for (int c = 0; c < cardsPath[i].Num[j].Num[k].Num.Count; c++)
                            listID.Add($"{i}|{j}|{k}|{c}");


            ReadGalleryCard();


            LoadColodList();
        }
        static void LoadColodList()
        {
            for (int i = colodGuild.Count; i < uiExtend.ColodList.childCount; i++)
                Creator.Des(uiExtend.ColodList.GetChild(i).gameObject);

            void SetButton(Button button, int a )
            {
                button.onClick.AddListener(() => SetColod(a));
            }

            GameObject go = null;
            for(int i = uiExtend.ColodList.childCount; i < colodGuild.Count; i++)
            {
                go =Creator.NewButton(ui, uiExtend.ColodList);
                SetButton( go.GetComponent<Button>(),i); //PageDown
            }

            for (int i = 0; i < colodName.Count; i++)
                uiExtend.ColodList.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = colodName[i];
        }
        static void SetColod(int a)
        {
            colod = a;
            cardsColod = new List<CardCase>();
            size = new List<int>();
            numColod = 0;
            cardSum = Saver.LoadColod(guild,a, cardsColod, size);
            uiExtend.NameTT.text = colodName[a];

            ReadCard(cardsColod[0], uiExtend.MainBody, false);
            ReadColodCard();
        }

        //static void SwitchGalleryMood()
        //{
        //    galleryMood = !galleryMood;
        //    Text text = ui.Buttons[9].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        //    if (galleryMood)
        //        text.text = "AddCard On";
        //    else
        //        text.text = "AddCard Off";
        //}
        static void RedactSwitch()
        {
            redactor = !redactor;
            //Text text = Creator.SetButtonText( ui.Buttons[4].gameObject,.transform.GetChild(0).gameObject.GetComponent<Text>();
            string text =   (redactor)? "Redact On":"Redact Off";
            Creator.SetButtonText(ui.Buttons[4].gameObject,text);
        }

        public static void RedactCard(int a)
        {
            //if (galleryMood)
            //{
            if (a < cards.Count)
                if (redactor)
                {
                    CardCase card = cards[a];
                    DeCoder.GetCore().LoadScene($"CardCreator|{card.Guild}/{card.CardClass}/{card.CardTayp}/{card.Id}|Gallery");

                }
                else
                    AddCardColod(a);
        }
        static void AddCardColod(int a)
        {
            //if (cardSum >= 40)
            //    return;

            if (cards[a].CardClass == 3)
            {
                cardsColod[0] = cards[a];
                ReadCard(cardsColod[0], uiExtend.MainBody, false);
                return;
            }


            for (int i=0;i<cardsColod.Count;i++)
                if( cardsColod[i].Id == cards[a].Id &&
                    cardsColod[i].Guild == cards[a].Guild &&
                    cardsColod[i].CardTayp == cards[a].CardTayp &&
                    cardsColod[i].CardClass == cards[a].CardClass
                    )
                {
                    if (size[i] < 3)
                    {
                        size[i]++;
                        cardSum++;
                        uiExtend.allCardCount.text = $"{cardSum}/40";

                        ReadColodCard();
                    }
                    return;
                }
            cardsColod.Add(cards[a]);
            size.Add(1);
            cardSum++;
            uiExtend.allCardCount.text = $"{cardSum}/40";
            //View(bodysColod, cardsColod, false, size);

            ReadColodCard();
        }
        public static void AddCardColodAlt(int a)
        {
            int realA = numColod * bodysColod.Count + a;

            if (realA < cardsColod.Count)
            {
                if (size[realA] < 3)
                {
                    size[realA]++;
                    cardSum++;
                    uiExtend.allCardCount.text = $"{cardSum}/40";
                    ReadCard(cardsColod[realA], bodysColod[realA], false, size[realA]);
                }
            }
        }
        public static void RemoveCardColod(int a)
        {
            int realA = numColod * bodysColod.Count +a;
            if (realA == 0)
                return;
            if (realA < cardsColod.Count)
            {
                if (redactor)
                {
                    CardCase card = cards[a];
                    DeCoder.GetCore().LoadScene($"CardCreator|{card.Guild}/{card.CardClass}/{card.CardTayp}/{card.Id}|Gallery");
                }
                else
                {
                    cardSum--;
                    size[realA]--;

                    if (size[realA] <= 0)
                    {
                        cardsColod.RemoveAt(realA);
                        CardClear(bodysColod[realA]);
                        return;
                    }
                    uiExtend.allCardCount.text = $"{cardSum}/40";
                    ReadCard( cardsColod[realA], bodysColod[realA], false, size[realA]);

                    //ReadColodCard();
                }
             
            }
        }

        static void ReadGalleryCard()
        {
            cards = new List<CardCase>();

            for (int i = num * bodys.Count; i < (num +1)* bodys.Count && i <listID.Count; i++)
            {
                List<int> nums = new List<int>(listID[i].Split('|').Select(int.Parse).ToArray());
                cards.Add(Sorter.ReadCard(nums[0], nums[1], nums[2], nums[3]));
            }
            uiExtend.cardCount.text = $"{num * bodys.Count}/{listID.Count}";

            View(bodys, cards, false);
        }
        static void ReadColodCard()
        {
            List<CardCase> localCardsColod = new List<CardCase>();

            for (int i = numColod * bodysColod.Count; i < (numColod + 1) * bodysColod.Count && i < cardsColod.Count -1; i++)
                localCardsColod.Add(cardsColod[i+1]);


            uiExtend.colodCount.text = $"{numColod * bodysColod.Count}/{cardsColod.Count}";
            View(bodysColod, localCardsColod, false);
        }

        static void NewColod()
        {
            SaveColod();
            colod = -1;
            uiExtend.NameTT.text = "NewColod";
            cardsColod = new List<CardCase>();
            size = new List<int>();
            size.Add(1);
           // List <SubInt> cardsPath = Sorter.GetCard();
           // int a = cardsPath[

            cardsColod.Add(Saver.LoadCard(guild, 0, 3, 0));//-загрузка маин карты
            View(bodysColod, cardsColod);

        }

        static void SaveColod()
        {
            if (guild == -1)
                return;

            if (cardsColod.Count ==0)
                return;

            if(colod == -1)
            {
                if (colodGuild.Count == 0)
                    colod = 0;
                else
                    colod = colodGuild[colodGuild.Count-1]+1;
                colodGuild.Add(colod);
                colodName.Add(uiExtend.NameTT.text);
                LoadColodList();
            }
            else
            {
                colodName[colod] = uiExtend.NameTT.text;
            }
            Saver.SaveColodBD(guild,colodName, colodGuild);
            //Creator.SetButtonText(uiExtend.ColodList.GetChild(colod).gameObject, uiExtend.NameTT.text);
            //uiExtend.ColodList.GetChild(colod).GetChild(0).transform.gameObject.GetComponent<Text>().text = uiExtend.NameTT.text;

            editColod = false;
            List<string> id = new List<string>();
            for (int i = 0; i < cardsColod.Count; i++)
                id.Add($"{cardsColod[i].Guild}|{cardsColod[i].CardClass}|{cardsColod[i].CardTayp}|{cardsColod[i].Id}");

            Saver.SaveColod(guild,colod, cardSum,  id,size);
        }
        static void DiliteColod()
        {
            if (guild == -1)
                return;
            if (colod == -1)
                return;
            cardsColod = new List<CardCase>();
            size = new List<int>();

            Saver.DliteColod(guild,colod);
        }
        static void LoadColod(int a)
        {
            numColod = 0;
            Saver.LoadColod(guild, a,cardsColod,size);
            ReadColodCard();
        }

        public static void Reset(BaseUi _ui, ColodConstructorUi _uiExtend)
        {
            uiExtend = _uiExtend;
            ui = _ui;
            redactor = false;
            //galleryMood = true;
            editColod = false;
            num = 0;
            numColod = 0;
            //NewPage( true,num, bodys.Count, Sorter.Get)
            ui.Buttons[0].onClick.AddListener(() => NextCard(true)); //PageUp
            ui.Buttons[1].onClick.AddListener(() => NextCard(false)); //PageDown
            ui.Buttons[2].onClick.AddListener(() => NextColodCard(true)); //PageUp
            ui.Buttons[3].onClick.AddListener(() => NextColodCard(false)); //PageDown

            ui.Buttons[4].onClick.AddListener(() => RedactSwitch()); //PageUp

            ui.Buttons[5].onClick.AddListener(() => DeCoder.GetCore().LoadScene($"CardCreator| |Gallery")); //PageUp
            ui.Buttons[6].onClick.AddListener(() => NewColod());//SaveWindowSet("NewCard")); //PageUp
            ui.Buttons[6].onClick.AddListener(() => SaveColod());//SaveWindowSet("NewCard")); //PageUp
            //ui.Buttons[7].onClick.AddListener(() => SaveWindowSet("NewCard")); //PageUp


            // ui.Buttons[5].onClick.AddListener(() => RedactSwitch()); //PageUp
            // ui.Buttons[4].onClick.AddListener(() => SaveWindowSet("NewCard"));
            //ui.Buttons[2].onClick.AddListener(() => DeCoder.GetCore().LoadScene($"CardCreator|")); //PageUp

            // ui.Buttons[6].onClick.AddListener(() => SaveWindowSet("NewColod")); //PageDown
            //ui.Buttons[7].onClick.AddListener(() => SaveColod()); //PageDown
            //ui.Buttons[8].onClick.AddListener(() => DeliteColod()); //PageDown
            //ui.Buttons[9].onClick.AddListener(() => SwitchGalleryMood()); //PageDown

            //SetGuild()

            //DeCoder.GetCore().//
            ui.ExitButton.onClick.AddListener(() => SaveWindowSet("Exit"));
            //ui.Buttons[2].onClick.AddListener(() => SaveWindowSet("CardCreator"));

            GameObject go = null;

            int a = 20;
            bodys = new List<CardBody>(new CardBody[a]);
            for (int i = 0; i < a; i++)
                bodys[i] = Creator.NewCardBody(ui,i);

            a = 8;
            bodysColod = new List<CardBody>(new CardBody[a]);
            for (int i = 0; i < a; i++)
                bodysColod[i] = Creator.NewCardBody(ui, uiExtend, i);
            // Sorter.SetGuild(0);
            //CoreSys core = DeCoder.GetCore();
            //for (int i = 1; i < core.bD[core.keyGuild].Base.Count; i++)
            //    Sorter.SplitGuild(i);
            if (cardsColod == null)
            {
                cardsColod = new List<CardCase>();
                size = new List<int>();
            }
            if (listID == null)
                listID = new List<string>();

            uiExtend.NameTT.text = "NewColod";
            uiExtend.allCardCount.text = $"{cardSum}/40";
            LoadGuildList();
            ReadColodCard();
            ReadGalleryCard();
            //View();
        }
        static void View(List<CardBody> bodys, List<CardCase>  card, bool mood =true, List<int> size = null)
        {
            int i = 0;
            if (size != null)
                for (; i < card.Count; i++)
                    ReadCard(card[i], bodys[i], mood, size[i]);
            else
                for (; i < card.Count; i++)
                    ReadCard(card[i], bodys[i], mood);

            for (; i < bodys.Count; i++)
                CardClear(bodys[i]);
        }
        public static void ReadCard(CardCase card, CardBody body, bool mood, int size =-1)
        {
            int a;
            CoreSys sys = DeCoder.GetCore();
            //Debug.Log(card);
            //Debug.Log(body);
            body.Avatar.sprite = card.Image;

            body.Name.text =$"{card.Id} "+ card.Name;
            string strMood = (mood) ? "All" : "Max";

            body.Stat.text = card.Stat[0].Read(strMood);
            for (int i = 1; i < card.Stat.Count; i++)
                body.Stat.text += "\n" +card.Stat[i].Read(strMood);

            if (card.Trait.Count > 0)
            {
                List<string> str = new List<string>();
                for (int i = 0; i < card.Trait.Count; i++)
                    for (int i1 = 0; i1 < card.Trait[i].Num.Count; i1++)
                    {
                        a = sys.head[-card.Trait[i].Head - 1].Index.FindIndex(x => x == card.Trait[i].Num[i1].Head);
                        if(a !=-1)
                            str.Add(sys.head[-card.Trait[i].Head - 1].Rule[a]);
                    }

                body.Trait.text = str[0];
                for (int i = 1; i < str.Count; i++)
                    body.Trait.text += "\n" + str[i];
            }
            else
                body.Trait.text = "";
          
            if(size != -1)
            {
                body.CountButton.active = true;
                body.Count.text = ""+size;
            }
            else
                body.CountButton.active = false;


            body.Mana.text = "" + card.Mana;
        }
        static void CardClear(CardBody body)
        {
            body.Avatar.sprite = null;

            body.Name.text = "";

            body.Stat.text = "";
            body.Trait.text = "";

            body.CountButton.active = false;

            body.Mana.text = "";
        }



        static int NewPage( bool add, int num, int size, int sizeAll)
        {
            if(add)
            {
                if((num+1) * size< sizeAll)
                    return num + 1;
            }
            else if (num >0)
                return num - 1;
            
            return num;
        }

        static void NextCard(bool next)
        {
            num= NewPage(next, num, bodys.Count, listID.Count);
            ReadGalleryCard();
        }
        static void NextColodCard(bool next)
        {
            numColod = NewPage(next, numColod, bodysColod.Count, cardsColod.Count);
            ReadColodCard();
        }
    }


    static public class Sorter
    {

        static CoreSys sys;
        static HideLibrary guild;
        static HideLibrary guildSort;

        // static HideLibrary cardSet;
        //static HideLibrary guild;
        static bool revers = false;

        // static List<int> indexFirstSort, indexSecondSort, indexThirdSort;

        //static List<int> listIndex;
        //  static List<int> sortIndex = new List<int>();
        // static List<int> lastIndex;
        public static int GetAllCard() { return guild.AllCard; }
        public static List<SubInt> GetCard() { return guild.Index; }
        public static void SetGuild(int a)
        {
            guild = Saver.LoadGuild(a);
            //cardSet.add(new setCase());
        }
/*
        static void SplitGuildLite(int a)
        {
            int b = card.FindIndex(x => x.Head == a);
            if (b != -1)
                return;//в противном случае вызвать перстроику всего кеша данных о картах с нул€
                //card.RemoveAt(b);

            b = card.Count;
            card.Add(Saver.LoadGuildCard(a));

            HideLibrary guild1 = Saver.LoadGuild(a);

            for (int i = 0; i < guild1.Legion.Index.Count; i++)
                guild.Legion.Add(guild1.Legion.Index[i], guild1.Legion.Size[i]);

            for (int i = 0; i < guild1.Civilian.Index.Count; i++)
                guild.Civilian.Add(guild1.Civilian.Index[i], guild1.Civilian.Size[i]);

            for (int i = 0; i < guild1.Race.Index.Count; i++)
                guild.Race.Add(guild1.Race.Index[i], guild1.Race.Size[i]);

            for (int i = 0; i < guild1.Tag.Index.Count; i++)
                guild.Tag.Add(guild1.Tag.Index[i], guild1.Tag.Size[i]);

            for (int i = 0; i < guild1.Stat.Index.Count; i++)
                guild.Stat.Add(guild1.Stat.Index[i], guild1.Stat.Size[i]);



            for (int i = 0; i < guild1.Index.Count; i++)
                guild.Index.Add(guild1.Index[i]);

        }
*/
        public static void SplitGuild(int a)
        {
            HideLibrary localGuild = Saver.LoadGuild(a);
            //SubIntFull cardList = Saver.LoadGuildCard(a);
            for (int i = 0; i < localGuild.Index.Count; i++)
            {
                a = guild.Index.FindIndex(x => x.Head == localGuild.Index[i].Head);
                if (a == -1)
                {
                    a = guild.Index.Count;
                    guild.Index.Add(localGuild.Index[i]);
                }
                else
                    for (int j = 0; j < guild.Index[i].Num.Count; j++)
                    {
                        int b = guild.Index[a].Find(localGuild.Index[i].Num[j].Head);
                        for (int k = 0; k < localGuild.Index[i].Num[j].Num.Count; k++)
                        {
                            int c = localGuild.Index[i].Num[j].Num[k].Head;
                            c = guild.Index[a].Num[b].Find(c);//Index(x => x.Head == c);
                            for (int h = 0; h < localGuild.Index[i].Num[j].Num[k].Num.Count; h++)
                            {
                                int d = localGuild.Index[i].Num[j].Num[k].Num[h].Head;
                                d = localGuild.Index[a].Num[b].Num[c].Find(d);
                                if (d != -1)
                                    localGuild.RemoveCard(ReadCard(i, j, k, h));
                            }
                        }
                    }
            }
            guild.Split(localGuild);
        }
        static void SimpleSort(List<SubInt> subInt)
        {
            for (int i = 0; i < subInt.Count; i++)
            {
                int a = i;
                for (int j = i + 1; j < subInt.Count; j++)
                    if (subInt[a].Head > subInt[j].Head)
                        a = j;
                if (a != i)
                {
                    SubInt sub = subInt[i];
                    subInt[i] = subInt[a];
                    subInt[a] = sub;
                }

            }
        }


        static void HardSort()
        {
            SimpleSort(guild.Index);

            for (int i = 0; i < guild.Index.Count; i++)
                guild.Index[i].Sort();



            CoreSys core = DeCoder.GetCore();
            Accses mainAccses = new Accses();
            MainBase main;
            //sys
            for (int i = 0; i < guild.Index.Count; i++)
            {
                main = core.bD[core.keyGuild].Base[guild.Index[i].Head];
                mainAccses.SplitLite(main.accses);
            }

            for (int i = 0; i < guild.Index.Count; i++)
                for(int j =0;j<guild.Index[i].Num.Count;j++)
                {
                    Accses bufferAccses = new Accses();
                    bufferAccses.SplitLite(mainAccses);

                    main = core.bD[core.keyCardTayp].Base[guild.Index[i].Num[j].Head];
                    bufferAccses.SplitLite(main.accses);
                    for (int k = 0; k < guild.Index[i].Num[j].Num.Count; k++)
                    {
                        Accses localAccses = new Accses();
                        localAccses.SplitLite(bufferAccses);

                        main = core.bD[core.keyCardClass].Base[guild.Index[i].Num[j].Num[k].Head];
                        localAccses.SplitLite(main.accses);
                        localAccses.AccsesComplite();
                        for (int h = 0; h < guild.Index[i].Num[j].Num[k].Num.Count; h++)
                        {
                            CardCase card = ReadCard(i, j, k, h);
                            if (!mainAccses.AccsesCard(card))
                            {

                                guild.RemoveCard(card);
                                //guild.Index[i].Num[j].Num[k].Num.RemoveAt(h);
                                h--;
                            }

                        }
                    }
                }
        }
        public static CardCase ReadCard(int i,int j, int k, int h)
        {
            return Saver.LoadCard(
                                 guild.Index[i].Head,
                                 guild.Index[i].Num[j].Head,
                                 guild.Index[i].Num[j].Num[k].Head,
                                 guild.Index[i].Num[j].Num[k].Num[h].Head);
        }

    }
}
public class HideLibrary
{
    public HideLibraryCase Legion = new HideLibraryCase();
    public HideLibraryCase Civilian = new HideLibraryCase();
    public HideLibraryCase Race = new HideLibraryCase();
    public HideLibraryCase Tag = new HideLibraryCase();
    public HideLibraryCase Stat = new HideLibraryCase();
    // public int Guild;
    //private int oldKey;
    public int AllCard;
    public List<SubInt> Index;//= new List<SubIntLite>();
    public HideLibrary( )
    {

    }
    //public HideLibrary(string str)
    //{
    //    //string[] mood = str.Split(';');
    //    SubInt sub = new SubInt(str, 4);
    //    Index = sub.Num;
    //    //for(int i = 0; i < mood.Length; i++)
    //    //    Index[i] = new SubInt(mood[i], '|');

    //}

    public void AddCard(CardCase card)
    {
        int a = Index.FindIndex(x => x.Head == card.Guild);
        if (a == -1)
        {
            a = Index.Count;
            Index.Add(new SubInt(card.Guild));
           // AllCard++;
        }
        Debug.Log(a);

        SubInt listIndex = Index[a];
        a = listIndex.Find(card.CardTayp);
        int b = listIndex.Num[a].Find(card.CardClass);
        Debug.Log(a);

        if (card.Id == -1)
        {
            int c = listIndex.Num[a].Num[b].Num.Count;
            Debug.Log(c);
            if (c == 0)
                card.Id = c;
            else
                card.Id = listIndex.Num[a].Num[b].Num[c - 1].Head + 1;
            AllCard++;
        }
        listIndex.Num[a].Num[b].Find(card.Id);
        Debug.Log(a);



        Legion.Add(card.Legion);
        Civilian.Add(card.Civilian);
        Race.Add(card.Race);

        for (int i = 0; i < card.Trait.Count; i++)
            Tag.Add(card.Trait[i].Head);

        for (int i = 0; i < card.Stat.Count; i++)
            Stat.Add(card.Stat[i].GetStat());


    }

    public void RemoveCard(CardCase card)
    {
        int a = Index[card.Guild].Num[card.CardTayp].Num[card.CardClass].Find(card.Id);
        Index[card.Guild].Num[card.CardTayp].Num[card.CardClass].Num.RemoveAt(a);

        Legion.Remove(card.Legion);
        Civilian.Remove(card.Civilian);
        Race.Remove(card.Race);

        for (int i = 0; i < card.Trait.Count; i++)
            Tag.Remove(card.Trait[i].Head);

        for (int i = 0; i < card.Stat.Count; i++)
            Stat.Remove(card.Stat[i].GetStat());
        AllCard--;
    }
    public void SwitchCard( CardCase card)
    {
        if(card.Id == -1)
        {
            AddCard(card);
            return;
        }
        CardCase card1 = Saver.LoadCard(card.Guild, card.CardTayp, card.CardClass ,card.Id);
        if (card.Legion != card1.Legion)
        {
            Legion.Remove(card1.Legion);
            Legion.Add(card.Legion);
        }

        if (card.Civilian != card1.Civilian)
        {
            Civilian.Remove(card1.Civilian);
            Civilian.Add(card.Civilian);
        }

        if (card.Race != card1.Race)
        {
            Race.Remove(card1.Race);
            Race.Add(card.Race);
        }

        for (int i = 0; i < card.Trait.Count; i++)
            Tag.Add(card.Trait[i].Head);
        for (int i = 0; i < card1.Trait.Count; i++)
            Tag.Remove(card1.Trait[i].Head);

        for (int i = 0; i < card.Stat.Count; i++)
            Stat.Add(card.Stat[i].GetStat());
        for (int i = 0; i < card1.Stat.Count; i++)
            Stat.Remove(card1.Stat[i].GetStat());
    }

    public void Split(HideLibrary lib)
    {
        Legion.Split(lib.Legion);
        Civilian.Split(lib.Civilian);
        Race.Split(lib.Race);
        Tag.Split(lib.Tag);
        Stat.Split(lib.Stat);
        AllCard += lib.AllCard;
    }
}
public class HideLibraryCase
{
    public List<bool> Use = new List<bool>();
    public List<int> Index = new List<int>();
    public List<int> Size = new List<int>();

    public void Split(HideLibraryCase lib)
    {
        for (int i = 0; i < lib.Index.Count; i++)
            Add(lib.Index[i], lib.Size[i]);
    }

    public void Add(int index, int size =1)
    {
        int a = Index.FindIndex(x => x == index);
        if (a != -1)
        {
            Size[a] += size;
        }
        else
        {
            Use.Add(true);
            Index.Add(index);
            Size.Add(size);
        }
    }

    public void Remove(int index, int size=1)
    {
        int a = Index.FindIndex(x => x == index);
        if (a != -1)
            if (Size[a] < size)
            {
                Use.RemoveAt(a);
                Index.RemoveAt(a);
                Size.RemoveAt(a);
            }
            else
                Size[a] -= size;

    }

    public bool Find(int index)
    {
        int a = Index.FindIndex(x => x == index);
        return Use[a];
    }

    public int ReturnSize(int index)
    {
        int a = Index.FindIndex(x => x == index);
        if (a == -1)
            return 0;
        return Size[a];
    }

}
