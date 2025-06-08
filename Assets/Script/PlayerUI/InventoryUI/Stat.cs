using UnityEngine;

[System.Serializable]
public class Stat
{
    public float current;
    public float max;
    public float regenPerSecond;    //초당 증가량

    public void Regenerate(float time)  //시간을 받아서 그 시간만큼 회복
    {
        current = Mathf.Clamp(current + regenPerSecond * time, 0 , max);  //회복량 (감소량으로도 사용 가능)
    }

    public void Substract(float amount)
    {
        current = Mathf.Max(current - amount, 0);   //감소
    }

    public void Add(float amount)
    {
        current = Mathf.Min(current + amount, max); //증가
    }

    public bool IsMin => current <= 0;
    public bool IsMax => current >= max;
}
