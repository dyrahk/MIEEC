# -*- coding: utf-8 -*-
"""
Created on Thu Apr 22 14:08:53 2021

@authors: Rafael, Francisco, José
"""

import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import curve_fit
os.chdir('G:\\O meu disco\\MIEEC\\5º Ano\\2º Semestre\\EDA\\Trabalho 2\\Measurement Files')

#A) Read mesurement files

# 1. Id(Vgs) in transfer in saturation

#W=50 L=05
data_sat5=pd.read_csv('P1G2_W50L5_Transfer_saturation.csv',sep=',', skiprows=251)
data_sat5.drop('DataName', axis='columns', inplace=True)

#W=50 L=10
data_sat10=pd.read_csv('P1G2_W50L10_Transfer_saturation.csv',sep=',', skiprows=251)
data_sat10.drop('DataName', axis='columns', inplace=True)

#W=50 L=20
data_sat20=pd.read_csv('P1G2_W50L20_Transfer_saturation.csv',sep=',', skiprows=251)
data_sat20.drop('DataName', axis='columns', inplace=True)

# 2. Plot Id(Vgs)
plt.figure()
plt.title('Id(Vgs) - LTSPICE')
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(data_sat5[' VG'],data_sat5[' ID'],label='L=5')
plt.plot(data_sat10[' VG'],data_sat10[' ID'],label='L=10')
plt.plot(data_sat20[' VG'],data_sat20[' ID'],label='L=20')
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)

#W=50 L=05
data_lin5=pd.read_csv('P1G2_W50L5_Transfer_linear.csv',sep=',', skiprows=251)
data_lin5.drop('DataName', axis='columns', inplace=True)

#W=50 L=10
data_lin10=pd.read_csv('P1G2_W50L10_Transfer_linear.csv',sep=',', skiprows=251)
data_lin10.drop('DataName', axis='columns', inplace=True)

#W=50 L=20
data_lin20=pd.read_csv('P1G2_W50L20_Transfer_linear.csv',sep=',', skiprows=251)
data_lin20.drop('DataName', axis='columns', inplace=True)

plt.figure()
plt.title('Id(Vgs) Linear, VD=0,1 - LTSPICE')
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.plot(data_lin5[' VG'],data_lin5[' ID'],label='L=5')
plt.plot(data_lin10[' VG'],data_lin10[' ID'],label='L=10')
plt.plot(data_lin20[' VG'],data_lin20[' ID'],label='L=20')
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)
# 3. Id(Vds) output characteristics for several Vgs

#W=50 L=05
data_out5=pd.read_csv('P1G2_W50L5_Output.csv',sep=',', skiprows=256)
data_out5.drop('DataName', axis='columns', inplace=True)

#W=50 L=10
data_out10=pd.read_csv('P1G2_W50L10_Output.csv',sep=',', skiprows=256)
data_out10.drop('DataName', axis='columns', inplace=True)

#W=50 L=20
data_out20=pd.read_csv('P1G2_W50L20_Output.csv',sep=',', skiprows=256)
data_out20.drop('DataName', axis='columns', inplace=True)


# 4. Plot Id(Vds) for several Vgs

plt.figure()
plt.title('Id(Vds) - L=05')
plt.ylabel('Id')
plt.xlabel('Vds')
for i in np.arange(0,9,1):
    plt.plot(data_out5[' VD'][71*i:71+(71*i)], data_out5[' ID'][71*i:71+(71*i)],label='Vgs=%.1f V' %(i-1))
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)

plt.figure()
plt.title('Id(Vds) - L=10')
plt.ylabel('Id')
plt.xlabel('Vds')
for i in np.arange(0,9,1):
    plt.plot(data_out10[' VD'][71*i:71+(71*i)], data_out10[' ID'][71*i:71+(71*i)],label='Vgs=%.1f V' %(i-1))
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)

plt.figure()
plt.title('Id(Vds) - L=20')
plt.ylabel('Id')
plt.xlabel('Vds')
for i in np.arange(0,9,1):
    plt.plot(data_out20[' VD'][71*i:71+(71*i)], data_out20[' ID'][71*i:71+(71*i)],label='Vgs=%.1f V' %(i-1))
plt.legend(bbox_to_anchor=(1, 1))
plt.grid(linewidth=0.5)
del i

# b) Obtain model parameters Ksat,Vt, m, g, l (Id=ID_sat(1+l.vd)

def gm(Id,Vgs): #gm= dId/dVgs
    return np.diff(Id)/np.diff(Vgs)

def get_m_vt(Vgs, n, Vt):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0, lambda Vgs:(Vgs-Vt)/n])

def get_K(Vgs, K):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0, lambda Vgs:K*((Vgs-Vt)**m)*(1+L*7)])

def get_lambda(Id1,Id2,Vds1,Vds2):
    return (Id1-Id2)/(Id2*Vds1-Id1*Vds2)

def get_Id(Vgs, Vds, Vt, m, L, K):
    return np.piecewise(Vgs, [Vgs < Vt, Vgs >= Vt],[lambda Vgs:0, lambda Vgs:K*((Vgs-Vt)**m)*(1+L*Vds)])

def get_alpha(Vds, alpha):
    return np.piecewise(Vds, [Vds<alpha*(7-Vt), Vds>=alpha*(7-Vt)],
                        [lambda Vds: (2*K/alpha)*((7-Vt)**gamma)*(7-Vt-(Vds/(2*alpha)))*Vds*(1+L*Vds),
                         lambda Vds: K*((7-Vt)**(2+gamma))*(1+L*Vds)])

def get_Idout(Vds, Vgs, Vt, alpha, K, gamma, L):
    return np.piecewise(Vds, [Vds<Vt, Vds<alpha*(Vgs-Vt), Vds>=alpha*(Vgs-Vt)],
                        [lambda Vds:0,
                         lambda Vds: (2*K/alpha)*((Vgs-Vt)**gamma)*(Vgs-Vt-(Vds/(2*alpha)))*Vds*(1+L*Vds),
                         lambda Vds: K*((Vgs-Vt)**(2+gamma))*(1+L*Vds)])    

U=[]
# Obtenção dos valores de gm 
gmx= gm(data_sat5[' ID'], data_sat5[' VG'])

# Obtenção dos valores de Id/gm
id_gm=(data_sat5[' ID'][1:]/gmx).to_numpy()

#Obtenção de m, Vt e gamma= m - 2 
xx,xy=curve_fit(get_m_vt,data_sat5[' VG'][1:].values,id_gm)                                                  
m=xx[0]
Vt=xx[1]   
gamma= m - 2

#Obtenção de Lambda
L=get_lambda(data_out5[' ID'][629], data_out5[' ID'][638], data_out5[' VD'][629], data_out5[' VD'][638])

#Obtenção de K=K_gsat*(W/L)
xx,xy=curve_fit(get_K,data_sat5[' VG'].values,data_sat5[' ID'].values)
K=xx[0]

xx, xy=curve_fit(get_alpha,
                 data_out5[' VD'][data_out5[' VG']==7].values,
                 data_out5[' ID'][data_out5[' VG']==7].values)
alpha=xx[0]
del xx, xy

U.append([Vt, m , gamma, K, L, alpha])

# Obtenção dos valores de gm 
gmx= gm(data_sat10[' ID'], data_sat10[' VG'])

# Obtenção dos valores de Id/gm
id_gm=(data_sat10[' ID'][1:]/gmx).to_numpy()

#Obtenção de m, Vt e gamma= m - 2 
xx,xy=curve_fit(get_m_vt,data_sat10[' VG'][1:].values,id_gm)                                                  
m=xx[0]
Vt=xx[1]   
gamma= m - 2

#Obtenção de Lambda
L=get_lambda(data_out10[' ID'][629], data_out10[' ID'][638], data_out10[' VD'][629], data_out10[' VD'][638])

#Obtenção de K=K_gsat*(W/L)
xx,xy=curve_fit(get_K,data_sat10[' VG'].values,data_sat10[' ID'].values)
K=xx[0]

xx, xy=curve_fit(get_alpha,
                 data_out10[' VD'][data_out10[' VG']==7].values,
                 data_out10[' ID'][data_out10[' VG']==7].values)
alpha=xx[0]
del xx, xy


U.append([Vt, m , gamma, K, L, alpha])

# Obtenção dos valores de gm 
gmx= gm(data_sat20[' ID'], data_sat20[' VG'])

# Obtenção dos valores de Id/gm
id_gm=(data_sat20[' ID'][1:]/gmx).to_numpy()

#Obtenção de m, Vt e gamma= m - 2 
xx,xy=curve_fit(get_m_vt,data_sat20[' VG'][1:].values,id_gm)                                                  
m=xx[0]
Vt=xx[1]   
gamma= m - 2

#Obtenção de Lambda
L=get_lambda(data_out20[' ID'][629], data_out20[' ID'][638], data_out20[' VD'][629], data_out20[' VD'][638])

#Obtenção de K=K_gsat*(W/L)
xx,xy=curve_fit(get_K,data_sat20[' VG'].values,data_sat20[' ID'].values)
K=xx[0]

xx, xy=curve_fit(get_alpha,
                 data_out20[' VD'][data_out20[' VG']==7].values,
                 data_out20[' ID'][data_out20[' VG']==7].values)
alpha=xx[0]
del xx, xy

U.append([Vt, m , gamma, K, L, alpha])

params = pd.DataFrame(U)
params.columns = ['Vt','m','gamma','K', 'Lambda', 'alpha']
params['L'] = [5,10,20]
del U

# c) Plot Id(Vgs) from the model, againts Id from measurements
Id2=get_Id(np.arange(-4,7.1,0.1),7,params['Vt'][0],params['m'][0],params['Lambda'][0],params['K'][0])

# create figure and axis objects with subplots()
fig,ax = plt.subplots()

ax.plot(data_sat5[' VG'][data_sat5[' VG'] > params['Vt'][0]], Id2[data_sat5[' VG'] > params['Vt'][0]], 'b.')
ax.plot(data_sat5[' VG'][data_sat5[' VG'] > params['Vt'][0]], data_sat5[' ID'][data_sat5[' VG'] > params['Vt'][0]], 'b')
ax.set_xlabel("Vgs")
ax.set_ylabel("Id")
# d) Repeat c) in logaritmic scale
# twin object for two different y-axis on the sample plot
ax2=ax.twinx()
ax2.semilogy(data_sat5[' VG'][data_sat5[' VG'] > params['Vt'][0]], Id2[data_sat5[' VG'] > params['Vt'][0]], 'g.')
ax2.semilogy(data_sat5[' VG'][data_sat5[' VG'] > params['Vt'][0]], data_sat5[' ID'][data_sat5[' VG'] > params['Vt'][0]], color='g')
ax2.set_ylabel("log(ID)")
plt.show()
del ax, ax2, fig    

# c) Plot Id(Vgs) from the model, againts Id from measurements
Id2=get_Id(np.arange(-4,7.1,0.1),7,params['Vt'][1],params['m'][1],params['Lambda'][1],params['K'][1])

# create figure and axis objects with subplots()
fig,ax = plt.subplots()

ax.plot(data_sat10[' VG'][data_sat10[' VG'] > params['Vt'][1]], Id2[data_sat10[' VG'] > params['Vt'][1]], 'b.')
ax.plot(data_sat10[' VG'][data_sat10[' VG'] > params['Vt'][1]], data_sat10[' ID'][data_sat10[' VG'] > params['Vt'][1]], 'b')
ax.set_xlabel("Vgs")
ax.set_ylabel("Id")
# d) Repeat c) in logaritmic scale
# twin object for two different y-axis on the sample plot
ax2=ax.twinx()
ax2.semilogy(data_sat10[' VG'][data_sat10[' VG'] > params['Vt'][1]], Id2[data_sat10[' VG'] > params['Vt'][1]], 'g.')
ax2.semilogy(data_sat10[' VG'][data_sat10[' VG'] > params['Vt'][1]], data_sat10[' ID'][data_sat10[' VG'] > params['Vt'][1]], color='g')
ax2.set_ylabel("log(ID)")
plt.show()
del ax, ax2, fig   

# c) Plot Id(Vgs) from the model, againts Id from measurements
Id2=get_Id(np.arange(-4,7.1,0.1),7,params['Vt'][2],params['m'][2],params['Lambda'][2],params['K'][2])

# create figure and axis objects with subplots()
fig,ax = plt.subplots()

ax.plot(data_sat20[' VG'][data_sat20[' VG'] > params['Vt'][2]], Id2[data_sat20[' VG'] > params['Vt'][2]], 'b.')
ax.plot(data_sat20[' VG'][data_sat20[' VG'] > params['Vt'][2]], data_sat20[' ID'][data_sat20[' VG'] > params['Vt'][2]], 'b')
ax.set_xlabel("Vgs")
ax.set_ylabel("Id")
# d) Repeat c) in logaritmic scale
# twin object for two different y-axis on the sample plot
ax2=ax.twinx()
ax2.semilogy(data_sat20[' VG'][data_sat20[' VG'] > params['Vt'][2]], Id2[data_sat20[' VG'] > params['Vt'][2]], 'g.')
ax2.semilogy(data_sat20[' VG'][data_sat20[' VG'] > params['Vt'][2]], data_sat20[' ID'][data_sat20[' VG'] > params['Vt'][2]], color='g')
ax2.set_ylabel("log(ID)")
plt.show()
del ax, ax2, fig 
 
### PLOTS
# get_Idout(Vds, Vgs, Vt, alpha, K, gamma, L):
print(params['Vt'][0])
I=get_Idout(data_out5[' VD'].values, data_out5[' VG'].values, params['Vt'][0],params['alpha'][0],params['K'][0],params['gamma'][0],params['Lambda'][0])
   
# e) Repeat all for W/L= 50/05; 50/10; 50/20
# f) Derive conclusions on the scalability of the model

  