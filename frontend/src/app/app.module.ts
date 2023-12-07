import { NavBarComponent } from './component/nav-bar/nav-bar.component';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ModalComponent } from './component/modal/modal.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ListClientsComponent } from './list-clients/list-clients.component';
import { ClientService } from './services/client.service';
import { ClientDetailsComponent } from './client-details/client-details.component';
import { MessageService } from './services/message.service';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavBarComponent,
    ModalComponent,
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
    MatSelectModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatSnackBarModule
  ],
  providers: [
    ClientService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
