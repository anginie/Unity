using System;
using UnityEngine;

[Serializable]
public class DuelAction
{
    [SerializeField]
    public int actionId;

    [SerializeField]
    public string actionName;

    [SerializeField]
    public int vigorSelf;

    [SerializeField]
    public int vigorTarget;

    [SerializeField]
    public int[] damageCaused;

    [SerializeField]
    public int[] chancesCaused;

    [SerializeField]
    public int[] damageReduced;

    [SerializeField]
    public int[] chancesReduced;
}