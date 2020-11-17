import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommentModel, FileModel, FileService } from 'src/app/api/app.generated';

@Component({
  selector: 'app-file-details',
  templateUrl: './file-details.component.html',
  styleUrls: ['./file-details.component.scss'],
  providers: [FileService]
})
export class FileDetailsComponent implements OnInit {

  constructor(private fileService : FileService, private avRoute: ActivatedRoute, private toastr : ToastrService) {
  }

  file : FileModel;
  comments : CommentModel[];
  fileId : number;

  ngOnInit(): void {
    this.fileId = this.avRoute.snapshot.params["id"];

    this.getDetails();
  }

  getDetails(){

    this.fileService.getFileDetails(this.fileId).subscribe(res =>{
      this.file = res;
      this.fileService.getCommentsForFile(this.fileId).subscribe( comments =>{
        this.comments = comments;
      })
    });
  }

  commentText : string;

  comment(){
    if(this.chackForWhiteSpacesOrNull(this.commentText)==false){
      this.toastr.error("Comment is required!")
      return;
    }

    this.fileService.comment(this.fileId,this.commentText).subscribe(res =>{
      this.toastr.success("Succesfully commented");
      this.commentText = null;
      this.getDetails();
    }, error =>{
      this.toastr.error("Log-in first!");
    });
  }

  deleteComment(id : number){
    this.fileService.deleteComment(id).subscribe(res =>{
      this.toastr.success("Succesfully deleted");
      this.getDetails();
    }, error =>{
      this.toastr.error("Only administrators can access this!");
    })
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
