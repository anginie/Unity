//#define TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerGame : MonoBehaviour
{
    [SerializeField]
    Button btnDeal;
    [SerializeField]
    Button btnCompare;
    [SerializeField]
    Button btnGiveup;
    [SerializeField]
    Text txtResult;

    GameObject cardPrefab;
    List<Card> allCards = new List<Card>();
    List<Card> aCards = new List<Card>();
    List<Card> bCards = new List<Card>();
    List<CardUI> aCardUIs = new List<CardUI>();
    List<CardUI> bCardUIs = new List<CardUI>();

    int handCardsCount = 2;
    int dealCardsCount = 3;
    int maxDealCards = 5;

    bool resultShown;

    // Start is called before the first frame update
    void Start()
    {
        cardPrefab = Resources.Load<GameObject>("Card");
        InitCards();
#if !TEST
        // 随机洗牌
        allCards.Sort((a,b)=>Random.Range(0,1f)>0.5f?1:-1);
#endif
        btnDeal.onClick.AddListener(DealCard);
        btnCompare.onClick.AddListener(ShowResultCards);
        btnGiveup.onClick.AddListener(Restart);
        ActiveBtns(false);
        txtResult.text = "";
        StartCoroutine(CorotineShowCards());

        TestLongstrMain();
    }

    static void TestLongstrMain()
    {
        string input = "djkfdbcdkkkhijkijkhlmno";
        string longestConsecutiveString = EmptyLongestStr.FindLongestConsecutiveSubstring(input);
        Debug.Log("The  alongest consecutive substring is: " + longestConsecutiveString);
    }

    void InitCards()
    {
        allCards.Clear();
#if TEST
        // AI手牌
        allCards.Add(new Card(1, 10));
        allCards.Add(new Card(2, 11));
        // 玩家手牌
        allCards.Add(new Card(2, 9));
        allCards.Add(new Card(0, 10));
        // 公共牌
        allCards.Add(new Card(0, 8));
        allCards.Add(new Card(3, 13));
        allCards.Add(new Card(1, 11));
        allCards.Add(new Card(3, 12));
        allCards.Add(new Card(2, 1));
#else
        for (int i = 0; i < 52; i++)
        {
            allCards.Add(new Card(Mathf.CeilToInt(i / 13), i % 13 + 1));
        }
#endif
    }

    void DealCard()
    {
        if(!resultShown && dealCardsCount < maxDealCards)
        {
            Card card = allCards[dealCardsCount + handCardsCount * 2];
            ShowCard(card, new Vector3(dealCardsCount * 180 - 0.5f * Screen.width - 200, 0.5f * Screen.height - 160, 0));
            aCards.Add(card);
            bCards.Add(card);
            dealCardsCount++;
        }
    }
    CardUI ShowCard(Card card, Vector3 pos, bool showBg = false)
    {
        GameObject cardAGo = Instantiate(cardPrefab, transform);
        CardUI cardUI = cardAGo.GetComponent<CardUI>();
        cardUI.transform.localPosition = pos;
        cardUI.InitCardUI(card, showBg);
        cardUI.gameObject.SetActive(true);
        return cardUI;
    }

    IEnumerator CorotineShowCards()
    {
        yield return new WaitForSeconds(0.5f);
        // 发手牌
        for (int i = 0; i < handCardsCount; i++)
        {
            CardUI aCardUI = ShowCard(allCards[i], new Vector3(i * 200 - 0.5f * Screen.width + 300, 0.5f * Screen.height + 200, 0),true);
            aCards.Add(allCards[i]);
            aCardUIs.Add(aCardUI);

            yield return new WaitForSeconds(0.5f);
            CardUI bCardUI = ShowCard(allCards[i + handCardsCount], new Vector3(i * 200 - 0.5f * Screen.width - 100, -400, 0));
            bCardUIs.Add(bCardUI);
            bCards.Add(allCards[i + handCardsCount]);
            yield return new WaitForSeconds(0.5f);
        }
        // 发公共牌
        for (int i = 0; i < dealCardsCount; i++)
        {
            Card card = allCards[i + handCardsCount * 2];
            ShowCard(card, new Vector3(i * 180 - 0.5f * Screen.width - 200, 0.5f * Screen.height - 160, 0));
            aCards.Add(card);
            bCards.Add(card);
            yield return new WaitForSeconds(0.5f);
        }
        btnDeal.gameObject.SetActive(true);
        btnCompare.gameObject.SetActive(true);
        btnGiveup.gameObject.SetActive(true);
        ActiveBtns(true);
    }

    void ActiveBtns(bool b)
    {
        btnDeal.gameObject.SetActive(b);
        btnCompare.gameObject.SetActive(b);
        btnGiveup.gameObject.SetActive(b);
    }

    void ShowResultCards()
    {
        if (resultShown)
        {
            Restart();
        }
        else
        {
            ActiveBtns(false);
            StartCoroutine(CorotineShowResultCards());
        }
    }

    void Restart()
    {
        dealCardsCount = 3;
        resultShown = false;
        ActiveBtns(false);
        txtResult.text = "";
        btnCompare.GetComponentInChildren<Text>().text = "COMPARE";
        aCards.Clear();
        bCards.Clear();
        aCardUIs.Clear();
        bCardUIs.Clear();
        ClearCardsUI();

#if !TEST
        allCards.Sort((a, b) => Random.Range(0, 1f) > 0.5f ? 1 : -1);
#endif
        StartCoroutine(CorotineShowCards());
    }

    void ClearCardsUI()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator CorotineShowResultCards()
    {
        aCardUIs.ForEach(c => c.ShowBg(false));
        yield return new WaitForSeconds(0.5f);
        bCardUIs.ForEach(c => c.ShowBg(false));
        yield return new WaitForSeconds(0.5f);
        CardResult retA = GetMaxSet(aCards);
        List<Card> retACards = retA.cards;
        for (int i = 0; i < retACards.Count; i++)
        {
            ShowCard(retACards[i], new Vector3(i * 180 - 0.5f * Screen.width - 200, 0.5f * Screen.height + 500, 0));
        }
        CardResult retB = GetMaxSet(bCards);
        List<Card> retBCards = retB.cards;
        for (int j = 0; j < retBCards.Count; j++)
        {
            ShowCard(retBCards[j], new Vector3(j * 180 - 0.5f * Screen.width - 200, -700, 0));
        }
        ActiveBtns(true);
        resultShown = true;
        btnCompare.GetComponentInChildren<Text>().text = "NEW";

        int compareRet =  CompareCards(retA, retB);
        if (compareRet > 0)
        {
            txtResult.text = "PLAYER\nLOSE";
        }
        else if(compareRet < 0)
        {
            txtResult.text = "PLAYER\nWIN";
        }
        else
        {
            txtResult.text = "DRAW";
        }
    }

    #region 逻辑算法
    int CompareCards(CardResult retA, CardResult retB)
    {
        if (retA.cardSet == retB.cardSet)
        {
            return CompareCardsPoint(retA.cards, retB.cards);
        }
        else
        {
            return (int)retA.cardSet - (int)retB.cardSet;
        }
    }

    int CompareCardsPoint(List<Card> aLs, List<Card> bLs)
    {
        for (int i = 0; i < aLs.Count; i++)
        {
            if (aLs[i].point - bLs[i].point != 0)
            {
                return aLs[i].point - bLs[i].point;
            }
        }
        return 0;
    }


    CardResult GetMaxSet(List<Card> cards)
    {
        CardResult ret = new CardResult();
        int[] suits = new int[4];
        int[] points = new int[13];
        bool haveHulu = false;
        int sameSuit = -1;
        List<int> links = new List<int>();

        for (int i = 0; i < cards.Count; i++)
        {
            suits[cards[i].suit]++;
            points[cards[i].point - 1]++;
        }
        // 是否同花
        for (int s = 0; s < suits.Length; s++)
        {
            if (suits[s] > 4)
            {
                sameSuit = s;
            }
        }

        for (int p = 0; p < points.Length; p++)
        {
            if (points[p] > 3)
            {
                ret.pointsFour.Add(p + 1);
            }
            else if (points[p] > 2)
            {
                ret.pointsThree.Add(p + 1);
            }
            else if (points[p] > 1)
            {
                ret.pointsPair.Add(p + 1);
            }

            if (points[p] > 0)
            {
                if (links.Count == 0)
                {
                    links.Add(p + 1);
                }
                else if(links[links.Count-1]==p)
                {
                    links.Add(p + 1);
                }
            }
            else
            {
                if(links.Count<5)
                    links.Clear();
            }
        }

        if ((ret.pointsFour.Count > 0))
        {
            ret.cards.Clear();
            ret.cards.AddRange(cards.FindAll(c => c.point == ret.pointsFour[ret.pointsFour.Count - 1]));
            cards.Sort(new OrderCard());
            ret.cards.Add(cards.Find(c => c.point != ret.pointsFour.Count - 1));
            ret.cardSet = CardSet.SiTiao;
        }
        else if(ret.pointsThree.Count > 0)
        {
            ret.cards.Clear();
            int maxThree = ret.pointsThree[ret.pointsThree.Count - 1];
            ret.cards.AddRange(cards.FindAll(c => c.point == maxThree));
            // 葫芦
            if (ret.pointsThree.Count > 1)
            {
                haveHulu = true;
                List<Card> huluPairCards = cards.FindAll(c => c.point == ret.pointsThree[ret.pointsThree.Count - 2]);
                ret.cards.AddRange(huluPairCards.GetRange(0,2));
                ret.cardSet = CardSet.HuLu;
            }
            else if(ret.pointsPair.Count > 0)
            {
                haveHulu = true;
                ret.cards.AddRange(cards.FindAll(c => c.point == ret.pointsPair[ret.pointsPair.Count-1]));
                ret.cardSet = CardSet.HuLu;
            }
            else
            {
                List<Card> notThree = cards.FindAll(c => c.point != maxThree);
                notThree.Sort(new OrderCard());
                ret.cards.AddRange(notThree.GetRange(0, 2));
                ret.cardSet = CardSet.SanTiao;
            }
        }
        else if (ret.pointsPair.Count > 0)
        {
            ret.cards.Clear();
            ret.cards.AddRange(cards.FindAll(c => c.point == ret.pointsPair[ret.pointsPair.Count-1]));
            if (ret.pointsPair.Count > 1)
            {
                ret.cards.AddRange(cards.FindAll(c => c.point == ret.pointsPair[ret.pointsPair.Count - 2]));
                cards.Sort(new OrderCard());
                ret.cards.Add(cards.Find(c => c.point!= ret.pointsPair[ret.pointsPair.Count - 1] && c.point!= ret.pointsPair[ret.pointsPair.Count - 2]));
                ret.cardSet = CardSet.LiangDui;
            }
            else
            {
                List<Card> notPair = cards.FindAll(c => c.point != ret.pointsPair[ret.pointsPair.Count - 1]);
                notPair.Sort(new OrderCard());
                ret.cards.AddRange(notPair.GetRange(0, 3));
                ret.cardSet = CardSet.YiDui;
            }
        }
        else
        {
            ret.cards.Clear();
            cards.Sort(new OrderCard());
            ret.cards = cards.GetRange(0, 5);
        }

        // 有同花
        if (sameSuit > -1)
        {
            ret.cards.Clear();
            List<Card> sameSuitCards = cards.FindAll(c => c.suit == sameSuit);
            sameSuitCards.Sort(new OrderCard());
            // 同花且有顺子，判断是否有同花顺
            if (links.Count > 4)
            {
                List<Card> samesuitLink = new List<Card>();
                samesuitLink.Add(sameSuitCards[0]);
                for (int ss = 0; ss < sameSuitCards.Count-1; ss++)
                {
                    if (sameSuitCards[ss].point == sameSuitCards[ss+1].point+1)
                    {
                        samesuitLink.Add(sameSuitCards[ss + 1]);
                    }
                    else
                    {
                        if (samesuitLink.Count < 5)
                        {
                            samesuitLink.Clear();
                            samesuitLink.Add(sameSuitCards[ss + 1]);
                        }
                    }
                }
                if (samesuitLink.Count >= 5)
                {
                    ret.cards.AddRange(samesuitLink.GetRange(0, 5));
                    ret.cardSet = CardSet.TongHuaShun;
                }
            }
            else if ((ret.pointsFour.Count == 0) && !haveHulu)
            {
                ret.cards.AddRange(sameSuitCards.GetRange(0, 5));
                ret.cardSet = CardSet.TongHua;
            }
            // 皇家同花顺
            if (links.Count > 3 && links[links.Count - 1] == 13)
            {
                if (sameSuitCards[sameSuitCards.Count-1].point == 1)
                {
                    ret.cards.Clear();
                    ret.cards.Add(sameSuitCards[sameSuitCards.Count - 1]);
                    ret.cards.AddRange(sameSuitCards.GetRange(0, 4));
                    ret.cardSet = CardSet.HuangJia;
                }
            }
        }
        // 有顺子
        else if (links.Count > 4 && (ret.pointsFour.Count == 0) && !haveHulu)
        {
            ret.cards.Clear();
            for (int i = links.Count-1; i >= links.Count-5; i--)
            {
                ret.cards.Add(cards.Find(c => c.point == links[i]));
            }
            ret.cardSet = CardSet.ShunZi;
        }

        return ret;
    }
    #endregion 逻辑算法
}

public class CardResult
{
    public CardSet cardSet = CardSet.GaoPai;
    public List<Card> cards = new List<Card>();
    public List<Card> linkCards = new List<Card>();
    public List<int> pointsFour = new List<int>();
    public List<int> pointsThree = new List<int>();
    public List<int> pointsPair = new List<int>();
}

public enum CardSet
{
    GaoPai,
    YiDui,
    LiangDui,
    SanTiao,
    ShunZi,
    TongHua,
    HuLu,
    SiTiao,
    TongHuaShun,
    HuangJia
}

public class OrderCard : IComparer<Card>
{
    public int Compare(Card x, Card y)
    {
        if (x.point > y.point)
            return -1;
        else
            return 1;
    }
}
