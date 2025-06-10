using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;

public class BigIntegerTest : MonoBehaviour
{
    public TextMeshProUGUI BaseStat;
        public TextMeshProUGUI PlusStat;
            public TextMeshProUGUI ResultStat;

    BigInteger result;
    void Start()
    {
        result = new BigInteger();
    }
    void Update()
    {
        /*
                BigInteger a = BigInteger.Parse("123456789012345678901234567890");
                BigInteger b = new BigInteger(910121873);
                BigInteger result = a * b;
        */

        result = BigInteger.Parse(BaseStat.text) + BigInteger.Parse(PlusStat.text);
        //입력값이 biginteger가 넘어갈 경우가 좀 적음 그래서 안쓰는 위주로
        //스탯부분도 일정 단위가 넘어가면 새로운 가치의 재화로 전환

        //출력값을 biginteger로 권장
        ResultStat.text = NumberFormatter.Format(result);

    }
}
