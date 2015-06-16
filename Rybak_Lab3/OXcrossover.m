function xoverKids  = OXcrossover(parents,options,NVARS, ...
    FitnessFcn,thisScore,thisPopulation)
nKids = length(parents)/2;
xoverKids = cell(nKids,1); 
index = 1;
kidsIterator=1;
numberOfGenes=length(thisPopulation{parents(index)});
while kidsIterator<nKids
    parent1 = thisPopulation{parents(index)};
    index = index + 1;
    parent2 = thisPopulation{parents(index)};
    index = index + 1;
   XorPoint=mod(ceil(rand(1)*10),numberOfGenes );
if XorPoint==0
      XorPoint=XorPoint+1;
end

    child1=parent2;

 
    l=XorPoint+1;
for firstPartIterator=1:XorPoint
if ismember(parent1(firstPartIterator), parent2((XorPoint+1):numberOfGenes))==0
            child1(firstPartIterator)=parent1(firstPartIterator);
else
for secondPartIterator=l:numberOfGenes
if ismember(parent1(secondPartIterator), parent2((XorPoint+1):numberOfGenes))==0
                    child1(firstPartIterator)=parent1(secondPartIterator); l=l+1; break;
else
                    l=l+1;
end
end
end
end
    xoverKids{kidsIterator} = child1; 
    kidsIterator=kidsIterator+1;
    child2=parent1;
    xoverKids{kidsIterator} = child2;
    kidsIterator=kidsIterator+1;
end


