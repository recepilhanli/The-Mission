using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xRect : MonoBehaviour
{

    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.localPosition = new Vector2(Time.deltaTime*18 + rectTransform.localPosition.x, rectTransform.localPosition.y);
        if(rectTransform.position.x >= 785) rectTransform.localPosition = new Vector2(-785, rectTransform.localPosition.y);
    }
}
