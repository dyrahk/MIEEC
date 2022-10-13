#include<stdio.h>
#include<stdlib.h>
#include<string.h>

#define PASSWORD "cenas123"

typedef struct stock{
	int counter, id, cat;
	char modelo[20];
	float preco;
	int desc;
	char sec;
} STOCK;

typedef struct fact{

	unsigned long int idfact;
	unsigned long int nif;
	char nome[20];
	int idprod;
	float val;
	int dia, mes, ano;

} FACT;

int lertransac(FACT w[]){
	FILE *fp;
	fp=fopen("transacoes.txt", "r");
	unsigned long int tidfact, tnif;
	char tnome[20], s[100];
	int tidprod;
	float tval;
	int tdia, tmes, tano, i=0, x;

	if(fp==NULL){
		puts("Erro na leitura do ficheiro!");
		exit(1);
	}

	while(x!=EOF){

		x=fscanf(fp, " %lu %lu %s %d %f %d-%d-%d", &tidfact , &tnif, tnome, &tidprod, &tval, &tdia, &tmes, &tano);

		if(x!=8){
			fgets(s, 80, fp);
			continue;
		}
		if(x==-1)
			continue;
		if(tidfact<1)
			continue;
		if(tnif<1)
			continue;
		if(strlen(tnome)==0)
			continue;
		if(tidprod<1 ||tidprod>15)
			continue;
		if(tval<0)
			continue;
		if( tdia<1 || tdia>31 || tmes<1 || tmes>12 || tano<1)
			continue;

		w[i].idfact=tidfact;
		w[i].nif=tnif;
		strcpy(w[i].nome, tnome);
		w[i].idprod=tidprod;
		w[i].val=tval;
		w[i].dia=tdia;
		w[i].mes=tmes;
		w[i].ano=tano;

		i++;

	}
	fclose(fp);
	return i;
}

void lerfich(STOCK v[]){

	FILE *fp;
	int x=0, i=0, j=0, k=0, armazenado[15]={0};
	int tid, tcat, tdesc;
	float tpreco;
	char tmodelo[20], tsec, s[80];

	fp=fopen("stock.txt", "r");

	if(fp==NULL){
		puts("Erro na leitura do ficheiro!");
		exit(1);
	}

	for(i=0; i<15; i++)
		v[i].counter=0;

	while(x!=EOF){

		x=fscanf(fp, " %d %d %s %f %d %c", &tid, &tcat, tmodelo, &tpreco, &tdesc, &tsec);

		if(x==-1)
			continue;
		if(x!=6 ){
			fgets(s, 80, fp);
			k++;
			continue;
		}
		if( tid<1 || tid>15 ){
			k++;
			continue;
		}
		if(tcat<1 || tcat>5){
			k++;
			continue;
		}
		if(strlen(tmodelo)==0){
			k++;
			continue;
		}
		if(tpreco<0){
			k++;
			continue;
		}
		if(tdesc<0 || tdesc>100){
			k++;
			continue;
		}
		if(tsec<'A' || tsec>'N' ){
			k++;
			continue;
		}
		if(armazenado[tid-1]==0){
			v[tid-1].id=tid;
			v[tid-1].cat=tcat;
			strcpy(v[tid-1].modelo, tmodelo);
			v[tid-1].preco=tpreco;
			v[tid-1].desc=tdesc;
			v[tid-1].sec=tsec;
			v[tid-1].counter=1;
			armazenado[tid-1]=1;
		}
		else
			v[tid-1].counter+=1;

		k++;
		j++;

	}

	printf("Foram lidos %d produtos com sucesso de um total de %d\n\n", j, k);
	fclose(fp);
}

void sair(STOCK v[], FACT w[]){

	FILE *fp1, *fp2;
	int i, j;

	fp1=fopen("stock.txt", "w");
	fp2=fopen("transacoes.txt", "w");

	if(fp1==NULL || fp2==NULL){
		puts("Erro na leitura do ficheiro!");
		exit(1);
	}
	for(i=0; i<15; i++){
		for(j=v[i].counter; j>0; j--){
			fprintf(fp1, "%2d %d %s\t %4.2f\t %2d %c\n", v[i].id, v[i].cat, v[i].modelo, v[i].preco, v[i].desc, v[i].sec);
		}
	}

	for(i=0; i<500 ; i++){
		if(w[i].idfact==0 || w[i].nif==0 || strlen(w[i].nome)==0 || w[i].idprod==0 || w[i].val==0 ||
		   w[i].dia==0 || w[i].mes==0 || w[i].ano==0){
			break;
		}
		fprintf(fp2," %lu %lu %s %d %.2f %d-%d-%d\n", w[i].idfact , w[i].nif, w[i].nome, w[i].idprod,
														w[i].val, w[i].dia, w[i].mes, w[i].ano);

	}
	fclose(fp1);
	fclose(fp2);

	exit(1);
}

char menu(){

	char opt;

	do{

        puts("********* YKEA  -  CLIENTE *********");
        puts("\t1 - Ver Catálogo");
        puts("\t2 - Efectuar Compra");
        puts("\t3 - Histórico de Compras");
        puts("\t4 - Promoções");
        puts("\t5 - Mapa da Loja");
        puts("\tA - Modo Administrador");
        puts("\tS - Sair");

        printf("O que pretende fazer?  ");
        scanf(" %c", &opt);

        switch(opt){

            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case 'a':
            case 'A':
            case 's':
            case 'S': return opt;

            default: puts("Erro! Tente outra vez!\n\n");
        }

	}while(1);
}

void catcompleto(STOCK v[]){

	int k;

	for(k=0; k<15; k++)
			printf(" %2d %d %s\t %4.2f\t %2d %c - %d artigos em stock\n\n", v[k].id, v[k].cat, v[k].modelo,
																		  v[k].preco, v[k].desc, v[k].sec, v[k].counter);

}

void catcateg (STOCK v[]){

	int tcat, i;

	printf(" Qual a categoria que quer ver? (1 a 5) ");

	while((scanf(" %d", &tcat))!=1 || tcat<1 || tcat>5)
	{
		printf( "Leitura inválida! Tente outra vez: ");
	}

	for(i=0; i<15; i++){
		if(v[i].cat==tcat)
			printf(" %2d %d %s\t %4.2f\t %2d %c - %d artigos em stock\n\n", v[i].id, v[i].cat, v[i].modelo,
																		  v[i].preco, v[i].desc, v[i].sec, v[i].counter);
	}

}

void catalogo (STOCK v[]){

	char opt;


	printf("1 - Ver catálogo completo\n");
	printf("2 - Ver catálogo por categoria\n");
	printf("S - Voltar atrás\n");

	printf("\nO que pretende fazer?  ");

	while( (scanf(" %c", &opt))!=1 )
		printf(" Introduziu um caratcér inválido! Tente outra vez:  ");

	if( opt=='1')
		catcompleto(v);
	if( opt=='2')
		catcateg(v);
	if(opt=='s' || opt=='S')
		return;
}

int maiornum(FACT w[], int proxpos){


	int i, maior=w[0].idfact;


	for(i=1; i<proxpos; i++){
		if(w[i-1].idfact<=w[i].idfact)
			maior=w[i].idfact;
	}

	return maior+1;
}

int factura(int qtd[], STOCK  v[], int proxpos, FACT w[]){

	char nome[20];
	unsigned long int NIF, proxid;
	int i, j, dia, mes, ano;





	if(proxpos>=500){

		printf("Pedimos desculpa, mas não é possivel realizar mais compras hoje!\n");
		return 0;
	}
	else{
		proxid=maiornum(w, proxpos);

		printf(" Introduza o seu nome: ");
		while(scanf(" %s", nome)!=1)
			printf("Leitura inválida\n Introduza o seu nome: ");

		printf(" Introduza o seu NIF: ");
		while(scanf(" %lu", &NIF)!=1)
			printf("Leitura inválida\n Introduza o seu NIF: ");

		printf(" Introduza o dia: ");
		while(scanf(" %d", &dia)!=1)
			printf("Leitura inválida\n Introduza o dia: ");

		printf(" Introduza o mes: ");
		while(scanf(" %d", &mes)!=1)
			printf("Leitura inválida\n Introduza o mes: ");

		printf(" Introduza o ano: ");
		while(scanf(" %d", &ano)!=1)
			printf("Leitura inválida\n Introduza o ano: ");
		for(i=0; i<15; i++){
			if(qtd[i]==0){
				continue;
			}

			for(j=(qtd[i]); j>=1; j--){
				w[proxpos].idfact=proxid;
				w[proxpos].nif=NIF;
				strcpy(w[proxpos].nome,nome);
				w[proxpos].idprod=v[i].id;
				w[proxpos].val=v[i].preco - (v[i].preco* ((float)v[i].desc/100));
				w[proxpos].dia=dia;
				w[proxpos].mes=mes;
				w[proxpos].ano=ano;
				printf("%lu %lu %s %d %.2f %d %d %d\n", w[proxpos].idfact, w[proxpos].nif, w[proxpos].nome, w[proxpos].idprod, w[proxpos].val,
												 w[proxpos].dia, w[proxpos].mes, w[proxpos].ano, proxpos);
				proxpos++;
			}

		}
		return 1;
	}
}

void compra(STOCK v[], char mapa[][7], int x, FACT w[]){

	int idcompra, i, j, k, tqtd;
	int qtd[15]={0};
	int tstock[15];
	char sn, fim;
	float precofinal=0;

	inicmapa(mapa);

	for(j=0; j<15; j++)
		tstock[j]=v[j].counter;
	do{
		do{

			printf("Introduza o id do produto a comprar (Para sair introduza 0) : ");

			while(scanf(" %d", &idcompra)!=1 || idcompra>15 || idcompra<0)
				printf("Leitura inválida! Introduza o id do produto a comprar (Para sair introduza 0) : ");

			if(idcompra==0)
				return;

			if(tstock[idcompra-1]==0){
				puts("\nPedimos desculpa mas de momento não temos o produto em stock\n");
			}
			else{
				printf(" Temos %d produtos dispóníveis. Quantos pretende? ", tstock[idcompra-1] );
				scanf(" %d", &tqtd);

				if(qtd[idcompra-1]>tstock[idcompra-1])
					puts("Não temos a quantidade pretendida em stock");
				else
					qtd[idcompra-1]+=tqtd;
			}

			tstock[idcompra-1]-=tqtd;
			precofinal+=(tqtd*(v[idcompra-1].preco -(v[idcompra-1].preco* ((float)v[idcompra-1].desc/100))));
			printf(" Deseja mais algum produto? (s/n) ");
			scanf(" %c", &sn);

		}while(sn=='s');

		for(i=0; i<15; i++){
			if(	qtd[i]!=0){
				printf(" %d %d %s %.2f %d %c Quantidade: %d\n", v[i].id, v[i].cat, v[i].modelo, v[i].preco, v[i].desc, v[i].sec, qtd[i]);
			}
		}

		printf("Preço final: %.2f\n", precofinal);
		printf("Finalizar compra? (s/n) ");
		scanf(" %c", &fim);
		fflush(stdin);
	}while(fim=='n');

	mapasub(qtd, mapa, v);
	mostrarmapa(mapa);
	factura(qtd, v, x, w);

	for(k=0; k<15; k++)
		v[k].counter-=qtd[k];
}

void histcompras(FACT w[]){
		int i;
		char nome[20];

		printf("Introduza o nome do cliente: ");
		scanf(" %s", nome);

		for(i=0; i<500; i++){
			if(strcmp(nome, w[i].nome)==0)
				printf(" %lu %lu %s %d %.2f %d-%d-%d\n\n", w[i].idfact , w[i].nif, w[i].nome, w[i].idprod, w[i].val, w[i].dia, w[i].mes, w[i].ano);
		}

}

void reporstock(STOCK v[]){

		FILE *fp;
		fp=fopen("reposicao.txt", "r");

		int x, tid, tcat;
		char tmodelo[20], s[80];
		float tpreco;
		int tdesc;
		char tsec;


		if(fp==NULL){
			puts("Erro na leitura do ficheiro!");
			exit(1);
		}

		while(x!=EOF){

		x=fscanf(fp, " %d %d %s %f %d %c", &tid, &tcat, tmodelo, &tpreco, &tdesc, &tsec);

		if(x==-1)
			continue;
		if(x!=6 ){
			fgets(s, 80, fp);
			continue;
		}
		if( tid<1 || tid>15  ){
			continue;
		}
		if((tcat<1 || tcat>5) || (tcat!=v[tid-1].cat) ){
			continue;
		}
		if(strcmp(tmodelo, v[tid-1].modelo)!=0){
			continue;
		}
		if(tpreco<0){
			continue;
		}
		if((tdesc<0 || tdesc>100) || (tdesc!=v[tid-1].desc) ){
			continue;
		}
		if((tsec<'A' || tsec>'N') || (tsec!=v[tid-1].sec)){
			continue;
		}

		v[tid-1].counter+=1;
		}

}

void alterarprod(STOCK v[]){

	char precox, descontox, fds;
	int idalt, descalt;
	float precoalt;

	do{
		printf("Introduza o id do produto com preço a alterar: ");
		while(scanf(" %d", &idalt)!=1 || idalt<1 || idalt>15){
			printf("Introduziu um ID inválido. Tente outra vez: ");
		}

		printf("Alterar preço? (s/n) ");
		while(scanf(" %c", &precox)!=1 || precox!='s' || precox!='n'){
			printf("Introduziu uma opção inválida. Tente outra vez: ");
		}

		if(precox=='s'){

			printf("Introduza o novo preço do produto: ");
			while(scanf(" %f", &precoalt)!=1 || precoalt<0){
				printf("Introduziu um preço inválido. Tente outra vez: ");
			}

			v[idalt-1].preco=precoalt;
		}

		printf("Alterar desconto? (s/n) ");
		while(scanf(" %c", &descontox)!=1 ){
			printf("Leitura inválida. Deseja alterar o desconto? (s/n) ");
		}

		if(descontox=='s'){
			printf("Introduza o novo desconto do produto: ");
			while(scanf(" %d", &descalt)!=1 || descalt<=0){
				printf("Introduziu um desconto inválido. Tente outra vez: ");
			}
			v[idalt-1].desc=descalt;
		}

		printf("Deseja alterar mais algum produto? (s/n) ");
		while(scanf(" %c", &fds)!=1){
			printf("Introduziu um valor inválido. Tente outra vez: ");
		}
		printf("\n%c\n", fds);
	}while(fds=='s');
}

void admin (STOCK v[]){

		char tentativa[30];

		printf("Introduza a password: ");
		scanf(" %s", tentativa);

		if(strcmp(tentativa, PASSWORD)!=0)
			return;

		char opt;

		puts("********* YKEA  -  ADMIN *********");
        puts("\t1 - Reposição de Stock");
        puts("\t2 - Alterar produto");
        puts("\t3 - Dados Estatísticos");
        puts("\tS - Sair do modo Administrador");
        puts("**********************************");

        printf(" O que pretende fazer?  ");

        while( (scanf(" %c", &opt))!=1 )
		printf(" Introduziu um caratcér inválido! Tente outra vez:  ");

        do{
			switch(opt){
					case '1': reporstock(v); break;
					case '2': alterarprod(v); break;
					//case '3':
					case 's':
					case 'S': return;
					default: printf("Opção inválida!\n");
			}
		}while(1);
}

void promotudo(STOCK v[]){

	int i;
	float precodesc;

	for(i=0; i<15; i++)
		if(v[i].desc==0)
			continue;
		else{
			precodesc=((v[i].preco -(v[i].preco*((float)v[i].desc/100))));
			printf(" %2d %d %s\t %4.2f\t %2d %.2f\n", v[i].id, v[i].cat, v[i].modelo,
												v[i].preco, v[i].desc, precodesc);
		}
}

void promomod(STOCK v[]){

	int idsearch, i;
	float precodesc;

	printf("Introduza o id do modelo que pretende: ");

	while(scanf(" %d", &idsearch)!=1){
		printf("Leitura inválida! Tente outra vez\n");
	}

	for(i=0; i<15; i++){
		if(idsearch==v[i].id){
			if(v[i].desc!=0){
				precodesc=((v[i].preco -(v[i].preco*((float)v[i].desc/100))));
				printf(" %2d %d %s\t %4.2f\t %2d %.2f\n", v[i].id, v[i].cat, v[i].modelo,
												v[i].preco, v[i].desc, precodesc);
			}
			else
				printf("Este modelo não tem qualquer desconto\n");
		}
	}


}

promos(STOCK v[]){

	char opt, fim;

	do{

		puts("1- Ver todas as promoções existentes");
		puts("2- Ver promoções por modelo (ID)");
		puts("S- Voltar ao menu anterior");
		printf("Escolha a opção: ");

		while(scanf(" %c", &opt)!=1){
			printf("Leitura inválida, tente outra vez: ");
		}

		switch(opt){
			case '1': promotudo(v);break;
			case '2': promomod(v); break;
			case 's': return;

			default: puts("Caractér inválido. Tente outra vez!"); continue;
		}

		printf(" Pretende ver mais promoções? (s/n): ");

		while(scanf(" %c", &fim)!=1){
			printf("Leitura inválida, tente outra vez: ");
		}
	}while(fim!='n');
}

inicmapa(char mapa[][7]){

	int i, j;
	char c='A';

	for(i=0; i<2; i++){
		for(j=0; j<7; j++){
			mapa[i][j]=c;
			c++;
		}
	}
}

mapasub( int qtd[], char mapa[][7], STOCK v[]){

	int i, j, k;
	char ch;
	for(i=0; i<15; i++){
		if(qtd[i]==0)
			continue;
		else{
			for(j=0; j<2; j++){
				for(k=0; k<7; k++){
					if(mapa[j][k]==v[i].sec)
							mapa[j][k]='*';
				}
			}
		}
	}
}

mostrarmapa(char mapa[][7]){

	int i, j;

	for(i=0; i<2; i++){
		for(j=0; j<7; j++){
			printf(" %c ", mapa[i][j]);
		}
		printf("\n");
	}
}

main(){

	STOCK v[15];
	char opt;
	int x;
	FACT w[500]={0};
	char mapa[2][7];

	x=lertransac(w);
	lerfich(v);

	do{
		inicmapa(mapa);
		opt=menu();

		switch(opt){
			case '1': catalogo(v); break;
			case '2': compra(v, mapa, x, w); break;
			case '3': histcompras(w); break;
			case '4': promos(v);
			case '5': mostrarmapa(mapa); break;
			case 'a':
			case 'A': admin(v); break;
			case 's':
			case 'S': sair(v, w); break;
		}
	}while(1);
}
