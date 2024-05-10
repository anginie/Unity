//#define SHOW_ONCE
#define SHOW_DETAIL

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestDuel : MonoBehaviour
{
    delegate void DoTestDele();
    DoTestDele DoTestAction;

    int duelCount;
    int winCount;
    int loseCount;
    int drawCount;
    bool duelEnd = false;
    int round;
    int totalRound;

    Role RoleSelf;
    Role RoleTarget;
    List<DuelAction> Actions;

    Duel duel;

    int[] selfRates = new int[] { 10, 10, 10, 1000, 10, 10, 10, 10, 10, 10, 10, 10 };
    int[] targetRates = new int[] { 10, 10, 10, 1000, 10, 10, 10, 10, 10, 10, 10, 10 };

    void Start()
    {
        duel = GetComponent<Duel>();
        duel.OnRoundEnd += OnRoundEnd;
        duel.OnDuelEnd += OnDuelEnd;
        Duel.ParseConfig();
        Actions = Duel.Actions;

        TestTimes(100);
    }

    void Update()
    {
#if SHOW_ONCE
        if(!duelEnd && Time.realtimeSinceStartup > fromNow) {
			fromNow += 2f;
			DoAction ();
		}
#endif
    }

    void OnDuelEnd(string result)
    {
#if SHOW_DETAIL
        Debug.Log("Duel end " + result + " round: " + round);
#endif
        totalRound += round;
        duelCount++;
        if (result == "draw")
        {
            drawCount++;
        }
        else if (result == "lose")
        {
            loseCount++;
        }
        else if (result == "win")
        {
            winCount++;
        }
        duelEnd = true;
    }

    void OnRoundEnd(string result)
    {
        round++;
#if SHOW_DETAIL
        //Debug.Log(result);
#endif
    }


    public void InitRoles(int[] selfRate, int[] targetRate)
    {
        RoleSelf = new Role();
        RoleSelf.ActionRates = selfRate;
        RoleTarget = new Role();
        RoleTarget.ActionRates = targetRate;
    }


    void TestTimes(int tCount)
    {
        DoTestAction = null;
        DoTestAction += DoAction;
        int testCount = tCount;
        DoTestByTimes(testCount);
        Debug.Log("timeCost: " + (int)Time.realtimeSinceStartup +
            ">> win:" + winCount + " | draw:" + drawCount + " | lose:" + loseCount + " / testcount:" + testCount + " totalRound: " + totalRound);
    }

    void DoTestByTimes(int total)
    {
        InitRoles(selfRates, targetRates);
        StartDuel();
        totalRound = duelCount = winCount = loseCount = drawCount = 0;
        duelEnd = false;
        while (duelCount < total)
        {
            if (!duelEnd)
            {
                if (DoTestAction != null)
                    DoTestAction();
            }
            else
            {
                round = 0;
                duelEnd = false;
                StartDuel();
            }
        }
    }

    void StartDuel()
    {
        RoleSelf.hp = 100;
        RoleSelf.vigor = 5;
        RoleTarget.hp = 100;
        RoleTarget.vigor = 5;
        duelEnd = false;
    }

    void DoAction()
    {
        DoAction(GetRoleAction(RoleSelf, RoleTarget), GetRoleAction(RoleTarget, RoleSelf));
    }

    void DoAction(DuelAction actA, DuelAction actB)
    {
        RoleSelf.Act = actA;
        RoleTarget.Act = actB;
        duel.CaculateAction(RoleSelf, RoleTarget);
    }

    DuelAction GetRoleAction(Role pRoleSelf, Role pRoleTarget)
    {
        int actIndex = 0;
        int totalR = GetRate(pRoleSelf.ActionRates, pRoleSelf.ActionRates.Length - 1);
        int r = UnityEngine.Random.Range(0, totalR);
        for (int i = 0; i < pRoleSelf.ActionRates.Length; i++)
        {
            int iR = GetRate(pRoleSelf.ActionRates, i);
            if (r >= iR)
            {
                actIndex = i;
            }
        }
#if SHOW_ONCE
        Debug.Log("r: " + r + " actIndex: " + actIndex);
#endif
        DuelAction act = Actions[actIndex];
        if (pRoleSelf.vigor + act.vigorSelf < 0 || pRoleTarget.vigor < act.vigorTarget || pRoleSelf.vigor + act.vigorSelf > 5)
            return GetRoleAction(pRoleSelf, pRoleTarget);
        else
            return act;
    }

    int GetRate(int[] actionRates, int index)
    {
        if (index == 0)
            return actionRates[0];
        return actionRates[index] + GetRate(actionRates, index - 1);
    }
}
