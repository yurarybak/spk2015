package tsp;

import java.util.Date;

public class TSP_GA {

    public static void main(String[] args) {
    	int [] x = {5, 16, 15, 11, 0, 14, 20, 20, 3, 12, 15, 8, 13, 11, 12, 12, 9, 16, 13, 15, 20, 2, 5, 19, 0, 16, 14, 9, 2, 2};
    	int [] y = {17, 17, 3, 3, 14, 3, 4, 15, 9, 4, 13, 2, 0, 20, 6, 1, 10, 3, 20, 5, 7, 12, 13, 18, 8, 14, 9, 12, 10, 7}; 
   	
        // Create and add our cities
    	for(int i = 0; i < 30; i++){
    		TourManager.addCity(new City(x[i],y[i]));
    	}
    

        // Initialize population
        Population pop = new Population(100, true);
        System.out.println("Initial distance: " + pop.getFittest().getDistance());
        Date currentTimeBefore = new Date();
        long timeBefore = currentTimeBefore.getTime();

        // Evolve population for 10000 generations
        pop = GA.evolvePopulation(pop);
        for (int i = 0; i < 10000; i++) {
            pop = GA.evolvePopulation(pop);
        }
        Date currentTimeAfter = new Date();
        long timeAfter= currentTimeAfter.getTime();;
        long time = timeAfter-timeBefore;
        // Print final results;
        System.out.println("Length of way: " + pop.getFittest().getDistance());
        System.out.println("Time: " + time + "ms");
        System.out.println("Result:");     
        System.out.println(pop.getFittest());
    }
}