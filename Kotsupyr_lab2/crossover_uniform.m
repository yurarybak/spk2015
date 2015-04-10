function xoverKids  = crossover_uniform(parents, options, nvars, FitnessFcn, ... 
    unused,thisPopulation)

    leng = length(parents)/2;
        
     for j = 1:nvars
        maska1 = rand(1,leng);
    for i = 1:leng
        parents(i + leng);
        maska1(i);
        if (maska1(i) <=0.5) xoverKids(i,j)=parents(i);
        else xoverKids(i,j)=parents(i + 14);
        end
    end
     end

    xoverKids;
end