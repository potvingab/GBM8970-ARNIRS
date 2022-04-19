%% Enter filename and game type

filename = '2022_03_30_15_11_Master.txt';
game = 1; %0=N-Back ; 1=Stroop

% Make sure that the file .txt to be analysed is in the same pathway that 
% the Matlab .m
% When asked, always choose add to path

% Error ? Make sure that the file .txt to be analysed is a type file TXT  
% and not a type file TXT_ or text Document

%% Déclaration des variables/tableaux
clear tabParametersNB
clear tabCheckpointsNB
clear tabResponseTimeNB
clear tabParametersStroop
clear tabCheckpointsStroop
clear tabResponseTimeStroop

j = 1;
k = 1;

%%
file = fopen(filename);
info = fscanf(file, '%s');
infoSplit = split(info, ["Para","Chec",";"]);
infoString = string(infoSplit);

%% N-Back
if game==0
    for i=1:length(infoSplit)
        if infoString(i)=='meter'
            tabParametersNB(j,1) = infoString(i+1);
            tabParametersNB(j,2) = infoString(i+2);
            j=j+1;
        elseif infoString(i)=='kpoint'
            tabCheckpointsNB(k,1) = infoString(i+1);
            tabCheckpointsNB(k,2) = infoString(i+2);
            k=k+1;
        end
    end
    tabCheckpointsNB(k,:)="FIN";
    
    % Response time
    m=1;
    for p=1:length(tabCheckpointsNB)
        if ~contains(tabCheckpointsNB(p,1),'Start') == false
            tabResponseTimeNB(m,1)= "START";
            tabResponseTimeNB(m,2)= "START";
            m=m+1;
        end
        if ~contains(tabCheckpointsNB(p,1),'Spawn') == false
            
            if tabCheckpointsNB(p+1,1)== "Diff"
                tabResponseTimeNB(m,1)= "Diff";
                a(1) = duration(tabCheckpointsNB(p,2),'inputformat','hh:mm:ss.SSS');
                a(2) = duration(tabCheckpointsNB(p+1,2),'inputformat','hh:mm:ss.SSS');
                out = milliseconds(diff(a));
                tabResponseTimeNB(m,2)= out;
            elseif tabCheckpointsNB(p+1,1)== "Same"
                tabResponseTimeNB(m,1)= "Same";
                a(1) = duration(tabCheckpointsNB(p,2),'inputformat','hh:mm:ss.SSS');
                a(2) = duration(tabCheckpointsNB(p+1,2),'inputformat','hh:mm:ss.SSS');
                out = milliseconds(diff(a));
                tabResponseTimeNB(m,2)= out;
            else
                tabResponseTimeNB(m,1)="No Response";
                tabResponseTimeNB(m,2)="-";
            end
            m=m+1;
        end
    end 
end
%% Stroop
if game==1
    for i=1:length(infoString)
        if infoString(i)=='meter'
            tabParametersStroop(j,1) = infoString(i+1);
            tabParametersStroop(j,2) = infoString(i+2);
            j=j+1;
        elseif infoString(i)=='kpoint'
            if infoString(i+1)=='EndofMenu'
                tabCheckpointsStroop(k,1) = infoString(i+1);
                tabCheckpointsStroop(k,2) = infoString(i+2);
            elseif ~contains(infoString(i+1),'Difficulty')==false
                tabCheckpointsStroop(k,1) = infoString(i+1);
                tabCheckpointsStroop(k,2) = infoString(i+2);
            elseif ~contains(infoString(i+1),'Participant')==false
                tabCheckpointsStroop(k,1) = infoString(i+1);
                tabCheckpointsStroop(k,2) = infoString(i+3);
                tabCheckpointsStroop(k,3) = infoString(i+2);
            elseif ~contains(infoString(i+1),'Question')==false
                tabCheckpointsStroop(k,1) = infoString(i+1);
                tabCheckpointsStroop(k,2) = infoString(i+3);
                tabCheckpointsStroop(k,3) = infoString(i+2);
            elseif ~contains(infoString(i+1),'Result')==false
                tabCheckpointsStroop(k,1) = 'END ' + infoString(i+1);
                tabCheckpointsStroop(k,2) = infoString(i+2);
            elseif ~contains(infoString(i+1),'Average')==false
                tabCheckpointsStroop(k,1) = 'END ' + infoString(i+1);
                tabCheckpointsStroop(k,2) = infoString(i+2);
            end
            k=k+1;
        end
    end
    tabCheckpointsStroop(k,:)="FIN";
   
    % Response time
    m=1;
    for p=1:length(tabCheckpointsStroop)
        if ~contains(tabCheckpointsStroop(p,1),'Difficulty') == false
            tabResponseTimeStroop(m,1)= "START " + tabCheckpointsStroop(p,1);
            tabResponseTimeStroop(m,2)= "START " + tabCheckpointsStroop(p,1);
            m=m+1;
        end
        if ~contains(tabCheckpointsStroop(p,1),'Participant') == false
            tabResponseTimeStroop(m,1)= tabCheckpointsStroop(p,1);
            responseTime = split(tabCheckpointsStroop(p,3),[":","sec"]);
            tabResponseTimeStroop(m,2)= responseTime(2);
            m=m+1;
        end
    end  
end
    
  

