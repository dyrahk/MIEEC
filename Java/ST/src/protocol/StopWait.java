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
 * Protocol 3 : Stop & Wait protocol
 * 
 * @author 45558 and 43853(Put here your student numbers)
 */
public class StopWait extends Base_Protocol implements Callbacks {

    public StopWait(Simulator _sim, NetworkLayer _net) {
        super(_sim, _net);      // Calls the constructor of Base_Protocol
        // Initialize here all variables
        next_frame_to_send = 0;
        frame_expected = 0;
        
    }


    /**
     * CALLBACK FUNCTION: handle the beginning of the simulation event
     * @param time current simulation time
     */
    @Override
    public void start_simulation(long time) {
     sim.Log("\nStop&Wait Protocol\n\n");
     next_frame_send();
    }

    private boolean next_frame_send(){
        String packet= net.from_network_layer();
        if(packet != null){
            Frame frame = Frame.new_Data_Frame(next_frame_to_send, prev_seq(frame_expected), packet);
            if(sim.isactive_ack_timer()){
                sim.cancel_ack_timer();
            }
            
            sim.to_physical_layer(frame);
            next_frame_to_send = next_seq(next_frame_to_send);
            return true;
        }
        return false;
    }
    
    /**
     * CALLBACK FUNCTION: handle the end of Data frame transmission, start timer
     * @param time current simulation time
     * @param seq  sequence number of the Data frame transmitted
     */
    @Override
    public void handle_Data_end(long time, int seq) {
        sim.start_data_timer(seq);
    }
    
    
    /**
     * CALLBACK FUNCTION: handle the timer event; retransmit failed frames
     * @param time current simulation time
     * @param key  timer key
     */
    @Override
    public void handle_Data_Timer(long time, int key) {
        sim.Log(time + " Data Timeout ("+key+")\n");
        
        if(!sim.isactive_data_timer(key)){
            frame.set_DATA_frame(frame.seq(), prev_seq(frame_expected), frame.info());

            sim.to_physical_layer(frame);
        }
    }

    
    /**
     * CALLBACK FUNCTION: handle the ack timer event; send ACK frame
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

    
    /**
     * CALLBACK FUNCTION: handle the reception of a frame from the physical layer
     * @param time current simulation time
     * @param frame frame received
     */
    @Override
    public void from_physical_layer(long time, Frame frame) {
        sim.Log(time + " Frame received: " + frame.toString() + "\n");
        
        if (frame.kind() == Frame.DATA_FRAME) 
        {     // Check the frame kind
            if (frame.seq()== frame_expected )
            {    // Check the sequence number
                net.to_network_layer(frame.info()); // Send the frame to the network layer
                frame_expected = next_seq(frame_expected);
          
            } 

            sim.start_ack_timer();
            
            if(frame.ack()==prev_seq(next_frame_to_send)) 
            {     // Check sequence number
                sim.cancel_data_timer(prev_seq(next_frame_to_send));     //Cancel data timer
               
                next_frame_send();
            }
  
        }
               
       if (frame.kind() == Frame.ACK_FRAME)
       {
            if (frame.ack() == prev_seq(next_frame_to_send)) {
            sim.cancel_data_timer(prev_seq(next_frame_to_send));
            
            next_frame_send();
            }
       }

        
    }

    
    /**
     * CALLBACK FUNCTION: handle the end of the simulation
     * @param time current simulation time
     */
    @Override
    public void end_simulation(long time) {
        sim.Log("Stopping simulation\n");
    }
    
    
    /* Variables */
    
    /**
     * Reference to the simulator (Terminal), to get the configuration and send commands
     */
    //final Simulator sim;  -  Inherited from Base_Protocol
    
    /**
     * Reference to the network layer, to send a receive packets
     */
    //final NetworkLayer net;    -  Inherited from Base_Protocol
private int next_frame_to_send;
private int frame_expected;
private Frame frame;
}

