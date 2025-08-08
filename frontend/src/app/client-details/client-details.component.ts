import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../services/client.service';
import { ClientDetails } from '../interfaces/clientDetails';
import { format } from 'date-fns';
import { Client } from '../interfaces/client';
import { ModalComponent } from '../component/modal/modal.component';
import { MessageService } from '../services/message.service';
import { Contact } from '../interfaces/contact';

@Component({
  selector: 'app-client-details',
  templateUrl: './client-details.component.html',
  styleUrls: ['./client-details.component.css']
})
export class ClientDetailsComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private clientService: ClientService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
  )
  {}

  private currentId!: string;
  public currentClient!: ClientDetails;
  public listPossiblesClients!: Client[];

  getData(){
    this.getDetails();
    this.getOtherClients();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.currentId = params['id'];
      this.getData();
    });
  }

  getDetails(){
    this.clientService.getDetailsClient(this.currentId).subscribe((client: ClientDetails)=>{
      this.currentClient = client;
      this.getMessages();
      this.cdr.detectChanges();
  });
}
  getMessages(){
    this.currentClient.messages = this.currentClient.messages.sort((a, b) => {
      const dateA = new Date(a.createDate);
      const dateB = new Date(b.createDate);
      return dateA.getTime() - dateB.getTime();
    });
    this.formatarData();
  }

  getOtherClients(){
    this.clientService.getAllContactsPossible(this.currentId).subscribe((clients: Client[])=>{
      this.listPossiblesClients = clients;
    });
  }

  formatarData() {
    this.currentClient.messages.forEach(element => {
      const dataObj = new Date(element.sendMessageDate);
      element.sendMessageDateFormat = format(dataObj, 'yyyy/MM/dd HH:mm:ss');
    });
  }

  removeContact(contact: Client){
    const contactRemove: Contact = {
      clientId: this.currentId,
      contactId: contact.id
    }
    this.clientService.removeContact(contactRemove).subscribe(()=>{
      this.getData();
      this.openSnackBar(`Contato \' ${contact.name} \' removido da lista`, 'Fechar');
    });
  }

  openModalAddContact(){
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: 'Selecionar Contato',
        listContactsPlaceholder: 'Contatos',
        listContacts: this.listPossiblesClients,
      },
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        const obj: Contact = {
          contactId: result.client.id,
          clientId: this.currentId,
        }
        this.clientService.addContact(obj).subscribe(
          () => {
            this.getData();
            this.openSnackBar(`Contato \' ${result.name} \' adicionado a lista`, 'Fechar');
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

  openModalMessageForClient(client: Client){
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: 'MENSAGEM PARA \'' + client.name +'\'',
        namePlaceholder: 'Mensagem',
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        const obj: Object = {
          ClientReceviedId: client.id,
          ClientSendId: this.currentId,
          message: result.name
        }
        this.messageService.sendMessage(obj).subscribe(
          () => {
            this.openSnackBar(`Mensagem enviada para \' ${client.name} \'`, 'Fechar');
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


  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
        duration: 6000,
        verticalPosition: 'bottom',
        horizontalPosition: 'end'
    });
  }

  changeStatusClient(){
    this.clientService.changeStatus(this.currentId).subscribe(()=>{
      this.currentClient.isOnline = !this.currentClient.isOnline;
      this.openSnackBar(`Cliente \' ${this.currentClient.name} \' mudou de STATUS`, 'Fechar');
      setTimeout(()=>{this.getDetails()}, 3000);
    });
  }

}
