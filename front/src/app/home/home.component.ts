import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {saveName} from "../services/storage.service";

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
    private router: Router
  ) { }


  enter(): void {
    if (!this.formGroup.valid) {
      return ;
    }

    let name: string | null = this.nameControl.value;

    if (name == null) {
      return ;
    }

    saveName(name)
    this.router.navigateByUrl('/session');
  }


}
