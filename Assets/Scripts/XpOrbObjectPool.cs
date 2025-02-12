using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrbObjectPool : MonoBehaviour
{
    [SerializeField] XpOrb xpOrbPrefab;
    List<XpOrb> pool = new();

    
    public XpOrb RequestXpOrb(Transform requesteeTransform)
    {
        XpOrb orb = GetInactiveInPool();
        if (orb != null)
        {   
            orb.transform.position = requesteeTransform.position;
            orb.gameObject.SetActive(true);
            return orb;
        }
        else
        {
            orb = Instantiate(xpOrbPrefab, requesteeTransform.position, Quaternion.identity, transform);
            pool.Add(orb);
            orb.gameObject.SetActive(true);
            return orb;
        }
    }

    /// <returns>
    /// The first inactive Object in pool, if none return null
    /// </returns>
    XpOrb GetInactiveInPool()
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeSelf)
            {
                return item;
            }
        }
        return null;
    }

    public void Deactivate(XpOrb xpOrb)
    {
        xpOrb.gameObject.SetActive(false);
    }
}
