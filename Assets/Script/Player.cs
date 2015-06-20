using UnityEngine;
using System.Collections;
using System.Timers;
using System.Threading;

public class Player : MonoBehaviour {

	private Rigidbody2D rb2d;
	private GeneticAlgorithm geneticAlgorithm;
	
	//Phenotype
	private float locX = 0;
	private float locY = 0;
	private float locZ = 0;

	private float moveVelocity = 80f;
	private float moveMaxSpeed = 4.5f;
	private float jumpPower = 490f;
	public bool isIndividualEnd = false;
	//End Phenotype
	
	//Genotype
	private int[] genes;
	private int curCell = 0;
	public float fitnessValue = 0;
	public int fitnessValue2 = 0;
	public float fitnessValue3 = 0;
	//End Genotype

	private bool grounded = true;
	private bool collided = false;

	private Animator anim;
	private float gameTime = 0;
	private int intIndex;
	//private int count = 0;
	
	void Start() {
		locX = this.transform.localPosition.x;
		locY = this.transform.localPosition.y;
		locZ = this.transform.localPosition.z;

		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		anim = gameObject.GetComponent<Animator> ();

		Time.timeScale = 1f;

		string strIndex = gameObject.name.Substring (gameObject.name.Length - 1);

		int.TryParse(strIndex, out intIndex);

		geneticAlgorithm = GeneticAlgorithm.Instance;
		geneticAlgorithm.population[intIndex-1] = this;
		genes = new int[geneticAlgorithm.GENE_SIZE];
		genes = geneticAlgorithm.listGenes[intIndex-1];
		//randGenes ();
						
	}
	
	void FixedUpdate() {
		anim.SetBool ("Grounded", grounded);

		bool newCell = false;
	
		gameTime += Time.deltaTime;

		if (rb2d.velocity.x == 0) {
			fitnessValue3 += Time.deltaTime;
		}

		if (gameTime > 1) {
			gameTime -= 1;
			if (curCell != genes.Length-1) {
				curCell += 1;
				newCell = true;
			} 
		}

		if (geneticAlgorithm.checkIsEndGeneration ()) {						//End All
			if (geneticAlgorithm.reStart) {
				geneticAlgorithm.listGenes.RemoveAt(intIndex-1);
				geneticAlgorithm.listGenes.Insert(intIndex-1, genes);
				Application.LoadLevel(Application.loadedLevel);

				curCell = 0;
				fitnessValue = 0;
				fitnessValue2 = 0;
				fitnessValue3 = 0;//count=0;
			}
			
			return;
		}
		
		if (curCell == genes.Length - 1 && !isIndividualEnd) {				//End Individual

			isIndividualEnd = true;
			if (transform.localPosition.x > 100f) {
				fitnessValue = 100f;
			} else {
				fitnessValue = transform.localPosition.x;
			}

			fitnessValue = Mathf.Round(fitnessValue * 100f) / 100f;
			return;
		}


		//Jump
		if (newCell && getGene(curCell) == 1 /*Input.GetButtonDown("Jump")*/) {
			grounded = false;
			rb2d.AddForce(Vector2.up * jumpPower);
			newCell = false;
			fitnessValue2++;
		}
		//

		//Move ahead
		rb2d.AddForce (Vector2.right * moveVelocity * 1);
		//

		//Controling movement speed
		if (!grounded) {
			moveMaxSpeed = 1.6f;
		} else {
			moveMaxSpeed = 4.5f;	//4.7
		}

		if (transform.position.y > locY + 0.2) {
			moveMaxSpeed = 1.6f;
		}

		if (rb2d.velocity.x > moveMaxSpeed) {
			rb2d.velocity = new Vector2 (moveMaxSpeed, rb2d.velocity.y);
		}
		
	}

	void OnCollisionEnter2D (Collision2D hit) {
		if (hit.gameObject.name.ToString ().Contains("Lane")) {
			grounded = true;
			collided = false;
		}
		if (hit.gameObject.name.ToString ().Contains("collide")) {
			collided = true;
		}

	}

	//Getter & Setter
	public int getGene(int index) {
		return genes[index];
	}
	
	public int[] getGenes() {
		return genes;
	}
	
	public void setGene(int index, int gene) {
		this.genes[index] = gene;
	}
	
	public void setGenes(int[] genes) {
		this.genes = genes;
	}
	
	public string getStrGenes() {
		string strGenes = "";
		
		for (int i=0; i<genes.Length; i++) {
			strGenes += genes[i];
		}
		return strGenes;
	}
	//End Getter & Setter
	
}
