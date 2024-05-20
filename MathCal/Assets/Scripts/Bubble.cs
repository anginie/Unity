using DG.Tweening;
using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public TMP_Text TmpText;
    public Button btn;
    public Image ImgBubble;

    public int ySpeed;
    // 0不是运算符，1只有加减，2加减乘除
    public int calcType;

    public int numChar;


    void Start()
    {
        DoScaleTween();
    }

    void DoScaleTween()
    {
        var sq = DOTween.Sequence();
        sq.Append(ImgBubble.transform.DORotate(new Vector3(0, 0, -30), 2f));
        sq.Append(ImgBubble.transform.DORotate(new Vector3(0, 0, 0), 2f));
        sq.Insert(0f, ImgBubble.transform.DOScale(1.2f, 2f));
        sq.Insert(2f, ImgBubble.transform.DOScale(1f, 2f));
        //sq.Insert(0f, TmpText.transform.DOScale(1.2f, 2f));
        //sq.Insert(2f, TmpText.transform.DOScale(1f, 2f));
        sq.SetLoops(-1);
    }

    public void DoDisappear(TweenCallback tcallback)
    {
        ImgBubble.DOFade(0f, 0.5f).OnComplete(tcallback);
        ImgBubble.transform.DOScale(2f * Vector3.one, 0.5f).OnComplete(
            () =>
            {
                ImgBubble.DOFade(1f, 0.1f);
                ImgBubble.transform.DOScale(Vector3.one, 0.1f);
            });
    }

    public static string GetCalcTypeChar(int numChar)
    {
        switch (numChar)
        {
            case 1:
                return "+";
            case 2:
                return "-";
            case 3:
                return "×";
            case 4:
                return "÷";
            default:
                return "";
        }
    }

    public void Active(bool isActived)
    {
        if(btn==null)
            btn = GetComponent<Button>();
        btn.interactable = isActived;
    }

}