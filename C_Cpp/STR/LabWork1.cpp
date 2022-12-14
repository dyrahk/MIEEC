// LabWork1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

//#include "stdafx.h" //#include<pch.h> replace with pch.h

#include<conio.h>
#include<stdlib.h>
#include<stdio.h>
#include <windows.h>  //for Sleep function
extern "C" {
#include <FreeRTOS.h>
#include <task.h>
#include <semphr.h>
#include <interface.h> 
}

typedef struct {
	int x;
	int z;
} TPosition;

typedef struct {
	int plateNumber;
	int entryHour;     // for simplicity, parking services allways enter and leave the same day.
	int entryMinute;
	//…. 
} StandingRequest;


//semaphores
xSemaphoreHandle sem_x_done;
xSemaphoreHandle sem_z_done;
xSemaphoreHandle Semkit;

//mailboxes
xQueueHandle        mbx_x;  //for goto_x
xQueueHandle        mbx_z;  //for goto_z
xQueueHandle        mbx_xz;
xQueueHandle        mbx_input;
xQueueHandle		mbx_int;
xQueueHandle		mbx_req;



int getBitValue(uInt8 value, uInt8 n_bit);
int getBitValue(uInt8 value, uInt8 n_bit);

void move_x_left();
void move_x_right();
void stop_x();
int get_x_pos();
void goto_x(int x);

void move_z_up();
void move_z_down();
void stop_z();
int get_z_pos();
void goto_z(int z);

void move_y_inside();
void move_y_outside();
void stop_y();
int get_y_pos();
void goto_y(int y);

void goto_xz(int x, int z, bool _wait_till_done);

void goto_up_level();
void goto_down_level();

void put_piece();
void get_piece();

bool is_at_x(int pos);
bool is_at_y(int pos);
bool is_at_z(int pos);
bool is_at_z_down();
bool is_at_z_up();
bool is_at_cell(int x, int z);

uInt8 SemReadDigitalU8(int port);
void SemWriteDigitalU8(int port, uInt8 val);

void callibrate();

void vTaskHorizontal(void * pvParameters);
void vTaskVertical(void * pvParameters);
void goto_x_task(void *param);
void goto_z_task(void *param);
void goto_xz_task(void *param);
void task_storage_services(void *param);
void receive_instructions_task(void *ignore);

void switch1_event(void *param);
void switch2_event(void *param);

void show_menu();
void joystick();




void main(int argc, char **argv)
{
	create_DI_channel(0);
	create_DI_channel(1);
	create_DO_channel(2);

	printf("\nwaiting for hardware simulator...");
	WriteDigitalU8(2, 0);
	printf("\ngot access to simulator...");

	Semkit = xSemaphoreCreateCounting(1, 1);   //SEMAPHORE CREATION
	sem_x_done = xSemaphoreCreateCounting(1000, 0);  //
	sem_z_done = xSemaphoreCreateCounting(1000, 0);

	mbx_x = xQueueCreate(10, sizeof(int));
	mbx_z = xQueueCreate(10, sizeof(int));
	mbx_xz = xQueueCreate(10, sizeof(TPosition));
	mbx_input = xQueueCreate(10, sizeof(int));

	xTaskCreate(task_storage_services, "task_storage_services", 100, NULL, 0, NULL);
	xTaskCreate(goto_x_task, "goto_x_task", 100, NULL, 0, NULL);
	xTaskCreate(goto_z_task, "goto_z_task", 100, NULL, 0, NULL);
	xTaskCreate(goto_xz_task, "v_gotoxz_task", 100, NULL, 0, NULL);
	xTaskCreate(receive_instructions_task, "receive_instructions_task", 100, NULL, 0, NULL);
	xTaskCreate(switch1_event, "switch1_event", 100, NULL, 0, NULL);
	xTaskCreate(switch2_event, "switch2_event", 100, NULL, 0, NULL);



	SYSTEMTIME st;
	GetLocalTime(&st);
	printf("\n time = %u", st.wMilliseconds);
	_getch();

	callibrate();
	vTaskStartScheduler();
}


void vTaskHorizontal(void * pvParameters)
{
	while (TRUE)
	{
		//go right
		uInt8 aa = SemReadDigitalU8(2);
		SemWriteDigitalU8(2, (aa & (0xff - 0x40)) | 0x80);

		// wait till last sensor
		while (ReadDigitalU8(0) & 0x01) {
			taskYIELD();
		}
		// go left		
		aa = SemReadDigitalU8(2);
		WriteDigitalU8(2, (aa & (0xff - 0x80)) | 0x40);

		// wait till last sensor
		while ((ReadDigitalU8(0) & 0x04)) {
			taskYIELD();
		}
	}
}

void vTaskVertical(void * pvParameters)
{
	while (TRUE)
	{
		//go up

		uInt8 aa = SemReadDigitalU8(2);
		SemWriteDigitalU8(2, (aa & (0xff - 0x04)) | 0x08);

		// wait till last sensor		
		while ((ReadDigitalU8(0) & 0x40)) { vTaskDelay(1); }

		// go left		
		aa = SemReadDigitalU8(2);
		SemWriteDigitalU8(2, (aa & (0xff - 0x08)) | 0x04);

		// wait till last sensor		
		while ((ReadDigitalU8(1) & 0x08)) { vTaskDelay(1); }
	}
}

int getBitValue(uInt8 value, uInt8 n_bit)
// given a byte value, returns the value of bit n
{
	return(value & (1 << n_bit));
}

void setBitValue(uInt8  *variable, int n_bit, int new_value_bit)
// given a byte value, set the n bit to value
{
	uInt8  mask_on = (uInt8)(1 << n_bit);
	uInt8  mask_off = ~mask_on;
	if (new_value_bit)  *variable |= mask_on;
	else                *variable &= mask_off;
}

uInt8 SemReadDigitalU8(int port) {
	uInt8 val = 0;
	xSemaphoreTake(Semkit, portMAX_DELAY);
	val = ReadDigitalU8(port);
	xSemaphoreGive(Semkit);
	return val;
}

void SemWriteDigitalU8(int port, uInt8 val) {
	xSemaphoreTake(Semkit, portMAX_DELAY);
	WriteDigitalU8(port, val);
	xSemaphoreGive(Semkit);
}

void move_x_left()
{
	if (!is_at_x(1)) {
		uInt8 p = SemReadDigitalU8(2); //  read port 2
		setBitValue(&p, 6, 1);     //  set bit 6 to high level
		setBitValue(&p, 7, 0);      //set bit 7 to low level
		SemWriteDigitalU8(2, p); //  update port 2
	}
}

void move_x_right()
{
	uInt8 p = SemReadDigitalU8(2); //read port 2
	setBitValue(&p, 6, 0);    //  set bit 6 to  low level
	setBitValue(&p, 7, 1);      //set bit 7 to high level
	SemWriteDigitalU8(2, p); //update port 2
}

void stop_x()
{
	uInt8 p = SemReadDigitalU8(2); //read port 2
	setBitValue(&p, 6, 0);   //  set bit 6 to  low level
	setBitValue(&p, 7, 0);   //set bit 7 to low level
	SemWriteDigitalU8(2, p); //update port 2
}

int get_x_pos()
{
	uInt8 p = ReadDigitalU8(0);
	if (!getBitValue(p, 2))
		return 1;
	if (!getBitValue(p, 1))
		return 2;
	if (!getBitValue(p, 0))
		return 3;
	return(-1);
}

void goto_x(int x) {
	if (get_x_pos() < x)
		move_x_right();
	else if (get_x_pos() > x)
		move_x_left();
	//   while position not reached
	while (get_x_pos() != x)
		Sleep(1);
	stop_x();
}

void move_z_up() {
	uInt8 p = SemReadDigitalU8(2); //  read port 2
	setBitValue(&p, 3, 1);     //  set bit 3 to high level
	setBitValue(&p, 2, 0);      //set bit 2 to low level
	SemWriteDigitalU8(2, p); //  update port 2
}

void move_z_down() {
	uInt8 p = SemReadDigitalU8(2); //  read port 2
	setBitValue(&p, 2, 1);     //  set bit 2 to high level
	setBitValue(&p, 3, 0);      //set bit 3 to low level
	SemWriteDigitalU8(2, p); //  update port 2
}

void stop_z() {
	uInt8 p = SemReadDigitalU8(2); //read port 2
	setBitValue(&p, 2, 0);   //  set bit 2 to  low level
	setBitValue(&p, 3, 0);   //set bit 3 to low level
	SemWriteDigitalU8(2, p); //update port 2
}

int get_z_pos()
{
	uInt8 p0 = ReadDigitalU8(0);
	uInt8 p1 = ReadDigitalU8(1);
	if (!getBitValue(p1, 3))
		return 1;
	if (!getBitValue(p1, 1))
		return 2;
	if (!getBitValue(p0, 7))
		return 3;
	return(-1);
}

void goto_z(int z) {//  Similar to the previous one
	if (get_z_pos() < z)
		move_z_up();
	else if (get_z_pos() > z)
		move_z_down();
	//   while position not reached
	while (get_z_pos() != z)
		Sleep(1);
	stop_z();
}

void move_y_inside() {
	uInt8 p = SemReadDigitalU8(2); //  read port 2
	setBitValue(&p, 5, 1);     //  set bit 5 to high level
	setBitValue(&p, 4, 0);      //set bit 4 to low level
	SemWriteDigitalU8(2, p); //  update port 2
}
void move_y_outside() {
	uInt8 p = SemReadDigitalU8(2); //  read port 2
	setBitValue(&p, 4, 1);     //  set bit 4 to high level
	setBitValue(&p, 5, 0);      //set bit 5 to low level
	SemWriteDigitalU8(2, p); //  update port 2
}
void stop_y() {
	uInt8 p = SemReadDigitalU8(2); //read port 2
	setBitValue(&p, 4, 0);   //  set bit 2 to  low level
	setBitValue(&p, 5, 0);   //set bit 3 to low level
	SemWriteDigitalU8(2, p); //update port 2
}
int get_y_pos() {
	uInt8 p0 = ReadDigitalU8(0);
	if (!getBitValue(p0, 3)) //posição interior
		return 1;
	if (!getBitValue(p0, 4)) //posição central
		return 2;
	if (!getBitValue(p0, 5)) //posição exterior
		return 3;
	return(-1);
}
void goto_y(int y) {//  Similar to the previous one
	if (get_y_pos() < y)
		move_y_outside();
	else if (get_y_pos() > y)
		move_y_inside();
	//   while position not reached
	while (get_y_pos() != y)
		Sleep(1);
	stop_y();
}

void goto_up_level() {

	if (!is_at_z_up())
		move_z_up();
	while (!is_at_z_up())
		Sleep(1);
	stop_z();

}

void goto_down_level() {
	if (!is_at_z_down())
		move_z_down();
	while (!is_at_z_down()  /* …. similar to goto_z_up  */)
		Sleep(1);
	stop_z();

}


void put_piece() {
	goto_up_level();
	goto_y(1);
	goto_down_level();
	goto_y(2);
}
void get_piece() {
	goto_down_level();
	goto_y(1);
	goto_up_level();
	goto_y(2);
	goto_down_level();
}



bool is_at_x(int pos) {
	if (pos == get_x_pos())
		return true;
	return false;
}
bool is_at_z(int pos) {
	if (pos == get_z_pos())
		return true;
	return false;
}

bool is_at_z_down() {
	if (is_at_z(1) || is_at_z(2) || is_at_z(3))
		return true;
	return false;
}
bool is_at_z_up() {
	int vp0 = ReadDigitalU8(0);
	int vp1 = ReadDigitalU8(1);
	if (!getBitValue(vp0, 6) || !getBitValue(vp1, 0) || !getBitValue(vp1, 2))
		return true;
	return false;

}
bool is_at_y(int pos) {
	if (pos == get_y_pos())
		return true;
	return false;
}
bool is_at_cell(int x, int z) {
	if (is_at_x(x) && is_at_z(z))
		return true;
	return false;
}


void callibrate() {
	//if (!is_at_y(2)) {

	move_y_outside();
	while (get_y_pos() != 2)
		Sleep(10);
	stop_y();
	//}
	//if (!is_at_x(1)) {
	move_x_left();
	while (get_x_pos() != 1)
		Sleep(10);
	stop_x();
	//}
	//if (!is_at_z(1)) {
	move_z_down();
	while (get_z_pos() != 1)
		Sleep(10);
	stop_z();
	//}

}




void goto_xz(int x, int z, bool _wait_till_done)
{
	//goto_x(x);
	//goto_z(z);
	TPosition pos;
	pos.x = x;
	pos.z = z;
	xQueueSend(mbx_xz, &pos, portMAX_DELAY);
	if (_wait_till_done) {
		while (get_x_pos() != x) { taskYIELD(); }
		while (get_z_pos() != z) { taskYIELD(); }
	}
}

///////////////////////////////////////TASKS////////////////////////////////////////////

void goto_x_task(void *param)
{
	while (true)
	{
		int x;
		xQueueReceive(mbx_x, &x, portMAX_DELAY);
		goto_x(x);
		xSemaphoreGive(sem_x_done);
	}
}

void goto_z_task(void *param)
{
	while (true)
	{
		int z;
		xQueueReceive(mbx_z, &z, portMAX_DELAY);
		goto_z(z);
		xSemaphoreGive(sem_z_done);
	}
}

void goto_xz_task(void *param)
{
	while (true) {
		TPosition position;
		//wait until a goto request is received
		xQueueReceive(mbx_xz, &position, portMAX_DELAY);

		//send request for each goto_x_task and goto_z_task 
		xQueueSend(mbx_x, &position.x, portMAX_DELAY);
		xQueueSend(mbx_z, &position.z, portMAX_DELAY);

		//wait for goto_x completion
		xSemaphoreTake(sem_x_done, portMAX_DELAY);

		//wait for goto_z completion           
		xSemaphoreTake(sem_z_done, portMAX_DELAY);
	}
}

void task_storage_services(void *param)
{
	int cmd = -1;
	while (true) {
		show_menu();
		// get selected option from keyboard
		printf("\n\nEnter option=");
		xQueueReceive(mbx_input, &cmd, portMAX_DELAY);



		if (cmd == 'j') {
			printf("\nJoystick mode activated");
			int exit = 0;
			while (exit != 1) {
				joystick();
				xQueueReceive(mbx_input, &cmd, portMAX_DELAY);

				if (cmd == 'a') // hidden option
					move_x_left(); // not in the show_menu  
				if (cmd == 'd') // hidden option            
					move_x_right();  // not in the show_menu  
				if (cmd == 's') { // hidden option  
					stop_x();
					stop_y();
					stop_z();
				}
				if (cmd == 'w')
					move_z_up();
				if (cmd == 'x')
					move_z_down();
				if (cmd == 'q')
					move_y_inside();
				if (cmd == 'e')
					move_y_outside();
				if (cmd == 'g') // hidden option  
				{
					int x, z; // you can use scanf or one else you like
					printf("\nX=");
					xQueueReceive(mbx_input, &x, portMAX_DELAY);

					x = x - '0'; //convert from ascii code to number
					printf("\nZ=");
					xQueueReceive(mbx_input, &z, portMAX_DELAY);
					z = z - '0'; //convert from ascii code to number

					if (x >= 1 && x <= 3 && z >= 1 && z <= 3)
						goto_xz(x, z, false);
					else
						printf("\nWrong (x,z) coordinates, are you sleeping?... ");
				}
				if (cmd == 'r') {
					callibrate();
					exit = 1;
				}

			}

		}
		if (cmd == 't') // hidden option
		{
			WriteDigitalU8(2, 0); //stop all motores;
			vTaskEndScheduler(); // terminates application
		}
	}   // end while
}

void receive_instructions_task(void *ignore) {
	int c = 0;
	while (true) {
		c = _getwch();  // this function is new.
		putchar(c);
		xQueueSend(mbx_input, &c, portMAX_DELAY);
	}
}


void switch1_event(void *param) {
	int P0_curr = ReadDigitalU8(1);
	int P0_prev = P0_curr;
	while (true) {
		P0_curr = ReadDigitalU8(1);  // read current state 
		if (!getBitValue(P0_curr, 5) && getBitValue(P0_prev, 5)) {
			// Switch 1 activated
			printf("\nSwitch1 activated");
		}
		if (getBitValue(P0_curr, 5) && !getBitValue(P0_prev, 5)) {
			// Switch 1 deactivated

		}
		//now save the previous value of ports to be used next iteration
		P0_prev = P0_curr; // set current_state as previous_state
		vTaskDelay(10);

	}
}

void switch2_event(void *param) {
	int P0_curr = ReadDigitalU8(1);
	int P0_prev = P0_curr;
	while (true) {
		P0_curr = ReadDigitalU8(1);  // read current state 
		if (!getBitValue(P0_curr, 6) && getBitValue(P0_prev, 6)) {
			// Switch 2 activated
			//….
		}
		if (getBitValue(P0_curr, 6) && !getBitValue(P0_prev, 6)) {
			// Switch 2 deactivated
			  //….
		}
		//now save the previous value of ports to be used next iteration
		P0_prev = P0_curr; // set current_state as previous_state
		vTaskDelay(10);
	}
}
////////////////////////////////////////MENU & JOYSTICK///////////////////////////////////

void show_menu()
{
	printf("\npress: a, s, d, g or t");
	printf("\n j - enter joystick mode");
	printf("\n(1) sw1 ....... Put a part in a specific x, z position");
	printf("\n(2) sw2 ....... Retrieve a part from a specific x, z position");
	printf("\n(3) sw1 AND sw2  (slight) ……. EMERGENCY STOP");
	printf("\n(4) sw1 OR sw2  ………………….. RESUME");
	printf("\n(5) sw1 AND sw2  (long)  …….. end program");
	//…./….
}

void joystick() {

	printf("\n\n\nw - move up");
	printf("\na - move left");
	printf("\nd - move right");
	printf("\nx - move down");
	printf("\nq - move inside");
	printf("\ne- move outside");
	printf("\ns - stop");
	printf("\ng - go to (x,z) cell");
	printf("\nr - exit joystick mode");
	printf("\n\nEnter option=");
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
