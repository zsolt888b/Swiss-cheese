#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>


/* This function is based on this stackoverflow thread : */
/* https://stackoverflow.com/questions/2654480/writing-bmp-image-in-pure-c-c-without-other-libraries */

void l_bitmap(int height, int width, char* pixels)
{
  typedef struct                       /**** BMP file header structure ****/
      {
      unsigned int   bfSize;           /* Size of file */
      unsigned short bfReserved1;      /* Reserved */
      unsigned short bfReserved2;      /* ... */
      unsigned int   bfOffBits;        /* Offset to bitmap data */
      } BITMAPFILEHEADER;

  typedef struct                       /**** BMP file info structure ****/
      {
      unsigned int   biSize;           /* Size of info header */
      int            biWidth;          /* Width of image */
      int            biHeight;         /* Height of image */
      unsigned short biPlanes;         /* Number of color planes */
      unsigned short biBitCount;       /* Number of bits per pixel */
      unsigned int   biCompression;    /* Type of compression to use */
      unsigned int   biSizeImage;      /* Size of image data */
      int            biXPelsPerMeter;  /* X pixels per meter */
      int            biYPelsPerMeter;  /* Y pixels per meter */
      unsigned int   biClrUsed;        /* Number of colors used */
      unsigned int   biClrImportant;   /* Number of important colors */
      } BITMAPINFOHEADER;

  BITMAPFILEHEADER bfh;
  BITMAPINFOHEADER bih;

  /* Magic number for file. It does not fit in the header structure due to alignment requirements, so put it outside */
  unsigned short bfType=0x4d42;           
  bfh.bfReserved1 = 0;
  bfh.bfReserved2 = 0;
  bfh.bfSize = 2+sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER)+640*480*3;
  bfh.bfOffBits = 0x36;

  bih.biSize = sizeof(BITMAPINFOHEADER);
  bih.biWidth = width;
  bih.biHeight = height;
  bih.biPlanes = 1;
  bih.biBitCount = 24;
  bih.biCompression = 0;
  bih.biSizeImage = 0;
  bih.biXPelsPerMeter = 5000;
  bih.biYPelsPerMeter = 5000;
  bih.biClrUsed = 0;
  bih.biClrImportant = 0;

  FILE *file = fopen("preview_image.bmp", "wb");
  if (!file)
      {
        printf(" ERR - Could not write file\n");
        return;
      }

  /*Write headers*/
  fwrite(&bfType,1,sizeof(bfType),file);
  fwrite(&bfh, 1, sizeof(bfh), file);
  fwrite(&bih, 1, sizeof(bih), file);

  unsigned int num_of_pixels= bih.biHeight * bih.biWidth;

  /*Write bitmap*/
  for (int i = num_of_pixels - 1; i > 0 ; i-- ) 
  {
        /*compute some pixel values*/
        unsigned char r = pixels[ 3*i + 0 ];
        unsigned char g = pixels[ 3*i + 1 ];
        unsigned char b = pixels[ 3*i + 2 ];
        fwrite(&b, 1, 1, file);
        fwrite(&g, 1, 1, file);
        fwrite(&r, 1, 1, file);
  }
  if ( NULL!=file)
  {
    fclose(file);
    printf("INF: BMP file generated succesfully\n");
  }

}


void create_preview_image(int height, int width, char* pixels){

  l_bitmap(height, width, pixels);

}
