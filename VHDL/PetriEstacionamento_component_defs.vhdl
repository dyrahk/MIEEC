-- Net trabfinal - IOPT
-- Automatic code generated by IOPT2VHDL XSLT transformation.
-- by GRES Research Group - 2014 




  -- Component Definition:
  Component trabfinal IS
  Port(
      Clk : IN STD_LOGIC;
      PRES1_IN1 : IN STD_LOGIC;
      PRES2_IN1 : IN STD_LOGIC;
      TICKET_IN1 : IN STD_LOGIC;
      PRES1_OUT1 : IN STD_LOGIC;
      PRES2_OUT1 : IN STD_LOGIC;
      TICKET_OUT1 : IN STD_LOGIC;
      PRES1_IN2 : IN STD_LOGIC;
      PRES2_IN2 : IN STD_LOGIC;
      TICKET_IN2 : IN STD_LOGIC;
      PRES1_OUT2 : IN STD_LOGIC;
      PRES2_OUT2 : IN STD_LOGIC;
      TICKET_OUT2 : IN STD_LOGIC;
      PRES1_OUT3 : IN STD_LOGIC;
      PRES2_OUT3 : IN STD_LOGIC;
      TICKET_OUT3 : IN STD_LOGIC;
      AND01a : IN STD_LOGIC;
      AND01b : IN STD_LOGIC;
      Park0 : IN STD_LOGIC;
      Unpark0 : IN STD_LOGIC;
      Park1 : IN STD_LOGIC;
      Unpark1 : IN STD_LOGIC;
      CANC_IN1 : OUT STD_LOGIC;
      CANC_OUT1 : OUT STD_LOGIC;
      CANC_IN2 : OUT STD_LOGIC;
      CANC_OUT2 : OUT STD_LOGIC;
      CANC_OUT3 : OUT STD_LOGIC;
      CAPT : OUT INTEGER RANGE 0 TO 100;
      CAP0 : OUT INTEGER RANGE 0 TO 50;
      CAP1 : OUT INTEGER RANGE 0 TO 50;
      CIRC0 : OUT INTEGER RANGE 0 TO 100;
      CIRC1 : OUT INTEGER RANGE 0 TO 100;
      Enable : IN STD_LOGIC;
      Reset : IN STD_LOGIC
  );
  End Component trabfinal;



    -- Port Map Template:
    U_trabfinal : trabfinal Port Map(
        Clk => clk,
        PRES1_IN1 => PRES1_IN1,
        PRES2_IN1 => PRES2_IN1,
        TICKET_IN1 => TICKET_IN1,
        PRES1_OUT1 => PRES1_OUT1,
        PRES2_OUT1 => PRES2_OUT1,
        TICKET_OUT1 => TICKET_OUT1,
        PRES1_IN2 => PRES1_IN2,
        PRES2_IN2 => PRES2_IN2,
        TICKET_IN2 => TICKET_IN2,
        PRES1_OUT2 => PRES1_OUT2,
        PRES2_OUT2 => PRES2_OUT2,
        TICKET_OUT2 => TICKET_OUT2,
        PRES1_OUT3 => PRES1_OUT3,
        PRES2_OUT3 => PRES2_OUT3,
        TICKET_OUT3 => TICKET_OUT3,
        AND01a => AND01a,
        AND01b => AND01b,
        Park0 => Park0,
        Unpark0 => Unpark0,
        Park1 => Park1,
        Unpark1 => Unpark1,
        CANC_IN1 => CANC_IN1,
        CANC_OUT1 => CANC_OUT1,
        CANC_IN2 => CANC_IN2,
        CANC_OUT2 => CANC_OUT2,
        CANC_OUT3 => CANC_OUT3,
        CAPT => CAPT,
        CAP0 => CAP0,
        CAP1 => CAP1,
        CIRC0 => CIRC0,
        CIRC1 => CIRC1,
        Enable => enable,
        Reset => reset
    );

