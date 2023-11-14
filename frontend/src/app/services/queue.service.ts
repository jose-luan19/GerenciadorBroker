import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Queue } from '../interfaces/queue';
import { Response } from '../interfaces/response';


@Injectable({
  providedIn: 'root'
})
export class QueueService {

  constructor(private httpClient: HttpClient) { }
  private url = environment.apiUrl + "Queue/";
  private urlRabbit = environment.apiRabbitMQUrl + "queues";
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
  });

  getAll(){
    let list = this.httpClient.get<Queue[]>(this.url)
    return list
  }

  deleteQueue(id: string){
    const urlDelete = this.url+`${id}`
    const params = new HttpParams()
    .set('id', id);
    return this.httpClient.delete(urlDelete, { headers: this.headers, params });
  }

  createQueue(name: string){
    const urlPost = this.url+`?Name=${name}`
    return this.httpClient.post<Response>(urlPost,{})
  }
}
