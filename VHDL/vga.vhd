----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    16:23:29 10/30/2019 
-- Design Name: 
-- Module Name:    vga - Behavioral 
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

entity vga is
	Port ( clk: in std_logic;
			 reset: in std_logic;
			 char: in std_logic_vector(3 downto 0);
			 pos_selector: in std_logic_vector(2 downto 0); -- 4 caracteres + back color
			 rgb_in: in std_logic_vector(2 downto 0);
			 rgb_out: out std_logic_vector(2 downto 0);
			 hs: out std_logic;
			 vs: out std_logic);
	
end vga;

architecture Behavioral of vga is

	-------------Clock 25MHz (pixel clock)------------------ 
	signal clk25: std_logic:='0';
	-------------Contadores HS & VS
	signal count_hs: integer range 0 to 799:=0;
	signal count_vs: integer range 0 to 520:=0;
	-------------Para cor-----------------------------------
	signal draw_c1: std_logic_vector(2 downto 0):="000";
	signal draw_c2: std_logic_vector(2 downto 0):="000";
	signal draw_c3: std_logic_vector(2 downto 0):="000";
	signal draw_c4: std_logic_vector(2 downto 0):="000";
	signal draw_back: std_logic_vector(2 downto 0):="000";
	signal rgb_c1: std_logic_vector(2 downto 0):="111";
	signal rgb_c2: std_logic_vector(2 downto 0):="111";
	signal rgb_c3: std_logic_vector(2 downto 0):="111";
	signal rgb_c4: std_logic_vector(2 downto 0):="111";
	
	-------------Guardar caracteres-------------------------
	signal char1: std_logic_vector(3 downto 0):="0000";
	signal char2: std_logic_vector(3 downto 0):="0000";
	signal char3: std_logic_vector(3 downto 0):="0000";
	signal char4: std_logic_vector(3 downto 0):="0000";
   --------------------------------------------------------

	constant h1: integer:=0;
	constant h2: integer:=160;
	constant h3: integer:=320;
	constant h4: integer:=480;
	
	
	
	

begin

-----------------------counters VS & HS----------------------------	
	process(clk)
	begin
			if clk'event and clk='1' then 
			clk25<= not clk25;
		end if;
	end process;
	
	process(clk, reset)
	begin
		if reset ='1' then 
			count_hs<=0; 
			count_vs<=0;
		elsif clk'event and clk='1' then
			if clk25 = '1' then
				if count_hs=799 then 
					count_hs<=0; 
					if count_vs=520 then
						count_vs<=0;
					else
						count_vs<=count_vs+1;
					end if;					
				else
					count_hs<=count_hs+1;
				end if;
			end if;
		end if;
	end process;	
----------------------------------------------------------	
	hs<='0' when count_hs<96 else '1';
	vs<='0' when count_vs<2 else '1';
----------------------------------------------------------

refresh:	process(clk)
			begin
				if clk'event and clk='1' then
					if(count_hs<=143 or count_hs>783 or count_vs<31 or count_vs>510) then
						rgb_out(2 downto 0)<="000";
					
					elsif(count_vs>=150 and count_vs<=390 and count_hs>=148 and count_hs<=298) then
								rgb_out(2 downto 0)<=draw_c1(2 downto 0);
					
					elsif(count_vs>=150 and count_vs<=390 and count_hs>=308 and count_hs<=458) then
								rgb_out(2 downto 0)<=draw_c2(2 downto 0);
					
					elsif(count_vs>=150 and count_vs<=390 and count_hs>=468 and count_hs<=608) then
								rgb_out(2 downto 0)<=draw_c3(2 downto 0);
					
					elsif(count_vs>=150 and count_vs<=390 and count_hs>=628 and count_hs<=778) then
								rgb_out(2 downto 0)<=draw_c4(2 downto 0);
						
						else
								rgb_out(2 downto 0)<=draw_back(2 downto 0);
						end if;
				end if;
			end process;
	
selector:process(clk, pos_selector)
			begin
				if clk'event and clk='1' then
					if reset ='1' then 
						char1<="0000"; char2<="0000"; char3<="0000"; char4<="0000";
						rgb_c1<="111"; rgb_c2<="111"; rgb_c3<="111"; rgb_c4<="111";
						draw_back<="000";
					end if;
					
					
					case pos_selector is
						when "000" => rgb_c1(2 downto 0)<=rgb_in(2 downto 0);
										  char1(3 downto 0)<=char(3 downto 0);
						when "001" => rgb_c2(2 downto 0)<=rgb_in(2 downto 0);
										  char2(3 downto 0)<=char(3 downto 0);
						when "010" => rgb_c3(2 downto 0)<=rgb_in(2 downto 0);
										  char3(3 downto 0)<=char(3 downto 0);								  
						when "011" => rgb_c4(2 downto 0)<=rgb_in(2 downto 0);
										  char4(3 downto 0)<=char(3 downto 0);
		 
						when "100" => draw_back(2 downto 0)<= rgb_in(2 downto 0); --back color
						when others =>
					end case;
					
					case char1 is
					--0
						when "0000" => if (((count_vs>190 and count_vs<350)) 
												and (count_hs>188+h1 and count_hs<258+h1)) then
															
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
															
					--1	
						when "0001" => if (count_hs<258+h1) then
														
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--2
						when "0010" => if (((count_vs>190 and count_vs<250 and count_hs<258+h1) or 
												(count_vs>290 and count_vs<350 and count_hs>188+h1))) then
																
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--3	
						when "0011" => if (((count_vs>190 and count_vs<250) or (count_vs>290 and count_vs<350)) 
												and (count_hs<258+h1)) then
															
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--4
						when "0100" => if  ((count_vs<250 and count_hs>188+h1 and count_hs<258+h1) or 
													(count_vs>290 and count_hs<258+h1)) then
																
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--5
						when "0101" => if (((count_vs>190 and count_vs<250 and count_hs>188+h1) or 
												  (count_vs>290 and count_vs<350 and count_hs<258+h1))) then
																
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--6
						when "0110" => if (((count_vs>190 and count_vs<250 and count_hs>188+h1) or 
												(count_vs>290 and count_vs<350 and count_hs>188+h1 and count_hs<258+h1))) then
																	
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--7
						when "0111" => if ((count_vs>190) and (count_hs<258+h1)) then
																	
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
															end if;
					--8	
						when "1000" => if (((count_vs>190 and count_vs<250 and count_hs>188+h1 and count_hs<258+h1) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h1 and count_hs<258+h1))) then
																
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
					--9
						when "1001" => if (((count_vs>190 and count_vs<250 and count_hs>188+h1 and count_hs<258+h1) or 
												  (count_vs>290 and count_hs<258+h1))) then
																
													draw_c1(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c1(2 downto 0)<=rgb_c1(2 downto 0);
											end if;
														
										
										
						when others => draw_c1(2 downto 0)<=draw_back(2 downto 0);
					end case;
									
					case char2 is
					--0
						when "0000" => if (((count_vs>190 and count_vs<350)) 
												and (count_hs>188+h2 and count_hs<258+h2)) then
																	
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
													
					--1	
						when "0001" => if (count_hs<258+h2) then
														
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--2
						when "0010" => if (((count_vs>190 and count_vs<250 and count_hs<258+h2) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h2))) then
														
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--3	
						when "0011" => if (((count_vs>190 and count_vs<250) or (count_vs>290 and count_vs<350)) 
												and (count_hs<258+h2)) then
																
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--4
						when "0100" => if  ((count_vs<250 and count_hs>188+h2 and count_hs<258+h2) or 
												  (count_vs>290 and count_hs<258+h2)) then
																
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--5
						when "0101" => if (((count_vs>190 and count_vs<250 and count_hs>188+h2) or 
												(count_vs>290 and count_vs<350 and count_hs<258+h2))) then
																
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--6
						when "0110" => if (((count_vs>190 and count_vs<250 and count_hs>188+h2) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h2 and count_hs<258+h2))) then
															
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--7
						when "0111" => if ((count_vs>190) and (count_hs<258+h2)) then
															
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--8	
						when "1000" => if (((count_vs>190 and count_vs<250 and count_hs>188+h2 and count_hs<258+h2) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h2 and count_hs<258+h2))) then
														
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
					--9
						when "1001" => if (((count_vs>190 and count_vs<250 and count_hs>188+h2 and count_hs<258+h2) or 
												  (count_vs>290 and count_hs<258+h2))) then
													
													draw_c2(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c2(2 downto 0)<=rgb_c2(2 downto 0);
											end if;
						
						when others => draw_c2(2 downto 0)<=draw_back(2 downto 0);
					end case;
						

					case char3 is
					--0
						when "0000" => if (((count_vs>190 and count_vs<350)) 
												and (count_hs>188+h3 and count_hs<258+h3)) then
													
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--1
						when "0001" => if (count_hs<258+h3) then
															
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--2
						when "0010" => if (((count_vs>190 and count_vs<250 and count_hs<258+h3) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h3))) then
												
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--3	
						when "0011" => if (((count_vs>190 and count_vs<250) or (count_vs>290 and count_vs<350)) 
												and (count_hs<258+h3)) then
														
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--4
						when "0100" => if  ((count_vs<250 and count_hs>188+h3 and count_hs<258+h3) or 
												  (count_vs>290 and count_hs<258+h3)) then
												
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--5
						when "0101" => if (((count_vs>190 and count_vs<250 and count_hs>188+h3) or 
												(count_vs>290 and count_vs<350 and count_hs<258+h3))) then
												
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--6
						when "0110" => if (((count_vs>190 and count_vs<250 and count_hs>188+h3) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h3 and count_hs<258+h3))) then
															
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--7
						when "0111" => if ((count_vs>190) and (count_hs<258+h3)) then
													
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--8	
						when "1000" => if (((count_vs>190 and count_vs<250 and count_hs>188+h3 and count_hs<258+h3) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h3 and count_hs<258+h3))) then
												
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
					--9
						when "1001" => if (((count_vs>190 and count_vs<250 and count_hs>188+h3 and count_hs<258+h3) or 
												  (count_vs>290 and count_hs<258+h3))) then
													
													draw_c3(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c3(2 downto 0)<=rgb_c3(2 downto 0);
											end if;
									
						when others => draw_c3(2 downto 0)<=draw_back(2 downto 0);
					end case;
									
					case char4 is
					--0
						when "0000" => if (((count_vs>190 and count_vs<350)) 
												and (count_hs>188+h4 and count_hs<258+h4)) then
													
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--1
						when "0001" => if (count_hs<258+h4) then
														
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--2
						when "0010" => if (((count_vs>190 and count_vs<250 and count_hs<258+h4) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h4))) then
														
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--3	
						when "0011" => if (((count_vs>190 and count_vs<250) or (count_vs>290 and count_vs<350)) 
													and (count_hs<258+h4)) then
												
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--4
						when "0100" => if  ((count_vs<250 and count_hs>188+h4 and count_hs<258+h4) or 
												  (count_vs>290 and count_hs<258+h4)) then
													
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--5
						when "0101" => if (((count_vs>190 and count_vs<250 and count_hs>188+h4) or 
												  (count_vs>290 and count_vs<350 and count_hs<258+h4))) then
												
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--6
						when "0110" => if (((count_vs>190 and count_vs<250 and count_hs>188+h4) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h4 and count_hs<258+h4))) then
													
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--7
						when "0111" => if ((count_vs>190) and (count_hs<258+h4)) then
															
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--8	
						when "1000" => if (((count_vs>190 and count_vs<250 and count_hs>188+h4 and count_hs<258+h4) or 
												  (count_vs>290 and count_vs<350 and count_hs>188+h4 and count_hs<258+h4))) then
												
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					--9
						when "1001" => if (((count_vs>190 and count_vs<250 and count_hs>188+h4 and count_hs<258+h4) or 
												  (count_vs>290 and count_hs<258+h4))) then
													
													draw_c4(2 downto 0)<=draw_back(2 downto 0);
											else
													draw_c4(2 downto 0)<=rgb_c4(2 downto 0);
											end if;
					
								
						when others => draw_c4(2 downto 0)<=draw_back(2 downto 0);
					end case;

				end if;
			end process;

end Behavioral;