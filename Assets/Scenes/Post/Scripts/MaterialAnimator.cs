using System;
using UnityEngine;

[Serializable]
struct FloatCurve
{
    
    public string materialProperty;
    public AnimationCurve curve;


}

[RequireComponent(typeof(ParticleSystem))]
[ExecuteInEditMode]
public class MaterialAnimator : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;

    private ParticleSystem particles;

    [SerializeField] private FloatCurve[] curves;   

    [SerializeField] private float particleTime;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Evaluate()
    {
        particleTime = particles.time;
        foreach (FloatCurve curve in curves)
        {

            targetMaterial? .SetFloat(curve.materialProperty, curve.curve.Evaluate(particles.time));

        }
    }

    private void Update()
    {
        Evaluate();
    }
}