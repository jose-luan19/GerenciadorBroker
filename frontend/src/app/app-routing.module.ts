import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListQueuesComponent } from './list-queues/list-queues.component';
import { ListTopicsComponent } from './list-topics/list-topics.component';
import { ListClientsComponent } from './list-clients/list-clients.component';
import { ClientDetailsComponent } from './client-details/client-details.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'queues', component: ListQueuesComponent},
  { path: 'topics', component: ListTopicsComponent},
  { path: 'clients', component: ListClientsComponent},
  { path: 'client-details/:id', component: ClientDetailsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
