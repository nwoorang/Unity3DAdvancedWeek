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
    void Update()
    {
        /*
                BigInteger a = BigInteger.Parse("123456789012345678901234567890");
                BigInteger b = new BigInteger(910121873);
                BigInteger result = a * b;
                */

        BigInteger result = BigInteger.Parse(BaseStat.text) + BigInteger.Parse(PlusStat.text);
        ResultStat.text = NumberFormatter.Format(result);

    }
}
