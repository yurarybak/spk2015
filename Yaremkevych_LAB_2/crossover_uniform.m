function xoverKids  = crossover_uniform(parents, options, nvars, FitnessFcn, ... 
    unused,thisPopulation))%однорідне схрещування
    leng = length(parents)/2;     
     for j = 1:nvars
        maska = rand(1,leng);
         for i = 1:leng
            if (maska(i) <=0.5) xoverKids(i,j)=parents(i);
            else xoverKids(i,j)=parents(i + leng);
            end
        end
     end
end