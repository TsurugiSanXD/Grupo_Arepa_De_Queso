
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using System.Collections.Generic;

public class ControladorDemo : MonoBehaviour
{
    [Header("Pantallas")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelDemo;

    [Header("Interfaz")]
    [SerializeField] private TMP_Text textoEstado;

    [Header("Personaje")]
    [SerializeField] private Animator animatorPersonaje;

    [Header("Posicion del personaje por efecto")]
    [SerializeField] private Transform raizPersonaje;

    [SerializeField]
    private Vector3 posicionEfecto1 =
        new Vector3(-1.8f, 0f, 0f);

    [SerializeField]
    private Vector3 rotacionEfecto1 =
        new Vector3(0f, 90f, 0f);

    [SerializeField]
    private Vector3 posicionEfecto2 =
        new Vector3(0f, 0f, 0f);

    [SerializeField]
    private Vector3 rotacionEfecto2 =
        new Vector3(0f, 180f, 0f);

    [SerializeField]
    private Vector3 posicionEfecto3 =
        new Vector3(0f, 0f, 0f);

    [SerializeField]
    private Vector3 rotacionEfecto3 =
        new Vector3(0f, 180f, 0f);

    [Header("Efectos")]
    [SerializeField] private GameObject efecto1;
    [SerializeField] private GameObject efecto2;
    [SerializeField] private GameObject efecto3;

    [Header("Animaciones por efecto")]
    [SerializeField]
    private string animacionIdle =
        "Base Layer.Idle";

    [SerializeField]
    private string animacionEfecto1 =
        "Base Layer.Cast";

    [SerializeField]
    private string animacionEfecto2 =
        "Base Layer.AuraCast";

    [SerializeField]
    private string animacionEfecto3 =
        "Base Layer.SlashCast";

    [Header("Transicion suave a Idle")]
    [SerializeField, Range(0.05f, 1f)]
    private float duracionTransicionIdle = 0.25f;

    [SerializeField, Range(0.7f, 1f)]
    private float inicioRetornoAura = 0.92f;

    [SerializeField, Range(0.7f, 1f)]
    private float inicioRetornoEfecto3 = 0.92f;

    [Header("Sincronizacion Efecto 1")]
    [Tooltip("Negativo: VFX primero. Positivo: personaje primero.")]
    [FormerlySerializedAs("retrasoInvocacion")]
    [SerializeField] private float desfaseInvocacion = -0.8f;

    [Header("Sincronizacion Efecto 3")]
    [Tooltip("Segundos que espera el Slash antes de reproducirse.")]
    [SerializeField, Min(0f)]
    private float retrasoEfecto3 = 0f;

    private GameObject[] efectos;
    private Vector3[] posiciones;
    private Quaternion[] rotaciones;

    private readonly List<ParticleSystem> emisoresRaiz =
        new List<ParticleSystem>();

    private readonly List<ParticleSystem> pausados =
        new List<ParticleSystem>();

    private ParticleSystem[] particulas =
        new ParticleSystem[0];

    private int seleccionado = -1;

    private bool iniciado = false;
    private bool reproduciendo = false;
    private bool animacionIniciada = false;
    private bool animacionFinalizada = false;

    private bool esperandoAnimacion = false;
    private bool esperandoVFX = false;

    private float tiempoEspera = 0f;
    private string animacionActual = "";

    // =====================================
    // INICIALIZACION
    // =====================================

    void Start()
    {
        if (panelInicio == null ||
            panelDemo == null ||
            textoEstado == null ||
            animatorPersonaje == null ||
            efecto1 == null ||
            efecto2 == null ||
            efecto3 == null)
        {
            Debug.LogError(
                "ControladorDemo: faltan referencias en Systems."
            );
            enabled = false;
            return;
        }

        if (raizPersonaje == null)
            raizPersonaje = animatorPersonaje.transform.parent;

        if (raizPersonaje == null)
        {
            Debug.LogError(
                "ControladorDemo: falta Raiz Personaje."
            );
            enabled = false;
            return;
        }

        efectos = new GameObject[]
        {
            efecto1, efecto2, efecto3
        };

        posiciones = new Vector3[3];
        rotaciones = new Quaternion[3];

        for (int i = 0; i < efectos.Length; i++)
        {
            posiciones[i] =
                efectos[i].transform.localPosition;

            rotaciones[i] =
                efectos[i].transform.localRotation;

            ParticleSystem[] sistemas =
                efectos[i].GetComponentsInChildren<ParticleSystem>(true);

            foreach (ParticleSystem ps in sistemas)
            {
                var main = ps.main;
                main.playOnAwake = false;

                ps.Stop(
                    false,
                    ParticleSystemStopBehavior.StopEmittingAndClear
                );
            }

            efectos[i].SetActive(false);
        }

        panelInicio.SetActive(true);
        panelDemo.SetActive(false);

        textoEstado.text = "Selecciona un efecto";

        animatorPersonaje.applyRootMotion = false;
        VolverIdle();
    }

    // =====================================
    // POSICIONES DEL PERSONAJE
    // =====================================

    private void ColocarPersonaje(int numero)
    {
        Vector3 posicion;
        Vector3 rotacion;

        switch (numero)
        {
            case 0:
                posicion = posicionEfecto1;
                rotacion = rotacionEfecto1;
                break;

            case 1:
                posicion = posicionEfecto2;
                rotacion = rotacionEfecto2;
                break;

            case 2:
                posicion = posicionEfecto3;
                rotacion = rotacionEfecto3;
                break;

            default:
                return;
        }

        raizPersonaje.SetPositionAndRotation(
            posicion,
            Quaternion.Euler(rotacion)
        );
    }

    // =====================================
    // ANIMACIONES DEL PERSONAJE
    // =====================================

    private string ObtenerAnimacion()
    {
        switch (seleccionado)
        {
            case 0: return animacionEfecto1;
            case 1: return animacionEfecto2;
            case 2: return animacionEfecto3;
            default: return animacionIdle;
        }
    }

    private void VolverIdle()
    {
        animatorPersonaje.speed = 1f;

        animatorPersonaje.Play(
            animacionIdle, 0, 0f
        );

        animatorPersonaje.Update(0f);
    }

    private void VolverIdleSuave()
    {
        animatorPersonaje.CrossFadeInFixedTime(
            animacionIdle,
            duracionTransicionIdle,
            0,
            0f
        );
    }

    private void IniciarAnimacion()
    {
        esperandoAnimacion = false;
        animacionActual = ObtenerAnimacion();

        int hash = Animator.StringToHash(animacionActual);

        if (!animatorPersonaje.HasState(0, hash))
        {
            Debug.LogError(
                "No existe el estado: " + animacionActual
            );

            animacionIniciada = false;
            VolverIdle();
            return;
        }

        animacionIniciada = true;
        animacionFinalizada = false;

        animatorPersonaje.speed = 1f;
        animatorPersonaje.Play(animacionActual, 0, 0f);
        animatorPersonaje.Update(0f);
    }

    // =====================================
    // MENU Y SELECCION
    // =====================================

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
        DetenerParticulas();

        seleccionado = numero;

        iniciado = false;
        reproduciendo = false;

        animacionIniciada = false;
        animacionFinalizada = false;

        esperandoAnimacion = false;
        esperandoVFX = false;
        tiempoEspera = 0f;

        for (int i = 0; i < efectos.Length; i++)
        {
            efectos[i].SetActive(i == numero);

            efectos[i].transform.localPosition =
                posiciones[i];

            efectos[i].transform.localRotation =
                rotaciones[i];
        }

        ColocarPersonaje(numero);
        PrepararParticulas();
        VolverIdle();

        textoEstado.text =
            "Efecto " + (numero + 1) + " seleccionado";
    }

    // =====================================
    // PARTICULAS
    // =====================================

    private void PrepararParticulas()
    {
        emisoresRaiz.Clear();
        pausados.Clear();

        particulas = efectos[seleccionado]
            .GetComponentsInChildren<ParticleSystem>(true);

        HashSet<ParticleSystem> subEmitters =
            new HashSet<ParticleSystem>();

        foreach (ParticleSystem ps in particulas)
        {
            var modulo = ps.subEmitters;

            for (int i = 0; i < modulo.subEmittersCount; i++)
            {
                ParticleSystem sub =
                    modulo.GetSubEmitterSystem(i);

                if (sub != null)
                    subEmitters.Add(sub);
            }
        }

        foreach (ParticleSystem ps in particulas)
        {
            if (!subEmitters.Contains(ps) &&
                ps.gameObject.activeInHierarchy)
            {
                emisoresRaiz.Add(ps);
            }
        }
    }

    private void DetenerParticulas()
    {
        foreach (ParticleSystem ps in particulas)
        {
            if (ps != null)
            {
                ps.Stop(
                    false,
                    ParticleSystemStopBehavior.StopEmittingAndClear
                );
            }
        }

        pausados.Clear();
    }

    private void IniciarParticulas()
    {
        esperandoVFX = false;

        foreach (ParticleSystem ps in emisoresRaiz)
        {
            if (ps != null && ps.gameObject.activeInHierarchy)
                ps.Play(false);
        }
    }

    // =====================================
    // BOTON REPRODUCIR
    // =====================================

    public void Reproducir()
    {
        if (seleccionado == -1)
        {
            textoEstado.text = "Selecciona un efecto";
            return;
        }

        if (reproduciendo)
            return;

        if (!iniciado)
        {
            Reiniciar();
            return;
        }

        foreach (ParticleSystem ps in pausados)
        {
            if (ps != null && ps.gameObject.activeInHierarchy)
                ps.Play(false);
        }

        pausados.Clear();

        animatorPersonaje.speed = 1f;
        reproduciendo = true;

        textoEstado.text =
            "Reproduciendo efecto " + (seleccionado + 1);
    }

    // =====================================
    // BOTON PAUSAR
    // =====================================

    public void Pausar()
    {
        if (seleccionado == -1 || !reproduciendo)
            return;

        reproduciendo = false;
        animatorPersonaje.speed = 0f;

        pausados.Clear();

        foreach (ParticleSystem ps in particulas)
        {
            if (ps != null &&
                (ps.isPlaying || ps.particleCount > 0))
            {
                pausados.Add(ps);
                ps.Pause(false);
            }
        }

        textoEstado.text =
            "Efecto " + (seleccionado + 1) + " pausado";
    }

    // =====================================
    // BOTON REINICIAR
    // =====================================

    public void Reiniciar()
    {
        if (seleccionado == -1)
        {
            textoEstado.text = "Selecciona un efecto";
            return;
        }

        DetenerParticulas();

        iniciado = true;
        reproduciendo = true;

        animacionIniciada = false;
        animacionFinalizada = false;

        esperandoAnimacion = false;
        esperandoVFX = false;
        tiempoEspera = 0f;

        efectos[seleccionado].transform.localPosition =
            posiciones[seleccionado];

        efectos[seleccionado].transform.localRotation =
            rotaciones[seleccionado];

        ColocarPersonaje(seleccionado);
        VolverIdle();

        if (seleccionado == 0)
        {
            // DON GATO: admite adelanto o retraso.
            if (desfaseInvocacion < 0f)
            {
                IniciarParticulas();

                esperandoAnimacion = true;
                tiempoEspera = -desfaseInvocacion;
            }
            else if (desfaseInvocacion > 0f)
            {
                IniciarAnimacion();

                esperandoVFX = true;
                tiempoEspera = desfaseInvocacion;
            }
            else
            {
                IniciarAnimacion();
                IniciarParticulas();
            }
        }
        else if (seleccionado == 2)
        {
            // CROSS SLASH:
            // El personaje empieza inmediatamente.
            // El VFX puede esperar unos segundos.
            IniciarAnimacion();

            if (retrasoEfecto3 > 0f)
            {
                esperandoVFX = true;
                tiempoEspera = retrasoEfecto3;
            }
            else
            {
                IniciarParticulas();
            }
        }
        else
        {
            // AURA: ambos empiezan juntos.
            IniciarAnimacion();
            IniciarParticulas();
        }

        textoEstado.text =
            "Reproduciendo efecto " + (seleccionado + 1);
    }

    // =====================================
    // ACTUALIZACION
    // =====================================

    void Update()
    {
        if (!reproduciendo || seleccionado == -1)
            return;

        if (esperandoAnimacion || esperandoVFX)
        {
            tiempoEspera -= Time.deltaTime;

            if (tiempoEspera <= 0f)
            {
                if (esperandoAnimacion)
                    IniciarAnimacion();

                if (esperandoVFX)
                    IniciarParticulas();
            }
        }

        // Ya no hay movimiento de cubos de prueba.
        // Cada efecto utiliza sus propias particulas.

        if (animacionIniciada && !animacionFinalizada)
        {
            AnimatorStateInfo estado =
                animatorPersonaje.GetCurrentAnimatorStateInfo(0);

            float momentoRetorno = 1f;

            if (seleccionado == 1)
                momentoRetorno = inicioRetornoAura;

            if (seleccionado == 2)
                momentoRetorno = inicioRetornoEfecto3;

            if (estado.IsName(animacionActual) &&
                estado.normalizedTime >= momentoRetorno &&
                !animatorPersonaje.IsInTransition(0))
            {
                animacionFinalizada = true;
                VolverIdleSuave();
            }
        }
    }
}
