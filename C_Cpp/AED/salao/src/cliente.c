/*
 * doente.c
 *
 *  Created on: 1 de Abr de 2015
 *      Author: fernanda
 */

#include <stdlib.h>
#include <string.h>

#include "cliente.h"

struct _cliente{
	char * nome;
	int cabelo;
};

/***********************************************
criaCliente - Criacao da instancia da estrutura associada a um cliente.
Parametros:
	nome - nome do cliente
	cabelo - tipo de cabelo
Retorno: apontador para a  instancia criada
Pre-condicoes: nome != NULL && (cabelo == 0 || cabelo == 1)
***********************************************/
cliente criaCliente(char * nome, int cabelo){
	cliente d= (cliente) malloc(sizeof(struct _cliente));
	if (d == NULL) return NULL;
	d->nome = (char *) malloc(sizeof(char)* (strlen(nome)+1));
	if (d->nome == NULL){ free(d); return NULL;}
	strcpy(d->nome,nome);
	d->cabelo = cabelo;
	return d;
}

/***********************************************
destroiCliente - Liberta a memoria ocupada pela instancia da estrutura
associada ao cliente.
Parametros:
	d - cliente a destruir
Retorno:
Pre-condicoes: d != NULL
***********************************************/
void destroiCliente(cliente d){
	free(d->nome);
	free(d);
}
/***********************************************
nomeCliente - consulta do nome do cliente
Parametros:
	d - cliente
Retorno: nome do cliente
Pre-condicoes: d != NULL
***********************************************/
char * nomeCliente(cliente d){
	return d->nome;
}

/***********************************************
cabeloCliente - consulta o tipo de cabelo do cliente
Parametros:
	d - cliente
Retorno: 0, cabelo curto ou 1, cabelo comprido
Pre-condicoes: d != NULL
***********************************************/
int cabeloCliente(cliente d){
	return d->cabelo;
}

