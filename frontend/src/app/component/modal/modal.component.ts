import { Component, Inject} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

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
  shouldRoutingKey(): boolean {
    return this.data.routingKeyPlaceholder !== undefined;
  }
  isFormValid(): boolean {
    return !!this.data.name && (this.shouldRoutingKey() ? !!this.data.routingKey : true);
  }
}
