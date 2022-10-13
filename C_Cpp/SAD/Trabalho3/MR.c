#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <time.h>
#include<winsock2.h>

#pragma comment(lib,"ws2_32.lib") //Winsock Library

// Variaveis e estruturas
    HANDLE hSerial;
    DCB dcbSerialParams = {0};
    COMMTIMEOUTS timeouts = {0};

    WSADATA wsa;
    SOCKET s;
    struct sockaddr_in server;

    char message[300], server_reply[2000];
	int recv_size;
    char message_buffer[100]; // guardar a mensagem recebida pelo AQC

void init_wsock(){
    printf("\nInitialising Winsock...");
	if (WSAStartup(MAKEWORD(2,2),&wsa) != 0) //WSAStartup function is used to start or initialise winsock library
	{                                       
		printf("Failed. Error Code : %d\n",WSAGetLastError());
		exit(1);
	}
	
	printf("Initialised.\n");
}

void create_socket(){
    if((s = socket(AF_INET , SOCK_STREAM , 0 )) == INVALID_SOCKET) //socket() creates a socket and returns a socket descriptor
	{
		printf("Could not create socket : %d" , WSAGetLastError());
	}

	printf("Socket created.\n");
}

void connect2server(){
    server.sin_addr.s_addr = inet_addr("193.136.120.133");
	server.sin_family = AF_INET;
	server.sin_port = htons( 80 );

	//Connect to remote server
	if (connect(s , (struct sockaddr *)&server , sizeof(server)) < 0)
	{
		puts("connect error");
		exit(1);
	}
	
	puts("Connected");
}

void send2server(){
    //Send some data
	strcpy(message,"POST /~sad/ HTTP/1.1\r\nHost: 193.136.120.133\r\nContent-Type: application/json\r\nContent-Length: 100\r\n\r\n");
    strcat(message,message_buffer);

    if( send(s , message , strlen(message) , 0) < 0)
	{
		puts("Send failed");
		exit(1);
	}
    strcpy(message,"");
    //Receive a reply from the server
	if((recv_size = recv(s , server_reply , 2000 , 0)) == SOCKET_ERROR)
	{
		printf("recv failed");
	}
}

void close_wsock(){
    closesocket(s);
    WSACleanup();
}

void start_serial(){
        // Iniciar a comunicação série na porta COM_X
    fprintf(stderr, "Opening serial port...");
    hSerial = CreateFile(
                "\\\\.\\COM2", GENERIC_READ|GENERIC_WRITE, 0, NULL,
                OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );
    if (hSerial == INVALID_HANDLE_VALUE)
    {
            fprintf(stderr, "Error\n");
            exit(1);
    }
    else fprintf(stderr, "OK\n");
     
    // Set device parameters (9600 baud, 1 start bit,
    // 1 stop bit, no parity)
    dcbSerialParams.DCBlength = sizeof(dcbSerialParams);
    if (GetCommState(hSerial, &dcbSerialParams) == 0)
    {
        fprintf(stderr, "Error getting device state\n");
        CloseHandle(hSerial);
       exit(1);
    }
     
    dcbSerialParams.BaudRate = CBR_9600;
    dcbSerialParams.ByteSize = 8;
    dcbSerialParams.StopBits = ONESTOPBIT;
    dcbSerialParams.Parity = NOPARITY;
    if(SetCommState(hSerial, &dcbSerialParams) == 0)
    {
        fprintf(stderr, "Error setting device parameters\n");
        CloseHandle(hSerial);
        exit(1);
    }

    // Set COM port timeout settings
    timeouts.ReadIntervalTimeout = 50;
    timeouts.ReadTotalTimeoutConstant = 50;
    timeouts.ReadTotalTimeoutMultiplier = 10;
    timeouts.WriteTotalTimeoutConstant = 50;
    timeouts.WriteTotalTimeoutMultiplier = 10;
    if(SetCommTimeouts(hSerial, &timeouts) == 0)
    {
        fprintf(stderr, "Error setting timeouts\n");
        CloseHandle(hSerial);
        exit(1);
    }
}

void close_serial(){
    // Close serial port
    fprintf(stderr, "Closing serial port...");
    if (CloseHandle(hSerial) == 0)
    {
        fprintf(stderr, "Error\n");
        exit(1);
    }
    fprintf(stderr, "OK\n");
}

void write_byte(char *byte2send){
    DWORD bytes_written, total_bytes_written = 0;
    if(!WriteFile(hSerial, byte2send, 1, &bytes_written, NULL))
    {
        fprintf(stderr, "Error\n");
        CloseHandle(hSerial);
        exit(1);
    }   
    //fprintf(stdout, "%c\n", byte2send);
}

void menu(){
    printf("\n\n                MENU\n\n");
    printf("v - Verificar valores atuais\n");
    printf("c - Alterar valores limite de situacao de risco\n");
    printf("x - Encerrar programa\n\n");
    printf("Escolha uma opcao:");
}

int main(){

    int read=0; //para ver se acabou de ler
    int exit=0;
    int i=0;
    int j=0;
    int t,h,v;
    char bytes_to_send[1];
    char c,opt;
    char xml_message[200];
    char buff[10];
    FILE *fp; //fp- file pointer
    time_t tempo;
    
    printf("%s\n", ctime(&tempo));
     
    DWORD bytes_read;
    
    init_wsock();
	create_socket();
    connect2server();
    start_serial();
    
    fp = fopen("AQC.xml", "a"); //abrir o ficheiro XML em modo append
    /* fopen() return NULL if last operation was unsuccessful */
    if(fp == NULL)
    {
        /* File not created hence exit */
        printf("Unable to create file.\n");
        return 1;
    }


    strcpy(message_buffer, "");     
    printf("Para aceder ao menu, pressione uma tecla\n");
    do{
         
        
        if(kbhit()){
            getch();
            menu();
            bytes_to_send[0]=getche();
            printf("\n\n");
            switch (bytes_to_send[0])
            {
            case 'v':
                write_byte(bytes_to_send);             
                break;
            
            case 'c':
                write_byte(bytes_to_send);
                printf("Introduza o novo limite de temperatura (0-100C):");
                scanf("%d", &t);
                if(t<0 || t>100){
                    printf("Valor invalido!!!\n");
                    break;
                }
                printf("Introduza o novo limite de humidade(0-100%%):");
                scanf("%d", &h);
                if(h<0 || h>100){
                    printf("Valor invalido!!!\n");
                    break;
                }
                printf("Introduza o novo limite de velocidade do vento (0-105 Km/h):");
                scanf("%d", &v);
                if(v<0 || v>105){
                    printf("Valor invalido!!!\n");
                    break;
                }
                bytes_to_send[0]=(char)t;
                write_byte(bytes_to_send);
                 bytes_to_send[0]=(char)h;
                write_byte(bytes_to_send);
                bytes_to_send[0]=(char)v;
                write_byte(bytes_to_send);
                break;
            case 'x':
                exit=1;
                break;
            default:
                break;
            }
        }
        else{ 
            
            read=0;
 
            if(read==0){
                
                ReadFile(hSerial, &c, 1, &bytes_read, NULL);
                if (bytes_read != 1) continue;

                
                if (c != '\n' && c != '\r') 
                    strncat(message_buffer, &c, 1);
                else{ 
                    fprintf(stdout, "%s\r\n", message_buffer);
                    
                    if(message_buffer[0]=='{'){
                        
                        //Envio para o Server
                        init_wsock();
                        create_socket();
                        connect2server();
                        send2server();
                        close_wsock();
                        
                        //XML
                        tempo=time(NULL);
                        if(message_buffer[2]=='E'){
                        strcpy(xml_message,"<Msg2>\n\t<Hora>\n\t\t");
                        strcat(xml_message,ctime(&tempo));
                        strcat(xml_message,"\t<\\Hora>\n\t<Evento>\n\t\tRisco Elevado de Incendio\n\t<\\Evento>\n<\\Msg2>\n\n");
                        fputs(xml_message, fp);
                        strcpy(xml_message, "");
                        }
                        if(message_buffer[2]=='T'){
                            tempo=time(NULL);
                            strcpy(xml_message,"<Msg1>\n\t<Hora>\n\t\t");
                            strcat(xml_message,ctime(&tempo));
                            strcat(xml_message,"\t<\\Hora>\n\t<Valores>\n\t\t<Temperatura>");
                            i=3;
                            while(message_buffer[i++]!=':'){}
                            for(j=0;message_buffer[++i]!='"';j++){
                                buff[j]=message_buffer[i];
                            }
                            buff[j]='\0';
                            strcat(xml_message,buff);
                            strcat(xml_message,"<\\Temperatura>\n\t\t<Humidade>");
                            while(message_buffer[i++]!=':'){}
                            for(j=0;message_buffer[++i]!='"';j++){
                                buff[j]=message_buffer[i];
                            }
                            buff[j]='\0';
                            strcat(xml_message,buff);
                            strcat(xml_message,"<\\Humidade>\n\t\t<Velocidade Vento>");
                            while(message_buffer[i++]!=':'){}
                            for(j=0;message_buffer[++i]!='"';j++){
                                buff[j]=message_buffer[i];
                            }
                            buff[j]='\0';
                            strcat(xml_message,buff);
                            strcat(xml_message,"<\\Velocidade Vento>\n\t<\\Valores>\n<\\Msg1>\n\n");
                            fputs(xml_message, fp);
                            strcpy(xml_message, "");
                            strcpy(buff, "");
                            i=0;
                        }

                    }    
                    
                    strcpy(message_buffer, ""); //limpar o message_buffer     

                    read=1;
                }
            }
            
        }
    }while(exit==0); 
    
    close_serial();
    // exit normally
    return 0;
}