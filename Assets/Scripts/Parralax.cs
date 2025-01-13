using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    private MeshRenderer MeshRenderer;
    [SerializeField] float animationSpeed = 1f;
    [SerializeField] float randomFactor = 1f;

    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        MeshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, animationSpeed * Time.deltaTime * Random.Range(-randomFactor, randomFactor));
    }
}
