using UnityEngine;
using System.Collections.Generic;


public class Duel : MonoBehaviour
{
    public delegate void VoidDele (string ret);
    public VoidDele OnRoundEnd;
    public VoidDele OnDuelEnd;

    int round;
    int drawRound = 30;
    int maxVigor = 5;

    public static List<DuelAction> Actions = new List<DuelAction> ();

    public static void ParseConfig ()
    {
        TextAsset config = Resources.Load<TextAsset> ("action");
        Debug.Log("load config: " + config);
        Actions = JsonUtility.FromJson<Serialization<DuelAction>>(config.text).ToList();
    }

    public static DuelAction GetRoleAction (string actName)
    {
        for (int i = 0; i<Actions.Count; i++) {
            if (Actions [i].actionName == actName)
                return Actions [i];
        }
        return new DuelAction ();
    }

    public void CaculateAction (Role roleA, Role roleB)
    {
        int adc = GetActionResult (roleA.Act.chancesCaused, roleA.Act.damageCaused);
        int adr = GetActionResult (roleA.Act.chancesReduced,roleA.Act.damageReduced);
        int bdc = GetActionResult (roleB.Act.chancesCaused, roleB.Act.damageCaused);
        int bdr = GetActionResult (roleB.Act.chancesReduced,roleB.Act.damageReduced);
        int ad = (adc - bdr) > 0 ? adc - bdr : 0;
        int bd = (bdc - adr) > 0 ? bdc - adr : 0;

        roleA.vigor += roleA.Act.vigorSelf;
        roleA.vigor += roleB.Act.vigorTarget;
        roleA.vigor = (roleA.vigor < 0 ? 0 : roleA.vigor);
        roleA.vigor = Mathf.Clamp(roleA.vigor, 0, maxVigor);
        roleB.vigor += roleB.Act.vigorSelf;
        roleB.vigor += roleA.Act.vigorTarget;
        roleB.vigor = Mathf.Clamp(roleB.vigor, 0, maxVigor);

        roleA.hp -= bd;
        roleB.hp -= ad;
        string roundResult = "[Round " + round + "] " + roleA.Act.actionName + " vs " + roleB.Act.actionName + " >> " + 
            roleA.hp + "|" + roleA.vigor + "|" + adc + "-" + bdr + "=" + ad + " vs " + roleB.hp + "|" + roleB.vigor + "|" + bdc + "-" + adr + "=" + bd;
        if (OnRoundEnd != null)
            OnRoundEnd (roundResult);

        round++;
        if (round > drawRound) {
            EndDuel ("draw");
        } else {
            if (roleA.hp <= 0 && roleB.hp <= 0) {
                EndDuel ("draw");
            } else if (roleA.hp <= 0) {
                EndDuel ("lose");
            } else if (roleB.hp <= 0) {
                EndDuel ("win");
            }
        }
    }

    void EndDuel (string result)
    {
        round = 0;
        if (OnDuelEnd != null)
            OnDuelEnd (result);
    }

    int GetActionResult (int[] chances, int[] vals)
    {
        int ret = 0;
        if (chances != null && vals!=null && chances.Length > 0 && vals.Length > 0) {
            int chance = UnityEngine.Random.Range(0, chances[chances.Length-1]);
            int flag = 0;
            while(chances[flag] < chance)
            {
                flag++;
            }
            if (vals[flag] == vals[flag + 1])
            {
                ret = vals[flag];
            }
            else  if (vals[flag] < vals[flag + 1])
            {
                ret = UnityEngine.Random.Range(vals[flag], vals[flag + 1]);
            }
            //Debug.Log(chance + " >> " + flag + " : " + ret);
        }
        return ret;
    }
}