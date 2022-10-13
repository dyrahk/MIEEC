# -*- coding: utf-8 -*-
"""
Created on Mon Apr 12 10:59:50 2021

@authors: Rafael, Francisco, José
"""

import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import curve_fit
os.chdir('G:\\O meu disco\\MIEEC\\5º Ano\\2º Semestre\\EDA\\Trabalho 1')

############################################## PARTE 1 ########################################


##################### 1 - CARREGAMENTO DE DADOS #######################################

#Dados para Id(Vgs,1.2)
vgs12_700=pd.read_csv('Id_Vgs_12_700nm.txt',sep='\t')

#Dados para Id(Vds), com os 4 ensaios realizados (0.3, 0.6, 0.9, 1.2)
vds_700=pd.read_csv('Id_Vds_700nm.txt',sep='\t')

#Carregar os dados de Id(Vds) para uma dataframe, de modo a ter os 4 ensaios separados
U=[]
U.append(vgs12_700['v1'])
for i in range(0,4,1):
    U.append(vds_700['Id(M1)'][1+i*122:122+i*122].to_numpy())
    # i*122 - offset entre ensaios, contabilizando a linha da string
vds_700 = pd.DataFrame(U).transpose()
vds_700.columns = ['Vgs','0.3','0.6','0.9', '1.2']
del U, i

######################################################################################

##################### 2 - PLOT ID(VDS) PARA OS VÁRIOS VGS ############################

plt.figure()
plt.title('Id(Vds) para W=L=700nm')
plt.ylabel('Id')
plt.xlabel('Vds')
for vgs in np.arange(0.3,1.5,0.3):
    plt.plot(vds_700['Vgs'], vds_700[str(round(vgs,2))],label='Vgs=%.1f V' %vgs)
plt.plot(vds_700['Vgs'][100], vds_700['1.2'][100],'o', vds_700['Vgs'][120],  vds_700['1.2'][120],'o')
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)
del vgs

######################################################################################

##################### 3 - PLOT ID(VGS) COM VDS = 1.2 V ###############################

plt.figure()
plt.title('Id(Vgs) para W=L=700nm')

plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(vgs12_700['v1'],vgs12_700['Id(M1)'], label='Vds=1.2 V')
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)

######################################################################################

##################### 4 - OBTENÇÃO DOS PARÂMETROS LAMBDA, VT, N e B ##################

############ Definição de funções #################

def gm(Id,Vgs): #gm= dId/dVgs
    return np.diff(Id)/np.diff(Vgs)

def get_n_vt(Vgs, n, Vt):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0, lambda Vgs:(Vgs-Vt)/n])

def get_lambda(Id1,Id2,Vds1,Vds2):
    return (Id1-Id2)/(Id2*Vds1-Id1*Vds2)

def get_B(Vgs, B):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0, lambda Vgs:B*((Vgs-Vt)**n)*(1+L*1.2)])

##################################################

# Obtenção dos valores de gm 
gm_700 = gm(vgs12_700['Id(M1)'],vgs12_700['v1'])

# Obtenção dos valores de Id/gm
id_gm_700=(vgs12_700['Id(M1)'][1:]/gm_700).to_numpy()

#Obtenção de n e Vt
xx,xy=curve_fit(get_n_vt,vgs12_700['v1'][1:].values,id_gm_700)
                                                     
n=xx[0]
Vt=xx[1]   

#Obtenção dos valores Id/gm após o fitting
id_gm_new=get_n_vt(vgs12_700['v1'][1:].values,n,Vt) 

#Obtenção de Lambda
L=get_lambda(vds_700['1.2'][100], vds_700['1.2'][120], vds_700['Vgs'][100], vds_700['Vgs'][120])

#Obtenção de B
xx,xy=curve_fit(get_B,vgs12_700['v1'].values,vgs12_700['Id(M1)'].values)
B=xx[0]

del xx, xy

######################## PLOTS ##############################

#Id/gm antes e após fitting

plt.figure()
plt.title('Id/gm para W=L=700nm com Vds=1.2 V')
plt.ylabel('Id/gm')
plt.xlabel('Vgs')
plt.plot(vgs12_700['v1'][1:],id_gm_700, label='Obtido por LTSpice')
plt.plot(vgs12_700['v1'][1:],id_gm_new, label='Após fitting')
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)                                                

#Id(Vgs) com Vds = 1.2 V antes e após fitting
plt.figure()
plt.title('Id(Vgs) para W=L=70nm com Vds=1.2 V')
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(vgs12_700['v1'],vgs12_700['Id(M1)'], label='Obtido por LTSpice')
plt.plot(vgs12_700['v1'],get_B(vgs12_700['v1'].values,B),label='Após fitting')
plt.legend(bbox_to_anchor=(1, 1))
#Id(Vgs) com Vds = 1.2 V antes e após fitting em escala logarítmica
plt.figure()
plt.title('Id(Vgs) para W=L=700nm com Vds=1.2 V')
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(vgs12_700['v1'],vgs12_700['Id(M1)'], label='Obtido por LTSpice')
plt.plot(vgs12_700['v1'],get_B(vgs12_700['v1'].values,B),label='Após fitting')
plt.yscale('log')
plt.legend(bbox_to_anchor=(1, 1))

#############################################################

############################################## PARTE 2 ########################################

##################### 1 - get_ID FUNCTION #############################################


def get_Id(Vgs, Vds, Vt, n, L, B):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0, lambda Vgs:B*((Vgs-Vt)**n)*(1+L*Vds)])


##################### 2 - Id2(Vgs,Vds) matrix #########################################
Id2=get_Id(np.arange(0,1.21,0.01),1.2,Vt,n,L,B)

##################### 3 - Id2(Vgs) plot, with Vds = 1.2 V #############################
plt.figure()
plt.title('Id(Vgs) para W=L=700nm com Vds=1.2 V')
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(vgs12_700['v1'],get_B(vgs12_700['v1'].values,B),label='Após fitting')
plt.plot(vgs12_700['v1'],Id2, label='através de get_Id')
plt.yscale('log')
plt.legend(bbox_to_anchor=(1, 1))

##################### 4 - Relative Error of Id2(Vgs) in relation to Id1(Vgs) ##########
# i.e.,	Error	(Vgs)	=	(Id2	(Vgs)	-Id1	(Vgs))	/	Id1	(Vgs).
E_rel=((Id2-vgs12_700['Id(M1)'])/vgs12_700['Id(M1)'])*100
plt.figure()
plt.ylabel('%')
plt.xlabel('Vgs')
plt.title('Erro relativo de Id2(Vgs) em relação a Id(Vgs) para W=L=700nm')
plt.plot(vgs12_700['v1'],E_rel,'+')
plt.axis([Vt,1.2,0,25])