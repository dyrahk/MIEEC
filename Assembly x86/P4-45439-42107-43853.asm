; multi-segment executable file template.

data segment
    ; add your data here!
    
;********************************************************************
;                      Variaveis relativas ao Menu                  ;
;********************************************************************
    loading db "Loading...",0dH,0aH,0H  
    menu_p db "*****  Main Menu  *****", 0dH, 0aH, 0H
    op_l db   "L - List papers available to reference", 0dH, 0aH, 0H
    op_i db   "I - Insert a new paper reference      ", 0dH, 0aH, 0H
    op_p db   "P - Process new article               ", 0dH, 0aH, 0H
    op_d db   "D - Display statistics                ", 0dH, 0aH, 0H
    op_a db   "A - About                             ", 0dH, 0aH, 0H
    op_e db   "E - Exit                              ", 0dH, 0aH, 0H 
    
; *******************************************************************
; *******************************************************************
; *******************************************************************    
  
    filename db "C:\emu8086\vdrive\C\statistics.txt", 0   

;********************************************************************
;                      Variaveis relativas ao Menu                  ;
;********************************************************************

    path db "Path:",0dH,0aH,0h

; *******************************************************************
; *******************************************************************
; *******************************************************************    
    
    
;********************************************************************
;                      Variaveis relativas ao About                 ;
;********************************************************************
     
    about_str db "******   About   ******", 0dH, 0aH, 0H 
    elem1 db     "Carlos Morgado  - 42107", 0dH, 0aH, 0H
    elem2 db     "Miguel Carreiro - 45439", 0dH, 0aH, 0H
    elem3 db     "Rafael Oliveira - 43853", 0dH, 0aH, 0H

; *******************************************************************
; *******************************************************************
; *******************************************************************    
    

;********************************************************************
;                    Variaveis relativas aos botoes                 ;
;********************************************************************
   
    botao1 db "<<Main>>", 0dH, 0aH, 0H
    botao2 db "<<Next>>", 0dH, 0aH, 0H

; *******************************************************************
; *******************************************************************
; *******************************************************************

   end_byte db ? 
   intervalo db ? ;intervalo entre string
    
   str_n   dw  5 dup(0)
   end_1   dw  ?    
   
   database db 2600 dup(0) ; 1 p identificador; 25/25 strings; 1 stats
   ref_buffer db 50 dup(0)
   buffer db 60 dup(0)
   vezes_lidas_buf dw 0
   buffer_new db 54 dup(0) 
   list_count db 0
   pos_maior_num dw 0
   num_ref dw 0  ;num. de vezes do paper
   posi_X dw 35
   pos_y db 0
   pos_x db 0
   pos_cx dw 50
   buffer_stat db 5 dup(0)
   percentage dw 0 
   no_refs db "Sem referencias disponiveis", 0dH, 0aH, 0H
   test_file db "C:\test.txt"
   handle_exit dw ?
   handle_test dw 0
   paper db  "Title of paper:", 0dH, 0aH, 0H
   author db "Name of author(s):", 0dH, 0aH, 0H
   file_not_found db "File not found!",0dH,0aH,0H
   nl_file db 0dH,0aH
   bib_app db "Bibliography appendix:",0dH,0aH,0H
   ref_not_found db "Reference not found",0dh,0ah,0h
   click_to_main db "Click to main menu",0dh,0ah,0h 
   file_processed db "File processed",0dh,0ah,0h
   total_refs dw 0
   ; *******************************************************************
;                    Variaveis das strings relativas ao display
; *******************************************************************

    display1 db "Total usage: "
    display2 db "Results: "
    display3 db "Paper    Num.    Percentage "
    display4 db "$"

; *******************************************************************
    handle dw ?
  
ends

stack segment
    dw   128  dup(0)
ends

code segment
start:
; set segment registers:
    mov ax, data
    mov ds, ax
    mov es, ax


    call modo_video ;inicializacao do modo grafico
    call inic_rato  ;inicializacao do rato
    
    mov cx, 13
    mov dx, 0A0Eh       
    lea bp, loading
    call escreve_str 
    
    
    call SetPaperNum
    
    call modo_video
    
    
    ;ler o ficheiro aqui
    
    ;abre_ficheiro:
    ;lea dx, test_file
    ;mov al, 0
    ;call fopen 
    ;mov handler, ax
    
    
    
    
    
    ;fecha_ficheiro:
    ;mov bx, handle_test
    ;call fopen
    
    
    ;fim ficheiro
    nao_sair:
    
    call menu ; menu principal 
   
    call op_menu; rotina que devolve a opcao seleccionada
    
    
    cmp ax, 1
    je op_list
    cmp ax,2
    je op_insert
    cmp ax,3
    je op_process
    cmp ax,4
    je op_display
    cmp ax, 5
    je op_about
    
    jmp sair
    
    op_list:   xor cx,cx
               mov cl, list_count
               lea si, database
               lea di, buffer
               call list
               jmp nao_sair
    
    op_insert: xor ah, ah
               mov al, list_count
               lea di, database
               call insert
               mov list_count, al
               jmp nao_sair
              
    op_process:lea si, database
               lea di, ref_buffer
               lea dx, buffer
               call process
               mov total_refs, ax
               jmp nao_sair
               
    op_display:lea si, ref_buffer
               lea di, buffer_new
               call display
               jmp nao_sair          
    
    op_about:  call about
               jmp nao_sair
    
    sair:
    
    call exit
    
    mov ax, 4c00h ; exit to operating system.
    int 21h    
ends


; ************************************************
;           Funcoes Relativas ao Grafico
; ************************************************
    modo_video proc
        push ax
        xor ah,ah
        mov al, 13h
        int 10h
        pop ax
        ret
    endp
    
    
; ************************************************
; ************************************************
; ************************************************    
    
   
; ************************************************
;           Funcoes Relativas ao Rato
; ************************************************   
    
    
    inic_rato proc
        push ax
        xor ax,ax
        int 33h
        pop ax
        ret
    endp
    
    mostra_rato proc
        
        push ax
        mov ax, 01
        int 33h
        pop ax
        
        ret
    endp 
    
    click_rato proc
        
        push ax
        
        wait_click:
        
        mov ax, 3
        int 33h
        cmp bx, 0
        je  wait_click
        
        pop ax
        
        ret
    endp 
    
; ************************************************
; ************************************************
; ************************************************            
    
    
; ************************************************
;           Funcoes Relativas a Strings
; ************************************************

    escreve_str proc
        
        push ax
        push bx
        xor bh, bh
        mov bl, 7 ;Cinzento
        mov ax, 1301h; AH=13h , AL= 01h (atualiza o cursor apos escrita)
        int 10h
        pop bx
        pop ax
        
        ret 
    endp 

;*****************************************************************
; co - caracter output
; descricao: rotina que faz o output de um caracter para o ecra
; input - al=caracter a escrever
; output - nenhum
; destroi - nada
;*****************************************************************
    
    co proc
   
    push ax
    push dx
    mov ah,02H
    mov dl,al
    int 21H
    pop dx
    pop ax
    ret
    co endp
    
;*****************************************************************
; ReadStr
; descricao: le uma string do teclado para o endereco dado pelo registo DI.
; input - al=caracter a guardar 
;         cx=numero de caracteres a ler
; output - nenhum
; destroi - nada
;*****************************************************************   
        
    readStr proc
    
        push ax
        push cx
        push si
        push di
        
        
        leitura:
        mov ah, 1
    	int 21h
      
    	cmp al, 13
    	je fimleitura
    	         
    	cmp al,03Eh
    	jne proceed
    	
    	mov al,8
    	call co
    	mov al,':'
    	call co
    	
    	proceed:
    	mov byte [di], al
    	inc di
    	
    	dec cx
    	cmp cx, 0
    	je fim_read
    	jmp leitura
    	
    	fimleitura: 
    	 
         
        
        fim_read:
        pop di
        pop si
        pop cx
        pop ax
        
        ret
    
    endp
    
;*****************************************************************
; num_to_str
; descricao: Imprime um numero dado em AX
; input - ax = numero a imprimir
; output - nada
; destroi - nada
;*****************************************************************    
    num_to_str proc
        push ax
        push bx
        push cx
        push dx
        push di
            
        mov bx,10
        xor cx,cx
        xor dx,dx
        
        cmp ax,0      ;
        jne divide    ; caso em que
        add al,'0'    ; numero e 0       
        mov [di],al   ;
        inc di        ;
        jmp fim_nts   ;
        
        divide:      
        cmp ax,bx
        jb fim_div
        div bx 
        inc cx
        push dx
        xor dx,dx 
           
        jmp divide
            
        fim_div:
        cmp al,0
        je loop_co
        add al,'0'
        mov [di],al
        inc di
        cmp cx, 0
        je fim_nts
            
        loop_co:
        pop ax
        add ax,'0'
        mov [di],al
        inc di
        loop l
 
        fim_nts:
        pop di
        pop dx
        pop cx
        pop bx
        pop ax
            
        ret
    endp
    
    str_to_dec proc
            
        push bx
        push cx
            
            
        xor ax,ax
        xor cx,cx
        mov bx,10
            
        conv_dec:
        cmp [si], 0
        je fim_str_to_dec
            
            
        mov cl,[si]
        sub cl,'0'
        mul bx
        add ax,cx  
        inc si
        jmp conv_dec
             
        fim_str_to_dec:
        pop cx
        pop bx
            
        ret
    endp
    
    copy_string proc
        push si
        push cx
        
        copy_str:
        mov cx,1
        rep movsb
        
        dec si 
        dec di
        cmp [si],0
        je end_string
        inc si
        inc di
        jmp copy_str
        
        end_string:
        pop cx
        pop si
        ret
    endp
    
    newline proc
        push ax
        push dx
        mov ah, 02H
        mov dl, 0AH
        int 21H 
        mov dl, 0DH
        int 21H
        pop dx
        pop ax
        
        
     ret
    endp
      
; ************************************************
; ************************************************
; ************************************************ 


; ************************************************
;           Funcoes Relativas a Cursor
; ************************************************    
    
    show_cursor proc
            
        push ax
        push cx
          
        mov ch, 6
       	mov cl, 7
       	mov ah, 1
       	int 10h
       	
       	pop cx
       	pop ax
      	ret
    endp
    
    
    move_cursor proc
    
    	push ax
    	push bx
    	        
    	mov bh, 0
    	mov ah, 2
    	int 10h
    	    
    	pop bx
    	pop ax
    	ret
	endp

; ************************************************
; ************************************************
; ************************************************


; ************************************************
;           Funcoes Relativas a Ficheiros
; ************************************************    
    
    fopen proc
   
        mov ah, 3dh
        int 21h 

        ret
    endp
    
    
    fclose proc
        
        mov ah, 3eh
        int 21h
        
        ret
    endp
    
    fwrite proc
        mov ah, 40h
        int 21h
    
        ret
    endp
    
    newline_file proc
        push dx     
           
        mov cx,2
        lea dx, nl_file
        call fwrite
            
        pop dx
        ret
    endp
    
    check_eof proc
        
        push ax
        push bx
        push dx 
        
        mov cx,1
        
        mov ah,3fh
        int 21h
        
        cmp ax,0
        je EOF
        jmp NEOF
        
        
        EOF: mov cx,0
        
        NEOF:
        pop dx
        pop bx
        pop ax
        
        ret
    endp
  
; ************************************************
;           Funcoes Relativas ao Menu Principal
; ************************************************
  
  
    menu proc
        
        mov cx, 26
        mov dx, 0408h                                                                            
        lea bp, menu_p
        call escreve_str 
        
        mov cx, 41
        mov dl,01h
        add dh,2
        lea bp, op_l
        call escreve_str
        
        add dh,2
        lea bp, op_i
        call escreve_str
        
        add dh,2
        lea bp, op_p
        call escreve_str
        
        add dh,2
        lea bp, op_d
        call escreve_str
        
        add dh,2
        lea bp, op_a
        call escreve_str
        
        add dh,2
        lea bp, op_e
        call escreve_str
        
        ret
    endp
    
    
    op_menu proc
        
    click_fora:
    
    call click_rato
     
    
    cmp cx, 08h        ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    jb  click_fora     ;                                ;
    cmp cx, 026Ch      ; verifica se o clique do rato   ;
    ja  click_fora     ; esta dentro da zona com as     ;
    cmp dx, 2Bh        ; opcoes possiveis               ;
    jb  click_fora     ;                                ;
    cmp dx, 8BH        ;                                ;
    ja  click_fora     ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    
    
    cmp dx, 3Bh        ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    jb l               ;                                ;
    cmp dx, 4Bh        ;                                ;
    jb i               ;  Verifica qual das opcoes      ;
    cmp dx, 5Bh        ;  foi seleccionada. Se nenhuma  ;
    jb p               ;  das condicoes se verifica     ;
    cmp dx, 6Bh        ;  e a opcao 'e'                 ;
    jb d               ;                                ;
    cmp dx, 7Bh        ;                                ;
    jb a               ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    
       
       xor ax,ax
       jmp fim_opc
       
    
    l: mov ax, 1 
       jmp fim_opc
    
    i: mov ax, 2
       jmp fim_opc
    
    p: mov ax, 3
       jmp fim_opc   
    
    d: mov ax, 4
       jmp fim_opc
    
    a: mov ax, 5
    
     fim_opc:
        ret
    endp

; ************************************************
; ************************************************
; ************************************************      
    

; ************************************************
;           Funcoes Relativas ao About
; ************************************************

    
    about proc
        
        call modo_video 
    
        mov cx, 23
        mov dx, 0808h       
        lea bp, about_str
        call escreve_str 
        
        
        add dh, 2       
        lea bp, elem1
        call escreve_str 
    
        
        add dh, 2       
        lea bp, elem2
        call escreve_str 
    
        
        add dh, 2       
        lea bp, elem3
        call escreve_str
        
        
        call click_rato
        
        
        call modo_video
        
        
        ret
    endp

; ************************************************
; ************************************************
; ************************************************    


; ************************************************
;           Funcoes Relativas ao List
; ************************************************

    list proc
        
        call modo_video; limpar o ecra
        
        call botoes
        
        
        cmp cl,0
        je sem_refs
        xor ax,ax 
        xor ch,ch
        xor dx,dx
               
        next_list:
        add dh,2 
        call move_cursor
        inc al
        push ax
        push dx
        push si
        push di
         
        
               
        inc ch  ;contador de refs p/pag

        call ref_to_buffer
        inc di
        mov [di],'$'
        
        pop di
        pop si
        add si,52; proxima referencia a listar
        
        mov dx,di
        mov ah,9
        int 21h
        
        pop dx
        pop ax
        
        cmp al,cl
        je wait_main
         
        cmp ch, 10
        jb next_list
        
        max_refs_page:
        call click_botao 
        
        cmp bx, 2
        jne fim_list 
        xor ch,ch
        xor dx,dx ; reset cursor
        
        call modo_video
        call botoes
        jmp next_list
        
        
        sem_refs:
        mov cx, 30
        mov dx, 0808h       
        lea bp, no_refs
        call escreve_str
        
        wait_main:
        call click_botao
        cmp bx,1
        je fim_list
        jmp wait_main 
            

        
        fim_list:
        call modo_video
        
        ret        
    endp 
    
    entry_num proc   
        push ax
        
        mov al,'['
        call co   
       
        cmp ah,10
        jae casas2
        
        mov al,' '
        call co
        mov al,ah
        xor ah,ah
        call num_to_str
        mov ah, al
        jmp  entry_end
        
        casas2:
        mov al,ah
        xor ah,ah
        call num_to_str
        mov ah, al 
        
        entry_end:
        mov al,']'
        call co
        mov al,' '
        call co
        
        pop ax
        ret
    endp

    write_title proc
        push ax
        push si
        
        title_writer:
        mov al,[si]
        cmp al, 0
        je fim_title
        call co
        inc si
        jmp title_writer
        
        
        fim_title:
        
        pop si
        pop ax
        
        ret
    endp
    
; ************************************************
; ************************************************
; ************************************************    
    
    
    
; ************************************************
;           Funcoes Relativas ao Insert
; ************************************************
    
    insert proc

        start_insert:
        
        call modo_video
        
        cmp al, 50       ;verificar se atingiu limite de referencias
        je max_refs
        
        push di ;para novas entradas
        
        call str_insert
        call ref_livre
        call show_cursor
        
     	mov dx, 040Fh
     	call move_cursor
	    
	    mov cx,25        
        call readStr
        CMP [di],0
        Je invalid_ref
        
        add di, 25
        
        mov dx,0612h
        call move_cursor
        
        mov cx,25
        call readStr
        CMP [di],0
        Je invalid_ref
        
        inc al
        invalid_ref:
        
        pop di
        
        call click_botao 
        
        cmp bx, 2
        je start_insert
        
        max_refs:
        fim_insert:
        call modo_video
        ret
    endp
    
    ref_livre proc
        push ax
        push bx
        
        mov bx, 52   ;
        mul bx       ; Proxima posicao livre
        add di, ax   ;

        inc di; para comecar na primeira posicao de string
        
        pop bx
        pop ax
        
        ret 
    endp 
    
    
    str_insert proc
        push cx
        push dx
        
        mov cx, 15
        mov dx, 0400h
        lea bp, paper
        call escreve_str
        
        
        mov cx, 18
        add dh, 2
        lea bp, author
        call escreve_str 
        
        call botoes
        
        pop dx
        pop cx
        
        ret
    endp
    
; ************************************************
; ************************************************
; ************************************************    
 
    
; ************************************************
;           Funcoes Relativas ao Process
; ************************************************

    process proc
        
        call modo_video
        call show_cursor
        
        call read_path
        
        cmp ax,03h
        jne process_file
        
        call no_file
        jmp end_process
               
        process_file: 
        mov bx,ax ;handle
        start_process:
        
        mov al,'['
        call read_until_char ;funciona bem 
        cmp cx,0
        je  EOF_process
        call read_num_process
        cmp cx,0
        je  EOF_process
                             
        push di
        dec  ax
        add di, ax
        inc [di]
        pop di      
        jmp start_process
        
        EOF_process:
        
        call refs_to_file
        
        call fclose
        call processed
        end_process:
        
        mov cx,50
        xor ax,ax
        push di
        
        update_total_refs:
        cmp [di],0 
        je no_ut
        add al,[di]
        no_ut:
        inc di
        loop update_total_refs
        
        pop di
        
        call clean_buffer
        
        
        
        call click_rato
        call modo_video
        
        ret
    endp
    
    read_path proc
            
        push dx
        
        mov cx,8
        mov dx,0A05h
        lea bp, path
        call escreve_str 
        
        add dl,5
        call move_cursor

        mov cx,50
        call readstr
        
        mov dx,di
        mov al,2
        call fopen
        
        call clean_buffer
           
        pop dx
        
        ret
    endp
    
    clean_buffer proc
        push cx
        push di
            
        buf:
        cmp [di],0
        je fim_clean
        
        mov [di],0
        cmp cx,0
        je fim_clean
        inc di
        dec cx
        jmp buf
         
        fim_clean:
        pop di
        pop cx
         
        ret
    endp
    
    no_file proc
        push cx
        push dx
        push bp
        
        call modo_video
        
        mov cx,15
        mov dx,0A05h
        lea bp, file_not_found
        call escreve_str
        
        mov cx,21
        add dh,5
        lea bp, click_to_main
        call escreve_str 
        
        pop bp
        pop dx
        pop cx
        
        ret
    endp 
    
    processed proc
        
        push cx
        push dx
        push bp
        
        call modo_video
        
        mov cx,14
        mov dx,0A05h
        lea bp, file_processed
        call escreve_str
        
        mov cx,21
        add dh,5
        lea bp, click_to_main
        call escreve_str
        
 
        pop bp
        pop dx
        pop cx
        
        
        ret
    endp
    
    read_until_char proc
        push dx
        push si
        
        mov si,dx
        ler_char:
        
        call check_eof
        cmp cx,0
        je fim_read_u
        
        
        cmp al, [si]
        jne ler_char
        
        fim_read_u:
        pop si
        pop dx
        ret
    endp
    
    read_num_process proc    
        push dx  
        push si
        mov si,dx
        push si
        read_num:
        
        call check_eof
        cmp cx,0
        je erro_num_process
        
        
        
        cmp [si],']'
        je stop_read
        cmp [si],'0'
        jb erro_num_process
        cmp [si],'9'
        ja erro_num_process
        
        inc si
        inc dx
        jmp read_num
        
        stop_read:
        mov [si],0
        
        pop si
        call str_to_dec
        jmp fim_read_num
        
        erro_num_process:
        pop si
        fim_read_num:
        pop si
        pop dx
        
        ret
    endp
    
     
    
    
     
    
    refs_to_file proc
        push di
        push si
        push cx
        push dx
        
            
        call newline_file
        call bib_appendix 
            
        ;escrever as referencias
        
        mov cx, 50   
        
        next_ref_num:
        cmp cx,0
        je end_ref_buffer
        cmp [di],0
        jne ref_found
        
        inc di
        dec cx
        add si,52; proxima referencia
        jmp next_ref_num
        
        ref_found:
        push cx
        push di
        push si
        
        mov di,dx
        call ref_to_buffer
        
        
        jmp end_of_entry
        
       
        
        
        
        end_of_entry:
        mov cx,60
        call fwrite
        call newline_file
        pop si
        pop di
        pop cx
        inc di
        add si,52
        dec cx
        jmp next_ref_num
        
        end_ref_buffer:    
        pop dx
        pop cx
        pop si
        pop di
        ret
    endp    
    
    bib_appendix proc
        push dx
            
        mov cx,24
        lea dx, bib_app
        call fwrite
            
        pop dx
        ret
    endp
    
    reference_not_found proc
        push si
        push cx
        
        dec di
        lea si,ref_not_found
        mov cx,19
        rep movsb
        
        pop cx
        pop si
        ret
    endp

; ************************************************
;            Funcoes relativas ao display
;*************************************************

    display proc
        call modo_video
        
        copia:
            mov al, byte ptr[si]
            
            mov byte ptr[di], al
            inc si
            inc di
            inc vezes_lidas_buf
            cmp vezes_lidas_buf, 50
            jb copia
            mov byte ptr[di], 024h
            mov vezes_lidas_buf, 1
            
            
        ;***********************************
        ;escrever a parte inicial do display
        mov cx, offset display2
        sub cx, offset display1
        mov dl, 0
        mov dh, 0
        mov bp, offset display1
        mov bl, 1111b
        call write_position
        
        mov cx, offset display3 
        sub cx, offset display2
        mov dl, 0
        mov dh, 2
        mov bp, offset display2        	 
        mov bl,1111b
        call write_position
        
        mov cx, offset display4
        sub cx, offset display3
        mov dl, 0
        mov dh, 4
        mov bp, offset display3
        mov bl, 1111b
        call write_position
        
        call print_number_total_ref
        ;-------------------------------------
        back_buffer:
            mov ah, 0    
            lea di, buffer_new
            run_buffer:
                mov al, byte ptr[di]
                cmp al, 024h ;quando deteta $
                je fim_buf
        
                cmp ax, num_ref
                ja salto
                inc di
                inc vezes_lidas_buf
                jmp run_buffer
            
            salto:
                mov num_ref, ax
                mov bx, vezes_lidas_buf
                mov pos_maior_num, bx
                inc di
                inc vezes_lidas_buf            
                jmp run_buffer
            
            fim_buf:
                lea si, buffer_new
                mov ah, 0
                poe_zero:
                    mov al, byte ptr[si]
                    cmp ax, num_ref
                    je por_zero
                    inc si
                    jmp poe_zero
                    por_zero:
                        mov byte ptr[si], 0

                mov vezes_lidas_buf, 1            
            
        ;---------------------------------------
        cmp num_ref, 0
        je fim_disp
        call print_number
        mov num_ref, 0
        jmp back_buffer
                
        fim_disp:
            ;mov ax,0
            ;mov ah,1
            ;int 21h
            
            call click_rato
                
            call modo_video
            ret
    display endp
    
    print_number_aux proc
        mov bl,1111b
        mov cx, offset end_1 - offset str_n
        ;sub cx, offset str_n
        mov bp, offset str_n        	 
        call write_position
        ret
    print_number_aux endp
    
    print_number_total_ref proc
        mov ax, total_refs
        mov si, offset str_n
        call WriteUns
    
        mov dh, 0
        mov dl, 14
        call print_number_aux
        
        mov pos_y, 6
        mov pos_x, 0
        ret        
    print_number_total_ref endp
    
    print_number proc
                
        ;paper---------------
        mov ax, pos_maior_num
        mov si, offset str_n
        call WriteUns
        
        mov dh, pos_y
        mov dl, pos_x
        call print_number_aux
        ;--------------------
               
        ;n. de referencias---
        mov ax, num_ref
        mov si, offset str_n
        call WriteUns
    
        mov dh, pos_y
        add pos_x, 10
        mov dl, pos_x
        call print_number_aux
        ;--------------------
                
        ;percentagem---------
        mov ax, num_ref  
        push cx
        push bx
        mov bx, total_refs 
        call calc_perc
        pop bx
        pop cx
        
        mov si,offset str_n 
        call WriteUns
        
        mov dh, pos_y
        add pos_x, 10
        mov dl, pos_x
        call print_number_aux
        ;--------------------
    
        add pos_y, 2
        sub pos_x, 20
        ret
    print_number endp
    
    write_position proc
        push ax
        push bx
        
        mov al, 0
        mov bh, 0
        mov ah, 13h
        int 10h
        
        pop bx
        pop ax
        ret  
    write_position endp
    
    calc_perc proc
        cmp ax, 0
        je end_perc
    
        cmp bx, 0
        je end_perc
    
        push bx  ;---------------------P
        mov bx,100
        mul bx
        pop bx   ;---------------------O
    
        div bx 
        push ax  ;---------------------P
    
        push bx  ;---------------------P
        sub bx, dx
        mov cx, bx
        pop ax   ;---------------------O
    
        mov bx, 2
        mov dx, 0
        div bx
    
        cmp ax, cx
        jb perc 
        pop ax    ;---------------------O
        inc ax   
        jmp end_perc
        perc:
        pop ax  ;-----------------------O  
        end_perc:
        ret   
    calc_perc endp
    
    WriteUns proc
        ;guarda os valores dos registos si, di, bx, cx e dx na pilha
        push si
        push di
        push bx
        push cx
        push dx
        
        mov cl, 30h
        cmp ax,0
        je W_S1:   
        
        
        mov di,0 
        mov bx, 10 
        mov dx,0 
        mov cl,0
        
        W_L1:   
            push dx
            mov bx, 10
            div bx ; divide por 10
            add dl, 30h
            
            mov byte ptr [si],dl        
            inc di
            inc si 
            pop dx
            cmp ax,0
            jne W_L1
        
            dec di
            dec si 
            
            sub si, di
            add di, si 
        
        
            ;guarda o primeiro byte da string em cl
            mov cl, byte ptr [si]
            mov ax, si 
        
        
            ;inverte a ordem - primeiro para para ultimo
            W_L2:
                cmp di,ax
                jbe W_S1
            
            mov dl,byte ptr [di]
            mov byte ptr [si],dl
            
            dec di
            inc si
            jmp W_L2
              
            W_S1:
                mov byte ptr [si],cl
                mov byte ptr [si+1],0 
                mov byte ptr [si+2],0 

;vai buscar os valores dos registos di, bx, cx e dx que foram guardados na pilha no inicio da funcao
        pop dx
        pop cx
        pop bx
        pop di
        pop si
        Ret    
    endp    
       

; ************************************************
; ************************************************
; ************************************************
    

; ************************************************
;           Funcoes Relativas a Database
; ************************************************    
    
    SetPaperNum proc
        
        lea si, database
        mov al, 1
        
        SetPaperNumagain:                
        mov byte ptr[si], al
        inc al        
        cmp al, 50
        je fimSetPaper        
        
        add si, 52
        jmp SetPaperNumagain
        
        fimSetPaper:
        
        
        ret
    endp
    
    
; ************************************************
; ************************************************
; ************************************************
    

; ************************************************
;           Funcoes Relativas a Exit
; ************************************************

    exit proc
        
        call stat  
        
        
        mov si, offset database
        inc si
        cmp byte ptr [si], 0
        je exit_fim 

        mov bp, 0 
        
        mov di, offset buffer
        mov byte ptr [di], 91
        inc bp
        inc di 
        mov byte ptr [di], 49
        inc bp   
        inc di
        mov byte ptr [di], 93
        inc bp
        inc di
        mov byte ptr [di], 34 
        inc bp
                      
        mov si, offset database
        inc si        
        ver_again_exit:
        inc di
                 
        cmp byte ptr [si], 0
        je ver_fim_exit
        mov al, byte ptr [si]
        mov byte ptr [di], al
        inc bp
        inc si
        jmp ver_again_exit
        

        ver_again_exit2:
        inc bp
        inc si
        inc di         
        cmp byte ptr [si], 0
        je output_fim
        mov al, byte ptr [si]
        mov byte ptr [di], al       
        jmp ver_again_exit2
        
        
        
        ver_fim_exit:
        mov byte ptr [di], 34 
        inc bp
        inc di
        mov byte ptr [di], 44
        inc bp
        mov si, offset database
        add si, 25      
        mov dl, 1
        jmp ver_again_exit2:

        
        output_fim:
        
        mov byte ptr [di], 10
        mov al,1
        lea dx, test_file
        call fopen

        
        mov bx,ax
        lea dx, buffer
        mov cx,bp
        call fwrite
        
        
        mov di, offset buffer
        mov si, 60
        limpar_buffer:
        mov byte ptr[di], 0
        inc di
        dec si
        cmp si,0
        jne limpar_buffer
        
        
         
       
;        ;;;;;;;;;;;;;;;;;;;;;;;;;;;;
        
         
        
        ;;mov al, dl
;        mov si, offset database
;        ;verificar_fim:
;        inc si, 52
;        inc si 
;        ;dec al
;        ;cmp al, 0
;        ;jne verificar_fim 
;        cmp byte ptr [si], 0
;        je exit_fim
;        
;
;        mov bp, 0 
;        mov di, offset buffer
;        mov byte ptr [di], 91
;        inc bp
;        inc di 
;        mov byte ptr [di], 49
;        inc bp   
;        inc di
;        mov byte ptr [di], 93
;        inc bp
;        inc di
;        mov byte ptr [di], 34 
;        inc bp
;                      
;        ver_again_exit1:
;        inc di
;                 
;        cmp byte ptr [si], 0
;        je ver_fim_exit1
;        mov al, byte ptr [si]
;        mov byte ptr [di], al
;        inc bp
;        inc si
;        jmp ver_again_exit1
;        
;
;        ver_again_exit3:
;        inc bp
;        inc si         
;        cmp byte ptr [si], 0
;        je output_fim
;        mov al, byte ptr [si]
;        mov byte ptr [di], al
;        inc di
;        jmp ver_again_exit3
;        
;        
;        
;        ver_fim_exit1:
;        mov byte ptr [di], 34 
;        inc bp
;        inc di
;        mov byte ptr [di], 44
;        inc di
;        mov si, offset database
;        
;        
;        ;mov al, dl
;        ;verificar_fim2:
;        inc si, 52
;        ;dec al
;        ;cmp al, 0
;        ;jne verificar_fim2
;        ;inc dl
;        add si, 25
;        inc si      
;        jmp ver_again_exit3
        
        
        
        
        
        exit_fim:
        
        ret
    endp    

    

; ************************************************
; ************************************************
; ************************************************    
    

; ************************************************
;           Funcoes Relativas aos Botoes
; ************************************************    
    
    
    botoes proc
        push cx
        push dx
        
        mov cx, 11
        mov dx, 1701h       
        lea bp, botao1
        call escreve_str
        
        mov dx, 171Fh
        lea bp, botao2
        call escreve_str
        
        pop dx
        pop cx
        
        ret
    endp
    
    click_botao proc
        
        push cx
        push dx
        
        click:
        call click_rato
        
        cmp dx, 00B0h
        jb click
        
        
        cmp cx, 0090H
        jb click_main
       
        
        cmp cx, 01E5H
        jb click
        mov bx, 2 
        jmp fim_click
        
        click_main:
        mov bx, 1
        
        fim_click:
        
        pop dx
        pop cx
        
        ret
    endp 
    
    
    ref_to_buffer proc
        
        mov [di],'['
        inc di
        mov al, [si];numero da referencia
        inc si
        cmp al,10
        jae casas2_proc
        
        casas1_proc:
        mov [di],' '
        inc di
        call num_to_str 
        inc di
        jmp continue_process
        casas2_proc:
        call num_to_str
        add di,2 
        
        continue_process:
        mov [di],']'
        inc di      
        mov [di],' '
        inc di
        mov [di],'"'
        inc di 
        
        ;copiar a str nome 
        inc si
        cmp [si],0
        je nonexisting_ref
        dec si
        call copy_string
        add si,25
        
        mov [di],'"'
        inc di
        mov [di],','
        inc di
        mov [di],' '
        ;copiar a str autor
        
        call copy_string
        jmp end_ref_to_buffer   
        
        nonexisting_ref:
        call reference_not_found
        end_ref_to_buffer:
        
        ret
    endp
    
    stat proc
        mov vezes_lidas_buf, 0
        
        
        ;criar ficheiro
        mov al,1
        lea dx, filename
        call fopen
        
        mov handle, ax
        
        lea si, ref_buffer
        lea di, buffer_new
        
        copiaf:
            mov al, byte ptr[si]
            
            mov byte ptr[di], al
            inc si
            inc di
            inc vezes_lidas_buf
            cmp vezes_lidas_buf, 50
            jb copiaf
            mov byte ptr[di], 024h
            mov vezes_lidas_buf, 1
            ;-------------------------------------
            
            back_bufferf:
            mov ah, 0    
            lea di, buffer_new
            run_bufferf:
                mov al, byte ptr[di]
                cmp al, 024h ;quando deteta $
                je fim_buf_fich
        
                cmp ax, num_ref
                ja saltof
                inc di
                inc vezes_lidas_buf
                jmp run_bufferf
            
            saltof:
                mov num_ref, ax
                mov bx, vezes_lidas_buf
                mov pos_maior_num, bx
                inc di
                inc vezes_lidas_buf            
                jmp run_bufferf
            
            fim_buf_fich:
                lea si, buffer_new
                mov ah, 0
                poe_zerof:
                    mov al, byte ptr[si]
                    cmp ax, num_ref
                    je por_zerof
                    inc si
                    jmp poe_zerof
                    por_zerof:   
                        mov byte ptr[si], 0
                        mov vezes_lidas_buf, 1            
            
        ;---------------------------------------
        cmp num_ref, 0
        je fim_stati
        lea si, buffer_stat
        mov ax, pos_maior_num
        mov byte ptr[si], al
        inc si
        mov ax, num_ref
        mov byte ptr[si], al
        inc si
        mov ax, num_ref  
        push cx
        push bx
        mov bx, total_refs 
        call calc_perc
        pop bx
        pop cx
        mov percentage, ax
         
        mov byte ptr[si], al
        
        mov bx, handle
        lea dx, buffer_stat
        mov cx, 4
        call fwrite
        
        call newline_file
        
        mov num_ref, 0
        jmp back_bufferf
        
        fim_stati:
            call fclose
            ret
    stat endp

end start ; set entry point and stop the assembler.
