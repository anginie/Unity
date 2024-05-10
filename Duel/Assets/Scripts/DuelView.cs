using UnityEngine;
using UnityEngine.UI;

public class DuelView : MonoBehaviour {

	public GridLayoutGroup GdActions;
	public Slider PbSelfHp;
	public Slider PbSelfVigour;
	public Slider PbTargetHp;
	public Slider PbTargetVigour;
	public Text LaActions;
    public Button BtnReset;

	private Button[] btnActions;
    private Duel duel;
    private Role RoleSelf;
    private Role RoleTarget;

    bool showLog = false;

    void Start () {
        Duel.ParseConfig();

        btnActions = GdActions.GetComponentsInChildren<Button>();
        for (int i = 0; i < Duel.Actions.Count; i++)
        {
            DuelAction action = Duel.Actions[i];
            Button btn = btnActions[i];
            btn.GetComponentInChildren<Text>().text = action.actionName;
            btn.onClick.AddListener(() =>
            {
                Debug.Log(btn.gameObject.name + " clicked: " + action.actionName);

                if (RoleSelf.CheckAction(action.actionName))
                {
                    if (!showLog)
                    {
                        duel.OnRoundEnd += (ret) => { 
                            LaActions.text += ("\n" + ret);
                        };
                        duel.OnDuelEnd += (ret) =>
                        {
                            LaActions.text += "\n" + ret;
                            Reset();
                        };
                        showLog = true;
                    }

                    DoAction(action.actionName);
                }
                else
                {
                    Debug.Log("WARNING: Lack Vigor !!!");
                }
            });
        }

        duel = GetComponent<Duel>();
        duel.OnRoundEnd += Refresh;
        BtnReset.onClick.AddListener(Reset);
        Reset();
    }

    void DoAction(string actName)
    {
        RoleSelf.Act = Duel.GetRoleAction(actName);
        RoleTarget.Act = Duel.GetRoleAction(RandomAction());
        duel.CaculateAction(RoleSelf, RoleTarget);
    }

    void Reset()
    {
        InitRoles ();
		LaActions.text = "";
        Refresh("[Duel Start]");
    }

    void InitRoles()
    {
        RoleSelf = new Role();
        RoleSelf.hp = 100;
        RoleSelf.vigor = 5;

        RoleTarget = new Role();
        RoleTarget.hp = 100;
        RoleTarget.vigor = 5;
    }

    void Refresh(string ret)
    {
        PbSelfHp.value = 0.01f * RoleSelf.hp;
        PbSelfVigour.value = 0.2f * RoleSelf.vigor;
        PbTargetHp.value = 0.01f * RoleTarget.hp;
        PbTargetVigour.value = 0.2f * RoleTarget.vigor;



        for(int i = 0; i< Duel.Actions.Count; i++) {
            btnActions[i].enabled = (RoleSelf.vigor + Duel.Actions[i].vigorSelf >= 0);
        }
    }

    string RandomAction()
    {
        int actIndex = Random.Range(0, Duel.Actions.Count);
        string actName = Duel.Actions[actIndex].actionName;
        if (RoleTarget.CheckAction(actName))
        {
            return actName;
        }
        else
        {
            return RandomAction();
        }
    }
}
