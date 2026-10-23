# Computación Grafica
## Primer Ejercicio 
<br>

En este ejercicio se llevo acabo el desarrollo un efecto curativo, en este caso llamado "VFX Heal". Los integrantes del grupo Arepa De Queso desarrollamos el resto de este efecto tiendo como referencia la parte inicial hecha en clase y la referencia dejada por el profesor. Tiendo esto en cuenta los integrantes del grupo desarrollamos el loop del efecto y su dicipación final. Aqui una muestra final de cual fue el resultado.
<br>

#

https://github.com/user-attachments/assets/07a8030e-6e03-4012-906d-f54de4ab2d3a


# Segundo Ejercicio
<br>

Para este ejercicio se nos pidio realizar slashes, para los efectos del ataque de una espada.

#

**Resultado final**



https://github.com/user-attachments/assets/0d16aa90-3fd2-4801-adfe-446efb159ee0



#

**Concepto:**
<br>

Para el desarrollo de los 3 efectos se penso en cada uno como un elemento de la naturalesa, siendo el primero un rayo, el segundo fuego y el tercero viento.
<br>

#

**Proceso:**
<br>

Siguiendo los pasos explicados por el profesor en clase seguimos un orden en especifico para desarrollar el trabajo, siendo este:
<br>

**1)** Creación de un objeto individual para cada slash.
<br>

**2)** Creación de sus atributos unicos tales como: sus efectos propios para el hit al igual que sus efectos unicos como brazas o rafaga de viento.
<br>

**3)** Realización de los dibujos para cada slash, haciendo que cada uno tenga texturas unicas en cada apartado.
<br>

**4)** Creación de todos los materiales necesarios para los efectos, junto con la adición de sus respectivas texturas.
<br>

**5)** Implementación de todo los elementos en cada slash y todos los ajustes necesarios de sus efectos.
<br>

#

**Muestras graficas**
#

**Objetos:**
<br>

<img width="322" height="827" alt="image" src="https://github.com/user-attachments/assets/45f50717-dd26-4a09-86e5-95ab54e2b4bf" />


<br>

**Materiales:**
<br>

<img width="967" height="260" alt="image" src="https://github.com/user-attachments/assets/77057241-0dfe-411b-8fb9-47921c2409f9" />

<br>

**Texturas:**
<br>

<img width="916" height="262" alt="image" src="https://github.com/user-attachments/assets/34514e4b-a771-4b73-bdae-7f8c0b6e32b1" />


# Actividad 3 — Shader de Agua en Unity

## Descripción

Desarrollo de un shader de agua estilizada utilizando **Unity Shader Graph y Universal Render Pipeline (URP)**.

El proyecto implementa diferentes técnicas de representación de superficies acuáticas, incluyendo desplazamiento de vértices, refracción en espacio de pantalla, reflejos, iluminación especular, efectos de profundidad y generación de espuma.

Se parte de un shader base desarrollado durante las clases, que fue ampliado y modificado para integrar los requisitos de la actividad.

## Características implementadas

| Efecto | Descripción |
|---|---|
| **Vertex Displacement** | Deformación de la geometría mediante tres ondas sinusoidales con diferentes direcciones, frecuencias y velocidades. |
| **Screen Space Refractions** | Distorsión aproximada de la imagen de los objetos sumergidos mediante Scene Color y normales animadas. |
| **Cubemap Reflections** | Reflexiones del entorno utilizando un Cubemap y Fresnel. |
| **Depth Fog** | Simulación de pérdida progresiva de visibilidad según la profundidad mediante Scene Depth, Smoothstep y mezcla de colores. |
| **Intersection Foam** | Espuma generada alrededor de las intersecciones entre la superficie del agua y los objetos opacos. |
| **Blinn-Phong** | Cálculo manual de iluminación especular basado en las normales, la dirección de la luz y la dirección de la cámara. |
| **Wave Crest Foam** | Espuma generada a partir de la altura de las olas, con ruido para producir formas irregulares. |
| **Cartoon Stylization** | Estilización mediante una paleta de colores diferenciados, bandas basadas en profundidad y ruido procedural. |

## Implementación técnica

El shader utiliza las etapas Vertex y Fragment.

En la etapa Vertex se calcula el desplazamiento vertical mediante funciones sinusoidales. En la etapa Fragment se combinan los colores del agua, los reflejos, la refracción, la iluminación y las distintas máscaras de espuma.

Se utilizan propiedades editables desde el material para controlar las olas, la profundidad, las espumas, la iluminación y la apariencia visual.

## Demostración

El siguiente video muestra el shader funcionando en Unity y los efectos implementados.



https://github.com/user-attachments/assets/33487ca3-9b57-4e04-81dd-9feb756d64d9



## Ubicación del ejercicio

Los archivos correspondientes al ejercicio se encuentran en:

`Assets/Ejercicio4Agua`

## Herramientas utilizadas

- Unity
- Universal Render Pipeline (URP)
- Shader Graph
- HLSL para funciones auxiliares de iluminación
- Blender para la geometría de la escena

## Entregables

Proyecto y demostración disponibles en este repositorio de GitHub.

**Objetos:**
<br>
<br>

<img width="292" height="788" alt="image" src="https://github.com/user-attachments/assets/5035e14a-2994-455e-b0ed-4d6ceb47655e" />

<br>
<br>

**Shader:**
<br>
<br>

<img width="1492" height="721" alt="image" src="https://github.com/user-attachments/assets/aeda5a97-e647-4093-bc3f-573bbd854b2f" />



