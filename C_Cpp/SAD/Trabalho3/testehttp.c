/*
	Initialise Winsock
*/

#include<stdio.h>
#include<winsock2.h>

#pragma comment(lib,"ws2_32.lib") //Winsock Library

WSADATA wsa;
SOCKET s;
struct sockaddr_in server;

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

int main(int argc , char *argv[])
{
	char *message, server_reply[2000];
	int recv_size;

	init_wsock();
	create_socket();
    connect2server();

    //Send some data
	message = "POST /~sad/ HTTP/1.1\r\nHost: 193.136.120.133\r\nContent-Type: application/json\r\nContent-Length: 13\r\n\r\n{\"asd\":\"123\"}";
	if( send(s , message , strlen(message) , 0) < 0)
	{
		puts("Send failed");
		return 1;
	}
	puts("Data Send\n");

    //Receive a reply from the server
	if((recv_size = recv(s , server_reply , 2000 , 0)) == SOCKET_ERROR)
	{
		puts("recv failed");
	}
	
	puts("Reply received\n");
}