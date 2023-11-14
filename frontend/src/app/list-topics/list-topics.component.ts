import { ChangeDetectorRef, Component, OnInit, NgZone  } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ModalComponent } from '../component/modal/modal.component';
import { Topic } from '../interfaces/topic';
import { TopicService } from '../services/topic.service';
import { Response } from '../interfaces/response';

@Component({
  selector: 'app-list-topics',
  templateUrl: './list-topics.component.html',
  styleUrls: ['./list-topics.component.css']
})
export class ListTopicsComponent implements OnInit{

  constructor(
    private topicService: TopicService,
    private cdr: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private zone: NgZone
    ){}

  public list: Topic[] = [];

  ngOnInit(): void {
    this.getData();
  }

  getData(){
    this.topicService.getAll().subscribe((topics: Topic[])=>{
      this.list = topics.sort((a, b) => {
        const dateA = new Date(a.createDate);
        const dateB = new Date(b.createDate);
        return dateA.getTime() - dateB.getTime();
      });
    });
  }
  clickDeleteTopic(name: string){
    this.topicService.deleteTopic(name).subscribe(
      () => {
        this.openSnackBar('Tópico excluído', 'Fechar', true);
        this.getData();
        this.cdr.detectChanges();
      },
      (error) => {
        if(error.status === 400){
          this.openSnackBar(error.error, 'Fechar');
        }
      }
    );
  }
  createTopic(title: string, placeholderName: string, routingKey: string): void {
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: title,
        parameterPlaceholder: placeholderName,
        routingKeyPlaceholder: routingKey
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.topicService.createTopic(result.name, result.routingKey).subscribe(
            (response: Response) => {
              this.getData();
              this.cdr.detectChanges();
              this.openSnackBar(`Tópico \' ${response.name} \' criado`, 'Fechar', true);
            },
            (error) => {
              if(error.status === 400){
                this.openSnackBar(error.error, 'Fechar');
              }
            }
        );
      }
    });
  }
  openSnackBar(message: string, action: string, sucess: boolean = false) {
    this.snackBar.open(message, action, {
        duration: 6000,
        verticalPosition: 'bottom',
        horizontalPosition: 'end',
        panelClass: sucess ? ['success-snackbar'] : ['warning-snackbar']
    });
}
}
