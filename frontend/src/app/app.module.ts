import { NavBarComponent } from './component/nav-bar/nav-bar.component';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ListQueuesComponent } from './list-queues/list-queues.component';
import { QueueService } from './services/queue.service';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ModalComponent } from './component/modal/modal.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ListTopicsComponent } from './list-topics/list-topics.component';
import { TopicService } from './services/topic.service';
import { ListClientsComponent } from './list-clients/list-clients.component';
import { ClientService } from './services/client.service';
import { ClientDetailsComponent } from './client-details/client-details.component';
import { MessageService } from './services/message.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavBarComponent,
    ListQueuesComponent,
    ModalComponent,
    ListTopicsComponent,
    ListClientsComponent,
    ClientDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatSnackBarModule
  ],
  providers: [
    QueueService,
    TopicService,
    ClientService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
