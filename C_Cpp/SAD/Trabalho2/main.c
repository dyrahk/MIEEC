#include <p24fxxxx.h>
#include <stdio.h>
#include <string.h>

// Configuration Bits
#ifdef __PIC24FJ64GA004__ //Defined by MPLAB when using 24FJ64GA004 device
_CONFIG1( JTAGEN_OFF & GCP_OFF & GWRP_OFF & COE_OFF & FWDTEN_OFF & ICS_PGx1 & IOL1WAY_ON) 
_CONFIG2( FCKSM_CSDCMD & OSCIOFNC_OFF & POSCMOD_HS & FNOSC_PRI & I2C1SEL_SEC)
#else
_CONFIG1( JTAGEN_OFF & GCP_OFF & GWRP_OFF & COE_OFF & FWDTEN_OFF & ICS_PGx2) 
_CONFIG2( FCKSM_CSDCMD & OSCIOFNC_OFF & POSCMOD_HS & FNOSC_PRI)
#endif

#define AINPUTS 0xffd2 //1111 1111 1101 0010
#define TEMP 0
#define LEFT 2
#define RIGHT 3
#define POT 5
#define PASS "SAD2020\0"



void delay(int d){
	int i = 0;
	int j = 0;
	for (i = 0; i<d ; i++){ 
		for (j = 0; j < 300 ; j ++){}
	}
}

void initUART(){
	
	U2BRG = 0x0C; //Set Baudrate 12
	U2STA = 0;
	U2MODE = 0x8000; //Enable Uart for 8-bit data
					 //no parity, 1 Stop bit
	U2STAbits.UTXEN = 1;
		
}	

void transmitChar(char c){

		while(U2STAbits.UTXBF);
		
		U2TXREG=c;
}	

void transmitString(char s[]){
	int i=0;
		
	while(s[i]!='\0') transmitChar(s[i++]);
	transmitChar('\n');						
}
	

char receiveChar(){
		
	char ch;
	while(!U2STAbits.URXDA);	
	ch=U2RXREG;
	IFS1bits.U2RXIF=0;
	return ch;
}

	
char *receiveString(char *s){
		
		//int i=0;
	char ch;
	do{
        ch=receiveChar();
            //s[i++]=ch;
        *s = ch;
        s++;
    }while(ch!='\r' && ch!='\n');
	s--;
	*s = '\0';
    return s;
	//while((ch=receiveChar())!='\0')	
}

int password (){
	
	char pass[20] = {NULL};
		
	transmitString("Introduza a password: ");
	receiveString(pass);

	if(!strcmp(pass,PASS)){
		transmitString("Password correta. Bem vindo\r\n\n");
		return 1;
	}
	else{
		transmitString("Password incorreta! Tente outra vez.\r\n\n");
		return 0;
	}
}

void init_ADC(int amask){

	AD1PCFG = amask; 	
	AD1CON1 = 0x0000; 
	AD1CSSL = 0;      

	AD1CON2 = 0x0000; 
	AD1CON3 = 0x0002; // Manual Sample

	AD1CON1bits.ADON = 1;
}

int read_ADC (int ch){

	AD1CHS = ch;
	AD1CON1bits.SAMP = 1;
	delay(10);
	AD1CON1bits.SAMP = 0;
	while (!AD1CON1bits.DONE);
	return ADC1BUF0;
				
}

void inttoStr(int x, char *string){

	sprintf(string, "%d\r\n", x);
}



int main(void)
{

	initUART();
	
	TRISDbits.TRISD6 = 1;
	TRISDbits.TRISD13 = 1;
	TRISAbits.TRISA0 = 0;
	TRISAbits.TRISA1 = 0;
	TRISAbits.TRISA2 = 0;

	int temperatura;
	int LDR_left;
	int LDR_right;
	int potenciometro;
	char option;
 	char nome[5];
	int i=0;
	int bronze = 0;
	int moving = 0;
	int vent = 0;
    int threshold=50;

	init_ADC(AINPUTS);

	while (1) {
		if(!PORTDbits.RD13){

			if(password()){

				while(PORTDbits.RD13){

					delay(1);
					temperatura = read_ADC(TEMP); //temperatura
					LDR_left = read_ADC(LEFT); //LDR esquerdo
					LDR_right = read_ADC(RIGHT);	//LDR direito
					potenciometro = read_ADC(POT);	//potenciometro
			
					
				
					if( !PORTDbits.RD7){
						delay(2000);
						if(bronze == 0){
							bronze = 1;
							moving=0;	
							transmitString("Modo Bronze Ativado\r\n\n");
							PORTAbits.RA0 = 1;
							for( i = 0 ; i < 20000 ; i++){};
							PORTAbits.RA0 = 0;
							for( i = 0 ; i < 20000 ; i++){};
							
						}
						else if(bronze == 1){
							bronze = 0;
							moving=0;
							transmitString("Modo Sombra Ativado\r\n\n"); 
							PORTAbits.RA0 = 1;
							for( i = 0 ; i < 20000 ; i++){};
							PORTAbits.RA0 = 0;
							for( i = 0 ; i < 20000 ; i++){};
						}
					}  
			
					if(bronze == 0){
						if ( LDR_left + threshold < LDR_right && moving==0){
							transmitString("A rodar para a direita.\n\r");
							moving=1;
							PORTAbits.RA1 = 1;
							Nop();
							PORTAbits.RA2 = 0;
						}
						if (LDR_right + threshold < LDR_left && moving==0){
							transmitString("A rodar para a esquerda.\n\r");
							moving=1;
							PORTAbits.RA1 = 0;
							Nop();
							PORTAbits.RA2 = 1;
						}
						if ((LDR_left - threshold <= LDR_right && LDR_left + threshold >= LDR_right)  && moving==1){
							transmitString("Parado.\n\r");
							moving=0;
							PORTAbits.RA1 = 0;
							Nop();
							PORTAbits.RA2 = 0;
						}
					}
					if (bronze == 1) {
					
						if (LDR_left  > LDR_right + threshold && moving==0){
							transmitString("A rodar para a direita.\n\r");
							moving=1;
							PORTAbits.RA1 = 1;
							Nop();
							PORTAbits.RA2 = 0;
						}
						if (LDR_right  > LDR_left + threshold && moving==0){
							transmitString("A rodar para a esquerda.\n\r");
							moving=1;
							PORTAbits.RA1 = 0;
							Nop();
							PORTAbits.RA2 = 1;
						}
						if ((LDR_left - threshold <= LDR_right && LDR_left + threshold >= LDR_right) && moving==1){
							transmitString("Parado.\n\r");
							moving=0;
							PORTAbits.RA1 = 0;
							Nop();
							PORTAbits.RA2 = 0;
						}
					}
					
					if(temperatura >= 500 && vent==0){
					   transmitString("MUITO CALOR.\n\r Ventoinha ativada\r\n");
					   vent=1;
					}
					if(temperatura < 500 && vent==1){
					   transmitString("Ventoinha desativada\r\n");
					   vent=0;
					}
						   
					if(U2STAbits.URXDA)	option=receiveChar();
					
					if(option == 't' || option == 'T'){
						transmitString("\n\r A temperatura é: "); 
						inttoStr(temperatura, nome);
						transmitString(nome);
					}
	
					if(option == 'l' || option == 'L'){
						transmitString("\n\r O valor do LDR direito é: ");
						inttoStr(LDR_right, nome);
						transmitString(nome);
						transmitString("\n\r O valor do LDR esquerdo é: "); 
						inttoStr(LDR_left, nome);
						transmitString(nome);
					}
	
					if(option == 'p' || option == 'P'){
						transmitString("O valor do potenciometro é: ");
						inttoStr(potenciometro, nome);
						transmitString(nome);
					} 					
					 option='0';
				}						
			}				
		}
	}
}
