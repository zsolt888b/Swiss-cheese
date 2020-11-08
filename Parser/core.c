#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>
#include "caff.h"

static const char L_HELP_MSG[] = "Usage: native_parser [-c CAFF_PATH -o IMAGE_PATH]\n\n"
                                 "[-c CAFF_PATH ] - The path for the caff file to parse\n"
                                 "[-o NAME_OF_GENERATED_IMAGE ] - The path for the preview image, without extension\n"
                                 "If any of the arguments missing, its default value is used \n";

static const char L_DEF_IMAGE_PATH[] = "prev_image";


int main(int argc, char* argv[])
{
    printf("started\n");

    char* L_DEF_CAFF_FILE = (argc > 1) ? argv[1] : "1.caff";


    init_caff();
    caff* caffli=parse_caff_file(L_DEF_CAFF_FILE);

    create_preview_image(caffli->HEAD->info.height , caffli->HEAD->info.widht, caffli->HEAD->pixels);

    free_caff();

    printf("stopped\n");

    return 0;
}