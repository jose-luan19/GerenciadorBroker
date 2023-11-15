import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private httpClient: HttpClient) { }
  private url = environment.apiUrlHTTP + "Message/";

  sendMessage(obj: Object){
    return this.httpClient.post(this.url,obj)
  }
  getCountMessagesInRabbitMQ(){
    return this.httpClient.get<number>(this.url);
  }


}
