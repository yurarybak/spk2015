function mutationChildren = ivert_with_landslip(parents ,options,NVARS, ...
    FitnessFcn, state, thisScore,thisPopulation,mutationRate)
%������������ � ������
mutationChildren = cell(length(parents),1);
for i=1:length(parents)
    parent = thisPopulation{parents(i)}; 
    p = ceil(length(parent) * rand(1,2));%���������� ������ � 2 �����,
    %1�� ����� ������� ���� ���� �������������� �� ������� 2�� �����
    child = parent;
    %���� �������� ������ �� �����
    if(p(2)>p(1))
        k1 = parent(p(2));
        for j = (p(2)+1): length(parent)
            k2 = parent(j);
            child(j)=k1;
            k1=k2;
        end
         for j = 1: p(1)
            k2 = parent(j);
            child(j)=k1;
            k1=k2;
         end
        child(p(2)) = k1;
    end
  if(p(2)<p(1))
        k1 = parent(p(1));
        for j = p(2) : p(1)
            k2 = parent(j);
            child(j)=k1;
            k1=k2;
        end
   end
   child(p(2)) = parent(p(1));
   
    child;
    mutationChildren{i} = child; 
end
end

