# -*- coding: utf-8 -*-
"""
Created on Mon Mar 22 10:46:59 2021

@author: Rafael
"""
import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import curve_fit
os.chdir('G:\\O meu disco\\MIEEC\\5º Ano\\2º Semestre\\EDA\\Lab2')

#1
data=pd.read_csv('MeasuredIdVg1.csv',sep='\t')
print(data)

plt.plot(data['Vgs'],data['Id'],'.g')
plt.ylabel('Id')
plt.xlabel('Vgs')

#2
def Id_Vgs(Vx,Vt,Vds,Kx,Lx):
    return Kx*(Vx-Vt)**2*(1+Lx*Vds)

def Id_Vgs_t(Vx,Vt,Vds,Kx,Lx):
    return 2*Kx*((Vx-Vt)*Vds-0.5*Vds**2)*(1+Lx*Vds)
#Kx tem o 0.5

lb=[0,0,0,0]
ub=[1.0,1.5,1e-3,0.5]

#3
xx,xy=curve_fit(Id_Vgs,data['Vgs'],data['Id'], bounds=(lb,ub))
print(xx[0],xx[1],xx[2],xx[3])
#4
Id_new=Id_Vgs(data['Vgs'],xx[0],xx[1],xx[2],xx[3])

plt.plot(data['Vgs'],data['Id'],'.g',data['Vgs'],Id_new,'r')
plt.ylabel('Id')
plt.xlabel('Vgs')

#5
E_rel=(abs((Id_new-data['Id']))/data['Id'])*100
print(E_rel)

plt.figure()
plt.plot(data['Vgs'],E_rel)

#6

df=pd.DataFrame()

V=[]
vgs_r=np.arange(0.4,1.4,0.2)
vds_r=np.arange(0.0,1.21,0.01)
plt.figure()
for vgs in vgs_r :
    id=[]    
    for vds in vds_r:
        if vds > vgs - xx[0]:
            id.append(Id_Vgs(vgs,xx[0],vds,xx[2],xx[3]))    
        else:
            id.append(Id_Vgs_t(vgs,xx[0],vds,xx[2],xx[3]))                  
    plt.plot(vds_r, id)
    print(id)
    V.append(id)

df = pd.DataFrame(V).transpose()
df.columns = ['0.4','0.6','0.8','1.0', '1.2']
print (df)



data2=pd.read_csv('Idvds.csv',sep='\t',header=0)
print(data2)

plt.figure()
for vgs in vgs_r:
    plt.plot(vds_r, df[str(round(vgs,2))])
#plt.figure()
plt.plot(data2['Vds'], data2['Id'])


####################################################

data3=pd.read_csv('MeasuredIdVg2.csv',sep='\t')
print(data3)
