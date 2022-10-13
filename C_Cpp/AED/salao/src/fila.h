/*
 * fila.h
 */
#ifndef _H_FILA
#define _H_FILA
/* Tipo de dados: fila ---> comportamento “Primeiro a chegar, Primeiro a sair” */
typedef struct _fila * fila;

/* Prototipos das funcoes associadas a uma fila */

/***********************************************
criaFila - Criacao da instancia da estrutura associada a uma fila.
Parametros:
     cap - capacidade prevista da fila
Retorno: apontador para a  instancia criada
Pre-condicoes:
***********************************************/
fila criaFila(int cap);

/*destroiFila - Liberta a memoria ocupada pela instancia da estrutura associada à fila.
Parametros:
	f - fila a destruir
Retorno:
Pre-condicoes: f != NULL
***********************************************/
void destroiFila (fila f );

/***********************************************
destroiFilaEElems - Liberta a memoria ocupada pela instancia da estrutura associada à fila e os elementos.
Parametros:
	f - fila a destruir	destroi - função para destruição os elementos
Retorno:
Pre-condicoes: f != NULL
***********************************************/
void destroiFilaEElems(fila f, void (*destroi)(void *) );

/***********************************************
vaziaFila - Indica se a fila está vazia.
Parametros:
	f - fila
Retorno: 1- caso fila vazia; 0 - caso contrário
Pre-condicoes: f != NULL
***********************************************/
int vaziaFila(fila f);

/***********************************************
tamanhoFila - Consulta o numero de elementos na fila.
Parametros:
	f - fila
Retorno: numero de elementos na fila
Pre-condicoes: f != NULL
***********************************************/
int tamanhoFila(fila f);

/***********************************************
elementoFila – Retorna o elemento  à mais tempo na fila.
Parametros:
f – fila
Retorno: retorna o endereco do elemento
Pre-condicoes: f != NULL && tamanhoFila(f) != 0
***********************************************/
void * elementoFila(fila f);

/***********************************************
adicionaElemFila - Adiciona o elemento dado na fila.
Parametros:
	f – fila
	elem - endereco do elemento
Retorno:
Pre-condicoes: f != NULL
***********************************************/
void adicionaElemFila(fila f, void * elem);

/***********************************************
removeElemFila - Remove o elemento à mais tempo na fila.
Parametros:
	f – fila
Retorno: retorna o elemento
Pre-condicoes: f != NULL && vaziaFila(f) != 1
***********************************************/
void * removeElemFila(fila f);

#endif
