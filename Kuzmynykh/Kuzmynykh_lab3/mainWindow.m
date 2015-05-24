function varargout = mainWindow(varargin)
% MAINWINDOW M-file for mainWindow.fig
%      MAINWINDOW, by itself, creates a new MAINWINDOW or raises the existing
%      singleton*.
%
%      H = MAINWINDOW returns the handle to a new MAINWINDOW or the handle to
%      the existing singleton*.
%
%      MAINWINDOW('CALLBACK',hObject,eventData,handles,...) calls the local
%      function named CALLBACK in MAINWINDOW.M with the given input arguments.
%
%      MAINWINDOW('Property','Value',...) creates a new MAINWINDOW or raises the
%      existing singleton*.  Starting from the left, property value pairs are
%      applied to the GUI before mainWindow_OpeningFcn gets called.  An
%      unrecognized property name or invalid value makes property application
%      stop.  All inputs are passed to mainWindow_OpeningFcn via varargin.
%
%      *See GUI Options on GUIDE's Tools menu.  Choose "GUI allows only one
%      instance to run (singleton)".
%
% See also: GUIDE, GUIDATA, GUIHANDLES

% Edit the above text to modify the response to help mainWindow

% Last Modified by GUIDE v2.5 12-Mar-2014 01:21:17

% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @mainWindow_OpeningFcn, ...
                   'gui_OutputFcn',  @mainWindow_OutputFcn, ...
                   'gui_LayoutFcn',  [] , ...
                   'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end
% End initialization code - DO NOT EDIT


% --- Executes just before mainWindow is made visible.
function mainWindow_OpeningFcn(hObject, eventdata, handles, varargin)
% This function has no output args, see OutputFcn.
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
% varargin   command line arguments to mainWindow (see VARARGIN)

% Choose default command line output for mainWindow
handles.output = hObject;

% Update handles structure
guidata(hObject, handles);

% UIWAIT makes mainWindow wait for user response (see UIRESUME)
% uiwait(handles.figure1);


% --- Outputs from this function are returned to the command line.
function varargout = mainWindow_OutputFcn(hObject, eventdata, handles) 
% varargout  cell array for returning output args (see VARARGOUT);
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Get default command line output from handles structure
varargout{1} = handles.output;


% --- Executes on button press in btnSolve.
function btnSolve_Callback(hObject, eventdata, handles)
% hObject    handle to btnSolve (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
%% Defining borders. File 'usborder.mat' defining this borders.
borderString = get(handles.listboxBorder, 'string');
v = get(handles.listboxBorder, 'value');  %get current selection value
borderString = borderString{v}; %choose corresponding cell
load(borderString,'x','y','xx','yy');

%%
% Generating random locations of cities inside the border. 
% We use the INPOLYGON function to make sure that all
% the cities are inside or very close to the boundary.
citiesString = get(handles.editCitiesAmount,'String');
cities = STR2NUM(citiesString);
locations = zeros(cities,2);
n = 1;
while (n <= cities)
    xp = rand*1.5;
    yp = rand;
    if inpolygon(xp,yp,xx,yy)
        locations(n,1) = xp;
        locations(n,2) = yp;
        n = n+1;
    end
end

%%
% Given the list of city locations, 
% we can calculate the distance matrix for all the cities.
distances = zeros(cities);
for count1=1:cities,
    for count2=1:count1,
        x1 = locations(count1,1);
        y1 = locations(count1,2);
        x2 = locations(count2,1);
        y2 = locations(count2,2);
        distances(count1,count2)=sqrt((x1-x2)^2+(y1-y2)^2);
        distances(count2,count1)=distances(count1,count2);
    end;
end;

%%
% The custom crossover function takes a cell array, the population, and
% returns a cell array, the children that result from the crossover.
type CXcrossover.m

%%
% The custom mutation function takes an individual, which is an ordered set
% of cities, and returns a mutated ordered set.
type mutateSalesman.m

%%
% Fitness function for the traveling salesman problem. The
% fitness of an individual is the total distance traveled for an ordered
% set of cities. The fitness function also needs the distance matrix to
% calculate the total distance.
type salesmanFitness.m

%%
% GA call fitness function with just arguments 'x' and 'distances'. 
% We can use an anonymous function to capture the values of the additional argument, 
% the distances matrix. We create a function handle 'FitnessFcn' to an anonymous function
% that takes one input 'x', but calls 'salesmanFitness' with x,
% and distances. The variable, distances has a value when the function handle
% 'FitnessFcn' is created, so these values are captured by the anonymous function.
FitnessFcn = @(x) salesmanFitness(x,distances);

%%
% A custom plot function to plot the location of the cities and the current best route.
type plotSalesman.m

%%
% Once again we will use an anonymous function to create a function handle
% to an anonymous function which calls 'plotSalesman' with the
% additional argument 'locations'.
my_plot = @(options,state,flag) plotSalesman(options, ...
    state,flag,locations,borderString);

%% Genetic Algorithm Options Setup
% This is an options structure to indicate a custom data type
% and the population range.
options = gaoptimset('PopulationType', 'custom','PopInitRange', ...
    [1;cities]);

%%
% Choose the custom creation, crossover, mutation, and plot functions that
% we have created, as well as setting some stopping conditions.
generationsString = get(handles.editGenerations,'String');
populationString = get(handles.editPopulation,'String');
generationsNum = STR2NUM(generationsString);
populationNum = STR2NUM(populationString);
cities = STR2NUM(citiesString);
timeBegin = clock;
options = gaoptimset(options,'CreationFcn',@createSalesman, ...
    'CrossoverFcn',@CXcrossover, ...
    'MutationFcn',@mutateSalesman, ...
    'PlotFcn', my_plot, ...
    'Generations',generationsNum,'PopulationSize',populationNum, ...
    'StallGenLimit',generationsNum,'Vectorized','on');
[x,fval,reason,output] = ga(FitnessFcn,cities,options)
timeEnd = clock;
time=timeBegin-timeEnd %знаходження часу роботи оптимізації
 
 displayEndOfDemoMessage(mfilename)
%%
% Finally, we call the genetic algorithm with our problem information.
numberOfVariables = cities;
[x,fval,reason,output] = ga(FitnessFcn,numberOfVariables,options)


function editCitiesAmount_Callback(hObject, eventdata, handles)
% hObject    handle to editCitiesAmount (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editCitiesAmount as text
%        str2double(get(hObject,'String')) returns contents of editCitiesAmount as a double


% --- Executes during object creation, after setting all properties.
function editCitiesAmount_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editCitiesAmount (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


function editPopulation_Callback(hObject, eventdata, handles)
% hObject    handle to editPopulation (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editPopulation as text
%        str2double(get(hObject,'String')) returns contents of editPopulation as a double


% --- Executes during object creation, after setting all properties.
function editPopulation_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editPopulation (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function editGenerations_Callback(hObject, eventdata, handles)
% hObject    handle to editGenerations (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editGenerations as text
%        str2double(get(hObject,'String')) returns contents of editGenerations as a double


% --- Executes during object creation, after setting all properties.
function editGenerations_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editGenerations (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function edit4_Callback(hObject, eventdata, handles)
% hObject    handle to edit4 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of edit4 as text
%        str2double(get(hObject,'String')) returns contents of edit4 as a double


% --- Executes during object creation, after setting all properties.
function edit4_CreateFcn(hObject, eventdata, handles)
% hObject    handle to edit4 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


% --- Executes on selection change in listboxBorder.
function listboxBorder_Callback(hObject, eventdata, handles)
% hObject    handle to listboxBorder (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: contents = get(hObject,'String') returns listboxBorder contents as cell array
%        contents{get(hObject,'Value')} returns selected item from listboxBorder


% --- Executes during object creation, after setting all properties.
function listboxBorder_CreateFcn(hObject, eventdata, handles)
% hObject    handle to listboxBorder (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: listbox controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


