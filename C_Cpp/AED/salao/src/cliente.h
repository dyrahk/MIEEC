/*
 * cliente.h
 *
 *  Created on: 1 de Abr de 2015
 *      Author: fernanda
 */

#ifndef CLIENTE_H_
#define CLIENTE_H_

typedef struct _cliente * cliente;

/***********************************************
criaCliente - Criacao da instancia da estrutura associada a um cliente.
Parametros:
	nome - nome do cliente
	cabelo - tipo de cabelo
Retorno: apontador para a  instancia criada
Pre-condicoes: nome != NULL && (cabelo == 0 || cabelo == 1)
***********************************************/
cliente criaCliente(char * nome, int cabelo);

/***********************************************
destroiCliente - Liberta a memoria ocupada pela instancia da estrutura
associada ao cliente.
Parametros:
	d - cliente a destruir
Retorno:
Pre-condicoes: d != NULL
***********************************************/
void destroiCliente(cliente d);

/***********************************************
nomeCliente - consulta do nome do cliente
Parametros:
	d - cliente
Retorno: nome do cliente
Pre-condicoes: d != NULL
***********************************************/
char * nomeCliente(cliente d);

/***********************************************
cabeloCliente - consulta o tipo de cabelo do cliente
Parametros:
	d - cliente
Retorno: 0, cabelo curto ou 1, cabelo comprido
Pre-condicoes: d != NULL
***********************************************/
int cabeloCliente(cliente d);

#endif /* CLIENTE_H_ */
