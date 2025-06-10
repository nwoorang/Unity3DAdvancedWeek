using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Numerics;
public class StatusUI : MonoBehaviour
{
    int BaseAtk = 10;
    int BaseDEF = 5;
    int BaseHP = 100;
    int BaseCrit = 25;
    //장비착용을 하면 여기를 불러서 갱신
    public TextMeshProUGUI ATK;
    public TextMeshProUGUI DEF;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI CRIT;

    int IntAddATK=0;
    int IntAddDEF = 0;
    int IntAddHP = 0;
    int IntAddCRIT = 0;

    public TextMeshProUGUI AddATK;
    public TextMeshProUGUI AddDEF;
    public TextMeshProUGUI AddHP;
    public TextMeshProUGUI AddCRIT;

 BigInteger result1;
  BigInteger result2;
   BigInteger result3;
    BigInteger result4;
    public TextMeshProUGUI SumATK;
    public TextMeshProUGUI SumDEF;
    public TextMeshProUGUI SumHP;
    public TextMeshProUGUI SumCRIT;
    void Start()
    {
        ATK.text = BaseAtk.ToString();
        DEF.text = BaseDEF.ToString();
        HP.text = BaseHP.ToString();
        CRIT.text = BaseCrit.ToString();
        result1 = new BigInteger();
        result2 = new BigInteger();
        result3 = new BigInteger();
        result4 = new BigInteger();
                
    }
    public void ChangeStat(int wDamage, int wDefense, int wHP, int wCrit)
    {
        IntAddATK += wDamage;
        IntAddDEF += wDefense;
        IntAddHP += wHP;
        IntAddCRIT += wCrit;
        AddATK.text = IntAddATK.ToString();
        AddDEF.text = IntAddDEF.ToString();
        AddHP.text = IntAddHP.ToString();
        AddCRIT.text = IntAddCRIT.ToString();
        SumATK.text = (BaseAtk + IntAddATK).ToString();
        SumDEF.text = (BaseDEF + IntAddDEF).ToString();
        SumHP.text = (BaseHP + IntAddHP).ToString();
        SumCRIT.text = (BaseCrit + IntAddCRIT).ToString();


                                
    }
}
