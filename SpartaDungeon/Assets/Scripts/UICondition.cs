using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition jumpgauge;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;   
    }
}
