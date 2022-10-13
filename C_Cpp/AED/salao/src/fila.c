/*
 * filaVec.c
 */
#include <stdlib.h>

#include "fila.h"

/* Estrutura de dados: fila ---> comportamento “Primeiro a chegar, Primeiro a sair” */
struct _fila{
	void ** elems;
	int numElems;
	int capacidade;
};

/* Prototipos das funcoes associadas a uma fila */

/***********************************************
criaFila - Criacao da instancia da estrutura associada a uma fila.
Parametros:
     cap - capacidade prevista da fila
Retorno: apontador para a  instancia criada
Pre-condicoes:
***********************************************/
fila criaFila(int cap){
	fila f = (fila) malloc(sizeof(struct _fila));
	if (f == NULL) return NULL;
	f->elems = malloc(sizeof(void *)*cap);
	if (f->elems == NULL){
		free(f);
		return NULL;
	}
	f->capacidade = cap;
	f->numElems = 0;
	return f;
}


/*destroiFila - Liberta a memoria ocupada pela instancia da estrutura associada à fila.
Parametros:
	f - fila a destruir
Retorno:
Pre-condicoes: f != NULL
***********************************************/
void destroiFila (fila f ){
	free(f->elems);
	free(f);
}

/***********************************************
destroiFilaEElems - Liberta a memoria ocupada pela instancia da estrutura associada à fila e os elementos.
Parametros:
	f - fila a destruir	destroi - função para destruição os elementos
Retorno:
Pre-condicoes: f != NULL
***********************************************/
void destroiFilaEElems(fila f, void (*destroi)(void *) ){
	int i = 0;
	for(;i<f->numElems;i++)
		destroi(f->elems[i]);
	free(f->elems);
	free(f);
}

/***********************************************
vaziaFila - Indica se a fila está vazia.
Parametros:
	f - fila
Retorno: 1- caso fila vazia; 0 - caso contrário
Pre-condicoes: f != NULL
***********************************************/
int vaziaFila(fila f){
	if (f->numElems == 0)
		return 1;
	return 0;
}

/***********************************************
tamanhoFila - Consulta o numero de elementos na fila.
Parametros:
	f - fila
Retorno: numero de elementos na fila
Pre-condicoes: f != NULL
***********************************************/
int tamanhoFila(fila f){
	return f->numElems;
}

/***********************************************
elementoFila – Retorna o elemento  à mais tempo na fila.
Parametros:
f – fila
Retorno: retorna o endereco do elemento
Pre-condicoes: f != NULL && tamanhoFila(f) != 0
***********************************************/
void * elementoFila(fila f){
	return f->elems[0];
}
/* Aumenta o vector da fila */
void incFila (fila f){
	int i = 0;
	void ** aux = malloc(sizeof(void * ) * f->capacidade*2); // duplica
	for (;i<f->numElems;i++)
		aux[i] = f->elems[i];
	free(f->elems);
	f->elems = aux;
	f->capacidade = f->capacidade * 2;
}

/***********************************************
adicionaElemFila - Adiciona o elemento dado na fila.
Parametros:
	f – fila
	elem - endereco do elemento
Retorno:
Pre-condicoes: f != NULL
***********************************************/
void adicionaElemFila(fila f, void * elem){
	if (f->capacidade == f->numElems)
		incFila(f);
	f->elems[f->numElems++] = elem;
}

/***********************************************
removeElemFila - Remove o elemento à mais tempo na fila.
Parametros:
	f – fila
Retorno: retorna o elemento
Pre-condicoes: f != NULL && vaziaFila(f) != 1
***********************************************/
void * removeElemFila(fila f){
	int i = 0;
	void * aux = f->elems[0];
	f->numElems--;
	for(;i<f->numElems;i++)
		f->elems[i] = f->elems[i+1];
	return aux;
}

