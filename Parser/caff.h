#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>


typedef struct credit{
    struct tm creation_date;
    char* creator;
}credit;

typedef struct meta{
    int duration;
    int header_len;
    int len;
    int widht;
    int height;
    char* caption;
}ciff_meta;

typedef struct ciff{
    ciff_meta info;
    struct ciff* next_ciff;
    char* pixels;
}ciff;

typedef struct caff{
    credit crd;
    uint8_t ciff_num;
    ciff* HEAD;
    ciff* TAIL;
}caff;



int add_credit(caff* caff_file, char *creator,int len, struct tm creation_date);

int add_ciff(caff* caff_file,int len, char* pixels, ciff_meta info);

void print_caff(caff* caff_file);

void init_caff();

caff* get_caff();

void free_caff();

/* parser */

caff* parse_caff_file(char* filename);

/* image generation */

void create_preview_image(int height, int width, char* pixels);
