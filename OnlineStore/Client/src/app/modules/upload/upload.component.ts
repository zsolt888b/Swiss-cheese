import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FileParameter, FileService } from 'src/app/api/app.generated';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss'],
  providers: [FileService]
})
export class UploadComponent implements OnInit {

  constructor(private fileService : FileService, private router : Router,  private toastr: ToastrService) { }

  description : string;
  filename : string;
  price : number;
  fileToUpload: File = null;

  ngOnInit(): void {
  }

  upload(){
    if(this.chackForWhiteSpacesOrNull(this.filename)==false){
      this.toastr.error("File name is required!")
      return;
    }

    if(this.chackForWhiteSpacesOrNull(this.description)==false){
      this.toastr.error("Description is required!")
      return;
    }

    if(this.fileToUpload==null){
      this.toastr.error("File is required!")
      return;
    }

    let extension = this.fileToUpload.name.split('.').pop();
    if(extension!="caff"){
      this.toastr.error("File is not a CAFF file!")
      return;
    }

    if(this.price==null || this.price<0){
      this.toastr.error("Price is required!")
      return;
    }

    this.fileService.upload(this.fileToUpload? {data: this.fileToUpload, fileName : this.fileToUpload.name} : null, this.description, this.filename, this.price)
        .subscribe(res =>{
          if(res.length>1){
            this.toastr.error(res);
            this.router.navigate([""]);
          }else{
            this.toastr.success("File succesfully uploaded!");
            this.router.navigate([""]);
          }
        },error =>{

        });
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  chackForWhiteSpacesOrNull(string : String) : Boolean{
    if(string==null){
      return false;
    }
    if (!string.replace(/\s/g, '').length) {
      return false;
    }
    return true;
  }

}
