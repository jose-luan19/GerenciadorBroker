import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Client } from '../interfaces/client';
import { Response } from '../interfaces/response';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private httpClient: HttpClient) { }
  private url = environment.apiUrl + "Client/";
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
  });

  getAll(){
    let list = this.httpClient.get<Client[]>(this.url)
    return list
  }

  deleteClient(id: string){
    const urlDelete = this.url+`${id}`
    const params = new HttpParams()
    .set('id', id);
    return this.httpClient.delete(urlDelete, { headers: this.headers, params });
  }

  createClient(name: string){
    const urlPost = this.url+`?Name=${name}`
    return this.httpClient.post<Response>(urlPost,{})
  }

}
