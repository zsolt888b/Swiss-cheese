#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>

#include "caff.h"



typedef enum 
{
    START,
    NEW_FRAME,
    DATA,
    DONE,
    ERROR
} l_read_state;

typedef enum
{
    INIT=0,
    HEADER=1,
    CREDITS=2,
    ANIMATION=3
} l_frame_type;

typedef struct frame_properties
{
    l_frame_type type;
    int data_len;
    int data_read;
} l_frame_properties;

typedef struct parse_ctx
{
    FILE * fd;
    uint32_t file_size;
    l_frame_properties curr_frame;
    uint8_t ciff_num;
    uint8_t ciff_read;
    uint8_t b_credit_read;

} l_parse_ctx;


/* internal type definitions */

static const char* HEADER_PREFIX="CAFF";
static const int HEADER_PREFIX_LEN=4;
static const int HEADER_SIZE_LEN=8;
static const int CIFF_NUM_LEN=8;
static const uint8_t ID_LEN=1;
static const uint8_t FRAME_LEN=8;

/* CIFF internals*/
static const char* CIFF_PREFIX="CIFF";
static const uint8_t CIFF_PREFIX_LEN=4;
static const uint8_t CIFF_DURATION_LEN=8;
static const uint8_t CIFF_HEADER_SIZE_LEN=8;
static const uint8_t CIFF_DATA_SIZE_LEN=8;
static const uint8_t CIFF_WIDHT_SIZE_LEN=8;
static const uint8_t CIFF_HEIGHT_SIZE_LEN=8;
static const char CIFF_CAPTION_EOF='\n';
static const char CIFF_TAG_EOF='\0';
static const int CIFF_HEADER_STATIC_PART_LEN = 36;
/* INTERNAL GLOBALS */

l_read_state l_g_state;
l_parse_ctx l_g_ctx;



void l_init_frame_properties()
{
    l_g_ctx.curr_frame.data_len=0;
    l_g_ctx.curr_frame.data_read=0;
    l_g_ctx.curr_frame.type=INIT;
}

void l_init_ctx()
{
    l_g_ctx.b_credit_read=0;
    l_g_ctx.ciff_num=0;
    l_g_ctx.ciff_read=0;
    l_g_ctx.fd=NULL;
    l_init_frame_properties();
}

int l_read_header(caff* caff_file){

    int error = ( l_g_ctx.fd == NULL );
    error = error || (caff_file == NULL);

    uint8_t header_size=0;
    uint8_t ciff_num=0;
    char header_prefix[HEADER_PREFIX_LEN + 1];
    
    if(error){
        printf("ERR - caff file doesnt exist");    
    } else {
        error = fseek(l_g_ctx.fd, 9, SEEK_SET);
        error = error || (HEADER_PREFIX_LEN !=  fread(&header_prefix, 1, HEADER_PREFIX_LEN, l_g_ctx.fd));
        header_prefix[HEADER_PREFIX_LEN]='\0';
        error = error || (HEADER_SIZE_LEN != fread(&header_size, 1, HEADER_SIZE_LEN, l_g_ctx.fd));
        error = error || (CIFF_NUM_LEN != fread(&ciff_num, 1, CIFF_NUM_LEN, l_g_ctx.fd));

        l_g_ctx.ciff_num=ciff_num;
    }

    /*Check if magic caff is valid*/
    if(strcmp(header_prefix, HEADER_PREFIX) == 0 ? 0 : 1 ){
        printf("ERR - Magic caff is missing from the header\n");
        error=1;
    }

    return error;
}

int l_new_frame(caff* caff_file){

    int error=0;
    int frametype=0;

    error= (ID_LEN != fread(&frametype, 1, ID_LEN, l_g_ctx.fd));
    l_g_ctx.curr_frame.type=(l_frame_type)frametype;
    error = (FRAME_LEN != fread(&(l_g_ctx.curr_frame.data_len), 1 , FRAME_LEN, l_g_ctx.fd));

    return error;
}

int l_read_anim(caff* caff_file){

    int error = ( l_g_ctx.fd == NULL );
    error = error || (caff_file == NULL);
    ciff_meta ciff_info; 
    char* data = NULL;

    char ciff_prefix[CIFF_PREFIX_LEN + 1];

    /* CIFF meta data */
    error = error || (CIFF_DURATION_LEN     != fread(&ciff_info.duration, 1, CIFF_DURATION_LEN, l_g_ctx.fd));
    error = error || (CIFF_PREFIX_LEN       != fread(&ciff_prefix, 1, CIFF_PREFIX_LEN, l_g_ctx.fd));
    ciff_prefix[CIFF_PREFIX_LEN]='\0';
    error = error || (CIFF_HEADER_SIZE_LEN  != fread(&ciff_info.header_len, 1, CIFF_HEADER_SIZE_LEN, l_g_ctx.fd));
    error = error || (CIFF_DATA_SIZE_LEN    != fread(&ciff_info.len, 1, CIFF_DATA_SIZE_LEN, l_g_ctx.fd));
    error = error || (CIFF_WIDHT_SIZE_LEN   != fread(&ciff_info.widht, 1, CIFF_WIDHT_SIZE_LEN, l_g_ctx.fd));
    error = error || (CIFF_HEIGHT_SIZE_LEN  != fread(&ciff_info.height, 1, CIFF_HEIGHT_SIZE_LEN, l_g_ctx.fd));
    
    /* skip Tags and caption */
    int TAGS_LEN = ciff_info.header_len - CIFF_HEADER_STATIC_PART_LEN  ; 
    fseek(l_g_ctx.fd,TAGS_LEN,SEEK_CUR);

    /* CIFF content */
    uint64_t data_to_read = ciff_info.len;
    data = (char*) malloc( data_to_read * sizeof(char) );
    error = error || (data_to_read != fread(data, 1, data_to_read, l_g_ctx.fd));


    
    /*Check if magic caff is valid*/
    if(strcmp(ciff_prefix, CIFF_PREFIX) == 0 ? 0 : 1 ){
        printf("ERR - Magic ciff is missing from ciff header\n");
        error=1;
    }
    /*Chck if ciff content length is ok*/
    error = error || ( 3*(ciff_info.height+ciff_info.widht) == ciff_info.len );

    /*add ciff to caff stuct if valid*/
    if(!error){
        if(!add_ciff(caff_file, data_to_read , data , ciff_info) ){
            l_g_ctx.ciff_read++;
            caff_file->ciff_num++;
        }else{
            error = 1;
        }
    }

    /* free memory */
    free(data);

    return error;
}

int l_read_credit(caff* caff_file){

    int error = 0 ;
    struct tm creation_date;
    int creator_len = 0;
    char* creator = NULL;

    error = error || (2 !=  fread(&creation_date.tm_year, 1, 2, l_g_ctx.fd));
    error = error || (1 !=  fread(&creation_date.tm_mon, 1, 1, l_g_ctx.fd));
    error = error || (1 !=  fread(&creation_date.tm_wday, 1, 1, l_g_ctx.fd));
    error = error || (1 !=  fread(&creation_date.tm_hour, 1, 1, l_g_ctx.fd));
    error = error || (1 !=  fread(&creation_date.tm_min, 1, 1, l_g_ctx.fd));

    error = error || (8 !=  fread(&creator_len, 1, 8, l_g_ctx.fd));
    
    creator = (char*) malloc( creator_len*sizeof(char) );
    error = error || (creator_len !=  fread(creator, 1, creator_len, l_g_ctx.fd));
    add_credit(caff_file,creator,creator_len, creation_date);

    free(creator);

    return error;
};

int l_is_caff_valid(){

    int b_valid = 1;
        b_valid = ( 0 == (l_g_ctx.ciff_num-l_g_ctx.ciff_read));
        b_valid = b_valid || l_g_ctx.b_credit_read;

        if(b_valid){
            printf("INF - valid caff\n");
        } else{
            printf("INF - invalid caff\n");
        }

    return b_valid;
}

int l_is_done(){

    int done = 0;
    int read_from_file=ftell(l_g_ctx.fd);

    if( 0 == l_g_ctx.file_size-read_from_file){
        done=1;
    }

    return done;
};

void l_set_state(int error)
{

    switch (l_g_state){
        case START:
            if (!error){
                l_g_state=NEW_FRAME;
            } else { 
                l_g_state = ERROR;
            }
            break;
        case NEW_FRAME:
            if (!error){
                l_g_state=DATA;
            } else { 
                l_g_state = ERROR;
            }
            break;
        case DATA:
            if (!error){
                if(l_is_done()){
                    l_g_state = DONE;
                } else {
                    l_g_state = NEW_FRAME; 
                }
            }else{ 
                l_g_state=ERROR;
            }
            break;
        default:
            break;
    }

}

int l_read_data(caff* caff_file)
{

    int error = 0;

    switch (l_g_ctx.curr_frame.type)
            {
            case ANIMATION:
                if(0 != l_g_ctx.ciff_num-l_g_ctx.ciff_read){
                    error = l_read_anim(caff_file);
                } else{
                    printf("ERR - Too much anim block\n ");  
                    error = 1;    
                }
                break;
            case CREDITS:
                if (!l_g_ctx.b_credit_read){
                    error = l_read_credit(caff_file);
                } else{
                    printf("ERR - More than one credit block\n ");  
                    error = 1;                  
                }
                break;
            default:
                printf("ERR - Wrong frame type\n ");
                error = 1;
                break;
            }

    return error;
}

int l_init_file(char* filename){

    int error = 0;

    l_g_ctx.fd=fopen(filename, "r");        
    if( NULL != l_g_ctx.fd ){
        error = fseek(l_g_ctx.fd,0,SEEK_END);

        l_g_ctx.file_size=ftell(l_g_ctx.fd);
        error = error || ((-1 == l_g_ctx.file_size) ? 1 : 0); 
        error = error || fseek(l_g_ctx.fd,0,SEEK_SET);
    } else {
        printf("ERR - can not open file\n");
        error=1;
    }

    return error;
}

int l_close_file();

caff* parse_caff_file(char* filename)
{

    int error;
    l_g_state=START;

    l_init_ctx();
    error = l_init_file(filename);

    caff* caff_file=get_caff();

    error = (l_g_ctx.fd==NULL);


    if(!error){
        while(l_g_state != DONE && l_g_state != ERROR){
            switch (l_g_state){
                case START:
                    error = error || l_read_header(caff_file);
                    break;
                case NEW_FRAME:
                    error = error || l_new_frame(caff_file);
                    break;
                case DATA:
                    error = error || l_read_data(caff_file);
                    break;
                default:
                    break;
            }

            /* Set next state */
            l_set_state(error);

        }
        if (l_g_state == ERROR){
            printf("ERR - Parsing ended with error, aborting\n ");
            free_caff();
        } else if ( l_g_state == DONE ){
            error = !l_is_caff_valid();         
        }
    }
    
    if(l_g_ctx.fd != NULL ){
        fclose(l_g_ctx.fd);
    }

    return  error ? NULL : caff_file;
}

