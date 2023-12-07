import { Component, Inject} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {
  constructor(
    public dialogRef: MatDialogRef<ModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  shouldContacts(): boolean {
    return this.data.namePlaceholder === undefined && !!this.data.listContacts ? this.shouldLenghtListContacts() : false;
  }
  shouldLenghtListContacts(): boolean {
    return this.data.listContacts.length === 0;
  }
  isFormValid(): boolean {
    return !!this.data.name || !!this.data.client ;
  }
}
