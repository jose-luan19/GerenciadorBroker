import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ModalComponent } from '../component/modal/modal.component';
import { ClientService } from './../services/client.service';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Client } from '../interfaces/client';
import { Response } from '../interfaces/response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-clients',
  templateUrl: './list-clients.component.html',
  styleUrls: ['./list-clients.component.css']
})
export class ListClientsComponent implements OnInit{
  constructor(
    private clientService: ClientService,
    private cdr: ChangeDetectorRef,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router
    ){}

  public list: Client[] = [];

  ngOnInit(): void {
    this.getData();
  }

  getData(){
    this.clientService.getAll().subscribe((clients: Client[])=>{
      this.list = clients.sort((a, b) => {
        const dateA = new Date(a.createDate);
        const dateB = new Date(b.createDate);
        return dateA.getTime() - dateB.getTime();
      });
      this.cdr.detectChanges();
    });
  }
  clickDeleteClient(id: string){
    this.clientService.deleteClient(id).subscribe(
      () => {
        this.openSnackBar('Cliente excluído', 'Fechar');
        this.getData();
      },
      (error) => {
        if(error.status === 400){
          this.openSnackBar(error.error, 'Fechar');
        }
      }
    );
  }
  createClient(title: string, parameterString: string ): void {
    const dialogRef = this.dialog.open(ModalComponent, {
      width: '300px',
      data: {
        title: title,
        namePlaceholder: parameterString,
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.clientService.createClient(result.name).subscribe(
          (response: Response) => {
            this.openSnackBar(`Cliente \' ${response.name} \' criado`, 'Fechar');
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
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
        duration: 6000,
        verticalPosition: 'bottom',
        horizontalPosition: 'end'
    });
  }
  navigateToDetails(clientId: string) {
    // Aqui você deve navegar para a rota de detalhes do modelo
    this.router.navigate(['/client-details', clientId]);
  }

}
