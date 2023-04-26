import { Component } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { saveName, saveID } from '../services/storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

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
      this.moveToHome();
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
        this.moveToHome();
      }
      ).catch((error) => {
        console.log(error);
      }
      );
    });
  }

  moveToHome() {
    this.router.navigateByUrl('/home');


    // this.api.get({ endpoint: '/Session' }).then((response) => {
    //   console.log("Session found");
    //   this.router.navigateByUrl('/waiting-room');
    // }).catch((error) => {
    //   console.log(error);
    //   console.log("Session not found, creating new session");
    //   this.api.post({ endpoint: '/Session/createSession' }).then((response) => {
    //     console.log(response);
    //     console.log("Session created");
    //     this.router.navigateByUrl('/waiting-room');
    //   }
    //   ).catch((error) => {
    //     console.log(error);
    //   }
    //   );
    // });
  }

}
