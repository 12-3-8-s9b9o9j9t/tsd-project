import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getName, saveID, saveName, saveSessionIdentifier, setOwner } from "../services/storage.service";

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
    let sessionIdentifier: string | null = this.joinSessionControl.value;

    if (sessionIdentifier == null) {
      return ;
    }
    
    saveSessionIdentifier(sessionIdentifier);

    // TODO: move to session
    this.moveToSession(sessionIdentifier);

  }

  moveToSession(code: string): void {
    this.router.navigate(['/session', code, 'waiting-room']);
  }

  createSession(): void {
    this.api.post({ endpoint: '/Session/createSession', data: null }).then((response) => {
      console.log(response);
      console.log("Session created");

      saveSessionIdentifier(response.identifier);
      setOwner();

      this.moveToSession(response.identifier);
    }
    ).catch((error) => {
      console.error("Error when creating session :", error);
    });
  }
}