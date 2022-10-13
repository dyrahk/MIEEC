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