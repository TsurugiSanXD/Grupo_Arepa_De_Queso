
using UnityEngine;
using TMPro;

public class ControladorDemo : MonoBehaviour
{
    [Header("Pantallas")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelDemo;

    [Header("Interfaz")]
    [SerializeField] private TMP_Text textoEstado;

    [Header("Efectos de prueba")]
    [SerializeField] private GameObject efecto1;
    [SerializeField] private GameObject efecto2;
    [SerializeField] private GameObject efecto3;

    private GameObject[] efectos;
    private Vector3[] posicionesIniciales;
    private Quaternion[] rotacionesIniciales;

    private int efectoSeleccionado = -1;
    private bool reproduciendo = false;
    private float tiempoEfecto = 0f;

    void Start()
    {
        efectos = new GameObject[] { efecto1, efecto2, efecto3 };

        posicionesIniciales = new Vector3[3];
        rotacionesIniciales = new Quaternion[3];

        for (int i = 0; i < efectos.Length; i++)
        {
            if (efectos[i] == null)
            {
                Debug.LogError("Falta asignar el efecto " + (i + 1));
                enabled = false;
                return;
            }

            posicionesIniciales[i] = efectos[i].transform.localPosition;
            rotacionesIniciales[i] = efectos[i].transform.localRotation;
            efectos[i].SetActive(false);
        }

        panelInicio.SetActive(true);
        panelDemo.SetActive(false);
        textoEstado.text = "Selecciona un efecto";
    }

    public void IniciarDemo()
    {
        panelInicio.SetActive(false);
        panelDemo.SetActive(true);
    }

    public void SeleccionarEfecto1()
    {
        SeleccionarEfecto(0);
    }

    public void SeleccionarEfecto2()
    {
        SeleccionarEfecto(1);
    }

    public void SeleccionarEfecto3()
    {
        SeleccionarEfecto(2);
    }

    private void SeleccionarEfecto(int numero)
    {
        reproduciendo = false;
        tiempoEfecto = 0f;
        efectoSeleccionado = numero;

        for (int i = 0; i < efectos.Length; i++)
        {
            efectos[i].SetActive(i == numero);
            efectos[i].transform.localPosition = posicionesIniciales[i];
            efectos[i].transform.localRotation = rotacionesIniciales[i];
        }

        textoEstado.text = "Efecto " + (numero + 1) + " seleccionado";
    }

    public void Reproducir()
    {
        if (efectoSeleccionado == -1)
        {
            textoEstado.text = "Primero selecciona un efecto";
            return;
        }

        reproduciendo = true;
        textoEstado.text = "Reproduciendo efecto " + (efectoSeleccionado + 1);
    }

    public void Pausar()
    {
        if (efectoSeleccionado == -1)
            return;

        reproduciendo = false;
        textoEstado.text = "Efecto " + (efectoSeleccionado + 1) + " pausado";
    }

    void Update()
    {
        if (!reproduciendo || efectoSeleccionado == -1)
            return;

        tiempoEfecto += Time.deltaTime;

        Transform objeto = efectos[efectoSeleccionado].transform;

        // Movimiento vertical
        float altura = Mathf.Sin(tiempoEfecto * 3f) * 0.3f;

        objeto.localPosition =
            posicionesIniciales[efectoSeleccionado] +
            Vector3.up * altura;

        // Rotacion
        objeto.Rotate(0f, 80f * Time.deltaTime, 0f, Space.Self);
    }
}
