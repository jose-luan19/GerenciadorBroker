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
      console.log(this.currentClient);
    });
  }
  getOtherClients(){
    this.clientService.getAll().subscribe((clients: Client[])=>{
      this.listClients = clients.filter(item => item.id !== this.currentClient.id)
      console.log(this.listClients);
    });
  }

  getTopics(){
    this.topicService.getAll().subscribe((topics: Topic[])=>{
      this.listTopics = topics;
      console.log(this.listTopics);
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
      console.log('ID do cliente:', this.currentId);
    });
  }
  formatarData() {
    this.currentClient.messages.forEach(element => {
      const dataObj = new Date(element.sendMessageDate);
      element.sendMessageDateFormat = format(dataObj, 'yyyy/MM/dd HH:mm:ss');
    });
  }
}
