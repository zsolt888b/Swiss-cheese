CC=gcc

makeParser: caff.o parse_file.o bytes_to_bmp.o core.o
	$(CC) -c caff.c
	$(CC) -c parse_file.c
	$(CC) -c bytes_to_bmp.c
	$(CC) -c core.c
	$(CC) -o nativ_parser caff.o parse_file.o bytes_to_bmp.o core.c 
