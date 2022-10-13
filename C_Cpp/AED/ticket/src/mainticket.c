/*
 * mainticket.c
 *
 *  Created on: 20 de Mar de 2015
 *      Author: bf.duarte
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "ticket.h"


int main () {

char matricula[10];
int estadia, entradah, entradam, entradaminutos, saidaminutos, saidah, saidam;
ticket t;

setvbuf(stdout,NULL,_IONBF,0);



	printf("Matricula: ");
	scanf("%s", matricula);
	printf("Entrada (hh:mm): ");
	scanf("%d:%d", &entradah, &entradam);
	entradaminutos= 60*entradah + entradam;

	t=criaticket(matricula,entradaminutos);


	printf("Saida (hh:mm): ");
	scanf("%d:%d", &saidah, &saidam);
	saidaminutos= 60*saidah+saidam;

	darhorasaidaticket( t, saidaminutos);
	estadia=calculartempoticket(t);



	printf("O carro com matricula %s teve uma estadia de %d horas\n",matriculaticket(t) ,estadia);

	destroiticket(t);
	return 0;
}
