import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getName, resetOwner, saveID, saveName, saveSessionIdentifier, setOwner } from "../services/storage.service";
import { MatSnackBar } from '@angular/material/snack-bar';

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

  constructor(private api: ApiHelperService, private router: Router, private snackBar: MatSnackBar) {
    this.user = getName();
  }

  goToHistory(): void {
    this.router.navigate(['/history']);
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
    resetOwner();
    this.moveToSession(sessionIdentifier);
  }

  moveToSession(code: string): void {
    this.router.navigate(['/session', code, 'waiting-room']);
  }

  async createSession(): Promise<void> {
    await this.api.post({ endpoint: '/Session/createSession', data: null }).then((response) => {
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

  csvInputChange(fileInputEvent: any) {
    let file = fileInputEvent.target.files[0];

    const formData: FormData = new FormData();
    formData.append('jiraFile', file, file.name);

    this.api.post({ endpoint: '/Session/createSession', data: formData }).then((response) => {
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

  logout(): void {
    resetOwner();
    saveName('');
    saveID(-1);
    this.user = '';
    this.snackBar.open('Vous êtes déconnecté', 'Fermer', {
      duration: 2000,
    });
    this.router.navigate(['/login']);

  }
}