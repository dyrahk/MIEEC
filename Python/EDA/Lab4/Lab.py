# -*- coding: utf-8 -*-
"""
Created on Thu Apr  8 15:25:33 2021

@author: Rafael
"""
import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import curve_fit
os.chdir('G:\\O meu disco\\MIEEC\\5º Ano\\2º Semestre\\EDA\\Lab4')

data=pd.read_csv('Draft2_70nm_results.txt',sep='\t')
print(data)

df = pd.DataFrame()
V=[]
for i in range(0,6,1) :
    id=[]    
    for j in range(1,122,1) :
        if (i==0):
            id.append(data['v2'][j])
        else:
            id.append(data['Id(M1)'][i*j])    
    print(id)
    V.append(id)

df = pd.DataFrame(V).transpose()
df.columns = ['Vgs','0.4','0.6','0.8','1.0', '1.2']
print (df)