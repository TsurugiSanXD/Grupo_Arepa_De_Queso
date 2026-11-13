
using UnityEngine;

public class CamaraPorEfecto : MonoBehaviour
{
    [Header("Camara principal")]
    [SerializeField] private Camera camaraPrincipal;

    [Header("Encuadres")]
    [SerializeField] private Transform vistaEfecto1;
    [SerializeField] private Transform vistaEfecto2;
    [SerializeField] private Transform vistaEfecto3;

    [Header("Campo de vision por efecto")]
    [SerializeField, Range(20f, 90f)]
    private float fovEfecto1 = 45f;

    [SerializeField, Range(20f, 90f)]
    private float fovEfecto2 = 50f;

    [SerializeField, Range(20f, 90f)]
    private float fovEfecto3 = 45f;

    public void MostrarEfecto1()
    {
        AplicarEncuadre(vistaEfecto1, fovEfecto1);
    }

    public void MostrarEfecto2()
    {
        AplicarEncuadre(vistaEfecto2, fovEfecto2);
    }

    public void MostrarEfecto3()
    {
        AplicarEncuadre(vistaEfecto3, fovEfecto3);
    }

    private void AplicarEncuadre(Transform vista, float fov)
    {
        if (camaraPrincipal == null || vista == null)
        {
            Debug.LogWarning(
                "CamaraPorEfecto: falta asignar " +
                "la camara o una vista en el Inspector."
            );
            return;
        }

        camaraPrincipal.transform.SetPositionAndRotation(
            vista.position,
            vista.rotation
        );

        camaraPrincipal.fieldOfView = fov;
    }
}
