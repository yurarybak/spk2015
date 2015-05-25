function mutationChildren = mutateSalesman(parents ,options,NVARS, ...
    FitnessFcn, state, thisScore,thisPopulation,mutationRate)
%   MUTATE_PERMUTATION Custom mutation function for traveling salesman.
%   MUTATIONCHILDREN = MUTATE_PERMUTATION(PARENTS,OPTIONS,NVARS, ...
%   FITNESSFCN,STATE,THISSCORE,THISPOPULATION,MUTATIONRATE) mutate the
%   PARENTS to produce mutated children MUTATIONCHILDREN.
%
%   The arguments to the function are 
%     PARENTS: Parents chosen by the selection function
%     OPTIONS: Options structure created from GAOPTIMSET
%     NVARS: Number of variables 
%     FITNESSFCN: Fitness function 
%     STATE: State structure used by the GA solver 
%     THISSCORE: Vector of scores of the current population 
%     THISPOPULATION: Matrix of individuals in the current population
%     MUTATIONRATE: Rate of mutation

%   Copyright 2004 The MathWorks, Inc.
%   $Revision: 1.1.4.1 $  $Date: 2004/03/26 13:26:11 $

% Here we swap two elements of the permutation
mutationChildren = cell(length(parents),1);% Normally zeros(length(parents),NVARS);
numberOfGenes=length(thisPopulation{parents(1)});
for i=1:length(parents)
    parent = thisPopulation{parents(i)}; % Normally thisPopulation(parents(i),:)
%Define random point
   XorPoint=mod(ceil(rand(1)*10),numberOfGenes );
   if XorPoint==0
      XorPoint=XorPoint+1;
   end 
    child = parent;
    child(XorPoint) = parent(XorPoint+1);
    child(XorPoint+1) = parent(XorPoint);
    mutationChildren{i} = child; % Normally mutationChildren(i,:)
  
    
end
