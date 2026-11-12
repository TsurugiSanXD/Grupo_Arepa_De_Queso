
using UnityEngine;
using TMPro;

public class ControladorDemo : MonoBehaviour
{
    [Header("Pantallas")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelDemo;

    [Header("Interfaz")]
    [SerializeField] private TMP_Text textoEstado;

    [Header("Personaje")]
    [SerializeField] private Animator animatorPersonaje;

    [Header("Efectos de prueba")]
    [SerializeField] private GameObject efecto1;
    [SerializeField] private GameObject efecto2;
    [SerializeField] private GameObject efecto3;

    private const string IDLE = "Base Layer.Idle";
    private const string CAST = "Base Layer.Cast";

    private GameObject[] efectos;
    private Vector3[] posicionesIniciales;
    private Quaternion[] rotacionesIniciales;

    private int efectoSeleccionado = -1;
    private bool reproduciendo = false;
    private bool iniciado = false;
    private bool terminado = false;
    private float tiempoEfecto = 0f;

    void Start()
    {
        if (panelInicio == null || panelDemo == null ||
            textoEstado == null || animatorPersonaje == null)
        {
            Debug.LogError("Faltan referencias en ControladorDemo.");
            enabled = false;
            return;
        }

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

        animatorPersonaje.applyRootMotion = false;
        VolverAIdle();
    }

    private void VolverAIdle()
    {
        animatorPersonaje.speed = 1f;
        animatorPersonaje.Play(IDLE, 0, 0f);
        animatorPersonaje.Update(0f);
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
        efectoSeleccionado = numero;
        reproduciendo = false;
        iniciado = false;
        terminado = false;
        tiempoEfecto = 0f;

        for (int i = 0; i < efectos.Length; i++)
        {
            efectos[i].SetActive(i == numero);

            efectos[i].transform.localPosition =
                posicionesIniciales[i];

            efectos[i].transform.localRotation =
                rotacionesIniciales[i];
        }

        VolverAIdle();

        textoEstado.text =
            "Efecto " + (numero + 1) + " seleccionado";
    }

    public void Reproducir()
    {
        if (efectoSeleccionado == -1)
        {
            textoEstado.text = "Primero selecciona un efecto";
            return;
        }

        if (reproduciendo)
            return;

        // Si nunca se inició o ya terminó,
        // Play comienza una nueva reproducción.
        if (!iniciado || terminado)
        {
            Reiniciar();
            return;
        }

        // Si estaba pausado, continúa.
        reproduciendo = true;
        animatorPersonaje.speed = 1f;

        textoEstado.text =
            "Reproduciendo efecto " + (efectoSeleccionado + 1);
    }

    public void Pausar()
    {
        if (efectoSeleccionado == -1 || !reproduciendo)
            return;

        reproduciendo = false;
        animatorPersonaje.speed = 0f;

        textoEstado.text =
            "Efecto " + (efectoSeleccionado + 1) + " pausado";
    }

    public void Reiniciar()
    {
        if (efectoSeleccionado == -1)
        {
            textoEstado.text = "Primero selecciona un efecto";
            return;
        }

        tiempoEfecto = 0f;
        iniciado = true;
        terminado = false;
        reproduciendo = true;

        Transform objeto = efectos[efectoSeleccionado].transform;

        objeto.localPosition =
            posicionesIniciales[efectoSeleccionado];

        objeto.localRotation =
            rotacionesIniciales[efectoSeleccionado];

        animatorPersonaje.speed = 1f;

        // 0f significa comenzar en el primer fotograma.
        animatorPersonaje.Play(CAST, 0, 0f);
        animatorPersonaje.Update(0f);

        textoEstado.text =
            "Reproduciendo efecto " + (efectoSeleccionado + 1);
    }

    void Update()
    {
        if (!reproduciendo || efectoSeleccionado == -1)
            return;

        tiempoEfecto += Time.deltaTime;

        Transform objeto = efectos[efectoSeleccionado].transform;

        float altura = Mathf.Sin(tiempoEfecto * 3f) * 0.3f;

        objeto.localPosition =
            posicionesIniciales[efectoSeleccionado] +
            Vector3.up * altura;

        objeto.Rotate(0f, 80f * Time.deltaTime, 0f, Space.Self);

        // Detectar cuando termina la animación Cast.
        AnimatorStateInfo estado =
            animatorPersonaje.GetCurrentAnimatorStateInfo(0);

        if (estado.IsName(CAST) &&
            estado.normalizedTime >= 1f &&
            !animatorPersonaje.IsInTransition(0))
        {
            reproduciendo = false;
            terminado = true;

            VolverAIdle();

            textoEstado.text =
                "Efecto " + (efectoSeleccionado + 1) +
                " terminado. Pulsa Reiniciar";
        }
    }
}
