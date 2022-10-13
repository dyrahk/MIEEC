# -*- coding: utf-8 -*-
"""
Spyder Editor

This is a temporary script file.
"""
import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import minimize
# from scipy.optimize import curve_fit
os.chdir('G:\\O meu disco\\MIEEC\\5º Ano\\2º Semestre\\EDA\\Trabalho3\\LT')


##############################################################################
######################### Tech	file	UMC130nm #############################

miuR=1;	
miu=4*np.pi*1e-7*miuR;	
E0=8.854187817e-12;                               #F/m	
c=299792458;                                      #m/s		

# Layer	Oxide	(Metal)	

t=2.8e-6;	                                      #Metal thickness (m)	
Rsheet=10*1e-3;                                   #Sheet Resistance [Ohm/square] (=	Ro/t)	
Ro=Rsheet*t;				                      #Metal Resistance	[Ohm.m]	
toxide=5.42e-6;						              #Dielectric (Oxide) Thickness	(from metal to substrate)	
t_M_underpass=0.4e-6;	
toxide_Munderpass=toxide-t_M_underpass-4.76e-6;	
Erox=4;								              #Oxide Er	
Eox=E0*Erox;                                      #Oxide permitivity %Substrate	
Ersub=11.9;					                      #substrate Er	
Esub=E0*Ersub;								      #substrate permititvity	
Tsub=700e-6;								      #substrate thickness	
Sub_resistiv=2800;								  #28 ohm-cm ou	2800 Ohm-m	

##############################################################################
##############################################################################

##############################################################################
########################## Carregamento de Dados #############################
Lt_Quad=pd.read_csv('Quad.txt',sep='\t')
Lt_Quad.columns=['F','L']
Lt_Quad[['L','lixo']] = Lt_Quad['L'].str.split(',',expand=True)
Lt_Quad.drop('lixo', axis='columns', inplace=True)
Lt_Square=Lt_Quad['L'].astype(float)

Lt_Hexa=pd.read_csv('Hexa.txt',sep='\t')
Lt_Hexa.columns=['F','L']
Lt_Hexa[['L','lixo']] = Lt_Hexa['L'].str.split(',',expand=True)
Lt_Hexa.drop('lixo', axis='columns', inplace=True)
Lt_Hexagonal=Lt_Hexa['L'].astype(float)

Lt_Octa=pd.read_csv('Octa.txt',sep='\t')
Lt_Octa.columns=['F','L']
Lt_Octa[['L','lixo']] = Lt_Octa['L'].str.split(',',expand=True)
Lt_Octa.drop('lixo', axis='columns', inplace=True)
Lt_Octogonal=Lt_Octa['L'].astype(float)

tab1={'Shape':['Square', 'Hexagonal', 'Octagonal'],
       'N Sides':[4,6,8],
       'K1':[2.34, 2.33, 2.25],
       'K2':[2.75, 3.82, 3.55]}

##############################################################################
##############################################################################

##############################################################################
########################### Definiçao de funções #############################

def calc_Ls(n, d_avg, k1, k2, rox, miu0):
    return (n**2)*d_avg*k1*miu0 / (1 + k2*rox)

def calc_d_out(d_in, n , s, w):
    return d_in + 2*n*w + 2*(n-1)*s

def calc_d_in(d_out, n, s, w):
    return d_out - 2*n*w - 2*(n-1)*s

def calc_d_avg(d_in, d_out):
    return 0.5*(d_in + d_out)

def calc_rox(d_in, d_out):
    return (d_out - d_in)/(d_out + d_in)

#RS VARIA COM F
def calc_Rs(l, w, delta, t_eff):
    return l*Ro/(w*delta*t_eff)

def calc_delta(f, miux):
    return np.sqrt(Ro / (np.pi*f*miux))

def calc_t_eff(delta):
    return 1 - np.exp(-t/delta)

def calc_l(nside, davg, n):
    return nside * davg * n * np.tan(np.pi/nside)

def calc_Cp(n, w):
    return (n-1) * w**2 * Eox/toxide_Munderpass

def calc_Cox(l, w):
    return 0.5 * l * w * Eox/toxide

def calc_Csub(l, w):
    return 0.5 * l * w * Esub/Tsub

def calc_Rsub(l, w):
    return 2 * Tsub * Sub_resistiv / (l * w) 

def calc_Z(f, Ls, Rs, Cp, Cox, Csub, Rsub):
    
    ZCsub = -1j/(2*np.pi*f*Csub)
    
    Zpar = ZCsub * Rsub / (ZCsub + Rsub )
    
    ZCox = -1j/(2*np.pi*f*Cox)
    
    ZCp = -1j/(2*np.pi*f*Cp)
    
    ZSerie = Rs + 1j*2*np.pi*f*Ls 
    
    Z1 = Zpar + ZCox
    
    Z2 = ZCp * ZSerie / (ZCp + ZSerie)
    
    Z = Z1 * Z2 / (Z1 + Z2)

    return Z

def calc_L(Z, f):
    return Z.imag/(2*np.pi*f)
    
def calc_QF(Z):
    return Z.imag/Z.real

def calc_Fres(L,C):
    return 1/(2*np.pi*np.sqrt(L*C))

def parametros(freq, i, n_turns , space, width, dout):
    Din = calc_d_in(dout, n_turns, S, width)
    if Din < 30e-6:
        Din=30e-6
    Davg = calc_d_avg(Din, dout)
    Rox = calc_rox(Din, dout)
    Ls = calc_Ls(n_turns, Davg, tab1['K1'][i], tab1['K2'][i], Rox, miu)
    
    Delta = calc_delta(freq, miu)
    T_eff = calc_t_eff(Delta)
    l = calc_l(tab1['N Sides'][i], Davg, n_turns)
    Rs = calc_Rs(l, width, Delta, T_eff)
    
    Cp = calc_Cp(n_turns, width)
    Cox = calc_Cox(l, width)
    Csub = calc_Csub(l, width)
    Rsub = calc_Rsub(l, width)
          
    Z = calc_Z(freq, Ls, Rs, Cp, Cox, Csub, Rsub)
    L = calc_L(Z, freq)
    Qfactor= calc_QF(Z)
    Fres= calc_Fres(Ls, Cp)
    
    return [Din, Davg, Ls, Rs, Cp, Cox, Csub, Rsub, Z, L, Qfactor, Fres]

def plotter_error(a, b, xaxis, xlabel, title):
    E_rel=((a-b)/b)*100
    plt.figure()
    plt.ylabel('%')
    plt.xlabel(xlabel)
    plt.title(title)
    plt.xscale("log")
    plt.plot(xaxis, E_rel, '+')

##############################################################################
##############################################################################

##############################################################################
############################# Partes 1, 2 e 3 ################################

W = 10e-6 # • Largura da fita de metal, w
Dout = 340e-6 # • Diâmetro externo, Dout
S = 5e-6 # • Espaço entre fitas, s
n = 3 # • Número de voltas, n
f = 1e9

U=[]
U.append(parametros(f, 0, n, S, W, Dout)) 
U.append(parametros(f, 1, n, S, W, Dout))
U.append(parametros(f, 2, n, S, W, Dout))
params = pd.DataFrame(U)
params.columns = ['Din','Davg', 'Ls', 'Rs', 'Cp', 'Cox',
                  'Csub', 'Rsub', 'Z', 'L', 'Qfactor', 'Fres']
params['Shape'] = tab1['Shape']  

for i in range(0,3,1):
    
    Rvec1=[]
    Lvec1=[]
    Qvec1=[]
    Fvec1=[]
     
    for f in np.logspace(3, 11, num=81):
         
         Delta = calc_delta(f, miu)
         T_eff = calc_t_eff(Delta)
         l = calc_l(tab1['N Sides'][i], params['Davg'][i], n)
         Rs = calc_Rs(l, W, Delta, T_eff) 
         
         Z = calc_Z(f, params['Ls'][i], Rs, params['Cp'][i],
                    params['Cox'][i], params['Csub'][i], params['Rsub'][i])
         L = calc_L(Z, f)
         Qfactor= calc_QF(Z)
         Rvec1.append(Z.real)
         Fvec1.append(f)
         Lvec1.append(L)
         Qvec1.append(Qfactor)
     
    if(i==0):
        X=Lt_Square
    elif(i==1):
        X=Lt_Hexagonal
    else:
        X=Lt_Octogonal
        
    plt.figure()
    plt.ylabel("L(H)")
    plt.xlabel("F(Hz)")
    plt.title("L(f) %s" %tab1['Shape'][i])
    plt.xscale("log")
    plt.plot(Fvec1, Lvec1, label='Python')
    plt.plot(Fvec1, X, label='Ltspice')
    plt.legend(bbox_to_anchor=(1, 1)) 
    
    plotter_error(Lvec1, X, Fvec1, "F(Hz)", "Erro relativo de L - Python vs Ltspice %s" %tab1['Shape'][i])
    
    plt.figure()
    plt.ylabel("Qfactor")
    plt.xlabel("F(Hz)")
    plt.title("Qfactor(f) %s" %tab1['Shape'][i])
    plt.xscale("log")
    plt.xlim(1e8, 5e10)
    plt.ylim(-10,40)
    plt.plot(Fvec1, Qvec1)

##############################################################################
##############################################################################    

##############################################################################
################################## Parte 4 ###################################

W = 10e-6 # • Largura da fita de metal, w
Dout = 340e-6 # • Diâmetro externo, Dout
S = 5e-6 # • Espaço entre fitas, s
n = 3 # • Número de voltas, n



#Queremos obter novos W, Dout e N
L1 = 6.5e-9
f=2.4e9
x0 = (400e-6,2,20e-6)

boundd = (200e-6,1200e-6) #D_out
boundn = (1.5,8) #N
boundw = (10e-6,60e-6) #W
boundx = (boundd, boundn, boundw)

def get_L(x):
    Douto, No, Wo = x
    [Din, Davg, Ls, Rs, Cp, Cox, Csub, Rsub, Z, L, Qfactor, Fres] = parametros(f, 0, No , S, Wo, Douto)
    print("L=", L)
    return abs((L-L1))/L1

dmin=20e-6
def get_din_min(x):
    Douto, No, Wo = x
    return Douto - 2*No*Wo - 2*(No-1)*S - dmin
#é 2.4
fresmin = 2.4e10
def get_fres(x):
    Douto, No, Wo = x
    [Din, Davg, Ls, Rs, Cp, Cox, Csub, Rsub, Z, L, Qfactor, Fres] = parametros(f, 0, No , S, Wo, Douto)
  
    return Fres-fresmin

res=minimize(get_L, x0, method='SLSQP', bounds=boundx,
              constraints=({'type': 'ineq', 'fun':get_din_min},{'type': 'ineq', 'fun':get_fres}))
print("Res=", res)
results=res.x

p2=[]
p2.append(parametros(f, 0, results[1], S, results[2], results[0]))
params2 = pd.DataFrame(p2)
params2.columns = ['Din','Davg', 'Ls', 'Rs', 'Cp', 'Cox',
                  'Csub', 'Rsub', 'Z', 'L', 'Qfactor', 'Fres']

Rvec1=[]
Lvec1=[]
Qvec1=[]
Fvec1=[]
    
Rvec2=[]
Lvec2=[]
Qvec2=[]
Fvec2=[]
 
for f in np.logspace(3, 11, num=1000):
     
     Delta1 = calc_delta(f, miu)
     T_eff1 = calc_t_eff(Delta1)
     l1 = calc_l(tab1['N Sides'][0], params['Davg'][0], n)
     Rs1 = calc_Rs(l1, W, Delta1, T_eff1) 
     
     Z1 = calc_Z(f, params['Ls'][0], Rs1, params['Cp'][0],
                params['Cox'][0], params['Csub'][0], params['Rsub'][0])
     L1 = calc_L(Z1, f)
     Qfactor1= calc_QF(Z1)
     Rvec1.append(Z1.real)
     Fvec1.append(f)
     Lvec1.append(L1)
     Qvec1.append(Qfactor1)
    
     Delta = calc_delta(f, miu)
     T_eff = calc_t_eff(Delta)
     l = calc_l(tab1['N Sides'][0], params2['Davg'][0], results[1])
     Rs = calc_Rs(l, results[2], Delta, T_eff) 
     
     Z = calc_Z(f, params2['Ls'][0], Rs, params2['Cp'][0],
                params2['Cox'][0], params2['Csub'][0], params2['Rsub'][0])
     L = calc_L(Z, f)
     Qfactor= calc_QF(Z)
     Rvec2.append(Z.real)
     Fvec2.append(f)
     Lvec2.append(L)
     Qvec2.append(Qfactor)

plt.figure()
plt.ylabel("L(H)")
plt.xlabel("F(Hz)")
plt.title("L(f) %s" %tab1['Shape'][0])
plt.xscale("log")
plt.plot(Fvec1, Lvec1, label='Não optimizado')
plt.plot(Fvec2, Lvec2, label='Optimizado')
plt.legend(bbox_to_anchor=(1, 1)) 
plt.xlim(1e9,1e11)

# plotter_error(Lvec2, Lvec1, Fvec1, "F(Hz)", "Erro relativo de L - Python vs Ltspice %s" %tab1['Shape'][i])
    
plt.figure()
plt.ylabel("Qfactor")
plt.xlabel("F(Hz)")
plt.title("Qfactor(f) %s" %tab1['Shape'][0])
plt.xscale("log")
plt.xlim(1e8, 5e10)
plt.ylim(-10,40)
plt.plot(Fvec1, Qvec1, label='Não optimizado')
plt.plot(Fvec2, Qvec2, label='Optimizado')
plt.legend(bbox_to_anchor=(1, 1)) 
