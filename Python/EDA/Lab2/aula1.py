# -*- coding: utf-8 -*-
"""
Created on Thu Mar 18 15:18:34 2021

@author: Rafael
"""
import os
import pandas as pd
import matplotlib.pyplot as plt
os.chdir('C:\\Users\\Rafael\\Desktop\\5º Ano\\EDA')

data=pd.read_csv('IDvgs1.txt',sep='\t',header=0)

x=data['v2']
y=data['Id(M1)']

plt.plot(x,y)
plt.ylabel('Id')
plt.xlabel('Vgs')
plt.title('Gráfico')