import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { Queue } from '../interfaces/queue';
import { MatDialog } from '@angular/material/dialog';
import { ModalComponent } from '../component/modal/modal.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Response } from '../interfaces/response';
import { MessageService } from '../services/message.service';


@Component({
  selector: 'app-list-queues',
  templateUrl: './list-queues.component.html',
  styleUrls: ['./list-queues.component.css']
})
export class ListQueuesComponent implements OnInit {
  constructor(
    private queueService: QueueService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar
    ){}

  public list: Queue[] = [];
  public countMessages: number = 0;

  ngOnInit(): void {
    this.getCountMessages();
    this.getData();
  }

  getCountMessages(){
    this.messageService.getCountMessagesInRabbitMQ().subscribe((response) => {
      this.countMessages = response;
      this.cdr.detectChanges();
    });
  }

  getData(){
    this.queueService.getAll().subscribe((queues: Queue[])=>{
      this.list = queues.sort((a, b) => {
        const dateA = new Date(a.createDate);
        const dateB = new Date(b.createDate);
        return dateA.getTime() - dateB.getTime();
      });
    });
  }
  clickDeleteQueue(id: string){
    this.queueService.deleteQueue(id).subscribe(
      () => {
        this.openSnackBar('Fila excluída', 'Fechar', true);
        this.getData();
      },
      (error) => {
        if(error.status === 400){
          this.openSnackBar(error.error, 'Fechar');
        }
      }
    );
  }
  createQueue(title: string, parameterString: string ): void {
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: title,
        parameterPlaceholder: parameterString,
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.queueService.createQueue(result.name).subscribe(
          (response: Response) => {
            this.openSnackBar(`Fila \' ${response.name} \' criada`, 'Fechar', true);
            this.getData();
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
        duration: 6000, // Tempo em milissegundos que o alerta será exibido
        verticalPosition: 'bottom', // Posição vertical do alerta
        horizontalPosition: 'end', // Posição horizontal do alerta
        panelClass: sucess ? ['success-snackbar'] : ['warning-snackbar']
    });
  }

}
