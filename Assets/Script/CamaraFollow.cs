using UnityEngine;
using System.Collections;

// This class controls camera position
public class CamaraFollow : MonoBehaviour {

	private Vector2 velocity;

	private float smoothTimeX;
	private float smoothTimeY;

	private GeneticAlgorithm geneticAlgorithm;
	// Use this for initialization
	void Start () {
		geneticAlgorithm = GeneticAlgorithm.Instance;
	}
	
	// Update is called once per frame
	void Update () {

		Player player = geneticAlgorithm.population [0];

		for (int i=0; i < geneticAlgorithm.population.Length; i++) {
			if (player.transform.localPosition.x < geneticAlgorithm.population[i].transform.localPosition.x) {
				player = geneticAlgorithm.population[i];
			}
		}

		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x+10f, ref velocity.x, smoothTimeX);
		float posY = transform.position.y;//Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

		if (posX > 18.00 && posX < 60) {
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}

	}
}
