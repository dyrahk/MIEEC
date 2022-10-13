
import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.Semaphore;

public class ThreadIdentify extends Thread{
    
    static Integer lock = new Integer(0);
    private Mechanism mechanism;    
    private static final Semaphore semaphore = new Semaphore(0);
    private static final ArrayBlockingQueue messageQueue = new ArrayBlockingQueue(10);
    public boolean is_working;
                
    public ThreadIdentify(Mechanism mechanism) {
        this.mechanism = mechanism;
        is_working=false;
    }
    
    public boolean isWorking(){
        return is_working;
    }
    public static void startIdentification() {
        semaphore.release();
    }
        
    public static Integer peekPart() {
        return (Integer)messageQueue.peek();
    }
        
   public static Integer consumeMessage() {
        try {
            return (Integer)messageQueue.take();
        } catch (InterruptedException ex) {
            ex.printStackTrace();
        }
        return null;
    }
    
   public static Integer TryConsumeMessage() {
        synchronized (lock) {
            if (messageQueue.peek() != null) {
                try {
                    return (Integer) messageQueue.take();
                } catch (InterruptedException ex) {
                    ex.printStackTrace();
                }
            }
            return null;
        } 
    }



    public void run() {
        System.out.println("ThreadIdentify just started....");
        
        while(true) {            
            try {
                ThreadIdentify.semaphore.acquire();
                is_working=true;
                int part = mechanism.identify_part() + 1;
                messageQueue.add(part);
                is_working=false;
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
        }
        
    }
    
}