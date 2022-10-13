
import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author Rafael
 */
public class Cylinder_3 implements Cylinder{
   
    Hardware hardware;
    public Cylinder_3(Hardware __hardware){
    this.hardware = __hardware;
    }

    @Override
    public void moveForward() {
        Hardware.enterCriticalArea();
        int v = hardware.SafeReadPort(2);
        v = hardware.setBitValue(v, 6, true);
        v = hardware.setBitValue(v, 5, false);
        hardware.SafeWritePort(2, v);
        Hardware.leaveCriticalArea();
    }

    @Override
    public void moveBackward() {
        Hardware.enterCriticalArea();
        int v = hardware.SafeReadPort(2);
        v = hardware.setBitValue(v, 5, true);
        v = hardware.setBitValue(v, 6, false);
        hardware.SafeWritePort(2, v);
        Hardware.leaveCriticalArea();
    }

    @Override
    public void stop() {
        Hardware.enterCriticalArea();
        int v = hardware.SafeReadPort(2);
        v = hardware.setBitValue(v, 6, false);
        v = hardware.setBitValue(v, 5, false);
        hardware.SafeWritePort(2, v);
        Hardware.leaveCriticalArea();
    }

    @Override
    public void Goto(int position) {
        if(getPosition()!=position){
            if(position==0) moveBackward();
            if(position==1) moveForward();
            while(getPosition()!=position){
                try {
                    TimeUnit.MILLISECONDS.sleep(1);
                } catch (InterruptedException ex) {
                    Logger.getLogger(Cylinder_1.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
            stop();
        }
        /*if (get_x_pos() < x)
		move_x_right();
	else if (get_x_pos() > x)
		move_x_left();
	//   while position not reached
	while (get_x_pos() != x)
		Sleep(1);
	stop_x();*/   
    }

    @Override
    public int getPosition() {
        int v = hardware.SafeReadPort(0);
        if(!hardware.getBitValue(v, 2))
        return 0;
        else if( !hardware.getBitValue(v, 1))
        return 1;
        return -1;
    }
     
}
