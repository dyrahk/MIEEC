# -*- coding: utf-8 -*-
"""
Created on Mon Mar 22 10:46:59 2021

@author: Rafael
"""
import os
import pandas as pd
import matplotlib.pyplot as plt
from scipy.optimize import curve_fit
os.chdir('C:\\Users\\Rafael\\Desktop\\5º Ano\\EDA\\TP_22_3')

data=pd.read_csv('MeasuredIdVg1.csv',sep='\t',header=0)
print(data)

plt.plot(data['Vgs'],data['Id'],'.g')
plt.ylabel('Id')
plt.xlabel('Vgs')

def Id_Vgs(Vx,Vt,Vds,Kx,Lx):
    return Kx*(Vx-Vt)**2*(1+Lx*Vds)
#Kx tem o 0.5

lb=[0,0,0,1.1]
ub=[1.0,1e-3,0.5,1.5]

xx,xy=curve_fit(Id_Vgs,data['Vgs'],data['Id'], bounds=(lb,ub))

Id_new=Id_Vgs(data['Vgs'],xx[0],xx[1],xx[2],xx[3])

plt.plot(data['Vgs'],data['Id'],'.g',data['Vgs'],Id_new,'r')
plt.ylabel('Id')
plt.xlabel('Vgs')

E_rel=(abs((Id_new-data['Id']))/data['Id'])*100
print(E_rel)

plt.figure()
plt.plot(data['Vgs'],E_rel)