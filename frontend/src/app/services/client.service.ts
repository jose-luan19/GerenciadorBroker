import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Client } from '../interfaces/client';
import { Response } from '../interfaces/response';
import { ClientDetails } from '../interfaces/clientDetails';
import { Contact } from '../interfaces/contact';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private httpClient: HttpClient) { }
  private url = environment.apiUrlHTTP + "Client/";
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
  getDetailsClient(id: string){
    return this.httpClient.get<ClientDetails>(this.url + id);
  }
  changeStatus(id: string){
    return this.httpClient.put(this.url+ id, {});
  }
  getAllContactsPossible(id: string){
    return this.httpClient.get<Client[]>(this.url + "OthersContacts/" + id);
  }
  addContact(contact: Contact){
    return this.httpClient.post(this.url + "AddContact/", contact);
  }
  removeContact(contact: Contact){
    return this.httpClient.delete(this.url + "RemoveContact/", {body: contact});
  }
}
