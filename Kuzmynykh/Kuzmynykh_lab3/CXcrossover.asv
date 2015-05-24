function xoverKids  = CXcrossover(parents,options,NVARS, ...
    FitnessFcn,thisScore,thisPopulation)

%cycle crossover - custom function for traveling salesman
nKids = length(parents)/2;
xoverKids = cell(nKids,1); 
index = 1;
 %selected parents do crossovering
for i=1:nKids
    parent1 = thisPopulation{parents(index)};
    length(thisPopulation);
    if(parents(index) == length(thisPopulation))
        parent2 = thisPopulation{parents(index)-1};
    else 
        parent2 = thisPopulation{parents(index)+1};
    end
 
    child = parent1;
    child(1:length(parent2)) = 0;
    for j = 1:length(parent2)/2 
         m = length(parent1);
          while(m>0)
    if all(child ~= parent1(m))
             child(length(parent1)-j +1) =  parent1(m);
             break;
          end
          m=m-1;
          end
         
     for k = 1:length(parent2) 
         if all(child ~= parent2(k))
             child(j) =  parent2(k);
             break;
         end
         end   
    end
     xoverKids{i} = child
     index = index + 2;
end

