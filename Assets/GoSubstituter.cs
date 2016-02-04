using UnityEngine;
using System.Linq;
using System.Collections;

public class GoSubstituter : MonoBehaviour
{
    public const int Max = 600;
    static int Count = 0;
    public GameObject Substitue;
    bool StillNeedToUpdate = true;
    float WaitSeconds = 1;

    void Awake()
    {
        Substitue = Resources.Load("TriThing") as GameObject;
    }

    void Update()
    {
        WaitSeconds -= Time.deltaTime;

        if (StillNeedToUpdate && (WaitSeconds < 0))
        {
            StillNeedToUpdate = false;
            if (Count < Max)
            {
                Count++;
                Debug.Log("#" + Count + " " + (Count < Max));
                var inst = Instantiate(Substitue);
                inst.transform.parent = transform.parent;
                inst.transform.localPosition = transform.localPosition;
                inst.transform.localScale = transform.localScale;
                inst.transform.localRotation = transform.localRotation;
                //var list = inst.GetComponentsInChildren<GoSubstituter>();
                //foreach (var item in list)
                //{
                //    item.WaitSeconds =  1f;
                //}
                Destroy(this);
            }

        }
    }
}
