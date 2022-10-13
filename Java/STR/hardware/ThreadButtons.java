
import static java.lang.Thread.sleep;
import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.Semaphore;
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
public class ThreadButtons extends Thread {

    Hardware hardware;
    static Integer lock = new Integer(0);
    private Mechanism mechanism;
    private static final ArrayBlockingQueue next_piece1 = new ArrayBlockingQueue(10);
    private static final ArrayBlockingQueue next_piece2 = new ArrayBlockingQueue(10);

    public ThreadButtons(Hardware hard, Mechanism mechanism) {
        hardware = hard;
        this.mechanism = mechanism;

    }

    public static Integer peekPart1() {
        return (Integer)next_piece1.peek();
    }
        
    public static Integer TryConsumeMessage1() {
        synchronized (lock) {
            if (next_piece1.peek() != null) {
                try {
                    return (Integer) next_piece1.take();
                } catch (InterruptedException ex) {
                    ex.printStackTrace();
                }
            }
            return null;
        } 
    }
    
    public static Integer peekPart2() {
        return (Integer)next_piece2.peek();
    }
        
    public static Integer TryConsumeMessage2() {
        synchronized (lock) {
            if (next_piece2.peek() != null) {
                try {
                    return (Integer) next_piece2.take();
                } catch (InterruptedException ex) {
                    ex.printStackTrace();
                }
            }
            return null;
        } 
    }
   
   
    public void run() {
        while (true) {
            long time = System.currentTimeMillis();
            long time2 = 0;
            int button1Pressed = 0, button2Pressed = 0;
            int v = hardware.SafeReadPort(1);

            if (hardware.getBitValue(v, 4)) {
                button1Pressed += 1;
                do {
                    while(hardware.getBitValue(v, 4)){
                        v = hardware.SafeReadPort(1);
                        try {
                            sleep(1);
                        } catch (InterruptedException ex) {
                            Logger.getLogger(ThreadButtons.class.getName()).log(Level.SEVERE, null, ex);
                        }
                    }
                    v = hardware.SafeReadPort(1);
                    if(hardware.getBitValue(v, 4)) button1Pressed+=1;
                    time2 = System.currentTimeMillis();
                } while ((time2 - time) < 2000);
                next_piece1.add(button1Pressed);
                System.out.printf("Button 1 pressed %d times\n", button1Pressed);
            }
            if (hardware.getBitValue(v, 3)) {
                button2Pressed += 1;
                do {
                    while(hardware.getBitValue(v, 3)){
                        v = hardware.SafeReadPort(1);
                        try {
                            sleep(1);
                        } catch (InterruptedException ex) {
                            Logger.getLogger(ThreadButtons.class.getName()).log(Level.SEVERE, null, ex);
                        }
                    }
                    v = hardware.SafeReadPort(1);
                    if(hardware.getBitValue(v, 3)) button2Pressed+=1;
                    time2 = System.currentTimeMillis();
                } while ((time2 - time) < 2000);
                System.out.printf("Button 2 pressed %d times\n", button2Pressed);
                next_piece2.add(button2Pressed);
            }
        }
    }
}
