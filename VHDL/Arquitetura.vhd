----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    19:52:21 12/17/2019 
-- Design Name: 
-- Module Name:    Arquitetura - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_LOGIC_ARITH.ALL;
use IEEE.STD_LOGIC_UNSIGNED.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity Arquitetura is
	
	Port( clk: in std_logic;
			reset: in std_logic;
			enable: in std_logic;
			sel: in std_logic_vector (2 downto 0);
			i: in std_logic_vector (2 downto 0);
			piso: in std_logic;
			circular: in std_logic;
			total: in std_logic;
			
			CANC_IN1 : OUT STD_LOGIC;
			CANC_OUT1 : OUT STD_LOGIC;
			CANC_IN2 : OUT STD_LOGIC;
			CANC_OUT2 : OUT STD_LOGIC;
			CANC_OUT3 : OUT STD_LOGIC;
			CAP0 : OUT INTEGER RANGE 0 TO 50;
			CAP1 : OUT INTEGER RANGE 0 TO 50;
			CIRC0 : OUT INTEGER RANGE 0 TO 100;
			CIRC1 : OUT INTEGER RANGE 0 TO 100;
			LED: out std_logic_vector(7 downto 0);
			rgb_out : OUT std_logic_vector(2 downto 0);
			hs : OUT std_logic;
			vs : OUT std_logic
			
		 );
end Arquitetura;

architecture Behavioral of Arquitetura is

COMPONENT Demux
	PORT(
		ENABLE : IN std_logic;
		SEL : IN std_logic_vector(2 downto 0);
		I : IN std_logic_vector(2 downto 0);          
		O : OUT std_logic_vector(23 downto 0)
		);
	END COMPONENT;
	
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
  
  COMPONENT DATA2LED
	PORT(
		CLK : IN std_logic;
		CAPT : IN INTEGER RANGE 0 TO 100;         
		LED : OUT std_logic_vector(7 downto 0)
		);
	END COMPONENT;
	
	COMPONENT info2vga
	PORT(
		clk : IN std_logic;
		piso : IN std_logic;
		circular : IN std_logic;
		total : IN std_logic;
		CAPT : IN INTEGER RANGE 0 TO 100;
		CAP0 : IN INTEGER RANGE 0 TO 50;
		CAP1 : IN INTEGER RANGE 0 TO 50;
		CIRC0 : IN INTEGER RANGE 0 TO 100;
		CIRC1 : IN INTEGER RANGE 0 TO 100;          
		char : OUT std_logic_vector(3 downto 0);
		pos_selector : OUT std_logic_vector(2 downto 0);
		rgb_in : OUT std_logic_vector(2 downto 0)
		);
	END COMPONENT;

	COMPONENT vga
	PORT(
		clk : IN std_logic;
		reset : IN std_logic;
		char : IN std_logic_vector(3 downto 0);
		pos_selector : IN std_logic_vector(2 downto 0);
		PosandZoom : IN std_logic;
		rgb_in : IN std_logic_vector(2 downto 0);          
		rgb_out : OUT std_logic_vector(2 downto 0);
		hs : OUT std_logic;
		vs : OUT std_logic
		);
	END COMPONENT;

	signal o_aux: std_logic_vector(23 downto 0);
	--signal enable_aux: std_logic:='1';
	signal capt_aux,circ0_aux,circ1_aux: INTEGER RANGE 0 TO 100;
	signal cap0_aux, cap1_aux: INTEGER RANGE 0 TO 50;
	signal pos_aux,rgb_aux: std_logic_vector(2 downto 0);
	signal char_aux: std_logic_vector(3 downto 0);
	
	signal s_reset: std_logic;
	signal s_enable: std_logic;
	signal s_sel: std_logic_vector (2 downto 0);
	signal s_i: std_logic_vector (2 downto 0);
	signal s_piso:  std_logic;
	signal s_circular: std_logic;
	signal s_total: std_logic;
	
begin

	s_reset <= reset when rising_edge(clk);
	s_enable <= enable when rising_edge(clk);
	s_sel <= sel when rising_edge(clk);
	s_i <= i when rising_edge(clk);
	s_piso <= piso when rising_edge(clk);
	s_circular <= circular when rising_edge(clk);
	s_total <= total when rising_edge(clk);
	
	Inst_Demux: Demux PORT MAP(
		ENABLE => s_enable,
		SEL => s_sel,
		I => s_i,
		O => o_aux
	);
	
	Inst_trabfinal: trabfinal PORT MAP(
		Clk => clk,
		PRES1_IN1 => o_aux(0),
		PRES2_IN1 => o_aux(1),
		TICKET_IN1 => o_aux(2),
		PRES1_OUT1 => o_aux(6),
		PRES2_OUT1 => o_aux(7),
		TICKET_OUT1 => o_aux(8),
		PRES1_IN2 => o_aux(3),
		PRES2_IN2 => o_aux(4),
		TICKET_IN2 => o_aux(5),
		PRES1_OUT2 => o_aux(9),
		PRES2_OUT2 => o_aux(10),
		TICKET_OUT2 => o_aux(11),
		PRES1_OUT3 => o_aux(12),
		PRES2_OUT3 => o_aux(13),
		TICKET_OUT3 => o_aux(14),
		AND01a => o_aux(15),
		AND01b => o_aux(16),
		Park0 => o_aux(18),
		Unpark0 => o_aux(19),
		Park1 => o_aux(21),
		Unpark1 => o_aux(22),
		CANC_IN1 => CANC_IN1,
		CANC_OUT1 => CANC_OUT1,
		CANC_IN2 => CANC_IN2,
		CANC_OUT2 => CANC_OUT2,
		CANC_OUT3 => CANC_OUT3,
		CAPT => capt_aux,
		CAP0 => cap0_aux,
		CAP1 => cap1_aux,
		CIRC0 => circ0_aux,
		CIRC1 => circ1_aux,
		Enable => '1',
		Reset => s_reset
	);
	
	Inst_DATA2LED: DATA2LED PORT MAP(
		CLK => clk,
		CAPT => capt_aux,
		LED => LED
	);
	
	Inst_info2vga: info2vga PORT MAP(
		clk => clk,
		piso => s_piso,
		circular => s_circular,
		total => s_total,
		CAPT => capt_aux,
		CAP0 => cap0_aux,
		CAP1 => cap1_aux,
		CIRC0 => circ0_aux,
		CIRC1 => circ1_aux,
		char => char_aux,
		pos_selector => pos_aux,
		rgb_in => rgb_aux
	);
	
	Inst_vga: vga PORT MAP(
		clk => clk,
		reset => s_reset,
		char => char_aux,
		pos_selector => pos_aux,
		PosandZoom => '0',
		rgb_in =>rgb_aux ,
		rgb_out => rgb_out,
		hs => hs,
		vs => vs
	);

end Behavioral;

