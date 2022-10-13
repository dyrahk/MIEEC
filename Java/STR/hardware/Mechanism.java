/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author Rafael
 */
public class Mechanism {
    Hardware hardware;
    private Cylinder cyl_1, cyl_2, cyl_3;
    public Mechanism(Hardware hard, Cylinder c1, Cylinder c2, Cylinder c3) {
        this.hardware = hard;
        this.cyl_1 = c1;
        this.cyl_2 = c2;
        this.cyl_3 = c3;
    } 
    
    
    public Mechanism(Hardware h){
    hardware = h;
    }
    public void conveyorMove(){ //StartConveyor
        Hardware.enterCriticalArea();
        int x = hardware.SafeReadPort(2);
        x= hardware.setBitValue(x, 2 , true);
        hardware.SafeWritePort(2, x);
        Hardware.leaveCriticalArea();
    }
    
    public void conveyorStop(){ //Stop Conveyor
        Hardware.enterCriticalArea();
        int x = hardware.SafeReadPort(2);
        x= hardware.setBitValue(x, 2, false);
        hardware.SafeWritePort(2, x);
        Hardware.leaveCriticalArea();
    }
    public void cylinder_1_moveBackward() {
        this.cyl_1. moveBackward();
    }
    public void cylinder_1_moveForward() {
        this.cyl_1. moveForward();
    }
    public void cylinder_1_stop() {
        this.cyl_1.stop();
    }
    public void cylinder_1_Goto(int pos) { //pos = 0 or 1
        this.cyl_1. Goto(pos);
    }

    public int cylinder_1_getPosition() {
        return this.cyl_1.getPosition();
    }
    public boolean cylinder_1_isAtPosition (int pos) {
        return ( pos==this.cyl_1.getPosition() );
    }

    public void cylinder_2_moveBackward() {
        this.cyl_2. moveBackward();
    }
    public void cylinder_2_moveForward() {
        this.cyl_2. moveForward();
    }
    public void cylinder_2_stop() {
        this.cyl_2.stop();
    }
    public void cylinder_2_Goto(int pos) { //pos = 0 or 1
        this.cyl_2. Goto(pos);
    }

    public int cylinder_2_getPosition() {
        return this.cyl_2.getPosition();
    }
    public boolean cylinder_2_isAtPosition (int pos) {
        return ( pos==this.cyl_2.getPosition() );
    }
    
    public void cylinder_3_moveBackward() {
        this.cyl_3. moveBackward();
    }
    public void cylinder_3_moveForward() {
        this.cyl_3. moveForward();
    }
    public void cylinder_3_stop() {
        this.cyl_3.stop();
    }
    public void cylinder_3_Goto(int pos) { //pos = 0 or 1
        this.cyl_3. Goto(pos);
    }

    public int cylinder_3_getPosition() {
        return this.cyl_3.getPosition();
    }
    public boolean cylinder_3_isAtPosition (int pos) {
        return ( pos==this.cyl_3.getPosition() );
    }

    public int identify_part() {
        int nPatchs=0;
        int v=hardware.SafeReadPort(0);
        int s1=0, s2=0;
        // account the number of patches, in bits P1.5 and P1.6
        // until bit P0.0 goes to 1
        // return 0,1, or 2
        // like this:
        
        while( ! hardware.getBitValue(v, 0) ) {
            v=hardware.SafeReadPort(1);
            if(hardware.getBitValue(v, 5)) s1=1;
            if(hardware.getBitValue(v, 6)) s2=1;
            v=hardware.SafeReadPort(0);
        }
        nPatchs=s1+s2;
        System.out.printf("nPatchs= %d", nPatchs);
        return nPatchs;
    }
    
    public void callibration(){
        this.cyl_1.Goto(1);
        this.cyl_2.Goto(1);
        this.cyl_3.Goto(1);
        this.cyl_1.Goto(0);
        this.cyl_2.Goto(0);
        this.cyl_3.Goto(0);
    }
    
    public void light_On(){
        Hardware.enterCriticalArea();
        int v= hardware.SafeReadPort(2);
        v= hardware.setBitValue(v, 7, true);
        hardware.SafeWritePort(2, v);
        Hardware.leaveCriticalArea();
    }
    
    public void light_Off(){
        Hardware.enterCriticalArea();
        int v= hardware.SafeReadPort(2);
        v= hardware.setBitValue(v, 7, false);
        hardware.SafeWritePort(2, v);
        Hardware.leaveCriticalArea();
    }
}
