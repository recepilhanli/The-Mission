using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{

    [SerializeField]
    public Color Color;
    private Renderer render;

    void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        render.material.color = Color;
    }

    void Update()
    {
        if(render.material.color != Color) render.material.color = Color;

        gameObject.transform.rotation = Quaternion.Euler(90, 45, 270);
    }


}
