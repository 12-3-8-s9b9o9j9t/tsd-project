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
  passwordControl = new FormControl('', [Validators.required]);

  formGroup = new FormGroup({
    name: this.nameControl,
    password: this.passwordControl
  });

  constructor(
    private api: ApiHelperService,
    private router: Router
  ) { }


  async enter(): Promise<void> {
    if (!this.formGroup.valid) {
      return;
    }
    let name: string | null = this.nameControl.value;


    try {
      const response = await this.api.get({ endpoint: '/User/' + name });
      console.log("User found");

      if (name === null) {
        return;
      }
    
      saveName(name);
      saveID(response.id);
      this.moveToHome();
    } catch (error) {
      console.log("User not found, creating new user");
    
      try {
        const response = await this.api.post({ endpoint: '/User', data: { name: name } });
        console.log(response);
    
        if (name === null) {
          return;
        }
    
        saveName(name);
        saveID(response.id);
        this.moveToHome();
      } catch (error) {
        console.log(error);
      }
    }    
  }

  moveToHome() {
    this.router.navigateByUrl('/home');
  }
}
