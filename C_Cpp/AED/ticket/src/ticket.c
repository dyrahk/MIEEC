/*
 ============================================================================
 Name        : ticket.c
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : Hello World in C, Ansi-style
 ============================================================================
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "ticket.h"


struct _ticket {
	char matricula[10];
	int hora_entrada;
	int hora_saida;
	} ;


ticket criaticket( char *matricula, int h_e ) {

	ticket t;

	t = (ticket) malloc (sizeof(struct _ticket));

	if (t == NULL){
		return NULL;
	}

	strcpy (t->matricula,matricula);
	t->hora_entrada = h_e;
	t->hora_saida = 0;

	return t;
}

void destroiticket(ticket t){

	free (t);
}

void darhorasaidaticket ( ticket t, int h_s){

	t->hora_saida = h_s;
}

int calculartempoticket (ticket t){

	int total, entrada, saida, estadia;

	entrada=t->hora_entrada;
	saida = t->hora_saida;

	total = (saida - entrada);
	if((total%60)==0){
		estadia=total/60;
	}else{ estadia=(total/60)+1;}

	return estadia;
}

char* matriculaticket (ticket t){

	return t->matricula;
}

