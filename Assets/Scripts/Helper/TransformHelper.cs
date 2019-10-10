using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper{
    public static Transform DeepFind(this Transform parent, string target)
    {
        Transform temp = null;
        foreach (Transform child in parent)
        {
            if(child.name == target)
            {
                return child;
            }
            else
            {
                temp = child.DeepFind(target);
                if(temp != null)
                {
                    return temp;
                }
            }
        }
        return temp;
    }
}
