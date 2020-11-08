#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>
#include "caff.h"

/* Global CAFF file */
caff g_caff_file;

int add_credit(caff* caff_file,char* creator,int len, struct tm creation_date){

    /* int error=NULLCHECK(caff_file); */
    int error = 0;

    caff_file->crd.creation_date=creation_date;

    caff_file->crd.creator=(char*) malloc( len*sizeof(char) );
    memcpy(caff_file->crd.creator,creator,len);

    return error;
}

void free_caff(){

    /* free ciff */
    ciff *curr_ciff=g_caff_file.HEAD;
    ciff *next_cift;

    for(int i=0; i<g_caff_file.ciff_num; i++){
        next_cift=curr_ciff->next_ciff;
        free(curr_ciff->pixels);
        free(curr_ciff);
        curr_ciff=next_cift;
    }

    /* free credit */
    free(g_caff_file.crd.creator);

}

int add_ciff(caff* caff_file,int len, char* pixels, ciff_meta info)
{
    /* int error=NULLCHECK(caff_file); */
    int error = 0;
    // allocate memory for ciff
    ciff *new_ciff= (ciff*) malloc( sizeof(ciff));
    error = (NULL == new_ciff);

    //allocate memory for the pixel, set ciffs pointer to pixels
    new_ciff->pixels = (char*) malloc( len*sizeof(char));
    error = error || (NULL == new_ciff);
    //memcpy(new_ciff->pixels, pixels, len+1);
    memcpy(new_ciff->pixels, pixels, len);

    new_ciff->info=info;
    //new_ciff->pixels[len+1]='\0';

    if(NULL == caff_file->HEAD){
      
        /* First caff element - Init ciff pointers */
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
    return error;
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

void init_caff(){

    g_caff_file.ciff_num=0;
    g_caff_file.HEAD=NULL;
    g_caff_file.TAIL=NULL;
}


caff* get_caff(){

    return &g_caff_file;

}
