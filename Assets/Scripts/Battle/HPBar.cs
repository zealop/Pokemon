using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    private void Start()
    {
        //health.transform.localScale = new Vector3(0.5f, 1f);
    }

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHP;

        while(curHp - newHP > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curHp, 1f);

            yield return null;
        }

        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
