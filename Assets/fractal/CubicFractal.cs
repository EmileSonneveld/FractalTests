using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubicFractal : MonoBehaviour
{
    enum SignedDimentions
    {
        None,
        XPos, YPos, ZPos,
        XNeg, YNeg, ZNeg
    }

    Dictionary<SignedDimentions, Vector3> DimMap = new Dictionary<SignedDimentions, Vector3>()
    {
        { SignedDimentions.None, Vector3.zero},
        { SignedDimentions.XPos, Vector3.left},
        { SignedDimentions.YPos, Vector3.up},
        { SignedDimentions.ZPos, Vector3.forward},
        { SignedDimentions.XNeg, - Vector3.left},
        { SignedDimentions.YNeg, - Vector3.up},
        { SignedDimentions.ZNeg, - Vector3.forward},
    };

    public GameObject cube;

    void Start()
    {
        RecursePlaceCube(0, this.transform, SignedDimentions.None);
        // GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
    }

    public float scale = 0.6f;
    public int itterations = 4;

    void RecursePlaceCube(int itteration, Transform prevTrans, SignedDimentions dir)
    {
        var inst = Instantiate(cube);
        inst.transform.parent = prevTrans;
        inst.transform.localScale = Vector3.one * scale;
        inst.transform.localPosition = DimMap[dir] / 2; // + new Vector3(0,  prevTrans.transform.localScale.y/2, 0); // inst.transform.localScale.y/2 +

        itteration++;
        if (itteration > itterations)
            return;

        RecursePlaceCube(itteration, inst.transform, SignedDimentions.XPos);
        RecursePlaceCube(itteration, inst.transform, SignedDimentions.YPos);
        RecursePlaceCube(itteration, inst.transform, SignedDimentions.ZPos);
        RecursePlaceCube(itteration, inst.transform, SignedDimentions.XNeg);
        RecursePlaceCube(itteration, inst.transform, SignedDimentions.YNeg);
        RecursePlaceCube(itteration, inst.transform, SignedDimentions.ZNeg);

        //RecursePlaceCube(itteration, inst.transform, inst.transform.localScale.x * scale * Vector3.left);
        //RecursePlaceCube(itteration, inst.transform, inst.transform.localScale.y * scale * Vector3.up);
        //RecursePlaceCube(itteration, inst.transform, inst.transform.localScale.z * scale * Vector3.forward);
        //RecursePlaceCube(itteration, inst.transform, -inst.transform.localScale.x * scale * Vector3.left);
        //RecursePlaceCube(itteration, inst.transform, -inst.transform.localScale.y * scale * Vector3.up);
        //RecursePlaceCube(itteration, inst.transform, -inst.transform.localScale.z * scale * Vector3.forward);

    }
}
