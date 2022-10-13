/*
 * Sistemas de Telecomunicacoes 
 *          2016/2017
 */
package protocol;

import terminal.Simulator;
import simulator.Frame;
import terminal.NetworkLayer;

/**
 * Protocol 2 : Simplex Sender protocol which does not receive frames
 * 
 * @author 45558 and 43853 (Put here your student numbers)
 */
public class Simplex_snd extends Base_Protocol implements Callbacks {

    public Simplex_snd(Simulator _sim, NetworkLayer _net) {
        super(_sim, _net);      // Calls the constructor of Base_Protocol
        next_frame_to_send = 0;
        // ?
    }
   
     private boolean next_frame_send()
{
    // So podemos enviar um Data packet de cada vez
    //   devemos esperar pelo  DATA_END antes de enviar outro
    //   caso contrario o pacote pode se perder no canal
    String packet= net.from_network_layer();
    if (packet != null)
    {
        // The ACK field of the DATA frame is always the sequence number before zero, because no packets will be received
        frame = Frame.new_Data_Frame(next_frame_to_send, prev_seq(0), packet);
        sim.to_physical_layer(frame);
        next_frame_to_send= next_seq(next_frame_to_send); 
       
        return true;
        }
        return false;
    }

    
    /**
     * CALLBACK FUNCTION: handle the beginning of the simulation event
     * @param time current simulation time
     */
    @Override
    public void start_simulation(long time) {
        sim.Log("\nSimplex Sender Protocol\n\tOnly send data!\n\n");
        next_frame_send();     
    }

    /**
     * CALLBACK FUNCTION: handle the end of Data frame transmission, start timer
     * @param time current simulation time
     * @param seq  sequence number of the Data frame transmitted
     */
    @Override
    public void handle_Data_end(long time, int seq) {
        sim.Log(time + " Timeout " + seq + "\n");
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
            sim.to_physical_layer(frame);
        }
    }
    
    /**
     * CALLBACK FUNCTION: handle the reception of a frame from the physical layer
     * @param time current simulation time
     * @param frame frame received
     */
    @Override
    public void from_physical_layer(long time, Frame frame) {
        sim.Log(time + " Frame received: " + frame.toString() + "\n");
        if (frame.kind() == Frame.ACK_FRAME) 
       {
           if(frame.ack() == prev_seq(next_frame_to_send))
           {
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
    
    /**
     * Sequence number of the next data frame
     */
    private int next_frame_to_send;
    private Frame frame;
}
