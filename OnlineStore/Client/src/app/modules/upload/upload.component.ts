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
    this.fileService.upload(this.fileToUpload? {data: this.fileToUpload, fileName : this.fileToUpload.name} : null, this.description, this.filename, this.price)
        .subscribe(res =>{
          this.toastr.success("File succesfully uploaded!");
          this.router.navigate([""]);
        },error =>{
          this.toastr.error(error);
        });
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

}
