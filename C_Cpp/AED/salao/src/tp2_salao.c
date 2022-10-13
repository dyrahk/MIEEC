/*
 ============================================================================
 Name        : tp2_salao.c
 Author      :
 Version     :
 Copyright   : Your copyright notice
 Description : Hello World in C, Ansi-style
 ============================================================================
 */
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>


#include "cliente.h"
#include "salao.h"
#include "fila.h"

#define CAP 15

void interpretador(salao p);

int main(void){
	salao fct = criaSalao(CAP);
	interpretador(fct);
	destroiSalao(fct);
	return 1;
}


void interpretador(salao p){

	char linha[25], cmd, nome[30], opc;
	int corte;
	cliente x;

	setvbuf(stdout, NULL,_IONBF, 0);

	while (1){

		fgets(linha,25,stdin);
		cmd = toupper(linha[0]);

		switch (cmd){
		case 'C': if(sscanf(linha,"%c %d %[^\n]",&opc, &corte,nome)!= 3){
						printf("Dados invalidos\n"); break;
					}
				  else{
					    adClienteSalao(p, nome, corte); break;
				  }
		case 'S':if(vazioSalao(p)!=0){
					x=delClienteSalao(p);
					printf("Cliente %s paga %d Euros\n", nomeCliente(x), eurosSalao(p, corte));
					break;
				 }
					else{
						printf("Nao existe cliente a cortar o cabelo\n");
						break;
					}

		case 'N': printf("Clientes em espera: %d \n", esperaSalao(p)); break;
		case 'X': if(vazioSalao(p)!=1){
					printf("Existem clientes no salao\n"); break;
				  }
				  else{
					  printf("Em caixa: %d Euros\n", caixaSalao(p));
					  return;
				  }

		default: printf("Dados invalidos!"); break;
		}



	}

}


