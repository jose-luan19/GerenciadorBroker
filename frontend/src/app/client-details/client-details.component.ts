import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../services/client.service';
import { ClientDetails } from '../interfaces/clientDetails';
import { format } from 'date-fns';
import { Client } from '../interfaces/client';
import { TopicService } from '../services/topic.service';
import { Topic } from '../interfaces/topic';
import { ModalComponent } from '../component/modal/modal.component';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-client-details',
  templateUrl: './client-details.component.html',
  styleUrls: ['./client-details.component.css']
})
export class ClientDetailsComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private clientService: ClientService,
    private topicService: TopicService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router
    )
    {}

  private currentId!: string;
  public currentClient!: ClientDetails;
  public listClients!: Client[];
  public listTopics!: Topic[];

  getDetails(id: string){
    this.clientService.getDetailsClient(id).subscribe((client: ClientDetails)=>{
      this.currentClient = client
      this.formatarData();
      this.getOtherClients();
      this.getTopics();
    });
  }
  getOtherClients(){
    this.clientService.getAll().subscribe((clients: Client[])=>{
      this.listClients = clients.filter(item => item.id !== this.currentClient.id)
    });
  }

  async getTopics(){
    this.topicService.getAll().subscribe((topics: Topic[])=>{
      this.listTopics = topics;
    });
  }

  topicsSubscribe(id: string): boolean{
    var exist = this.currentClient.topics.some(item => item.id === id);
    return exist;
  }
  ngOnInit(): void {
    // Acesse o ID a partir do ActivatedRoute
    this.route.params.subscribe(params => {
      this.currentId = params['id']; // 'id' é o nome do parâmetro definido na rota
      this.getDetails(this.currentId);
    });
  }
  formatarData() {
    this.currentClient.messages.forEach(element => {
      const dataObj = new Date(element.sendMessageDate);
      element.sendMessageDateFormat = format(dataObj, 'yyyy/MM/dd HH:mm:ss');
    });
  }

  openModalMessageForClient(idClient: string, nameClient: string){
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: 'MENSAGEM PARA \'' + nameClient +'\'',
        parameterPlaceholder: 'Mensagem',
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        const obj: Object = {
          clientId: idClient,
          message: result.name
        }
        this.messageService.sendMessage(obj).subscribe(
          () => {
            this.getDetails(this.currentId);
            this.cdr.detectChanges();
            this.openSnackBar(`Mensagem enviada para \' ${nameClient} \'`, 'Fechar', true);
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

  openModalMessageForTopic(idTopic: string, nameTopic: string){
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: 'MENSAGEM PARA TÓPICO \'' + nameTopic + '\'',
        parameterPlaceholder: 'Mensagem',
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        const obj: Object = {
          topicId: idTopic,
          message: result.name
        }
        this.messageService.sendMessage(obj).subscribe(
          () => {
            this.getDetails(this.currentId);
            this.cdr.detectChanges();
            this.openSnackBar(`Mensagem enviada para tópico \' ${nameTopic} \'`, 'Fechar', true);
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
  subscribeInTopic(idTopic: string, nameTopic: string){
    const subscribe: Object ={
      topicId: idTopic,
      clientId: this.currentId
    }
    this.clientService.subscribe(subscribe).subscribe(async () => {
      await this.getTopics();
      this.cdr.detectChanges();
      this.openSnackBar(`Tópico \' ${nameTopic} \' assinadoo`, 'Fechar', true);
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
