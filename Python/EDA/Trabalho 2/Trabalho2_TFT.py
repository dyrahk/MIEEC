# -*- coding: utf-8 -*-
"""
Created on Mon May  3 15:44:23 2021

@author: Rafael
"""

import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import curve_fit
os.chdir('G:\\O meu disco\\MIEEC\\5º Ano\\2º Semestre\\EDA\\Trabalho 2\\Measurement Files')

##############################################################################
########################## CARREGAMENTO DE DADOS #############################

#W=50 L=05

data_sat5=pd.read_csv('P1G2_W50L5_Transfer_saturation.csv',sep=',', skiprows=251)
data_sat5.drop('DataName', axis='columns', inplace=True)
data_out5=pd.read_csv('P1G2_W50L5_Output.csv',sep=',', skiprows=256)
data_out5.drop('DataName', axis='columns', inplace=True)

#W=50 L=10

data_sat10=pd.read_csv('P1G2_W50L10_Transfer_saturation.csv',sep=',', skiprows=251)
data_sat10.drop('DataName', axis='columns', inplace=True)
data_out10=pd.read_csv('P1G2_W50L10_Output.csv',sep=',', skiprows=256)
data_out10.drop('DataName', axis='columns', inplace=True)

#W=50 L=20

data_sat20=pd.read_csv('P1G2_W50L20_Transfer_saturation.csv',sep=',', skiprows=251)
data_sat20.drop('DataName', axis='columns', inplace=True)
data_out20=pd.read_csv('P1G2_W50L20_Output.csv',sep=',', skiprows=256)
data_out20.drop('DataName', axis='columns', inplace=True)

##############################################################################
##############################################################################

##############################################################################
################################### FUNCS ####################################

def plotter_Id_vds(L_val, data_out):
    plt.figure()
    plt.title('Id(Vds) - L=%02d' %(L_val))
    plt.ylabel('Id')
    plt.xlabel('Vds')
    for i in data_out[' VG'].unique():
        plt.plot(data_out[' VD'][data_out[' VG']==i], data_out[' ID'][data_out[' VG']==i],label='Vgs=%.1f V' %(i))
    plt.legend(bbox_to_anchor=(1, 1))
    plt.grid(linewidth=0.5)

def plotter_Id_vgs(params,i,data_sat):
    # c) Plot Id(Vgs) from the model, againts Id from measurements
    Id2=get_Id(np.arange(-4,7.1,0.1),7,params['Vt'][i],params['m'][i],params['Lambda'][i],params['K'][i])
    
    # create figure and axis objects with subplots()
    fig,ax = plt.subplots()
  
    plt.title('Id(Vgs) - Medidos vs Modelo 1 - L=%02d' %(5*(2**i)))
    
    l1=ax.plot(data_sat[' VG'][data_sat[' VG'] > params['Vt'][i]], Id2[data_sat[' VG'] > params['Vt'][i]], 'b.')[0]
    l2=ax.plot(data_sat[' VG'][data_sat[' VG'] > params['Vt'][i]], data_sat[' ID'][data_sat[' VG'] > params['Vt'][i]], 'b')[0]
    ax.set_xlabel("Vgs")
    ax.set_ylabel("Id")
    
    # d) Repeat c) in logaritmic scale
    # twin object for two different y-axis on the sample plot
    ax2=ax.twinx()
    l3=ax2.semilogy(data_sat[' VG'][data_sat[' VG'] > params['Vt'][i]], Id2[data_sat[' VG'] > params['Vt'][i]], 'g.')[0]
    l4=ax2.semilogy(data_sat[' VG'][data_sat[' VG'] > params['Vt'][i]], data_sat[' ID'][data_sat[' VG'] > params['Vt'][i]], color='g')[0]
    ax2.set_ylabel("log(ID)")
    
    line_labels = ["Medido", "Modelo", "Medido (log)", "Modelo (log)"]
    fig.legend([l1, l2, l3, l4],     # The line objects
           labels=line_labels,   # The labels for each line
           loc="lower right",   # Position of legend
           borderaxespad=0.1,    # Small spacing around legend box
           )
    plt.show()
    del ax, ax2, fig, l1, l2, l3, l4    

def plotter_Id_vds_vs(L_val,data_out, I_out):
    plt.figure()
    plt.title('Id(Vds) - L=%02d' %(L_val))
    plt.ylabel('Id')
    plt.xlabel('Vds')
    for i in I5[' VG'].unique():
        plt.plot(data_out[' VD'][data_out[' VG']==i], data_out[' ID'][data_out[' VG']==i],label='Vgs=%.1f V' %(i))
        plt.plot(I_out[' VD'][I_out[' VG']==i], I_out[' ID'][I_out[' VG']==i],"--",label='Vgs=%.1f V' %(i), color=plt.gca().lines[-1].get_color())
    plt.legend(bbox_to_anchor=(1, 1))
    plt.grid(linewidth=0.5)   

def plotter_Id_model2(data_sat, data_out, Rs_Ix, Rd_Ix, parameters, i):
    
    plt.figure()
    plt.title('Id(Vgs) - MEDIDO VS MODELO (contabilizando RS) L=%02d' %parameters['L'][i])
    plt.ylabel('Id')
    plt.xlabel('Vgs')
    plt.plot(data_sat[' VG'][data_sat[' VG']>=parameters['Vt'][i]],
             data_sat[' ID'][data_sat[' VG']>=parameters['Vt'][i]],
             label="Medido")
    plt.plot(data_sat[' VG'][data_sat[' VG']>=parameters['Vt'][i]],
             Rs_Ix,
             "--",label="Modelo2")
    plt.legend(bbox_to_anchor=(1, 1))
    plt.grid(linewidth=0.5)
    
    
    plt.figure()
    plt.title('Id(Vgs) - MEDIDO VS MODELO (contabilizando RS e RD) L=%02d' %parameters['L'][i])
    plt.ylabel('Id')
    plt.xlabel('Vgs')
    plt.plot(data_sat[' VG'][data_sat[' VG']>=parameters['Vt'][i+1]],
             data_sat[' ID'][data_sat[' VG']>=parameters['Vt'][i+1]],
             label="Medido")
    plt.plot(data_sat[' VG'][data_sat[' VG']>=parameters['Vt'][i+1]],
             Rd_Ix,
             "--",label="Modelo2")
    plt.legend(bbox_to_anchor=(1, 1))
    plt.grid(linewidth=0.5)
    
    plt.figure()
    plt.title('Id(Vds), VGS = 7 V - MEDIDO VS MODELO (contabilizando RS e RD) L=%02d' %parameters['L'][i])
    plt.ylabel('Id')
    plt.xlabel('Vds')
    plt.plot(data_out[' VD'][568:],data_out[' ID'][568:], label="Medido")
    plt.plot(data_out[' VD'][568:],
             get_Rd_L((data_out[' VG'][568:],data_out[' VD'][568:],data_out[' ID'][568:]),
                      parameters['Vt'][i+1], parameters['m'][i+1], parameters['K'][i+1], 
                      parameters['Rs'][i+1], parameters['Rd'][i+1], parameters['Lambda'][i+1]),
             "--",label="Modelo2")
    plt.legend(bbox_to_anchor=(1, 1))
    plt.grid(linewidth=0.5)
 
def plotter_error(a, b, xaxis, xlabel, title):
    E_rel=((a-b)/b)*100
    plt.figure()
    plt.ylabel('%')
    plt.xlabel(xlabel)
    plt.title(title)
    plt.plot(xaxis, E_rel, '+')
    
def gm(Id,Vgs): #gm= dId/dVgs
    return np.diff(Id)/np.diff(Vgs)

def get_m_vt(Vgs, n, Vt):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0,
                                                    lambda Vgs:(Vgs-Vt)/n])

def get_K(Vgs, K):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0,
                                                    lambda Vgs:K*((Vgs-Vt)**m)*(1+L*7)])

def get_lambda(Id1,Id2,Vds1,Vds2):
    return (Id1-Id2)/(Id2*Vds1-Id1*Vds2)

def get_Id(Vgs, Vds, Vt, m, L, K):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0,
                                                    lambda Vgs:K*((Vgs-Vt)**m)*(1+L*Vds)])

def get_alpha(Vds, alpha):
    return np.piecewise(Vds, [Vds<alpha*(7-Vt), Vds>=alpha*(7-Vt)],
                        [lambda Vds: (2*K/alpha)*((7-Vt)**gamma)*(7-Vt-(Vds/(2*alpha)))*Vds*(1+L*Vds),
                         lambda Vds: K*((7-Vt)**(2+gamma))*(1+L*Vds)])

def calc_parameters(data_out, data_sat):
    
    global m, Vt, gamma, K, L, alpha
    # Obtenção dos valores de gm 
    gmx= gm(data_sat[' ID'], data_sat[' VG'])
    
    # Obtenção dos valores de Id/gm
    id_gm=(data_sat[' ID'][1:]/gmx).to_numpy()
    
    #Obtenção de m, Vt e gamma= m - 2 
    xx,xy=curve_fit(get_m_vt,data_sat[' VG'][1:].values,id_gm)                                                  
    m=xx[0]
    Vt=xx[1]   
    gamma= m - 2
    #Obtenção de Lambda
    L=get_lambda(data_out[' ID'][629], data_out[' ID'][638],
                 data_out[' VD'][629], data_out[' VD'][638])
    
    #Obtenção de K=K_gsat*(W/L)
    xx,xy=curve_fit(get_K,data_sat[' VG'].values,data_sat[' ID'].values)
    K=xx[0]
    
    xx, xy=curve_fit(get_alpha,
                     data_out[' VD'][data_out[' VG']==7].values,
                     data_out[' ID'][data_out[' VG']==7].values)
    alpha=xx[0]
    U=[Vt, m , gamma, K, L, alpha]  
    del Vt, m , gamma, K, L, alpha
    return U

def get_Idout(Vds, Vx, alpha, K, gamma, m, L):
    return np.piecewise(Vds, [Vds<alpha*(Vx), Vds>=alpha*(Vx)],
                        [lambda Vds: (2*K/alpha)*((Vx)**gamma)*(Vx-(Vds/(2*alpha)))*Vds*(1+L*Vds),
                         lambda Vds: K*(Vx**m)*(1+L*Vds)])    

def calc_Iout(data_out,params,j):
    I=[]
    for i in range(0,len(data_out),1):
        if(data_out[' VG'][i]>=params['Vt'][j]):
            I.append([data_out[' VG'][i], data_out[' VD'][i],
                      get_Idout(data_out5[' VD'][i], data_out5[' VG'][i]-params['Vt'][j],
                               params['alpha'][j],params['K'][j],params['gamma'][j],
                               params['m'][j],params['Lambda'][j])])
    I_df = pd.DataFrame(I)
    I_df.columns=[' VG',' VD',' ID']
    return I_df

def get_Rs(data, Vt, m, K, Rs):
    Vgs,Id=data
    return K*((Vgs-Rs*Id-Vt)**m)

def get_Rd_L(data, Vt, m, K, Rs, Rd, L):
    Vgs,Vds,Id=data
    return K*((Vgs-Rs*Id-Vt)**m)*(1+L*(Vds-(Rd+Rs)*Id))

def calc_parameters2(data_out, data_sat, L, p00, bounds_Rs, bounds_Rd):
    U=[]
    xx,xy=curve_fit(get_Rs, (data_sat[' VG'][data_sat[' VG']>=p00['Vt']],data_sat[' ID'][data_sat[' VG']>=p00['Vt']]),
                    data_sat[' ID'][data_sat[' VG']>=p00['Vt']],maxfev=4000,
                    p0=[p00['Vt'], p00['m'], p00['K'], 100],
                    bounds=bounds_Rs)    
    U.insert(0,[xx[0],xx[1],xx[2],xx[3],0,0,L])

    xx,xy=curve_fit(get_Rd_L, (data_out[' VG'][629:],data_out[' VD'][629:],data_out[' ID'][629:]),
                    data_out[' ID'][629:],maxfev=4000,
                    p0=[xx[0],xx[1],xx[2],xx[3],100,p00['Lambda']],
                    bounds=bounds_Rd)
    U.insert(1,[xx[0],xx[1],xx[2],xx[3],xx[4],xx[5],L])
    
    return U
    
##############################################################################
##############################################################################

# Cálculo Parâmetros Modelo 1

U=[]
U.append(calc_parameters(data_out5,data_sat5)) 
U.append(calc_parameters(data_out10,data_sat10))
U.append(calc_parameters(data_out20,data_sat20))
parameters = pd.DataFrame(U)
parameters.columns = ['Vt','m','gamma','K', 'Lambda', 'alpha']
parameters['L'] = [5,10,20]  
del U

# Cálculo das correntes de saída Modelo 1 
I5=calc_Iout(data_out5,parameters,0)
I10=calc_Iout(data_out10,parameters,1)
I20=calc_Iout(data_out20,parameters,2)

# Cálculo Parâmetros Modelo 2
parameters2 = pd.DataFrame()
parameters2= parameters2.append(calc_parameters2(data_out5,data_sat5,5,
                                                  parameters.loc[0],
                                                  ([-2,2.5,0,0],[1,4,0.001,300]),
                                                  ([-2,2.5,0,0,0,0],[1.5,4,0.001,300,300,1])))
parameters2= parameters2.append(calc_parameters2(data_out10,data_sat10,10,
                                                  parameters.loc[1],
                                                  ([-2,2,0,0],[1,3,0.001,200]),
                                                  ([-2,2,0,0,0,0],[1.5,3,0.001,200,200,1])))
parameters2= parameters2.append(calc_parameters2(data_out20,data_sat20,20,
                                                  parameters.loc[2],
                                                  ([-2,2,0,0],[1,3,0.001,200]),
                                                  ([-2,2,0,0,0,0],[1.5,3,0.001,200,200,1])))
parameters2.columns = ['Vt','m','K', 'Rs', 'Rd', 'Lambda','L']
parameters2 = parameters2.reset_index(drop=True)
    
# Cálculo das Correntes tendo em conta Rs e Rd (Saturação) Modelo 2
Rs_I5=get_Rs((data_sat5[' VG'][data_sat5[' VG']>=parameters2['Vt'][0]],
              data_sat5[' ID'][data_sat5[' VG']>=parameters2['Vt'][0]]),
             parameters2['Vt'][0], parameters2['m'][0], parameters2['K'][0],parameters2['Rs'][0])

Rs_I10=get_Rs((data_sat10[' VG'][data_sat10[' VG']>=parameters2['Vt'][2]],
               data_sat10[' ID'][data_sat10[' VG']>=parameters2['Vt'][2]]),
             parameters2['Vt'][2], parameters2['m'][2], parameters2['K'][2],parameters2['Rs'][2])

Rs_I20=get_Rs((data_sat20[' VG'][data_sat20[' VG']>=parameters2['Vt'][4]],
               data_sat20[' ID'][data_sat20[' VG']>=parameters2['Vt'][4]]),
             parameters2['Vt'][4], parameters2['m'][4], parameters2['K'][4],parameters2['Rs'][4])

Rd_I5=get_Rd_L((data_sat5[' VG'][data_sat5[' VG']>=parameters2['Vt'][1]],
                data_sat5[' VD'][data_sat5[' VG']>=parameters2['Vt'][1]],
                data_sat5[' ID'][data_sat5[' VG']>=parameters2['Vt'][1]]),
                parameters2['Vt'][1], parameters2['m'][1], parameters2['K'][1],
                parameters2['Rs'][1], parameters2['Rd'][1], parameters2['Lambda'][1])

Rd_I10=get_Rd_L((data_sat10[' VG'][data_sat10[' VG']>=parameters2['Vt'][3]],
                 data_sat10[' VD'][data_sat10[' VG']>=parameters2['Vt'][3]],
                 data_sat10[' ID'][data_sat10[' VG']>=parameters2['Vt'][3]]),
                parameters2['Vt'][3], parameters2['m'][3], parameters2['K'][3],
                parameters2['Rs'][3], parameters2['Rd'][3], parameters2['Lambda'][3])

Rd_I20=get_Rd_L((data_sat20[' VG'][data_sat20[' VG']>=parameters2['Vt'][5]],
                 data_sat20[' VD'][data_sat20[' VG']>=parameters2['Vt'][5]],
                 data_sat20[' ID'][data_sat20[' VG']>=parameters2['Vt'][5]]),
                parameters2['Vt'][5], parameters2['m'][5], parameters2['K'][5],
                parameters2['Rs'][5], parameters2['Rd'][5], parameters2['Lambda'][5])
##############################################################################
################################### PLOTS ####################################

#Plot Id(Vgs) Saturation

plt.figure()
plt.title('Id(Vgs) Saturation, VDS=7 V - MEDIDOS')
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(data_sat5[' VG'],data_sat5[' ID'],label='L=5')
plt.plot(data_sat10[' VG'],data_sat10[' ID'],label='L=10')
plt.plot(data_sat20[' VG'],data_sat20[' ID'],label='L=20')
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)

#Plot Id(Vds) for several Vgs

plotter_Id_vds(5, data_out5)
plotter_Id_vds(10, data_out10)
plotter_Id_vds(20, data_out20)

#Plot Id(Vgs) from the model, againts Id from measurements

plotter_Id_vgs(parameters,0,data_sat5)
Id2=get_Id(np.arange(-4,7.1,0.1),7,parameters['Vt'][0],parameters['m'][0],parameters['Lambda'][0],parameters['K'][0])
plotter_error(Id2[data_sat5[' VG'] > parameters['Vt'][0]],
              data_sat5[' ID'][data_sat5[' VG'] > parameters['Vt'][0]],
              data_sat5[' VG'][data_sat5[' VG'] > parameters['Vt'][0]],
              "Vgs", "Erro relativo de Id2(Vgs) em relação a Id(Vgs) medido para L=05")

plotter_Id_vgs(parameters,1,data_sat10)
Id2=get_Id(np.arange(-4,7.1,0.1),7,parameters['Vt'][1],parameters['m'][1],parameters['Lambda'][1],parameters['K'][1])
plotter_error(Id2[data_sat10[' VG'] > parameters['Vt'][1]],
              data_sat10[' ID'][data_sat10[' VG'] > parameters['Vt'][1]],
              data_sat10[' VG'][data_sat10[' VG'] > parameters['Vt'][1]],
              "Vgs", "Erro relativo de Id2(Vgs) em relação a Id(Vgs) medido para L=10")

plotter_Id_vgs(parameters,2,data_sat20)
Id2=get_Id(np.arange(-4,7.1,0.1),7,parameters['Vt'][2],parameters['m'][2],parameters['Lambda'][2],parameters['K'][2])
plotter_error(Id2[data_sat20[' VG'] > parameters['Vt'][2]],
              data_sat20[' ID'][data_sat20[' VG'] > parameters['Vt'][2]],
              data_sat20[' VG'][data_sat20[' VG'] > parameters['Vt'][2]],
              "Vgs", "Erro relativo de Id2(Vgs) em relação a Id(Vgs) medido para L=20")

# Plot Id(Vds) for several Vgs, , againts Id from measurements
plotter_Id_vds_vs(5,data_out5, I5)
plotter_error(I5[' ID'][I5[' VG']==7].values,
              data_out5[' ID'][data_out5[' VG']==7].values,
              data_out5[' VD'][data_out5[' VG']==7],
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=05")

plotter_Id_vds_vs(10,data_out10, I10)
plotter_error(I10[' ID'][I10[' VG']==7].values,
              data_out10[' ID'][data_out10[' VG']==7].values,
              data_out10[' VD'][data_out10[' VG']==7],
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=10")

plotter_Id_vds_vs(20,data_out20, I20)
plotter_error(I20[' ID'][I20[' VG']==7].values,
              data_out20[' ID'][data_out20[' VG']==7].values,
              data_out20[' VD'][data_out20[' VG']==7],
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=20")

# Plots Id
plotter_Id_model2(data_sat5,data_out5, Rs_I5, Rd_I5, parameters2, 0)
plotter_error(Rs_I5.values,
              data_sat5[' ID'][data_sat5[' VG']>=parameters2['Vt'][0]].values,
              data_sat5[' VG'][data_sat5[' VG']>=parameters2['Vt'][0]].values,
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=05")
plotter_error(Rd_I5.values,
              data_sat5[' ID'][data_sat5[' VG']>=parameters2['Vt'][1]].values,
              data_sat5[' VG'][data_sat5[' VG']>=parameters2['Vt'][1]].values,
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=05")

plotter_Id_model2(data_sat10,data_out10, Rs_I10, Rd_I10, parameters2, 2)
plotter_error(Rs_I10.values,
              data_sat10[' ID'][data_sat10[' VG']>=parameters2['Vt'][2]].values,
              data_sat10[' VG'][data_sat10[' VG']>=parameters2['Vt'][2]].values,
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=10")
plotter_error(Rd_I10.values,
              data_sat10[' ID'][data_sat10[' VG']>=parameters2['Vt'][3]].values,
              data_sat10[' VG'][data_sat10[' VG']>=parameters2['Vt'][3]].values,
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=10")


plotter_Id_model2(data_sat20,data_out20, Rs_I20, Rd_I20, parameters2, 4)
plotter_error(Rs_I20.values,
              data_sat20[' ID'][data_sat20[' VG']>=parameters2['Vt'][4]].values,
              data_sat20[' VG'][data_sat20[' VG']>=parameters2['Vt'][4]].values,
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=20")
plotter_error(Rd_I20.values,
              data_sat20[' ID'][data_sat20[' VG']>=parameters2['Vt'][5]].values,
              data_sat20[' VG'][data_sat20[' VG']>=parameters2['Vt'][5]].values,
              "Vds", "Erro relativo de Id2(Vds) em relação a Id(Vds) medido para L=20")
##############################################################################
##############################################################################