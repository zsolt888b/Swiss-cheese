import { Component, OnInit } from '@angular/core';
import { FileModel, FileService } from 'src/app/api/app.generated';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ FileService]
})
export class HomeComponent implements OnInit {

  constructor(private fileService : FileService) { }

  files : FileModel[];
  fileName : string;

  ngOnInit(): void {
    this.getFiles();
  }

  getFiles(){
    this.fileService.getFiles(this.fileName).subscribe(res =>{
      this.files=res;
    })
  }

  download(id : number){
    this.fileService.download(id).subscribe(res =>{
      saveAs(res.data, res.fileName);
    })
  }
}
