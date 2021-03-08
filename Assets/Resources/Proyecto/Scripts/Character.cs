using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sprite;
    Vector3 initPos;
    TextMeshProUGUI text;
    Image buttonColor;

    public static int score;
    public static string color;
    int colorID;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        initPos = transform.position;
        text = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        buttonColor = GameObject.Find("ColorButton").GetComponent<Image>();
        color = "normal";
    }

    // Update is called once per frame
    void Update()
    {
        // puntuación según la distancia desde el inicio hasta el final

        score = Mathf.RoundToInt(transform.position.x - initPos.x);

        // correguir puntuación en caso de ser un valor negativo

        if (score < 0)
            score = 0;

        // Mostrar puntuación

        text.text = "Score: " + score;

        // movimiento a la derecha

        if (Input.GetKey(KeyCode.D))
        {
            // el movimiento se limita al largo de la plataforma

            if (transform.position.x < 8.9f)
                transform.position += new Vector3(Time.deltaTime * 3, 0);
            sprite.flipX = false;
        }

        // movimiento a la izquierda

        if (Input.GetKey(KeyCode.A))
        {
            // el movimiento se limita al largo de la plataforma

            if (transform.position.x > -8.66f)
                transform.position -= new Vector3(Time.deltaTime * 3, 0);
            sprite.flipX = true;
        }

        // detener animación si se para

        if (!Input.GetKey(KeyCode.D) & !Input.GetKey(KeyCode.A))
        {
            animator.speed = 0;
        }

        // continuar con la animación si se mueve

        else
        {
            animator.speed = 1;
        }
    }

    public void CambiarColor()
    {
        // cambia el color del personaje según cada caso

        switch (colorID % 3)
        {
            case 0:
                sprite.color = Color.magenta;
                color = "magenta";
                break;
            case 1:
                sprite.color = Color.green;
                color = "verde";
                break;
            case 2:
                sprite.color = Color.white;
                color = "normal";
                break;
        }

        // se aumenta en uno el valor del caso para que toque otro caso en el siguiente click

        colorID++;

        // se iguala el color del sprite del botón con el del personaje

        buttonColor.color = sprite.color;
    }
}
