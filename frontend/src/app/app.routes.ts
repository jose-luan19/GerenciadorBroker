import { ListQueuesComponent } from './list-queues/list-queues.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'queues', component: ListQueuesComponent},
];
