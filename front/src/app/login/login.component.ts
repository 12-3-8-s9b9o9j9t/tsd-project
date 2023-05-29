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

  isError = false;
  isRegister = false;
  checked = false;

  formGroup = new FormGroup({
    name: this.nameControl,
    password: this.passwordControl
  });

  constructor(
    private api: ApiHelperService,
    private router: Router
  ) { }


  onCheckboxChange(event: any) {
    this.checked = !event.target.checked;
  }

  async signIn(): Promise<void> {
    if (!this.formGroup.valid) {
      return;
    }
    let name: string = ""
    let password: string = ""

    if (this.nameControl.value !== null && this.passwordControl.value !== null) {
      name = this.nameControl.value;
      password = this.passwordControl.value;
    }

    try {
      const response = await this.api.post({ endpoint: '/User/auth/login', data: { name: name, password: password }});
      console.log("User found");

      if (name === null) {
        return;
      }
    
      saveName(response.name);
      saveID(response.id);
      this.moveToHome();
    } catch (error) {
      this.isError = true;
      console.log("User not found");
    }    
  }

  async register(): Promise<void> {

    if (!this.formGroup.valid) {
      return;
    }

    let name: string = ""
    let password: string = ""

    if (this.nameControl.value !== null && this.passwordControl.value !== null) {
      name = this.nameControl.value;
      password = this.passwordControl.value;
    }

    try {
      const response = await this.api.post({ endpoint: '/User', data: { name: name, password: password }});
      console.log("User created");
      this.signIn();
    } catch (error) {
      this.isError = true;
      console.log("Erorr creating user");
    }
    
  }

  moveToRegister(bool: boolean): void {
    this.isRegister = bool;
    this.isError = false;
    this.nameControl.reset();
    this.passwordControl.reset();
  }

  moveToHome() {
    this.router.navigateByUrl('/home');
  }
}
