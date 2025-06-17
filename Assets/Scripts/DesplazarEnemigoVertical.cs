using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesplazarEnemigoVertical : MonoBehaviour
{
	// Variables públicas para definir los límites de movimiento y la velocidad del enemigo
	public float minY;
	public float maxY;
	public float TiempoEspera = 5f;
	public float Velocidad = 1f;

	// Variable privada para almacenar el objetivo de movimiento
	private GameObject _LugarObjetivo;

	// Método llamado al inicio antes de la primera actualización de frames
	void Start()
	{
		// Actualiza el objetivo inicial
		UpdateObjetivo();
		// Inicia la co-rutina de patrullaje
		StartCoroutine("Patrullar");
	}

	// Método llamado una vez por cuadro (frame)
	void Update()
	{
		// No se necesita lógica en Update para este script
	}

	// Método para actualizar la posición del objetivo
	private void UpdateObjetivo()
	{
		// Si es la primera vez, iniciar el patrullaje hacia arriba
		if (_LugarObjetivo == null) {
			_LugarObjetivo = new GameObject("Sitio_objetivo");
			_LugarObjetivo.transform.position = new Vector2(transform.position.x, maxY);
			return;
		}

		// Si el objetivo está en la posición máxima, cambiarlo a la posición mínima
		if (_LugarObjetivo.transform.position.y == maxY) {
			_LugarObjetivo.transform.position = new Vector2(transform.position.x, minY);
		}
		// Si el objetivo está en la posición mínima, cambiarlo a la posición máxima
		else if (_LugarObjetivo.transform.position.y == minY) {
			_LugarObjetivo.transform.position = new Vector2(transform.position.x, maxY);
		}
	}

	// Co-rutina para mover el enemigo
	private IEnumerator Patrullar()
	{
		// Mientras la distancia al objetivo sea mayor que un pequeño umbral
		while (Vector2.Distance(_LugarObjetivo.transform.position, transform.position) > 0.02f) {
			// Calcular la dirección hacia el objetivo
			Vector2 direction = _LugarObjetivo.transform.position - transform.position;
			float yDirection = direction.y;

			// Mover el enemigo en la dirección del objetivo
			transform.Translate(direction.normalized * Velocidad * Time.deltaTime);

			// Esperar hasta el siguiente frame
			yield return null;
		}

		// En este punto, se alcanzó el objetivo, se establece nuestra posición en la del objetivo
		// Debug.Log("Se alcanzo el Objetivo");
		transform.position = new Vector2(_LugarObjetivo.transform.position.x, transform.position.y);

		// Esperar un momento antes de volver a movernos
		// Debug.Log("Esperando " + TiempoEspera + " segundos");
		yield return new WaitForSeconds(TiempoEspera);

		// Actualizar el objetivo y reiniciar la co-rutina de patrullaje
		// Debug.Log("Se espera lo necesario para que termine y vuelva a empezar movimiento");
		UpdateObjetivo();
		StartCoroutine("Patrullar");
	}
}