function pop = create_permutations(NVARS,FitnessFcn,options)
%������� ��� ��������� ���������
totalPopulationSize = sum(options.PopulationSize);
n = NVARS;
pop = cell(totalPopulationSize,1);
for i = 1:totalPopulationSize
    pop{i} = randperm(n); 
end

