/*
 * Sistemas de Telecomunicacoes 
 *          2016/2017
 */
package protocol;

import terminal.Simulator;
import simulator.Frame;
import terminal.NetworkLayer;
import terminal.Terminal;

/**
 * Protocol 5 : Selective Repeat protocol
 *
 * @author ????? and ????? (Put here your student numbers)
 */
public class SelectiveRepeat extends Base_Protocol implements Callbacks {

    public SelectiveRepeat(Simulator _sim, NetworkLayer _net) {
        super(_sim, _net);      // Calls the constructor of Base_Protocol

        // Initialize object fields
        buff = new String[sim.get_max_sequence()+1];
        next_frame_to_send = 0;
        frame_expected = 0;
        old_frame = 0;
        ack_sent = new int[sim.get_max_sequence()+1]; //0 se n recebeu ack; 1 se o ack já foi recebido; null se não tiver nada a mandar
       
    }

    
     private boolean next_frame_send(){
        String packet= net.from_network_layer();
        if(packet != null){
           frame = Frame.new_Data_Frame(next_frame_to_send, prev_seq(frame_expected), packet);
            buff[next_frame_to_send]= frame.info();
            ack_sent[next_frame_to_send]=0; // 0- ainda n recebido
            sim.to_physical_layer(frame);
            next_frame_to_send = next_seq(next_frame_to_send);

           if(sim.isactive_ack_timer()){
                sim.cancel_ack_timer();
                }
           
            n_ack++;
            return true;
        }
        return false;
    }
    /**
     * CALLBACK FUNCTION: handle the beginning of the simulation event
     *
     * @param time current simulation time
     */
    @Override
    public void start_simulation(long time) {
        sim.Log("\nSelective Repeat Protocol\n\n");
        next_frame_send();
    }

    /**
     * CALLBACK FUNCTION: handle the end of Data frame transmission, start timer
     * and send next until reaching the end of the sending window
     *
     * @param time current simulation time
     * @param seq sequence number of the Data frame transmitted
     */
    @Override
    public void handle_Data_end(long time, int seq) {
        sim.start_data_timer(seq);
        if(n_ack < sim.get_send_window())
            next_frame_send();
    }

    /**
     * CALLBACK FUNCTION: handle the timer event; retransmit failed frames
     *
     * @param time current simulation time
     * @param key timer key
     */
    @Override
    public void handle_Data_Timer(long time, int key) {
        sim.Log(time + " Data Timeout ("+key+")\n");
        
        if(!sim.isactive_data_timer(key) && buff[key]!=null ){
            frame.set_DATA_frame(key , prev_seq(frame_expected), buff[key]);
            sim.to_physical_layer(frame);    
        }
    }

    /**
     * CALLBACK FUNCTION: handle the ack timer event; send ACK frame
     *
     * @param time current simulation time
     */
    @Override
    public void handle_ack_Timer(long time) {
        if (Terminal.debug) {
            sim.Log(time + "Ack_timeout\n");
        }
        Frame frameack = Frame.new_Ack_Frame(prev_seq(frame_expected));
        sim.to_physical_layer(frameack);
    }

    public void ack_check(Frame frame){
        int aux = frame.ack();
        int i;
        for(i=0;i<sim.get_max_sequence()+1;i++){
            if(ack_sent[i]==0){
                
            }
        }
            
            while(between(old_frame,aux,next_frame_to_send)){
            sim.cancel_data_timer(old_frame);
            n_ack--;
            buff[old_frame]=null;
            old_frame=next_seq(old_frame);
            }
            if(n_ack<sim.get_send_window()){
                next_frame_send();
            }
    }
    
    /**
     * CALLBACK FUNCTION: handle the reception of a frame from the physical
     * layer
     *
     * @param time current simulation time
     * @param frame frame received
     */
    @Override
    public void from_physical_layer(long time, Frame frame) {
       sim.Log(time + " Frame received: " + frame.toString() + "\n");
        
        if (frame.kind() == Frame.ACK_FRAME)
       {
            ack_check(frame);
            
       }
        if (frame.kind() == Frame.DATA_FRAME) 
        {     // Check the frame kind
            
            sim.start_ack_timer();
            ack_check(frame);
            if (frame.seq()== frame_expected )
            {    // Check the sequence number
                net.to_network_layer(frame.info()); // Send the frame to the network layer
                frame_expected = next_seq(frame_expected);
         
            } 
  
        }
    }

    /**
     * CALLBACK FUNCTION: handle the end of the simulation
     *
     * @param time current simulation time
     */
    @Override
    public void end_simulation(long time) {
        sim.Log("Stopping simulation\n");
    }

    /* Variables */
    /**
     * Reference to the simulator (Terminal), to get the configuration and send
     * commands
     */
    //final Simulator sim;  -  Inherited from Base_Protocol
    /**
     * Reference to the network layer, to send a receive packets
     */
    //final NetworkLayer net;    -  Inherited from Base_Protocol
    
    private int next_frame_to_send;
    private int frame_expected;
    private Frame frame;
    private String [] buff;
    private int [] ack_sent;
    private int old_frame; 
    private int n_ack; //por confirmar
}
