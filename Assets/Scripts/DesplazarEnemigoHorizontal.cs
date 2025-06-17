using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que controla el movimiento horizontal de un enemigo entre dos puntos.
public class DesplazarEnemigoHorizontal : MonoBehaviour
{
	// Posición mínima en el eje X a la que el enemigo puede moverse.
	public float minX;
	// Posición máxima en el eje X a la que el enemigo puede moverse.
	public float maxX;
	// Tiempo de espera en segundos antes de que el enemigo comience a moverse nuevamente.
	public float TiempoEspera = 2f;
	// Velocidad de movimiento del enemigo.
	public float Velocidad = 1f;
	// Objeto que representa el objetivo al que el enemigo se moverá.
	private GameObject _LugarObjetivo;

	// Método llamado al inicio del juego. Inicializa el objetivo y comienza la rutina de patrullaje.
	void Start()
	{
		UpdateObjetivo();
		StartCoroutine("Patrullar");
	}

 	 // Actualiza la posición del objetivo al que el enemigo se moverá.
 	private void UpdateObjetivo()
	{
		if (_LugarObjetivo == null)
		{
			_LugarObjetivo = new GameObject("Sitio_objetivo");
			_LugarObjetivo.transform.position = new Vector2(minX, transform.position.y);
			transform.localScale = new Vector3(2, 2, 2);
			return;
		}

		if (_LugarObjetivo.transform.position.x == minX)
		{
			_LugarObjetivo.transform.position = new Vector2(maxX, transform.position.y);
			transform.localScale = new Vector3(-2, 2, 2);
		}
		else if (_LugarObjetivo.transform.position.x == maxX)
		{
			_LugarObjetivo.transform.position = new Vector2(minX, transform.position.y);
			transform.localScale = new Vector3(2, 2, 2);
		}
	}

 	 // Corrutina que controla el movimiento del enemigo hacia el objetivo y espera antes de moverse nuevamente.
 	private IEnumerator Patrullar()
	{
		while (Vector2.Distance(transform.position, _LugarObjetivo.transform.position) > 0.05f)
		{
			Vector2 direction = _LugarObjetivo.transform.position - transform.position;
			float xDirection = direction.x;

			transform.Translate(direction.normalized * Velocidad * Time.deltaTime);

			yield return null;
		}

		// Debug.Log("Se alcanzo el Obejitvo");
		transform.position = new Vector2(_LugarObjetivo.transform.position.x, transform.position.y);

		// Debug.Log("Esperando " + TiempoEspera + " segundos");
		yield return new WaitForSeconds(TiempoEspera);

		// Debug.Log("Se espera lo necesario para que termine y vuelva a empezar movimiento");
		UpdateObjetivo();
		StartCoroutine("Patrullar");
	}
}
