#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>


#define FILENAME "2.caff"


typedef struct credit{
    struct tm creation_date;
    char* creator;
}credit;

typedef struct meta{
    int ms;
    int widht;
    int height;
    char* caption;
}meta;

typedef struct ciff{
    meta info;
    struct ciff* next_ciff;
    char* pixels;
}ciff;

typedef struct caff{
    credit crd;
    int ciff_num;
    ciff* HEAD;
    ciff* TAIL;
}caff;

int add_credit();

int add_ciff(caff* caff_file,int len, char* pixels)
{
    /* int error=NULLCHECK(caff_file); */
    int error = 0;
    // allocate memory for ciff
    ciff *new_ciff= (ciff*) malloc( sizeof(ciff));

    //allocate memory for the pixel, set ciffs pointer to pixels
    new_ciff->pixels = (char*) malloc( len*sizeof(char));    
    new_ciff->pixels = pixels;

    if(NULL == caff_file->HEAD){
        
        caff_file->HEAD=new_ciff;
        caff_file->TAIL=new_ciff;
        new_ciff->next_ciff=caff_file->HEAD;

    } else {

        caff_file->TAIL->next_ciff=new_ciff;

        //set the new ciff pointer to HEAD
        new_ciff->next_ciff= caff_file->HEAD;

        //set the last ciffs pointer to this ciff
        caff_file->TAIL=new_ciff;

    }

}

void print_caff(caff* caff_file){

    caff *tmp_head= caff_file;

    printf("\n ciff HEAD: %s \n", tmp_head->HEAD->pixels);
    printf("\n ciff TAIL: %s \n", tmp_head->TAIL->pixels);


    for (int i=0;i!=19;i++){
        
        printf("\n ciff pixels: %s \n", tmp_head->HEAD->pixels);
        tmp_head->HEAD=tmp_head->HEAD->next_ciff;
    }
}

caff init_caff(caff* caff_to_init){

    caff_to_init->crd;
    caff_to_init->ciff_num=0;
    caff_to_init->HEAD=NULL;
    caff_to_init->TAIL=NULL;

}



int main(){

    int error=0;
    char read_char;
    int i=0;

    char* pixels[10]={"1","2","3","4", "5", "6", "7", "8", "9", "c"};
    int len= strlen(pixels[0]);

    caff caff_file;
    init_caff(&caff_file);

    for (int i=0; i!=10; i++){
        add_ciff(&caff_file,len,pixels[i]);
    }

    print_caff(&caff_file);

    return error;
}
