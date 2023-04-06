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

  nameControl = new FormControl('', [Validators.required]);

  formGroup = new FormGroup({
    name: this.nameControl,
  });

  constructor(
    private api: ApiHelperService,
    private router: Router
  ) { }


  enter(): void {
    if (!this.formGroup.valid) {
      return;
    }
    let name: string | null = this.nameControl.value;


    this.api.get({ endpoint: '/User/' + name }).then((response) => {
      console.log("User found");
      console.log(response);
      if (name === null) {
        return;
      }
      saveName(name);
      saveID(response.id);
      this.moveToSession();
    }).catch((error) => {
      console.log(error);
      console.log("User not found, creating new user");
      this.api.post({ endpoint: '/User', data: { name: name } }).then((response) => {
        console.log(response);
        if (name === null) {
          return;
        }
        saveName(name);
        saveID(response.id);
        this.moveToSession();
      }
      ).catch((error) => {
        console.log(error);
      }
      );
    });
  }

  moveToSession() {
    this.api.get({ endpoint: '/Session' }).then((response) => {
      console.log("Session found");
      this.router.navigateByUrl('/waiting-room');
    }).catch((error) => {
      console.log(error);
      console.log("Session not found, creating new session");
      this.api.post({ endpoint: '/Session/createSession' }).then((response) => {
        console.log(response);
        console.log("Session created");
        this.router.navigateByUrl('/waiting-room');
      }
      ).catch((error) => {
        console.log(error);
      }
      );
    });
  }
}