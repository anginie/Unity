public class Role
{
    public int hp;
    public int vigor;
    public DuelAction Act;
    public int[] ActionRates;
    public bool CheckAction(string actionName)
    {
        DuelAction action = Duel.GetRoleAction(actionName);
        return vigor + action.vigorSelf >= 0;
    }
}
