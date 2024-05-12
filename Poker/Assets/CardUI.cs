using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField]
    Image imgSuit;
    [SerializeField]
    Text txtPoint;
    [SerializeField]
    Image imgBg;

    public void InitCardUI(Card card, bool showBg = false)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("suit");
        imgSuit.sprite = sprites[card.suit];
        txtPoint.text = pointShow(card.point);
        imgBg.gameObject.SetActive(showBg);
    }

    public void ShowBg(bool b)
    {
        imgBg.gameObject.SetActive(b);
    }

    string pointShow(int point)
    {
        if (point==10)
            return "T";
        if (point == 11)
            return "J";
        if (point == 12)
            return "Q";
        if (point == 13)
            return "K";
        if (point == 1)
            return "A";
        return "" + point;
    }
}

public class Card
{
    // 花色，0-3红黑梅方
    public int suit;
    // 点数，1-13
    public int point;

    public Card(int suit, int point)
    {
        this.suit = suit;
        this.point = point;
    }
}