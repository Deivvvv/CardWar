using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XMLSaver;
using Coder;

namespace SubSys 
{
    public class Crator : MonoBehaviour
    {
        public static CardBody NewCardBody(BaseUi ui)
        {
            GameObject go = Instantiate(ui.CardOrig);
            go.transform.SetParent(ui.MenuExtend);
           return go.GetComponent<CardBody>();
           
        }
    }

    static public class Gallery 
    {
        static List<CardCase> cards;
        static List<CardBody> bodys;
        static List<int> listIndex; 
        static int num;


        static void NewPathCard()
        {
            List<SubInt> cardsPath = Sorter.GetCard();
            cards = new List<CardCase>();
            int b = 0;
            int size=0;
            int cardModSize = num * bodys.Count;
            for (int i = 0; i < cardsPath.Count && cards.Count != bodys.Count; i++)
                for (int j = 0; j < cardsPath[i].Num.Count && cards.Count != bodys.Count; j++)
                    for (int k = 0; k < cardsPath[i].Num[j].Num.Count && cards.Count != bodys.Count; k++)
                        //for (int h = 0; h < cards[i].Num[j].Num[k].Num.Count; h++)
                        if (size + cardsPath[i].Num[j].Num[k].Num.Count < cardModSize)
                            size += cardsPath[i].Num[j].Num[k].Num.Count;
                        else
                        {
                            for (int a = cardModSize - size; a < cardsPath[i].Num[j].Num[k].Num.Count && cards.Count != bodys.Count; a++)
                            {
                                cards.Add(Sorter.ReadCard(i, j, k, a));

                            }
                            cardModSize = size +cardsPath[i].Num[j].Num[k].Num.Count;

                        }
            View(bodys, cards);
        }


        public static void Reset(BaseUi ui )
        {
            
            num = 0;
            //NewPage( true,num, bodys.Count, Sorter.Get)
            ui.Buttons[0].onClick.AddListener(() => num = NewPage(true, num, bodys.Count, Sorter.GetAllCard())); //PageUp
            ui.Buttons[1].onClick.AddListener(() => num = NewPage(false, num, bodys.Count, Sorter.GetAllCard())); //PageDown
            GameObject go = null;

            int a = 20;
            bodys = new List<CardBody>(new CardBody[a]);
            for (int i = 0; i < a; i++)
                bodys[i] = Crator.NewCardBody(ui);
          
            //View();
        }
        static void View(List<CardBody> bodys, List<CardCase>  card)
        {
            for (int i = 0; i < card.Count; i++)
                ReadCard(card[i], bodys[i]);
            for (int i= card.Count; i < bodys.Count; i++)
                CardClear(bodys[i]);
        }
        public static void ReadCard(CardCase card, CardBody body)
        {
            CoreSys sys = DeCoder.GetCore();

            body.Avatar.sprite = card.Image;

            body.Name.text = card.Name;

            body.Stat.text = card.Stat[0].Read("All");
            for (int i = 1; i < card.Stat.Count; i++)
                body.Stat.text += "\n" +card.Stat[i].Read("All");

            if (card.Trait.Count > 0)
            {
                List<string> str = new List<string>();
                for (int i = 0; i < card.Trait.Count; i++)
                    for (int i1 = 0; i1 < card.Trait[i].Num.Count; i1++)
                        str.Add(sys.head[-card.Trait[i].Head-1].Rule[card.Trait[i].Num[i1].Head]);

                body.Trait.text = str[0];
                for (int i = 1; i < str.Count; i++)
                    body.Trait.text += "\n" + str[i];
            }
            else
                body.Trait.text = "";
          


            body.Mana.text = "" + card.Mana;
        }
        static void CardClear(CardBody body)
        {
            body.Avatar.sprite = null;

            body.Name.text = "";

            body.Stat.text = "";
            body.Trait.text = "";

            body.Mana.text = "";
        }



        static int NewPage( bool add, int num, int size, int sizeAll)
        {
            if(add)
            {
                if(num * size + size> sizeAll)
                    return num + 1;
            }
            else if (num >0)
                return num - 1;
            
            return num;
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
        static void SetGuild(int a)
        {
            guild = Saver.LoadGuild(a);
            //cardSet.add(new setCase());
        }
/*
        static void SplitGuildLite(int a)
        {
            int b = card.FindIndex(x => x.Head == a);
            if (b != -1)
                return;//в противном случае вызвать перстроику всего кеша данных о картах с нуля
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
        static void SplitGuild(int a)
        {
            HideLibrary localGuild = Saver.LoadGuild(a);
            //SubIntFull cardList = Saver.LoadGuildCard(a);
            for (int i = 0; i < localGuild.Index.Count; i++)
            {
                a = guild.Index.FindIndex(x => x.Head == localGuild.Index[i].Head);
                if (a == -1)
                {
                    a = guild.Index.Count;
                    guild.Index.Add(new SubInt(localGuild.Index[i].Head));
                }

                for (int j = 0; j < guild.Index[i].Num.Count; j++)
                {
                    int b = guild.Index[a].Find(localGuild.Index[i].Num[j].Head);
                    for(int k=0;k< localGuild.Index[i].Num[j].Num.Count; k++)
                    {
                        int c = localGuild.Index[i].Num[j].Num[k].Head;
                        c = guild.Index[a].Num[b].Find(c);//Index(x => x.Head == c);
                        for(int h=0;h< localGuild.Index[i].Num[j].Num[k].Num.Count; h++)
                        {
                            int d = localGuild.Index[i].Num[j].Num[k].Num[h].Head;
                            d = localGuild.Index[a].Num[b].Num[c].Find(d);
                            if(d !=-1) 
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
    public HideLibrary(string str)
    {
        string[] mood = str.Split(';');

        Index = new List<SubInt>(new SubInt[mood.Length]);
        for(int i = 0; i < mood.Length; i++)
            Index[i] = new SubInt(mood[i], '|');

    }

    public void AddCard(CardCase card)
    {
        int a = Index.FindIndex(x => x.Head == card.Guild);
        if (a == -1)
        {
            a = Index.Count;
            Index.Add(new SubInt(card.Guild));
            AllCard++;
        }

        SubInt listIndex = Index[a];
        a = listIndex.Find(card.CardTayp);
        int b = listIndex.Num[a].Find(card.CardClass);

        if (card.Id == -1)
        {
            int c = Index[a].Num[b].Num.Count;
            card.Id = (c == 0) ? 0 :
                listIndex.Num[a].Num[b].Num[
                listIndex.Num[a].Num[b].Num[c - 1].Head
                ].Head + 1;
            listIndex.Num[a].Num[b].Find(card.Id);
        }
        listIndex.Num[a].Num[b].Find(card.Id);



        Legion.Add(card.Legion);
        Civilian.Add(card.Civilian);
        Race.Add(card.Race);

        for (int i = 0; i < card.Trait.Count; i++)
            Tag.Add(card.Trait[i].Head);

        for (int i = 0; i < card.Stat.Count; i++)
            Stat.Add(card.Stat[i].Get("Stat"));


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
            Stat.Remove(card.Stat[i].Get("Stat"));
        AllCard--;
    }
    public void SwitchCard( CardCase card)
    {
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
            Stat.Add(card1.Stat[i].Get("Stat"));
        for (int i = 0; i < card1.Stat.Count; i++)
            Stat.Remove(card1.Stat[i].Get("Stat"));
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
