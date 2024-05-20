using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLaunch : MonoBehaviour
{
    public Button BtnStart;
    public ToggleGroup TggMaxNum;
    public ToggleGroup TggCalType;
    public GameObject GoGame;
    public GameObject GoLaunch;

    public GameObject BubblesContainer;
    public TMP_Text CalcText;
    public TMP_Text ResultText;
    public AudioSource Audio;

    List<Bubble> bubbleList = new List<Bubble>();
    List<int> posList = new List<int>();

    int posIndex;
    int MaxBubblesInScreen = 24;
    int MaxPos = 12;

    int MaxNum = 10;
    int resultNum;
    int currentNum;
    int currentCalc;
    // 0初始化未输入内容，1输入数字未输入运算符号，2输入运算符号
    int calcState;
    int SelectCalcType = 1;

    void Start()
    {
        BtnStart.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        MaxNum = int.Parse(TggMaxNum.GetFirstActiveToggle().GetComponentInChildren<Text>().text);
        SelectCalcType = TggCalType.GetFirstActiveToggle().name == "AM" ? 1 : 2;
        Debug.Log("Game start");

        GoGame.SetActive(true);
        GoLaunch.SetActive(false);

        InitPosList();
        resultNum = Random.Range(0, SelectCalcType == 2 ? MaxNum * MaxNum : MaxNum * 2);
        ResultText.text = resultNum.ToString();
        StartCoroutine(CreateBubbles());
    }

    IEnumerator CreateBubbles()
    {
        GameObject bubblePrefab = Resources.Load<GameObject>("Bubble");
        for (int i = 0; i < MaxBubblesInScreen; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject bubbleGo = Instantiate(bubblePrefab, BubblesContainer.transform);
            Bubble bubble = bubbleGo.GetComponent<Bubble>();
            bubble.GetComponent<Button>().onClick.AddListener(()=>OnClicked(bubble));
            RandomResetBubble(bubble);
            ActiveBubble(bubble);
            bubbleList.Add(bubble);
        }
    }

    void RandomResetBubble(Bubble bubble)
    {
        int flag = Random.Range(0, 3);
        if (flag > 0)
        {
            ResetBubble(bubble, Random.Range(1, MaxNum));
        }
        else
        {
            ResetBubble(bubble, Random.Range(1, SelectCalcType == 1 ? 3 : 5), SelectCalcType);
        }
    }

    void ResetBubble(Bubble bubble, int numChar, int calcType = 0)
    {
        float xPos = (1f * posList[posIndex] / posList.Count - 0.5f) * Screen.width + bubble.GetComponent<RectTransform>().sizeDelta.x;
        posIndex++;
        if (posIndex > posList.Count - 1)
        {
            posList = RandomList(posList);
            posIndex = 0;
        }
        bubble.transform.localPosition = new Vector3(xPos, -0.5f * Screen.height, 0);
        //bubble.ySpeed = MaxPos * 2;
        bubble.ySpeed = Random.Range(MaxPos * 20, MaxPos * 25);
        bubble.numChar = numChar;
        bubble.calcType = calcType;
        bubble.TmpText.text = calcType != 0 ? Bubble.GetCalcTypeChar(bubble.numChar) : numChar.ToString();
    }

    void InitPosList()
    {
        string logPos = "";
        for (int i = 0; i < MaxPos; i++)
        {
            posList.Add(i);
        }
        for (int j = 0; j < MaxPos; j++)
        {
            logPos = logPos + posList[j].ToString() + ",";
        }
        Debug.Log("Before Random: " + logPos);
        posList = RandomList(posList);
    }

    List<int> RandomList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, list.Count - 1);
            int tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
        string logPos = "";
        for (int k = 0; k < posList.Count; k++)
        {
            logPos = logPos + posList[k].ToString() + ",";
        }
        Debug.Log("After Random: " + logPos);
        return list;
    }

    void OnClicked(Bubble bubble)
    {
        Debug.Log(" clicked " + bubble.numChar);
        if (bubble.calcType == 0 )
        {
            if (currentCalc == 0)
            {
                currentNum = bubble.numChar;
                CalcText.text = currentNum.ToString();
                bubble.DoDisappear(()=>RandomResetBubble(bubble));
                calcState = 1;
                ActiveBubbles();
            }
            else
            {
                int calcResult = Calc(currentNum, bubble.numChar, currentCalc);
                CalcText.text = currentNum.ToString() + Bubble.GetCalcTypeChar(currentCalc) + bubble.numChar + "=" + calcResult;
                currentNum = calcResult;
                bubble.DoDisappear(() => RandomResetBubble(bubble));
                calcState = 1;
                ActiveBubbles();
            }
        }
        else
        {
            currentCalc = bubble.numChar;
            CalcText.text = currentNum.ToString() + Bubble.GetCalcTypeChar(currentCalc);
            calcState = 2;
            bubble.DoDisappear(() => RandomResetBubble(bubble));
            ActiveBubbles();
        }
    }

    int Calc(int a, int b, int optChar)
    {
        switch (optChar)
        {
            case 1:
                return a + b;
            case 2:
                return a - b;
            case 3:
                return a * b;
            case 4:
                return Mathf.CeilToInt(a / b);
            default:
                return a;
        }
    }

    void ActiveBubble(Bubble bubble)
    {
        bubble.Active(
            (calcState == 0 && bubble.calcType == 0)
            || (calcState == 1 && bubble.calcType != 0)
            || calcState == 2);
    }

    void ActiveBubbles()
    {
        Audio.Play();
        CheckResult();
        for (int i = 0; i < bubbleList.Count; i++)
        {
            ActiveBubble(bubbleList[i]);
        }
    }

    void CheckResult()
    {
        if (currentNum == resultNum)
        {
            CalcText.transform.parent.GetComponent<Image>().color = Color.green;
        }
    }
    
    void Update()
    {
        for (int i = 0;i < bubbleList.Count; i++)
        {
            bubbleList[i].transform.Translate(0.001f * bubbleList[i].ySpeed * Vector3.up);
            if(bubbleList[i].transform.localPosition.y > 0.5f * Screen.height + 100)
            {
                RandomResetBubble(bubbleList[i]);
                ActiveBubble(bubbleList[i]);
            }
        }
    }
}