/*
 * ticket.h
 *
 *  Created on: 20 de Mar de 2015
 *      Author: bf.duarte
 */

#ifndef TICKET_H_
#define TICKET_H_

typedef struct _ticket *ticket;

ticket criaticket ( char *matricula, int h_e);
void destroiticket (ticket t);
int calculartempoticket ( ticket t);
void darhorasaidaticket ( ticket t, int h_s );
char* matriculaticket ( ticket t);


#endif /* TICKET_H_ */
