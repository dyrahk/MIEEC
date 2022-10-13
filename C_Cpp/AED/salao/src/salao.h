/*
 * salao.h
 *
 *  Created on: 1 de Abr de 2015
 *      Author: fernanda
 */

#ifndef SALAO_H_
#define SALAO_H_

typedef struct _salao * salao;

/***********************************************
criaSalao - Criacao da instancia da estrutura associada a um salao.
Parametros:
	cap - capacidade prevista do salao
Retorno: apontador para a  instancia criada
Pre-condicoes:
***********************************************/
salao criaSalao(int cap);

/***********************************************
destroiSalao - Liberta a memoria ocupada pela instancia da estrutura
associada ao salao.
Parametros:
	c - salao a destruir
Retorno:
Pre-condicoes: c != NULL
***********************************************/
void destroiSalao (salao c);

/***********************************************
esperaSalao - consulta o numero de clientes em espera no salao.
Parametros:
	c - salao
Retorno:numero de clientes em espera
Pre-condicoes: c != NULL
***********************************************/
int esperaSalao(salao c);

/***********************************************
vazioSalao - indica se existem clientes no salao.
Parametros:
	c - salao
Retorno: 1, caso nao exista clientes, e 0, caso contrario
Pre-condicoes: c != NULL
***********************************************/
int vazioSalao(salao c);

/***********************************************
caixaSalao - consulta o dinheiro na caixa do salao.
Parametros:
	c - salao
Retorno: euros em caixa
Pre-condicoes: c != NULL
***********************************************/
int caixaSalao(salao c);

/***********************************************
adClienteSalao - adiciona um cliente ao salao para um corte de cabelo curto ou comprido.
Caso não esteja ninguem a ser atendido, esse cliente passa a ser atendido. Caso contrário fica na sala de espera.
Parametros:
	c - salao
	nome - nome do cliente
	corte - tipo de corte
Retorno:
Pre-condicoes: c != NULL && nome != NULL && (corte == 0 || corte == 1)
***********************************************/
void adClienteSalao(salao c, char * nome, int corte);

/***********************************************
delClienteSalao - remove o cliente que esta a ser atendido no salao, caso exista.
Neste momento o cliente com mais tempo na sala de espera passa a ser atendido
Parametros:
	c - salao
Retorno: o cliente que estava a ser atendido ou NULL, caso não exista
Pre-condicoes: c != NULL
***********************************************/
cliente delClienteSalao(salao c);

/***********************************************
eurosSalao - consulta o valor do corte de cabelo no salao. O corte de cabelo curto (corte = 0)
tem um custo de 10 Euros, enquanto que o cabelo comprido (corte = 1) será de 15 Euros
Parametros:
	c - salao
	corte - tipo de cabelo
Retorno: valor do corte
Pre-condicoes: c != NULL && (corte == 0 || corte == 1)
***********************************************/
int eurosSalao(salao c, int corte);

#endif /* SALAO_H_ */
