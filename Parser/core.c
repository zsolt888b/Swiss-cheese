#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>
#include "caff.h"
#include <sys/resource.h>

static const char L_HELP_MSG[] = "\nUsage: native_parser [ CAFF_PATH ]\n"
                                 "\t[ CAFF_PATH ] - The path for the caff file to parse\n";

static char *L_DEF_CAFF_FILE;

int l_parse_args(int argc, char* argv[]){
    int help=0;

    /* Parse arguments*/
    if(argc != 2){
        printf("%s",L_HELP_MSG);
        help = 1;
    }else{
        L_DEF_CAFF_FILE=argv[1];
    }

    return !help;
}

int main(int argc, char* argv[])
{
    int error = 0;
    int run = l_parse_args(argc,argv);
    
    if(run){
        /* Init the the global caff structure*/
        init_caff();
        
        /* parse the given caff file */
        printf("INF - parsing the file\n");
        caff* parsed_caff=parse_caff_file(L_DEF_CAFF_FILE);
        error = ( NULL == parsed_caff ); 
        
        if( !error )
        {
            
            /* Create the preview image */
            create_preview_image(parsed_caff->HEAD->info.height , parsed_caff->HEAD->info.widht, parsed_caff->HEAD->pixels);

            /* free up caff */
            free_caff();
        }else{
            printf("ERR - aborting image generation\n");
        }
    }

    return error;
}