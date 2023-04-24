import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getName, saveID, saveName } from "../services/storage.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  sessionCode: string = '1';
  user: string = '';

  joinSessionControl = new FormControl('', [Validators.required]);

  formGroup = new FormGroup({
    sessionCode: this.joinSessionControl,
  });

  constructor(private api: ApiHelperService, private router: Router) {
    this.user = getName();
  }


  enter(): void {
    if (!this.formGroup.valid) {
      return;
    }
    let session: string | null = this.joinSessionControl.value;
    // TODO: check if session exists

    // TODO: move to session
    this.moveToSession(this.sessionCode);

  }

  moveToSession(code: string): void {
    this.router.navigate(['/session', code, 'waiting-room']);

  }

  createSession(): void {
    //TODO create session and get code
    this.api.post({ endpoint: '/Session/createSession' }).then((response) => {
      console.log(response);
      console.log("Session created");
      this.router.navigate(['/session', this.sessionCode, 'waiting-room']);
    }
    ).catch((error) => {
      console.log(error);
    });

  }
}