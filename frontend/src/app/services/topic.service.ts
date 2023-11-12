import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Topic } from '../interfaces/topic';
import { Response } from '../interfaces/response';


@Injectable({
  providedIn: 'root'
})
export class TopicService {

  constructor(private httpClient: HttpClient) { }
  private url = environment.apiUrl + "Topic/";
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
  });

  getAll(){
    let list = this.httpClient.get<Topic[]>(this.url)
    return list
  }

  deleteTopic(name: string){
    const params = new HttpParams()
    .set('topicName', name);
    return this.httpClient.delete(this.url, { headers: this.headers, params });
  }

  createTopic(name: string, routingKey: string ){
    const urlPost = this.url+`?Name=${name}&RoutingKey=${routingKey}`
    return this.httpClient.post<Response>(urlPost,{});
  }
}
