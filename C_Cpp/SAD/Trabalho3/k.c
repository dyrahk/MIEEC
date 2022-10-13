
//
 
#define WINVER 0x0500
 
#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h> 
#define MESSAGE_LENGTH 100
 
// Declare variables and structures
HANDLE hSerial = INVALID_HANDLE_VALUE;
DCB dcbSerialParams = {0};
COMMTIMEOUTS timeouts = {0};
DWORD dwBytesWritten = 0;
char dev_name[MAX_PATH] = "";
int dev_number = -1;
int baudrate = 9600;
int simulate_keystrokes = 0;
int id = -1;
 
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

    // Set device parameters:
    //  baudrate (9600), 1 start bit, 1 stop bit, no parity
    dcbSerialParams.DCBlength = sizeof(dcbSerialParams);
    if (GetCommState(hSerial, &dcbSerialParams) == 0)
        exit_message("Error getting device state", 1);
    dcbSerialParams.BaudRate = baudrate;
    dcbSerialParams.ByteSize = 8;
    dcbSerialParams.StopBits = ONESTOPBIT;
    dcbSerialParams.Parity = NOPARITY;
    if(SetCommState(hSerial, &dcbSerialParams) == 0)
        exit_message("Error setting device parameters", 1);
 
    // Set COM port timeout settings
    timeouts.ReadIntervalTimeout = 50;
    timeouts.ReadTotalTimeoutConstant = 50;
    timeouts.ReadTotalTimeoutMultiplier = 10;
    timeouts.WriteTotalTimeoutConstant = 50;
    timeouts.WriteTotalTimeoutMultiplier = 10;
    if(SetCommTimeouts(hSerial, &timeouts) == 0)
        exit_message("Error setting timeouts", 1);
 }

void CloseSerialPort()
{
    if (hSerial != INVALID_HANDLE_VALUE)
    {
        // Close serial port
        fprintf(stderr, "\nClosing serial port...");
        if (CloseHandle(hSerial) == 0)
            fprintf(stderr, "Error\n");
        else fprintf(stderr, "OK\n");
    }
}
 
void exit_message(const char* error_message, int error)
{
    // Print an error message
    fprintf(stderr, error_message);
    fprintf(stderr, "\n");
 
    // Exit the program
    exit(error);
}
 
void simulate_keystroke(char c)
{
    // This structure will be used to create the keyboard
    // input event.
    INPUT ip;
 
    // Set up a generic keyboard event.
    ip.type = INPUT_KEYBOARD;
    ip.ki.wScan = 0; // hardware scan code for key
    ip.ki.time = 0;
    ip.ki.dwExtraInfo = 0;
 
    // Press the key
    // Currently only alphabet lettes, spaces, commas and full stops.
    if (c >= 0x61 && c <= 0x7A) c -= 0x20;
    if (c >= 0x30 && c <= 0x39) ip.ki.wVk = c;
    else if (c >= 0x41 && c <= 0x5A) ip.ki.wVk = c;
    else if (c == ' ') ip.ki.wVk = VK_SPACE;
    else if (c == ',') ip.ki.wVk = VK_OEM_COMMA;
    else if (c == '.') ip.ki.wVk = VK_OEM_PERIOD;
    else if (c == '\b') ip.ki.wVk = VK_BACK;
    else if (c == '\t') ip.ki.wVk = VK_TAB;
    else if (c == '\n') ip.ki.wVk = VK_RETURN;
    else return;
 
    ip.ki.dwFlags = 0; // 0 for key press
    SendInput(1, &ip, sizeof(INPUT));
 
    // Release the key
    ip.ki.dwFlags = KEYEVENTF_KEYUP; // KEYEVENTF_KEYUP for key release
    SendInput(1, &ip, sizeof(INPUT));
}
 
int main(int argc, char *argv[])
{
    int n = 1;
     
    start_serial();
 
    // Read text and print to console (and maybe simulate keystrokes)
    int state = 1;
    //int i;
    char c;
    char message_buffer[MESSAGE_LENGTH];
    DWORD bytes_read;
 
    // Depending on whether a robot id has been specified, either
    // print all incoming characters to the console or filter by
    // the specified id number
    printf("%d", id);
    if (id == -1)
    {
        // No robot id specified - print everything to console
        while(1)
        {
            ReadFile(hSerial, &c, 1, &bytes_read, NULL);
            if (bytes_read == 1)
            {
                printf("%c", c);
                if (simulate_keystrokes == 1) simulate_keystroke(c);
            }
        }
    }
    else
    {
        // A robot id was specified - use a state machine to parse
        // incoming text and display only messages in the correct
        // format matching the specified id number
        while(1)
        {
            // Read next character
            ReadFile(hSerial, &c, 1, &bytes_read, NULL);
            if (bytes_read != 1) continue;
 
            if (state == 1)
            {
                // State 1: Waiting for '<' character to start message
                n = 0;
                strcpy(message_buffer, "");
                if (c == '<') state = 2;
            }
            else if (state == 2)
            {
                // State 2: Reading robot id one digit at a time
                if (c >= '0' && c <= '9') n = 10*n + (c-48);
                else if (c == '@') state = 3;
                else state = 1;
            }
            else if (state == 3)
            {
                // Got a '@' character, so now reading actual message
                if (n != id) state = 1;
                else if (c != '>') strncat(message_buffer, &c, 1);
                else if (c == '>') state = 4;
                if (strlen(message_buffer) >= MESSAGE_LENGTH-1) state = 4;
            }
            else if (state == 4)
            {
                // State 4: Display complete message
                fprintf(stdout, "%s\n", message_buffer);
                state = 1;
            }
            else state = 1;
        }
    }
 
    return 0;
}