using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecenterImagePivot : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var img = GetComponent<Image>();
        float offsetY = img.rectTransform.rect.height * Mathf.Abs((img.sprite.pivot.y / img.sprite.rect.height) - .5f);

        img.rectTransform.offsetMin = new Vector2(img.rectTransform.offsetMin.x, -offsetY);
        img.rectTransform.offsetMax = new Vector2(img.rectTransform.offsetMax.x, -offsetY);
    }

}
