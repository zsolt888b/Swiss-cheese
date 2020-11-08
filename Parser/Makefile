CC=gcc
ENABLE_SSP:=-fstack-protector-all


makeParser: caff.o parse_file.o bytes_to_bmp.o core.o
	
	#ENABLE_ASLR
	sudo sysctl -w kernel.randomize_va_space=2
	
	$(CC) -c caff.c
	$(CC) -c parse_file.c
	$(CC) -c bytes_to_bmp.c
	$(CC) -c core.c

	# Compile with stack smashing protection 
	$(CC)  $(ENABLE_SSP) -o  nativ_parser caff.o parse_file.o bytes_to_bmp.o core.c 
