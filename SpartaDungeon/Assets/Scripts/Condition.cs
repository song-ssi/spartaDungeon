using UnityEngine;
using UnityEngine.UI;

// 컨디션 값 조정, 컨디션 ui 업데이트
public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue+amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }
    public bool PossibleJump(float amount) 
    {
        return amount <= curValue;
    }  

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
