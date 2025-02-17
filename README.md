# Pang Unity Project

## Descripción
Este proyecto es una adaptación del clásico juego **Super Pang** (conocido en América como Super Buster Bros)  desarrollado en Unity 2D. El jugador debe derribar burbujas utilizando unos proyectiles (estrellas). Si el proyectil toca la burbuja la burbuja desaparecerá pero si el jugador no logra derribar la burbuja y esta toca el suelo se dividirá en dos burbujas más pequeñas y más rápidas. El jugador tiene que intentar derribar las nuevas burbujas sin que le toquen. Cada toque le quitará una vida. Si consigue eliminar todas las burbujas del escenario pasará al siguiente nivel. En el siguiente nivel hay una burbuja mas y cambia el escenario. 

## Características del juego
- **Mecánica principal:**  
  Las burbujas empiezan siendo grandes y pueden dividirse hasta dos veces al tocar el suelo. Si son impactadas por un proyectil desaparecen tengan el tamaño que tengan.  Cada burbuja tiene **tres estados** (grande, mediana y pequeña).  
   
  - Si el jugador pierde todas sus vidas, se detiene el juego y se muestra la pantalla de **Game Over**. Dando la opción de salir o de jugar de nuevo. 
  - El jugador puede moverse horizontalmente dentro de un área limitada y disparar estrellas para destruir burbujas.  Los movimientos se hacen mediante las flechas del teclado lateral. 

- **Progresión de niveles:**  
  El juego tiene tres niveles, cada uno con un número creciente de burbujas grandes que representan un desafío progresivo:  
  - **Nivel 1:** Dos burbujas grandes.  
  - **Nivel 2:** Tres burbujas grandes.  
  - **Nivel 3:** Cuatro burbujas grandes.  

  Cada nivel tiene su propio fondo dinámico, y el progreso depende de destruir todas las burbujas del nivel actual.

---

## Estructura del proyecto
- **Assets/**:  
  Contiene todos los recursos del juego, incluyendo sprites, prefabs, sonidos y scripts.  

- **Scripts/**:  

  - **PlayerController.cs:** Controla el movimiento horizontal del jugador y permite disparar arpones.  
    - **Entrada del usuario:** El jugador se mueve usando las flechas (izquierda/derecha) y dispara con la barra espaciadora.  
    - **Límites de movimiento:** El jugador no puede salir del área definida entre los límites izquierdo y derecho.  

  - **Bubble.cs:** Maneja el comportamiento de las burbujas, incluyendo su división y destrucción.  
    - **Colisiones:** Detecta el contacto con proyectiles, el suelo, el techo o el jugador.  
    - **División:** Una burbuja grande puede dividirse en dos burbujas medianas, y estas a su vez en burbujas pequeñas.  
    - **Destrucción:** Cuando una burbuja es golpeada por un proyectil, se destruye sea cual sea su estado de división. 

    **Projectile.cs:** Controla el comportamiento del proyectil disparado por el jugador.  
    - **Velocidad y destrucción:** El proyectil se mueve hacia arriba y se destruye automáticamente después de 3 segundos o al colisionar con una burbuja o el techo.  
    - **Lógica de disparo:** Verifica si ha pasado el tiempo mínimo entre disparos para evitar spam de proyectiles.  
    - **Colisiones:** Cuando impacta una burbuja, activa su destrucción y se autodestruye.

  - **GameManager.cs:** Gestiona el sistema de vidas, el panel de Game Over y el reinicio del juego.  
    - **Sistema de vidas:** El jugador empieza con 5 vidas y pierde una cada vez que una burbuja lo toca.  
    - **Reinicio del juego:** Permite reiniciar el juego desde el primer nivel tras un Game Over. 

  - **LevelManager.cs:** Controla la lógica de los niveles, el cambio de fondo y la generación de burbujas.  
    - **Cambio de niveles:** Cambia dinámicamente el fondo y el número de burbujas según el nivel actual.  
    - **Detección del progreso:** Verifica si quedan burbujas en la escena y marca el nivel como completado si no hay más burbujas.  
    - **Reinicio de niveles:** Elimina todas las burbujas y reinicia el juego desde el primer nivel si es necesario.


## Cómo ejecutar el proyecto
**Requisitos:**
- Unity 2021 o superior

**Pasos:**
1. Abre Unity y selecciona **Open Project**.
2. Navega hasta la carpeta donde está almacenado el proyecto.
3. Abre la escena principal (MainScene).
4. Presiona **Play** para comenzar a jugar.

## Decisiones de diseño
El diseño del juego está basado en decisiones que garantizan una jugabilidad equilibrada, fluida y fácilmente extensible:

- **División y destrucción de burbujas:**  
  Cada burbuja grande puede dividirse hasta **dos veces**. La primera división genera dos burbujas medianas, y la segunda genera burbujas pequeñas. Las burbujas desaparecen al ser golpeadas por el proyectil, manteniendo una progresión de dificultad natural.

- **Movimiento limitado del jugador:**  
  El jugador puede moverse solo dentro de un rango definido (actualmente entre **-13 y 13**). Este límite permite mantener el equilibrio del juego y evita que el jugador escape de las burbujas o acceda a áreas no deseadas. Además, esta restricción facilita ajustar el área jugable según las necesidades de futuros niveles.

- **Cambio dinámico de niveles y fondos:**  
  El **LevelManager** se encarga de cargar diferentes escenarios automáticamente en cada nivel, lo que mejora la inmersión del jugador y permite una fácil ampliación del juego al agregar más niveles o desafíos visuales.

- **Componentes desacoplados y modulares:**  
  - Los scripts principales (`Bubble.cs`, `PlayerController.cs`, `LevelManager.cs`, `GameManager.cs`, `Projectile.cs`) están diseñados de forma independiente, de modo que cada uno gestiona una parte específica del juego. 
  - Este enfoque modular permite modificar o agregar nuevas funcionalidades sin afectar el resto del proyecto. Por ejemplo, se podrían agregar nuevas mecánicas de burbujas simplemente modificando o extendiendo el script `Bubble.cs` sin cambios en los demás scripts. Es también muy sencillo incluir nuevos niveles. 

---

## Futuras mejoras (opcional)
La estructura modular y el diseño limpio del proyecto permiten añadir nuevas características de manera sencilla y eficaz. A continuación detallamos una serie de mejoras que es posible implementar. 

- **Implementar burbujas especiales:**  (nuevas funcionalidades posibles) 
  El script `Bubble.cs` se puede extender fácilmente para agregar diferentes tipos de burbujas con comportamientos únicos:  
  - **Burbujas explosivas:** Al ser destruidas, eliminan burbujas cercanas.  
  - **Burbujas veloces:** Se mueven más rápido y son más difíciles de impactar.  
  - **Burbujas temporales:** Desaparecen después de un tiempo determinado, creando presión adicional para el jugador.  

  **Cómo implementarlo:**  
  - Crear clases derivadas de `Bubble` o usar un sistema basado en etiquetas o enumeraciones (`BubbleType`) para asignar comportamientos únicos a cada tipo de burbuja.

- **Añadir modos de juego:**  
  La lógica existente es flexible y puede adaptarse para introducir nuevos modos de juego:  
  - **Modo Contrarreloj:** El jugador debe destruir la mayor cantidad de burbujas posibles en un tiempo limitado.  
  - **Modo Supervivencia:** Las burbujas aparecen continuamente, y el jugador debe sobrevivir el mayor tiempo posible.  
  - **Modo Multinivel:** Introducir más de tres niveles con dificultades crecientes o burbujas especiales.

  **Cómo implementarlo:**  
  - Usar el **LevelManager** como base para controlar el progreso y las condiciones de victoria o derrota.  
  - Agregar configuraciones específicas para cada modo, como temporizadores (`Modo Contrarreloj`) o generación continua de burbujas (`Modo Supervivencia`).

- **Sistema de puntuación:**  
  Agregar un sistema de puntuación basado en la cantidad de burbujas destruidas y el tiempo restante.

- **Multijugador local o en línea:**  
  Implementar un modo cooperativo o competitivo en el que dos jugadores intenten eliminar burbujas simultáneamente.

La arquitectura modular y la separación de responsabilidades entre los scripts aseguran que estas expansiones puedan implementarse fácilmente, manteniendo el código limpio y escalable.

