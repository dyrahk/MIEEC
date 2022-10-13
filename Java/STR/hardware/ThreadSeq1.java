
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
public class ThreadSeq1 extends Thread{
    
    Hardware hardware;
    private Mechanism mechanism;
    private static final Semaphore semaphore = new Semaphore(1);
    boolean is_working;
    int seq1,seq2,parts1,parts2,parts3,recycled;
    
    public ThreadSeq1(Hardware hard, Mechanism mechanism) {
        this.hardware=hard;
        this.mechanism=mechanism;
        is_working=false;
        seq1=seq2=parts1=parts2=parts3=recycled=0;
    }
    
    public boolean isWorking(){
        return is_working;
    }
    public void flash(int s_val, int limit){
        long time, time2;
        time=System.currentTimeMillis();
        do{
            mechanism.light_On();
            try {
            sleep(s_val);
            } catch (InterruptedException ex) {
                Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
            }
            mechanism.light_Off();
            try {
                sleep(s_val);
            } catch (InterruptedException ex) {
                Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
            }
            time2 = System.currentTimeMillis();
        }while((time2 - time) < limit);
    }
   
    public void stats(){
        System.out.printf("\nStation 1 sequences completed : %d\n", seq1);
        System.out.printf("Station 2 sequences completed : %d\n", seq2);
        System.out.printf("Total A parts : %d\n", parts1);
        System.out.printf("Total B parts : %d\n", parts2);
        System.out.printf("Total C parts : %d\n", parts3);
        System.out.printf("Total Recycled parts : %d\n\n", recycled);
    }
    public void run() {
        System.out.println("ThreadSeq1 just started....");
        int[] sequence1={1,1,2,2,3,3};
        int[] sequence2={1,2,3,1,2,3};
        int nextDesiredPart1=0; //it needs to 
        int nextDesiredPart2=0;
        
        
        while(true) {
            try {
                sleep(1);
            } catch (InterruptedException ex) {
                Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
            }
            
            Integer partIncoming = ThreadIdentify.peekPart();
            Integer button1Part = ThreadButtons.peekPart1();
            Integer button2Part = ThreadButtons.peekPart2();
            
            if(partIncoming!=null && ThreadButtons.peekPart1() != null){ //Se o botao 1 foi premido pelo utilizador
                if(ThreadIdentify.TryConsumeMessage() !=null && ThreadButtons.TryConsumeMessage1() !=null){
                    is_working=true;
                    try {
                        
                        semaphore.acquire();
                        mechanism.conveyorStop();
                        mechanism.cylinder_2_Goto(1);
                        mechanism.cylinder_2_Goto(0);
                        mechanism.conveyorMove();
                        semaphore.release();
                        flash(250, 1000);
                        if(button1Part==1) parts1+=1;
                        if(button1Part==2) parts2+=1;
                        if(button1Part==3) parts3+=1;
                    } catch (InterruptedException ex) {
                        Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                    }
                    is_working=false;
                    stats();
                }
            }
            else 
                if(partIncoming!=null && ThreadButtons.peekPart2() != null){ //Se o botao 2 foi premido pelo utilizador
                    if(ThreadIdentify.TryConsumeMessage() !=null && ThreadButtons.TryConsumeMessage2() !=null){
                        int v;
                        is_working=true;
                        do{
                            v=hardware.SafeReadPort(1);
                            try {
                                    sleep(1);
                                } catch (InterruptedException ex) {
                                    Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                                }
                                }while(!hardware.getBitValue(v, 7));
                        try {
                            semaphore.acquire();
                            mechanism.conveyorStop();
                            mechanism.cylinder_3_Goto(1);
                            mechanism.cylinder_3_Goto(0);
                            mechanism.conveyorMove();
                            semaphore.release();
                            flash(250, 2000);
                            if(button2Part==1) parts1+=1;
                            if(button2Part==2) parts2+=1;
                            if(button2Part==3) parts3+=1;
                            } catch (InterruptedException ex) {
                                Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                            }
                        is_working=false;
                        stats();
                    }
                }
                else
                    if(partIncoming!=null && partIncoming ==sequence1[nextDesiredPart1])  { //Seq1 wants the incoming part
                        if(ThreadIdentify.TryConsumeMessage() !=null) {
                            is_working=true;
                            try {
                                //if we get here, then seq1 can take the incoming part
                                // stop conveyor
                                // do goto(1), goto(0)
                                //update next desired part
                                semaphore.acquire();
                                mechanism.conveyorStop();
                                mechanism.cylinder_2_Goto(1);
                                mechanism.cylinder_2_Goto(0);
                                mechanism.conveyorMove();
                                semaphore.release();
                                if(sequence1[nextDesiredPart1]==1) parts1+=1;
                                if(sequence1[nextDesiredPart1]==2) parts2+=1;
                                if(sequence1[nextDesiredPart1]==3) parts3+=1;
                                nextDesiredPart1+=1;
                                if (nextDesiredPart1==6){
                                    nextDesiredPart1=0;
                                    seq1+=1;
                                }
                                flash(250, 1000);

                            } catch (InterruptedException ex) {
                                Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                            }
                            is_working=false;
                            stats();
                        }
                    }
            
                    else 
                        if(partIncoming!=null && partIncoming == sequence2[nextDesiredPart2]) { // Sequencia 2

                            int v;
                            is_working=true;
                            do{
                                v=hardware.SafeReadPort(1);
                                try {
                                    sleep(1);
                                } catch (InterruptedException ex) {
                                    Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                                }
                            }while(!hardware.getBitValue(v, 7));

                            if(ThreadIdentify.TryConsumeMessage() != null) {
                                try {
                                    semaphore.acquire();
                                    mechanism.conveyorStop();
                                    mechanism.cylinder_3_Goto(1);
                                    mechanism.cylinder_3_Goto(0);
                                    mechanism.conveyorMove();
                                    semaphore.release();
                                    if(sequence2[nextDesiredPart2]==1) parts1+=1;
                                    if(sequence2[nextDesiredPart2]==2) parts2+=1;
                                    if(sequence2[nextDesiredPart2]==3) parts3+=1;
                                    nextDesiredPart2+=1;
                                    if (nextDesiredPart2==6){
                                        nextDesiredPart2=0;
                                        seq2+=1;
                                    }
                                    flash(250, 2000);
                                } catch (InterruptedException ex) {
                                    Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                                }     
                            }
                            is_working=false;
                            stats();
                        } 

                        else
                            if(partIncoming!=null){
                                int v;
                                is_working=true;
                                do{
                                    v=hardware.SafeReadPort(1);
                                    try {
                                        sleep(1);
                                    } catch (InterruptedException ex) {
                                        Logger.getLogger(ThreadSeq1.class.getName()).log(Level.SEVERE, null, ex);
                                    }
                                }while(!hardware.getBitValue(v, 7));

                                if(ThreadIdentify.TryConsumeMessage() != null) {
                                    flash(100, 1000);
                                }
                                recycled+=1;
                                is_working=false;
                                stats();
                            }
        }
                                     
    }
}
