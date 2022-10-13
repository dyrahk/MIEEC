public class Hello {
 /*static public void main(String args[]) {
 System.out.println("Hello world...");
 Hardware h = new Hardware();
 System.out.println("Hello world..2.");
 h.create_di(0);
 System.out.println("Hello world..3.");
 }*/
        public static void main(String []args){       
        Hardware h = new Hardware();
        h.create_di(0);                
        h.create_di(1);                
        h.create_do(2);                        
        Cylinder cyl1= new Cylinder_1(h);
        Cylinder cyl2= new Cylinder_2(h);
        Cylinder cyl3= new Cylinder_3(h);
        Mechanism mechanism = new Mechanism(h, cyl1, cyl2, cyl3);
        ThreadIdentify threadIdentify = new ThreadIdentify(mechanism);
        ThreadSeq1 threadSeq1 = new ThreadSeq1(h, mechanism);
        ThreadButtons threadButtons = new ThreadButtons(h, mechanism);
        ////////
        threadIdentify.start();
        threadSeq1.start();
        threadButtons.start();
        
        
        java.awt.EventQueue.invokeLater(new Runnable(){
            public void run(){
                new GUI(h, mechanism, threadIdentify, threadSeq1).setVisible(true);
            }
        });
    }   
}
