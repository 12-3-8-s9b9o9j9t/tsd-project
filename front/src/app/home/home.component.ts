import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { saveID, saveName } from "../services/storage.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  sessionControl = new FormControl('', [Validators.required]);

  formGroup = new FormGroup({
    name: this.sessionControl,
  });

  constructor(
    private api: ApiHelperService,
    private router: Router
  ) { }


  enter(): void {
    if (!this.formGroup.valid) {
      return;
    }
    let session: string | null = this.sessionControl.value;
    // TODO: check if session exists

    // TODO: move to session
    this.moveToSession();

  }

  moveToSession() {
    
  }

  createSession(): void {
    
  }
}