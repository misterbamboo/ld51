using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PivotFIx : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(gameObject.GetComponent<Image>().sprite.pivot);
    }
}
