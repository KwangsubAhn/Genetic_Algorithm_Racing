using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class GeneticAlgorithm {
	
	private const int POPULATION_NO = 8;
	public int GENE_SIZE = 20;

	public Player[] population = new Player[POPULATION_NO];
	public List<int[]> listGenes = new List<int[]>();
	public bool reStart = false;

	private System.Random rand = new System.Random();
	//private int[] crossGenes;

	//Find the best two players
	public int[] FindBestIndividual() {
		int bestIndividual = 0;
		int bestTwoIndividual = 0;

		//Find the best individual
		//Fitness1 - Distance: How far the player runs
		for (int i=0; i<population.Length; i++) {
			if (population[bestIndividual].fitnessValue < population[i].fitnessValue) {
				bestIndividual = i;
			}
		}

		//Fitness2 - Jump: How many times the player jumps
		for (int i=0; i<population.Length; i++) {
			if (i!=bestIndividual) {
				if (population[bestIndividual].fitnessValue == population[i].fitnessValue) {
					if (population[bestIndividual].fitnessValue2 > population[i].fitnessValue2) {
						bestIndividual = i;
					}
				}
			}

		}

		//Fitness3 - Stuck Time: How long the player get stuck in hurdle
		for (int i=0; i<population.Length; i++) {
			if (i!=bestIndividual) {
				if (population[bestIndividual].fitnessValue == population[i].fitnessValue) {
					if (population[bestIndividual].fitnessValue2 == population[i].fitnessValue2) {
						if (population[bestIndividual].fitnessValue3 > population[i].fitnessValue3) {
							bestIndividual = i;
						}
						
					}
				}
			}

		}
		//End Find the best individual

		if (bestIndividual == 0) {bestTwoIndividual++;}

		//Fine the second best individual
		//Fitness1 - Distance: How far the player runs
		for (int i=0; i<population.Length; i++) {
			if (i != bestIndividual) {
				if (population[bestTwoIndividual].fitnessValue < population[i].fitnessValue) {
					bestTwoIndividual = i;
				}
			}
		}

		//Fitness2 - Jump: How many times the player jumps
		for (int i=0; i<population.Length; i++) {
			if (i != bestIndividual) {
				if (population[bestTwoIndividual].fitnessValue == population[i].fitnessValue) {
					if (population[bestTwoIndividual].fitnessValue2 > population[i].fitnessValue2) {
						bestTwoIndividual = i;
					}
				}
			}

		}

		//Fitness3 - Stuck Time: How long the player get stuck in hurdle
		for (int i=0; i<population.Length; i++) {
			if (i!=bestIndividual) {
				if (i != bestIndividual) {
					if (population[bestIndividual].fitnessValue == population[i].fitnessValue) {
						if (population[bestIndividual].fitnessValue2 == population[i].fitnessValue2) {
							if (population[bestIndividual].fitnessValue3 > population[i].fitnessValue3) {
								bestIndividual = i;
							}
							
						}
					}
				}
			}

		}

		return new int[]{bestIndividual, bestTwoIndividual};
	}
	
	//n-point crossover	
	public int[] Crossover (int[] parent1, int[] parent2, int numOfPoint) {
		//crossover point = random 0 ~ GENE SIZE

		int[] pntCrossover = new int[numOfPoint];
		for (int i=0; i<pntCrossover.Length; i++) {
			double dblXover = rand.NextDouble();
			pntCrossover[i] = ((int)(dblXover * GENE_SIZE));
		}	

		int[] crossover = new int[20];
		bool bCross = false;
		for (int i=0; i<GENE_SIZE; i++) {
			for (int j=0; j<pntCrossover.Length; j++){
				if (i==pntCrossover[j]) {
					if (bCross) {
						bCross = false;
					} else {
						bCross = true;
					}
				}
			}

			if (bCross) {
				if (numOfPoint % 2 == 0) {
					crossover[i] = parent1[i];
				} else {
					crossover[i] = parent2[i];
				}
			} else {
				if (numOfPoint % 2 == 0) {
					crossover[i] = parent2[i];
				} else {
					crossover[i] = parent1[i];
				}
			}

		}

		return crossover;
	}

	//Select 3 genes and flip the values ( ex) 0 -> (0/1) or 1 -> 0)
	public int[] Mutation(int[] parent) {
		int pntMutate1 = ((int)(rand.NextDouble() * 18)+1);			//0 ~ 19
		int pntMutate2 = ((int)(rand.NextDouble() * 18)+1);			//0 ~ 19
		int pntMutate3 = ((int)(rand.NextDouble() * 18)+1);			//0 ~ 19

		int[] mutation = new int[20];

		for (int i=0; i<GENE_SIZE; i++) {
			if ((i == pntMutate1) || (i == pntMutate2) || (i == pntMutate3)) {
				if (parent[i] == 0) {
					double jumpHeight = rand.NextDouble();
					int jumpLevel = ((int)(jumpHeight * 2));			//0 ~ 1
					mutation[i] = jumpLevel;
				} else {
					mutation[i] = 0;
				}

			} else {
				mutation[i] = parent[i];
			}
			
		}
		
		return mutation;
	}
	
	public bool checkIsEndGeneration() {
		for (int i=0; i<population.Length; i++) {
			Player player = population[i];
			
			if (!player.isIndividualEnd) {
				return false;
			} 
		}
		return true;
	}

	private void Checker(object parameter) {
		int generationNum = 0;
		bool isEndGeneration = false;
				
		while (true) {
			reStart = false;
			Thread.Sleep (1000);
			
			if (checkIsEndGeneration()) {

				generationNum++;
				int[] indexBestTwo = FindBestIndividual();
				//nextPopulation.Add(FindBestIndividual());
				Debug.Log("Generation(" + generationNum.ToString() + ")" + " Best fitness(" + (indexBestTwo[0]+1) + ") " + population[indexBestTwo[0]].fitnessValue + " , " + population[indexBestTwo[0]].fitnessValue2 + " , " + population[indexBestTwo[0]].fitnessValue3);
				Debug.Log("Best one : " + (indexBestTwo[0]+1).ToString() + " Gene: " + population[indexBestTwo[0]].getGene(0) + population[indexBestTwo[0]].getGene(1) + population[indexBestTwo[0]].getGene(2) + population[indexBestTwo[0]].getGene(3) + population[indexBestTwo[0]].getGene(4) + population[indexBestTwo[0]].getGene(5) + population[indexBestTwo[0]].getGene(6) + population[indexBestTwo[0]].getGene(7) + population[indexBestTwo[0]].getGene(8) + population[indexBestTwo[0]].getGene(9) + population[indexBestTwo[0]].getGene(10) + population[indexBestTwo[0]].getGene(11) + population[indexBestTwo[0]].getGene(12) + population[indexBestTwo[0]].getGene(13) + population[indexBestTwo[0]].getGene(14) + population[indexBestTwo[0]].getGene(15) + population[indexBestTwo[0]].getGene(16) + population[indexBestTwo[0]].getGene(17) + population[indexBestTwo[0]].getGene(18) + population[indexBestTwo[0]].getGene(19));
				Debug.Log("Best two : " + (indexBestTwo[1]+1).ToString() + " Gene: " + population[indexBestTwo[1]].getGene(0) + population[indexBestTwo[1]].getGene(1) + population[indexBestTwo[1]].getGene(2) + population[indexBestTwo[1]].getGene(3) + population[indexBestTwo[1]].getGene(4) + population[indexBestTwo[1]].getGene(5) + population[indexBestTwo[1]].getGene(6) + population[indexBestTwo[1]].getGene(7) + population[indexBestTwo[1]].getGene(8) + population[indexBestTwo[1]].getGene(9) + population[indexBestTwo[1]].getGene(10) + population[indexBestTwo[1]].getGene(11) + population[indexBestTwo[1]].getGene(12) + population[indexBestTwo[1]].getGene(13) + population[indexBestTwo[1]].getGene(14) + population[indexBestTwo[1]].getGene(15) + population[indexBestTwo[1]].getGene(16) + population[indexBestTwo[1]].getGene(17) + population[indexBestTwo[1]].getGene(18) + population[indexBestTwo[1]].getGene(19));
				
				
				for (int i=0; i<population.Length; i++) {
					//keep the best two gene for next generation
					if ( (i != indexBestTwo[0]) && (i != indexBestTwo[1]) ) {
						//crossover genes with 33% posibility from the best gene
						/*if (i % 3 == 0) {
							double dblNumberOfPoint = rand.NextDouble();
							int numberOfPoint = ((int)(dblNumberOfPoint * 10));
							Debug.Log("##crossover##" + i);
							int[] crossGenes = Crossover(population[indexBestTwo[0]].getGenes(), population[indexBestTwo[1]].getGenes(), numberOfPoint+1);
							population[i].setGenes(crossGenes);

						} 
						//mutate genes with 67% posibility
						else if (i % 3 == 1){Debug.Log("##mutate1##" + i);
							int[] mutateGenes = Mutation(population[indexBestTwo[0]].getGenes());
							population[i].setGenes(mutateGenes);
						} else {Debug.Log("##mutate2##" + i);
							int[] mutateGenes = Mutation(population[indexBestTwo[1]].getGenes());
							population[i].setGenes(mutateGenes);
						}*/

						//Mutate 100%
						if (i % 2 == 1){
							int[] mutateGenes = Mutation(population[indexBestTwo[0]].getGenes());
							population[i].setGenes(mutateGenes);
						} else {
							int[] mutateGenes = Mutation(population[indexBestTwo[1]].getGenes());
							population[i].setGenes(mutateGenes);
						}
					} 
				}
				
				reStart = true;
				
				Thread.Sleep (1000);
						
				for (int i=0; i<population.Length; i++) {
					//population[i].genes = nextPopulation[i].genes;
					population[i].isIndividualEnd = false;
				}

				isEndGeneration = false;
				Thread.Sleep (1000);

			}
		}
	}

	//Getting instance Singlton
	private static GeneticAlgorithm instance = null;
	private static readonly object padlock = new object();
	
	public GeneticAlgorithm() {
		Thread t = new Thread(new ParameterizedThreadStart(Checker));
		t.Start("Checker");
		int[] initGene = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		listGenes.Add(initGene);listGenes.Add(initGene);listGenes.Add(initGene);listGenes.Add(initGene);
		listGenes.Add(initGene);listGenes.Add(initGene);listGenes.Add(initGene);listGenes.Add(initGene);
	}

	public static GeneticAlgorithm Instance {
		get {
			lock (padlock) {
				if (instance == null) {
					instance = new GeneticAlgorithm();

				}
				return instance;
			}
		}
	}



	//End Singlton

	/*public Player RouletteWheelSelection() {
		Player rdnSelection = null;

		float totalFitness = GetTotalFitness ();

		double diceRoll = rand.NextDouble();

		double r = diceRoll * totalFitness;

		float[] csum = new float[population.Length];
		float sum = 0;
		for (int i=0; i<population.Length; i++) {
			sum += population[i].fitnessValue;
			csum[i] = sum; 
		}

		for (int i=0; i<population.Length; i++) {
			if (csum[i] > r) {
				rdnSelection = population[i];
				break;
			}
		}

		return rdnSelection;

	}*/
	
}
